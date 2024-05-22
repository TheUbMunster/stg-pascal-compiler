using PascalCompiler.Scanner;
using PascalCompiler.Lexer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Reflection;

namespace PascalCompiler.Parser
{
   public class ASTNode
   {
      /*
       * Options I've considered for this parser design:
       * 
       * 1. Explicit FlattenedData (yields properties in-order)
       * Upsides:
       *     - Lets me automate NodeLength
       *     - Lets me automate FileLocation so long as there exists at least one data member
       * Downsides:
       *     - Requires per-node implementation of IEnumerable
       *     - Requires creating & managing an object that can represent either a Token or an ASTNode
       *     - Currently doesn't account for the skippable tokens
       * 
       * 2. Attributes
       * Upsides:
       *     - Lets me automate NodeLength
       *     - Lets me automate FileLocation so long as there exists at least one data member
       * Downsides:
       *     - Requires lots of reflection
       *     - Not easy to implment skippable tokens
       *     
       * 3. Two dictionaries<int, ASTNode/Token> (key represents the order that that grammar element exists for this Node)
       * Upsides:
       *     - Lets me automate NodeLength
       *     - Lets me automate FileLocation so long as there exists at least one data member
       *     - Easy to implement skippable tokens
       * Downsides:
       *     - Hard to bind names to Nodes & Tokens
       *     - If there's optional properties and/or variable length properties (e.g., list of ASTNode), then you can't know where your desired value sits in the dictionary.
       *     
       * 4. All manual w/ skippables appended at the end
       * Upsides:
       *     - Avoids most downsides from the other options.
       * Downsides:
       *     - Skippable tokens are just tacked on the end, they aren't where they belong.
       *     - Users of FileLocation & NodeLength probably care about the skippable content
       *     - 
       *     
       * 5. All manual w/ each Token member containing a list, where the last element in the list is the element we care about, and the rest are the skippables that came before it.
       * Upsides:
       *     - Each token entry (member) represents itself, as well as the skippables that came before it
       *     - Subsequent ASTNode members will handle their own skippables
       *     - Wouldn't be hard to implement a flattened view for NodeLength & FileLocation (if an entry exists).
       *     - flattened view would only consist of tokens, no ASTNodes
       * Downsides:
       *     - flattened view would be manually written per ASTNode.
       */

      private static readonly IReadOnlySet<TokenType> skippableTokenTypes = new HashSet<TokenType>() //not IReadOnlyList bc I need List<>.Contains()
      { //tokens of these types are treated as though they don't exist during the parsing step.
         TokenType.Comment,
         TokenType.WHITESPACE,
         TokenType.LINEBREAK
      };
      public static readonly ASTNode UndefinedNode = new ASTNode() { nodeType = ASTNodeType.UNDEFINED };
      protected ASTNode() { }
      protected Source MySource { get; init; } = null!;
      private ASTNodeType? nodeType = null!;
      public ASTNodeType NodeType 
      {
         get
         {
            if (nodeType == null)
            {
               if (Enum.TryParse<ASTNodeType>(GetType().Name, out ASTNodeType n))
                  nodeType = n;
               else
                  throw new Exception($"Could not retrieve the ASTNodeType for object of type {GetType().Name}");
            }
            return nodeType.Value;
         }
      }
      public int FileLocation { get; set; } = -1;
      public int NodeLength { get => FlattenedView().Select(x => x.TokenLength).Sum(); }
      public string Content { get => FlattenedView().Select(x => x.Content).Aggregate((x, y) => x + y); }

      #region Parse
      public static ASTNode? Parse(Source source, ref int index)
      {
         ASTNode? result = null;
         result ??= Program.Parse(source, ref index); //there are a couple of non-recipie non-terminals, but this is the beginning node for the grammar.
         return result;
      }

      protected static TokenNode PopToken(Source source, ref int index)
      {
         //todo: change this so that it returns a list of tokens, containing at least one token, where the last (and possibly only)
         //token in the list is a non-skippable token, and all the others are the skippable tokens before it.
         //this way the caller can consume the token length of all these other tokens

         //potentially return the non-skippable via an out parameter, and make the return value the list of skippables?
         //(or vice versa, i.e., return the skippable ones via an out parameter and return the non-skippable)?
         var tokens = source.LexerTokens;
         Token tok = Token.UndefinedToken;
         List<Token> skippableTokens = new List<Token>();
         for (; index < tokens.Count; index++)
         {
            if (skippableTokenTypes.Contains(tokens[index].Type))
               skippableTokens.Add(tokens[index]);
            else
            {
               tok = tokens[index];
               break;
            }
         }
         index++; //fix off-by-one
         return new TokenNode(skippableTokens, tok);
      }

      /// <summary>
      /// This function asserts that all the necessary fields are assigned during construction.
      /// This includes situations where it might be legal for only some fields to be assigned,
      /// but only in specific combinations. Required to be overrided and implemented in subclasses.
      /// 
      /// If assertion fails, that indicates a bug in the compiler, not a syntax/parse error in the source.
      /// </summary>
      protected virtual void AssertCorrectStructure()
      {
         //asserts that all the data fields are assigned correctly. If not, this adds a message to the source
         //and throws an exception.
         throw new NotImplementedException();
      }

      protected internal virtual IEnumerable<Token> FlattenedView()
      {
         //iterates through this ast node's data members.
         //if it is a token, yield it
         //if it is an ASTNode, then yield node.FlattenedView()
         throw new NotImplementedException();
      }
      #endregion

      #region Object Overrides
      public override sealed string ToString()
      {
         throw new Exception($"The regular object.ToString() method was called on an object of type {GetType().Name}, you probably should have used ToString(int, bool)");
      }

      public virtual string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         throw new Exception($"This ASTNode's ToString(int, bool) function was not overrided by it's deriver! Type: {GetType().Name}");
      }
      #endregion
   }

   public static class Parser
   {
      public static void Parse(Source source)
      {
         //i'm 80% sure that since "Program" is the start symbol, and the program ?must? be terminated by the last "end ." in the file, that
         //any file parsed will result in one mega ASTNode, not a list?
         int ind = 0;
         bool firstIter = true;
         while (ind < source.LexerTokens.Count/* && source.LexerTokens[ind].Type != TokenType.END_OF_FILE*/)
         {
            int indBefore = ind;
            ASTNode? node = ASTNode.Parse(source, ref ind);
            if (node == null/* || node.NodeType == ASTNodeType.UNDEFINED*/)
            {
               source.AppendMessage(new Source.Message(Source.CompilerPhase.Parse, Source.Severity.Error, "No valid candidates for parsing token", source.LexerTokens[ind].FileLocation, true));
               throw new Exception(); //todo: put a message in this??
            }
            else if (indBefore == ind)
            {
               source.AppendMessage(new Source.Message(Source.CompilerPhase.Parse, Source.Severity.Error, "The root parse function parsed a non-null node, and yet it consumed no tokens! No progress in the token stream was made, so this will go on forever; breaking free.", node.FileLocation, true));
               throw new Exception(); //todo: put a message in this??
            }
            if (firstIter)
            {
               firstIter = false;
               if (node.NodeType != ASTNodeType.Program)
               {
                  source.AppendMessage(new Source.Message(Source.CompilerPhase.Parse, Source.Severity.Error, "The first AST Node parsed was not the 'Program' node!", node.FileLocation, true));
                  throw new Exception(); //todo: put a message in this??
               }
            }

            source.AddParserNode(node);
         }
         source.ParsingComplete();
      }
   }
}
