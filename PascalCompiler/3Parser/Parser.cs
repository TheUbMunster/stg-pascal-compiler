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
      protected ASTNode() { }
      protected Source MySource { get; init; } = null!;
      public virtual ASTNodeType NodeType { get; private init; }
      //todo: automate this with the custom data attribute thing described for
      //nodelength
      public int FileLocation { get; set; } = -1; //instead of assigning the backup value sometimes and not others, just assign this manually all the time so we don't get confused.
      //todo: automate this via making a custom attribute to put on the data members,
      //then have this grab all the non-null data members and aggregate their length.
      //(you'll need to add a data member that holds all the skippables).
      public int NodeLength { get; protected set; } = -1;
      //public int NodeLength 
      //{
      //   get
      //   {
      //      return GetType().GetProperties()
      //         .Where(prop => prop.GetCustomAttribute<ParserDataAttribute>(false) != null)
      //         .OrderBy(x => x.GetCustomAttribute<ParserDataAttribute>(false)!.PropertyOrder)
      //         .Select<PropertyInfo, int>(prop =>
      //         {
      //            switch (prop.GetValue(this))
      //            {
      //               case ASTNode node:
      //                  return node.NodeLength;
      //               case Token tok:
      //                  return tok.TokenLength;
      //               case IReadOnlyList<object> list: 
      //                  Type listElemType = list.GetType().GetGenericArguments().Single();
      //                  if (listElemType.GetCustomAttribute(typeof(ParserDataContainerAttribute), false) != null)
      //                  {  //list of structs that are ParserDataContainerAttribute
      //                     int aggr = 0;
      //                     foreach (object e in list)
      //                     {
      //                        aggr += e.GetType().GetProperties().Select(dataProp =>
      //                        {
      //                           switch (dataProp.GetValue(this))
      //                           {
      //                              case ASTNode node:
      //                                 return node.NodeLength;
      //                              case Token tok:
      //                                 return tok.TokenLength;
      //                              default:
      //                                 throw new Exception(); //todo add message?
      //                           }
      //                        }).Aggregate((x, y) => x + y);
      //                     }
      //                     return aggr;
      //                  }
      //                  else if (listElemType.IsAssignableTo(typeof(ASTNode)))
      //                  {  //list of parser data (i.e., a list of ASTNode)
      //                     int aggr = 0;
      //                     foreach (ASTNode n in list.Cast<ASTNode>())
      //                     {
      //                        aggr += n.NodeLength;
      //                     }
      //                     return aggr;
      //                  }
      //                  else if (listElemType.IsAssignableTo(typeof(Token)))
      //                  {  //list of parser data (i.e., a list of Token)
      //                     int aggr = 0;
      //                     foreach (Token n in list.Cast<Token>())
      //                     {
      //                        aggr += n.TokenLength;
      //                     }
      //                     return aggr;
      //                  }
      //                  else
      //                     throw new Exception(); //todo add message?
      //               default:
      //                  throw new Exception(); //todo add message?
      //            }
      //         }).Aggregate((x, y) => x + y);
      //   } 
      //}
      public string Content { get; protected set; } = "Content of this AST node was not set via the Parser.";

      public static ASTNode? Parse(Source source, ref int index)
      {
         ASTNode? result = null;
         result ??= Program.Parse(source, ref index);
         //list of:
         //result ??= [ASTNodeCategory].Parse(tokens, ref index);
         //where [ASTNodeCategory] is a type that one or more terminals derive from.
         return result;
      }

      protected static Token PopToken(Source source, ref int index, bool firstNonskippableToken = true)
      {
         //todo: change this so that it returns a list of tokens, containing at least one token, where the last (and possibly only)
         //token in the list is a non-skippable token, and all the others are the skippable tokens before it.
         //this way the caller can consume the token length of all these other tokens

         //potentially return the non-skippable via an out parameter, and make the return value the list of skippables?
         //(or vice versa, i.e., return the skippable ones via an out parameter and return the non-skippable)?
         var tokens = source.LexerTokens;
         Token? tok = Token.UndefinedToken;
         for (; index < tokens.Count; index++)
         {
            if (skippableTokenTypes.Contains(tokens[index].Type))
               continue;
            else
            {
               tok = tokens[index];
               break;
            }
         }
         index++; //fix off-by-one
         return tok!;
      }

      //this doesn't work because Consume would need a way to generally consume a thing and assign it to a member, but do we assign it
      //to the first member, or the second, or the third? What if one of the members is a list of things? How do we know if we need to 
      //add it to the list or assign it to the member(s) after the list?
      //suffers from the same problem as flattened data.
      //protected bool Consume(TokenType tokenType)
      //{
      //   throw new NotImplementedException();
      //}

      //protected bool Consume(ASTNodeType nodeType)
      //{
      //   throw new NotImplementedException();
      //}

      //this concept is useful, but can't work this way because the Consume() methods don't work.
      //implement this concept in straight code that goes inside each parse function.
      //protected void BeginUnifiedConsumableBlock()
      //{
      //   //if this method is called, all subsequent calls to Consume()
      //   //must pass. If any of them fail, then they all fail and none are consumed.
      //   //if all pass and then EndUnifiedConsumableBlock() is called, then all are consumed.
      //}

      //protected void EndUnifiedConsumableBlock()
      //{

      //}

      public override sealed string ToString()
      {
         throw new Exception($"The regular object.ToString() method was called on an object of type {GetType().Name}, you probably should have used ToString(int, bool)");
         //return ToString(0, false);
      }

      public virtual string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         return $"This ASTNode's ToString(int, bool) function was not overrided by it's deriver! Type: {GetType().Name}";
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
