//Copyright TheUbMunster, 2024, all rights reserved.

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
      protected internal Source MySource { get; init; } = null!;
      private ASTNodeType? nodeType = null!;
      public ASTNodeType NodeType 
      {
         get
         {
            if (nodeType == null)
            {
               Type nodeClass = GetType();
               if (this is IAmbiguous) //a placeholder (Factor.Ambiguous etc.) stands in for its base class, see IAmbiguous
                  nodeClass = nodeClass.BaseType!;
               if (Enum.TryParse<ASTNodeType>(nodeClass.Name, out ASTNodeType n))
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
      /// Returns true if the lexer tokens in [startIndex, endIndex) form a single lexical token in the sense of 6.1.8: token
      /// separators (whitespace, linebreaks, comments) may come before the first token, but never between the rest.
      /// The nonterminals of 6.1 (identifier, digit-sequence, unsigned-real, ...) are assembled by the parser out of
      /// single-character tokens, so without this check "foo bar" would be accepted as the identifier "foobar" and
      /// "1 .5" as the real "1.5". Note that the lexer doesn't emit separators as tokens; it attaches them to the token
      /// that follows them (Token.AbsorbedTokens).
      /// </summary>
      protected static bool IsSingleLexicalToken(Source source, int startIndex, int endIndex)
      {
         var tokens = source.LexerTokens;
         int i = startIndex;
         while (i < endIndex && skippableTokenTypes.Contains(tokens[i].Type))
            i++; //separators before the first token are fine, they belong to it.
         for (i = i + 1; i < endIndex; i++)
         {
            if (skippableTokenTypes.Contains(tokens[i].Type) || tokens[i].AbsorbedTokens.Count > 0)
               return false;
         }
         return true;
      }

      /// <summary>One alternative of a nonterminal, in the shape of every Parse method: returns the node or null, and advances index only on success.</summary>
      protected delegate T? Alternative<T>(Source source, ref int index) where T : ASTNode;

      /// <summary>
      /// For a nonterminal whose alternatives can be spelled identically (see IAmbiguous). Every alternative is parsed from the
      /// same position; the ones that consumed the most tokens survive, because a shorter match is not another reading of the same
      /// text but a prefix of it (the variable-access "f" is not a reading of "f(1)"). One survivor is returned as is; two or
      /// more are handed to <paramref name="placeholder"/> (normally the Ambiguous.Create of the class) and the placeholder is
      /// returned; null when no alternative matched. index is advanced past the survivors' tokens.
      ///
      /// Dropping the shorter readings is a greedy choice, and it is only correct when the shorter reading could not have been continued:
      /// the token that a longer reading went on with must not be one that may legally follow the nonterminal, otherwise the shorter
      /// reading plus that continuation would be a second parse of the parent and the ambiguity would not be local to this node.
      /// Every call site states why that holds for it; it is a property of the grammar's FOLLOW sets and can be checked from them.
      /// </summary>
      protected static T? ParseLongest<T>(Source source, ref int index, Func<Source, IReadOnlyList<T>, T> placeholder, params Alternative<T>[] alternatives) where T : ASTNode
      {
         List<T> survivors = new List<T>();
         int survivorsEnd = -1;
         foreach (Alternative<T> alternative in alternatives)
         {
            int tind = index;
            T? candidate = alternative(source, ref tind);
            if (candidate == null)
               continue;
            if (tind > survivorsEnd)
            {
               survivors.Clear();
               survivorsEnd = tind;
            }
            if (tind == survivorsEnd)
               survivors.Add(candidate);
         }
         if (survivors.Count == 0)
            return null;
         index = survivorsEnd;
         return survivors.Count == 1 ? survivors[0] : placeholder(source, survivors);
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
         //"program" is the start symbol, and a program is terminated by its final '.', so a source file parses to exactly one
         //Program node. The only things allowed after it are skippable tokens (whitespace, linebreaks, comments) and the EOF token.
         var tokens = source.LexerTokens;
         int ind = 0;
         ASTNode? node = ASTNode.Parse(source, ref ind);
         if (node == null)
         {
            int errorLocation = tokens.Count > 0 ? tokens[Math.Min(ind, tokens.Count - 1)].FileLocation : 0;
            source.AppendMessage(new Source.Message(Source.CompilerPhase.Parse, Source.Severity.Error, "No valid candidates for parsing token", errorLocation, true));
            return;
         }
         if (node.NodeType != ASTNodeType.Program)
         {
            source.AppendMessage(new Source.Message(Source.CompilerPhase.Parse, Source.Severity.Error, "The first AST Node parsed was not the 'Program' node!", node.FileLocation, true));
            return;
         }
         source.AddParserNode(node);

         while (ind < tokens.Count && (Token.absorbedTypes.Contains(tokens[ind].Type) || tokens[ind].Type == TokenType.EOF))
            ind++;
         if (ind < tokens.Count)
         {
            source.AppendMessage(new Source.Message(Source.CompilerPhase.Parse, Source.Severity.Error, "Unexpected content after the end of the program", tokens[ind].FileLocation, true));
            return;
         }
         source.ParsingComplete();
      }
   }
}
