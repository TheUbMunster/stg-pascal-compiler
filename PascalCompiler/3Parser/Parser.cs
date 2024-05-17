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

namespace PascalCompiler.Parser
{
   public class ASTNode
   {
      private static readonly IReadOnlySet<TokenType> skippableTokenTypes = new HashSet<TokenType>() //not IReadOnlyList bc I need List<>.Contains()
      { //tokens of these types are treated as though they don't exist during the parsing step.
         TokenType.Comment,
         TokenType.WHITESPACE,
         TokenType.LINEBREAK
      };
      protected ASTNode()
      {
         //this automatically assigns the NodeType enum without derived types needing to worry about it.
         //THIS ONLY WORKS IF THE "ASTNodeType" NAME MATCHES THE CORRESPONDING CLASS NAME!!!

         //this needs to be re-addressed.
         //Type tt = GetType();
         //if (tt == typeof(ASTNode))
         //   NodeType = ASTNodeType.UNDEFINED; //this can only happen if someone directly instantiates an object of type ASTNode (don't do that)
         //while (tt!.BaseType != typeof(ASTNode))
         //   tt = tt.BaseType!; //should be impossible to error, since this code is only executed by implementers?
         //string typename = tt.Name; //gets name of direct derived instance
         //try
         //{
         //   NodeType = Enum.Parse<ASTNodeType>(typename);
         //}
         //catch (ArgumentException e)
         //{
         //   throw new AggregateException("It seems like one of your derived ASTNode types does not match an ASTNodeType enum name", e);
         //}
         //at this point just use GetType().Name and use that instead of an enum lol.
      }
      //when derived classes call the ctor, can we automate assigning the MySource?
      //maybe implement this concept in the base class ASTNode?
      //protected List<ASTNode || Token> children;
      //That way we can automate things like FileLocation as get => children[0].FileLocation
      //however, automating FileLocation sometimes can't be automated, because e.g., label-declaration-part from 6.2.1
      //can sometimes be comprised by nothing and therefore has no tokens to check the filelocation of.
      //perhaps it should be automated via at the whenever you create the ASTNode, you pass in the file location
      //via the constructor arguments (instead of through new() { } property building syntax).
      //or maybe keep a FallbackFileLocation that can be assigned separately and used if the children list is empty.

      //and automate ConcatContent get => string.Join("", children.Select(x => x.Content));
      //same thing with NodeLength

      //after thinking about the above for a bit, I've made a few observations and had a few thoughts:
      //an element of "data" within an AST node is either another AST node or a lexer token.
      //sometimes this data is organized in complex structures (e.g., a list of ast nodes, or a matrix
      //of tuples of ast nodes and lexer tokens).
      //Although a list of "children" would be useful for the above reasons, any consumers of these
      //types could sometimes have a hard time finding what they're looking for in this array.
      //therefore, we should have an abstract coroutine in ASTNode that's overridden in each of the
      //inheritors that yields enumerator elements "in order". That way, the above features like
      //ConcatContent can be automatically generated.
      //The IEnumerable type needs to encapsulate either ASTNodes or Lexer tokens, and a call
      //to content/concatcontent or filelocation needs to be router to the proper object depending
      //on what the object is.
      //this would most easily be implemented with a shared interface, but that would tie both the
      //lexer and the parser to a shared backing source, and I'd rather keep them more separate.
      //instead, maybe a wrapper class for this functionality?
      protected Source MySource { get; init; } = null!;
      public virtual ASTNodeType NodeType { get; private init; }
      public int FileLocation { get; set; } = -1; //instead of assigning the backup value sometimes and not others, just assign this manually all the time so we don't get confused.
      public int NodeLength { get; protected set; } = -1;
      public string Content { get; protected set; } = "Content of this AST node was not set via the Parser.";

      public static ASTNode? Parse(Source source, ref int index)
      {
         ASTNode? result = null;
         //list of:
         //result ??= [ASTNodeCategory].Parse(tokens, ref index);
         //where [ASTNodeCategory] is a type that one or more terminals derive from.
         return result;
      }

      protected static Token PopToken(Source source, ref int index, bool firstNonskippableToken = true)
      {
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

   #region Node Grammatical Classes

   public class ActualParameter : ASTNode
   {
      public Expression? Expression { get; private init; } = null!;
      public VariableAccess? VariableAccess { get; private init; } = null!;
      public ProcedureIdentifier? ProcedureIdentifier { get; private init; } = null!;
      public FunctionIdentifier? FunctionIdentifier { get; private init; } = null!;
      public override ASTNodeType NodeType { get => ASTNodeType.ActualParameter; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         if (prettyPrint)
            return $"{new string('\t', indentLevel)}({NodeType}{Environment.NewLine}" +
                   Expression != null ? $"{new string('\t', indentLevel + 1)}{Expression!.ToString(indentLevel + 1, prettyPrint)}{Environment.NewLine}" : string.Empty +
                   VariableAccess != null ? $"{new string('\t', indentLevel + 1)}{VariableAccess!.ToString(indentLevel + 1, prettyPrint)}{Environment.NewLine}" : string.Empty +
                   ProcedureIdentifier != null ? $"{new string('\t', indentLevel + 1)}{ProcedureIdentifier!.ToString(indentLevel + 1, prettyPrint)}{Environment.NewLine}" : string.Empty +
                   FunctionIdentifier != null ? $"{new string('\t', indentLevel + 1)}{FunctionIdentifier!.ToString(indentLevel + 1, prettyPrint)}{Environment.NewLine}" : string.Empty +
                   $"{new string('\t', indentLevel)})";
         else
            return $"({NodeType}" +
                   Expression != null ? $"{Expression!.ToString(indentLevel + 1, prettyPrint)}" : string.Empty +
                   VariableAccess != null ? $"{VariableAccess!.ToString(indentLevel + 1, prettyPrint)}" : string.Empty +
                   ProcedureIdentifier != null ? $"{ProcedureIdentifier!.ToString(indentLevel + 1, prettyPrint)}" : string.Empty +
                   FunctionIdentifier != null ? $"{FunctionIdentifier!.ToString(indentLevel + 1, prettyPrint)}" : string.Empty +
                   $")";
      }
      protected override void AssertCorrectStructure()
      {
         int totalNonNull = 0;
         totalNonNull += Expression != null ? 1 : 0;
         totalNonNull += VariableAccess != null ? 1 : 0;
         totalNonNull += ProcedureIdentifier != null ? 1 : 0;
         totalNonNull += FunctionIdentifier != null ? 1 : 0;
         //should be exactly one non-null
         if (totalNonNull != 1)
         {
            throw new InvalidOperationException($"Compiler parse error in {GetType().Name} (compiler bug): invalid object state.");
         }
      }
      public new static ActualParameter? Parse(Source source, ref int index)
      {
         int tind = index;
         ActualParameter? node = null;


         //I don't like this pattern for grammar like "a = b | c | d | e .". Think of a better one
         //body
         Expression? e = Expression.Parse(source, ref tind);
         VariableAccess? v = null;
         ProcedureIdentifier? p = null;
         FunctionIdentifier? f = null;
         if (e == null)
         {
            v = VariableAccess.Parse(source, ref tind); //we only want to attempt to parse if the previous one failed.
            if (v == null)
            {
               p = ProcedureIdentifier.Parse(source, ref tind); //"" ""
               if (p == null)
               {
                  f = FunctionIdentifier.Parse(source, ref tind); //"" ""
               }
            }
         }

         if (e != null || v != null || p != null || f != null) //only one can be non-null
         {
            ASTNode content = ((((ASTNode?)e ?? v) ?? p) ?? f)!;
            node = new ActualParameter()
            {
               MySource = source,
               FileLocation = content.FileLocation,
               NodeLength = content.NodeLength,
               Expression = e,
               VariableAccess = v,
               ProcedureIdentifier = p,
               FunctionIdentifier = f,
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ActualParameterList : ASTNode
   {
      public Token OpenParenToken { get; private init; } = null!;
      public ActualParameter ActualParameter { get; private init; } = null!;
      public IReadOnlyList<(Token commaToken, ActualParameter actualParameter)> SecondaryActualParameters { get; private set; } = null!;
      public Token CloseParenToken { get; private init; } = null!;
      public override ASTNodeType NodeType { get => ASTNodeType.ActualParameterList; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {
         if (OpenParenToken == null || ActualParameter == null || SecondaryActualParameters == null || CloseParenToken == null)
            throw new InvalidOperationException($"Compiler parse error in {GetType().Name} (compiler bug): invalid object state.");
      }
      public new static ActualParameterList? Parse(Source source, ref int index)
      {
         int tind = index, lengthAggr = 0;
         ActualParameterList? node = null;

         Token openParenTok = PopToken(source, ref tind); lengthAggr += openParenTok.TokenLength;
         if (openParenTok.Type == TokenType.OpenParen)
         {
            ActualParameter? firstActualParameter = ActualParameter.Parse(source, ref tind); lengthAggr += firstActualParameter?.NodeLength ?? 0;
            if (firstActualParameter != null)
            {
               List<(Token commaToken, ActualParameter actualParameter)> secondaryActualParameters = new();
               {
                  while (true)
                  {
                     int pretind = tind;
                     Token tempComma = PopToken(source, ref tind);
                     ActualParameter? tempActualParameter = ActualParameter.Parse(source, ref tind);
                     if (tempComma.Type == TokenType.Comma && tempActualParameter != null)
                     { //both exist (add to collection and continue)
                        lengthAggr += tempComma.TokenLength;
                        lengthAggr += tempActualParameter.NodeLength;
                        secondaryActualParameters.Add((tempComma, tempActualParameter));
                     }
                     else //in any other situation
                     {
                        tind = pretind;
                        break;
                     }
                     //else if (tempComma.Type != TokenType.Comma && tempActualParameter == null)
                     //{ //neither exist (end gracefully)
                     //   tind = pretind;
                     //   break;
                     //}
                     //else if (tempComma.Type == TokenType.Comma && tempActualParameter == null)
                     //{ //just comma exists (revert comma and end)
                     //   tind = pretind;
                     //   break;
                     //}
                     //else if (tempComma.Type != TokenType.Comma && tempActualParameter != null)
                     //{ //suprising but technechally possible, consumed a non comma, then successfully parsed an ActualParameter (bad parse, revert both).
                     //   tind = pretind;
                     //   break;
                     //}
                  }
               }
               Token closingParenTok = PopToken(source, ref tind); lengthAggr += closingParenTok.TokenLength;
               if (closingParenTok.Type == TokenType.CloseParen)
               {
                  node = new ActualParameterList()
                  {
                     MySource = source,
                     FileLocation = openParenTok.FileLocation,
                     NodeLength = lengthAggr, //unfortunately this doesn't include the lengths of the skippable tokens.
                     OpenParenToken = openParenTok,
                     ActualParameter = firstActualParameter,
                     SecondaryActualParameters = secondaryActualParameters,
                     CloseParenToken = closingParenTok,
                  };
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class AddingOperator : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.AddingOperator; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static AddingOperator? Parse(Source source, ref int index)
      {
         int tind = index;
         AddingOperator? node = null;

         //body

         //if (fullyValid)
         node = new AddingOperator() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   //unneeded
   //public class ApostropheImage : ASTNode
   //{
   //   //content fields (tokens & nodes) go here.
   //   public override ASTNodeType NodeType { get => ASTNodeType.ApostropheImage; }
   //   public override string ToString()
   //   {
   //      return $"{GetType().Name}.ToString(): Not yet implemented";
   //      //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
   //   }
   //   public new static ApostropheImage? Parse(Source source, ref int index)
   //   {
   //      int tind = index;
   //      ApostropheImage? node = null;

   //      //body

   //      //if (fullyValid)
   //      node = new ApostropheImage() { MySource = source, FileLocation = -69, NodeLength = 420 };

   //      index = node == null ? index : tind;
   //      return node;
   //   }
   //}

   public class ArrayType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ArrayType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ArrayType? Parse(Source source, ref int index)
      {
         int tind = index;
         ArrayType? node = null;

         //body

         //if (fullyValid)
         node = new ArrayType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ArrayVariable : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ArrayVariable; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ArrayVariable? Parse(Source source, ref int index)
      {
         int tind = index;
         ArrayVariable? node = null;

         //body

         //if (fullyValid)
         node = new ArrayVariable() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class AssignmentStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.AssignmentStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static AssignmentStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         AssignmentStatement? node = null;

         //body

         //if (fullyValid)
         node = new AssignmentStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class BaseType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.BaseType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static BaseType? Parse(Source source, ref int index)
      {
         int tind = index;
         BaseType? node = null;

         //body

         //if (fullyValid)
         node = new BaseType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class Block : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.Block; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static Block? Parse(Source source, ref int index)
      {
         int tind = index;
         Block? node = null;

         //body

         //if (fullyValid)
         node = new Block() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class BooleanExpression : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.BooleanExpression; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static BooleanExpression? Parse(Source source, ref int index)
      {
         int tind = index;
         BooleanExpression? node = null;

         //body

         //if (fullyValid)
         node = new BooleanExpression() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class BoundIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.BoundIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static BoundIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         BoundIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new BoundIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class BufferVariable : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.BufferVariable; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static BufferVariable? Parse(Source source, ref int index)
      {
         int tind = index;
         BufferVariable? node = null;

         //body

         //if (fullyValid)
         node = new BufferVariable() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class CaseConstant : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.CaseConstant; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static CaseConstant? Parse(Source source, ref int index)
      {
         int tind = index;
         CaseConstant? node = null;

         //body

         //if (fullyValid)
         node = new CaseConstant() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class CaseConstantList : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.CaseConstantList; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static CaseConstantList? Parse(Source source, ref int index)
      {
         int tind = index;
         CaseConstantList? node = null;

         //body

         //if (fullyValid)
         node = new CaseConstantList() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class CaseIndex : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.CaseIndex; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static CaseIndex? Parse(Source source, ref int index)
      {
         int tind = index;
         CaseIndex? node = null;

         //body

         //if (fullyValid)
         node = new CaseIndex() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class CaseListElement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.CaseListElement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static CaseListElement? Parse(Source source, ref int index)
      {
         int tind = index;
         CaseListElement? node = null;

         //body

         //if (fullyValid)
         node = new CaseListElement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class CaseStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.CaseStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static CaseStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         CaseStatement? node = null;

         //body

         //if (fullyValid)
         node = new CaseStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   //unneeded
   //public class CharacterString : ASTNode
   //{
   //   //content fields (tokens & nodes) go here.
   //   public override ASTNodeType NodeType { get => ASTNodeType.CharacterString; }
   //   public override string ToString()
   //   {
   //      return $"{GetType().Name}.ToString(): Not yet implemented";
   //      //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
   //   }
   //   public new static CharacterString? Parse(Source source, ref int index)
   //   {
   //      int tind = index;
   //      CharacterString? node = null;

   //      //body

   //      //if (fullyValid)
   //      node = new CharacterString() { MySource = source, FileLocation = -69, NodeLength = 420 };

   //      index = node == null ? index : tind;
   //      return node;
   //   }
   //}

   public class ComponentType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ComponentType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ComponentType? Parse(Source source, ref int index)
      {
         int tind = index;
         ComponentType? node = null;

         //body

         //if (fullyValid)
         node = new ComponentType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ComponentVariable : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ComponentVariable; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ComponentVariable? Parse(Source source, ref int index)
      {
         int tind = index;
         ComponentVariable? node = null;

         //body

         //if (fullyValid)
         node = new ComponentVariable() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class CompoundStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.CompoundStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static CompoundStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         CompoundStatement? node = null;

         //body

         //if (fullyValid)
         node = new CompoundStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ConditionalStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ConditionalStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ConditionalStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         ConditionalStatement? node = null;

         //body

         //if (fullyValid)
         node = new ConditionalStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ConformantArrayParameterSpecification : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ConformantArrayParameterSpecification; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ConformantArrayParameterSpecification? Parse(Source source, ref int index)
      {
         int tind = index;
         ConformantArrayParameterSpecification? node = null;

         //body

         //if (fullyValid)
         node = new ConformantArrayParameterSpecification() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ConformantArraySchema : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ConformantArraySchema; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ConformantArraySchema? Parse(Source source, ref int index)
      {
         int tind = index;
         ConformantArraySchema? node = null;

         //body

         //if (fullyValid)
         node = new ConformantArraySchema() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class Constant : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.Constant; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static Constant? Parse(Source source, ref int index)
      {
         int tind = index;
         Constant? node = null;

         //body

         //if (fullyValid)
         node = new Constant() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ConstantDefinition : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ConstantDefinition; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ConstantDefinition? Parse(Source source, ref int index)
      {
         int tind = index;
         ConstantDefinition? node = null;

         //body

         //if (fullyValid)
         node = new ConstantDefinition() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ConstantDefinitionPart : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ConstantDefinitionPart; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ConstantDefinitionPart? Parse(Source source, ref int index)
      {
         int tind = index;
         ConstantDefinitionPart? node = null;

         //body

         //if (fullyValid)
         node = new ConstantDefinitionPart() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ConstantIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ConstantIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ConstantIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         ConstantIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new ConstantIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ControlVariable : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ControlVariable; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ControlVariable? Parse(Source source, ref int index)
      {
         int tind = index;
         ControlVariable? node = null;

         //body

         //if (fullyValid)
         node = new ControlVariable() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   //unneeded
   //public class Digit : ASTNode
   //{
   //   //content fields (tokens & nodes) go here.
   //   public override ASTNodeType NodeType { get => ASTNodeType.Digit; }
   //   public override string ToString()
   //   {
   //      return $"{GetType().Name}.ToString(): Not yet implemented";
   //      //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
   //   }
   //   public new static Digit? Parse(Source source, ref int index)
   //   {
   //      int tind = index;
   //      Digit? node = null;

   //      //body

   //      //if (fullyValid)
   //      node = new Digit() { MySource = source, FileLocation = -69, NodeLength = 420 };

   //      node?.AssertCorrectStructure();
   //      return node;
   //   }
   //}

   public class DigitSequence : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.DigitSequence; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static DigitSequence? Parse(Source source, ref int index)
      {
         int tind = index;
         DigitSequence? node = null;

         //body

         //if (fullyValid)
         node = new DigitSequence() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class Directive : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.Directive; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static Directive? Parse(Source source, ref int index)
      {
         int tind = index;
         Directive? node = null;

         //body

         //if (fullyValid)
         node = new Directive() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class DomainType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.DomainType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static DomainType? Parse(Source source, ref int index)
      {
         int tind = index;
         DomainType? node = null;

         //body

         //if (fullyValid)
         node = new DomainType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ElsePart : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ElsePart; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ElsePart? Parse(Source source, ref int index)
      {
         int tind = index;
         ElsePart? node = null;

         //body

         //if (fullyValid)
         node = new ElsePart() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class EmptyStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.EmptyStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static EmptyStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         EmptyStatement? node = null;

         //body

         //if (fullyValid)
         node = new EmptyStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class EntireVariable : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.EntireVariable; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static EntireVariable? Parse(Source source, ref int index)
      {
         int tind = index;
         EntireVariable? node = null;

         //body

         //if (fullyValid)
         node = new EntireVariable() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class EnumeratedType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.EnumeratedType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static EnumeratedType? Parse(Source source, ref int index)
      {
         int tind = index;
         EnumeratedType? node = null;

         //body

         //if (fullyValid)
         node = new EnumeratedType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class Expression : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.Expression; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static Expression? Parse(Source source, ref int index)
      {
         int tind = index;
         Expression? node = null;

         //body

         //if (fullyValid)
         node = new Expression() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class Factor : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.Factor; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static Factor? Parse(Source source, ref int index)
      {
         int tind = index;
         Factor? node = null;

         //body

         //if (fullyValid)
         node = new Factor() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FieldDesignator : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FieldDesignator; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FieldDesignator? Parse(Source source, ref int index)
      {
         int tind = index;
         FieldDesignator? node = null;

         //body

         //if (fullyValid)
         node = new FieldDesignator() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FieldDesignatorIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FieldDesignatorIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FieldDesignatorIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         FieldDesignatorIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new FieldDesignatorIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FieldIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FieldIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FieldIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         FieldIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new FieldIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FieldList : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FieldList; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FieldList? Parse(Source source, ref int index)
      {
         int tind = index;
         FieldList? node = null;

         //body

         //if (fullyValid)
         node = new FieldList() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FieldSpecifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FieldSpecifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FieldSpecifier? Parse(Source source, ref int index)
      {
         int tind = index;
         FieldSpecifier? node = null;

         //body

         //if (fullyValid)
         node = new FieldSpecifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FileType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FileType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FileType? Parse(Source source, ref int index)
      {
         int tind = index;
         FileType? node = null;

         //body

         //if (fullyValid)
         node = new FileType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FileVariable : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FileVariable; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FileVariable? Parse(Source source, ref int index)
      {
         int tind = index;
         FileVariable? node = null;

         //body

         //if (fullyValid)
         node = new FileVariable() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FinalValue : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FinalValue; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FinalValue? Parse(Source source, ref int index)
      {
         int tind = index;
         FinalValue? node = null;

         //body

         //if (fullyValid)
         node = new FinalValue() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FixedPart : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FixedPart; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FixedPart? Parse(Source source, ref int index)
      {
         int tind = index;
         FixedPart? node = null;

         //body

         //if (fullyValid)
         node = new FixedPart() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ForStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ForStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ForStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         ForStatement? node = null;

         //body

         //if (fullyValid)
         node = new ForStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FormalParameterList : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FormalParameterList; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FormalParameterList? Parse(Source source, ref int index)
      {
         int tind = index;
         FormalParameterList? node = null;

         //body

         //if (fullyValid)
         node = new FormalParameterList() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FormalParameterSection : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FormalParameterSection; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FormalParameterSection? Parse(Source source, ref int index)
      {
         int tind = index;
         FormalParameterSection? node = null;

         //body

         //if (fullyValid)
         node = new FormalParameterSection() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FractionalPart : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FractionalPart; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FractionalPart? Parse(Source source, ref int index)
      {
         int tind = index;
         FractionalPart? node = null;

         //body

         //if (fullyValid)
         node = new FractionalPart() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FunctionBlock : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FunctionBlock; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FunctionBlock? Parse(Source source, ref int index)
      {
         int tind = index;
         FunctionBlock? node = null;

         //body

         //if (fullyValid)
         node = new FunctionBlock() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FunctionDeclaration : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FunctionDeclaration; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FunctionDeclaration? Parse(Source source, ref int index)
      {
         int tind = index;
         FunctionDeclaration? node = null;

         //body

         //if (fullyValid)
         node = new FunctionDeclaration() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FunctionDesignator : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FunctionDesignator; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FunctionDesignator? Parse(Source source, ref int index)
      {
         int tind = index;
         FunctionDesignator? node = null;

         //body

         //if (fullyValid)
         node = new FunctionDesignator() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FunctionHeading : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FunctionHeading; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FunctionHeading? Parse(Source source, ref int index)
      {
         int tind = index;
         FunctionHeading? node = null;

         //body

         //if (fullyValid)
         node = new FunctionHeading() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FunctionIdentification : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FunctionIdentification; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FunctionIdentification? Parse(Source source, ref int index)
      {
         int tind = index;
         FunctionIdentification? node = null;

         //body

         //if (fullyValid)
         node = new FunctionIdentification() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FunctionIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FunctionIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FunctionIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         FunctionIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new FunctionIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class FunctionalParameterSpecification : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.FunctionalParameterSpecification; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static FunctionalParameterSpecification? Parse(Source source, ref int index)
      {
         int tind = index;
         FunctionalParameterSpecification? node = null;

         //body

         //if (fullyValid)
         node = new FunctionalParameterSpecification() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class GotoStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.GotoStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static GotoStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         GotoStatement? node = null;

         //body

         //if (fullyValid)
         node = new GotoStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class IdentifiedVariable : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.IdentifiedVariable; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static IdentifiedVariable? Parse(Source source, ref int index)
      {
         int tind = index;
         IdentifiedVariable? node = null;

         //body

         //if (fullyValid)
         node = new IdentifiedVariable() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class Identifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.Identifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static Identifier? Parse(Source source, ref int index)
      {
         int tind = index;
         Identifier? node = null;

         //body

         //if (fullyValid)
         node = new Identifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class IdentifierList : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.IdentifierList; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static IdentifierList? Parse(Source source, ref int index)
      {
         int tind = index;
         IdentifierList? node = null;

         //body

         //if (fullyValid)
         node = new IdentifierList() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class IfStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.IfStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static IfStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         IfStatement? node = null;

         //body

         //if (fullyValid)
         node = new IfStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class IndexExpression : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.IndexExpression; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static IndexExpression? Parse(Source source, ref int index)
      {
         int tind = index;
         IndexExpression? node = null;

         //body

         //if (fullyValid)
         node = new IndexExpression() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class IndexType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.IndexType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static IndexType? Parse(Source source, ref int index)
      {
         int tind = index;
         IndexType? node = null;

         //body

         //if (fullyValid)
         node = new IndexType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class IndexTypeSpecification : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.IndexTypeSpecification; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static IndexTypeSpecification? Parse(Source source, ref int index)
      {
         int tind = index;
         IndexTypeSpecification? node = null;

         //body

         //if (fullyValid)
         node = new IndexTypeSpecification() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class IndexedVariable : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.IndexedVariable; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static IndexedVariable? Parse(Source source, ref int index)
      {
         int tind = index;
         IndexedVariable? node = null;

         //body

         //if (fullyValid)
         node = new IndexedVariable() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class InitialValue : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.InitialValue; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static InitialValue? Parse(Source source, ref int index)
      {
         int tind = index;
         InitialValue? node = null;

         //body

         //if (fullyValid)
         node = new InitialValue() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class Label : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.Label; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static Label? Parse(Source source, ref int index)
      {
         int tind = index;
         Label? node = null;

         //body

         //if (fullyValid)
         node = new Label() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class LabelDeclarationPart : ASTNode
   {
      //items to complete a derived ASTNode:
      //Add all relevant ASTNode/Lexer Token data members
      //Write the ToString() function
      //In parser: need to assign data members & "FileLocation" & "MySource" & "NodeLength
      public Token? LabelToken { get; private set; } = null;
      public Label? Label { get; private set; } = null;
      public IReadOnlyList<(Token commaToken, Label label)>? SecondaryLabels { get; private set; } = null;
      public Token? SemicolonToken { get; private set; } = null;
      public override ASTNodeType NodeType { get => ASTNodeType.LabelDeclarationPart; }

      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         if (LabelToken != null)
         { //no data members are null
            if (prettyPrint)
               return $"{new string('\t', indentLevel)}({NodeType}{Environment.NewLine}" +
                      $"{new string('\t', indentLevel + 1)}{LabelToken!}{Environment.NewLine}" +
                      $"{new string('\t', indentLevel + 1)}{Label!}{Environment.NewLine}" +
                      $"{SecondaryLabels!.Select(x =>
                      $"{new string('\t', indentLevel + 1)}{x.commaToken}{Environment.NewLine}" +
                      $"{new string('\t', indentLevel + 1)}{x.label}{Environment.NewLine}").Aggregate((a, b) => a + b)}" +
                      $"{new string('\t', indentLevel + 1)}{SemicolonToken!}{Environment.NewLine}" +
                      $"{new string('\t', indentLevel)})";
            else
               return $"({NodeType} {LabelToken!} {Label!} {SecondaryLabels!.Select(x => $"{x.commaToken} {x.label}").Aggregate((a, b) => a + b)} {SemicolonToken!})";
         }
         else //all data members are null
            return $"({NodeType})";
      }
      protected override void AssertCorrectStructure()
      {
         int totalNonNull = 0;
         totalNonNull += LabelToken != null ? 1 : 0;
         totalNonNull += Label != null ? 1 : 0;
         totalNonNull += SecondaryLabels != null ? 1 : 0;
         totalNonNull += SemicolonToken != null ? 1 : 0;
         //should be exactly one non-null
         if (totalNonNull != 0 && totalNonNull != 4)
         {
            throw new InvalidOperationException($"Compiler parse error in {GetType().Name} (compiler bug): invalid object state.");
         }
      }
      //This particular parse function contains most of the patterns used. Reference it as an example
      public new static LabelDeclarationPart? Parse(Source source, ref int index)
      {
         //first of all, here is how the spec defines this item (see 6.2.1):
         //label-declaration-part = [ 'label' label { ',' label } ';' ] .
         //note that the contents of label-declaration-part are all encapsulated in [] (0 or 1 of), which means
         //in this case, if parsing fails, you should still successfully create an effectively empty label-declaration-part.
         //my probably correct interpretation of the definition says that a label-declaration-part is comprised of:
         //0 or 1: (label-word-symbol "label" 0 or more: (comma-special-symbol "label") semicolon-special-symbol)
         //where "label" refers to an actual label value 0-9999 (see section 6.1.6)
         //sometimes there are *functionally* identical interpretation of the spec in terms of executing the grammar.
         //I try to stick to the spec for clairity.

         //prelude (most parse functions should have this)
         int tind = index, lengthAggr = 0;
         LabelDeclarationPart? node = null; //matches return type/containing class type
         //grab all contiguous simple tokens
         Token firstLabelWordSymbolTok = PopToken(source, ref tind); lengthAggr += firstLabelWordSymbolTok.TokenLength;
         //if the next item is an ASTNode, then verify your tokens collected so far
         if (firstLabelWordSymbolTok.Type == TokenType.Label)
         {
            //one by one, parse and verify your required ASTNodes, then descend one nested if deeper
            Label? firstLabelValue = Label.Parse(source, ref tind); lengthAggr += firstLabelValue?.NodeLength ?? 0;
            if (firstLabelValue != null)
            {
               List<(Token comma, Label labelValue)> secondaryLabels = new();
               {
                  while (true) //we require both a comma and and a label
                  {
                     int pretind = tind;
                     Token tempComma = PopToken(source, ref tind);
                     Label? tempLabelValue = Label.Parse(source, ref tind);
                     if (tempComma.Type == TokenType.Comma && tempLabelValue != null)
                     { //both exist (add to collection and continue)
                        lengthAggr += tempComma.TokenLength;
                        lengthAggr += tempLabelValue.NodeLength;
                        secondaryLabels.Add((tempComma, tempLabelValue));
                     }
                     else //in any other situation
                     {
                        tind = pretind;
                        break;
                     }
                  }
               }
               //secondaryLabels are already verified.
               Token closingSemicolon = PopToken(source, ref tind); lengthAggr += closingSemicolon.TokenLength;
               if (closingSemicolon.Type == TokenType.Semicolon)
               {
                  //we've met all the requirements for this AST node as per the 6.2.1 definition of label-declaration-part, so create the node
                  node = new LabelDeclarationPart()
                  { //at this point in execution, the following must be true
                     MySource = source,
                     FileLocation = firstLabelWordSymbolTok.FileLocation,
                     //NodeLength = lengthAggr, //the sum of all the tokens & nodes that comprise the body of this
                     LabelToken = firstLabelWordSymbolTok, //Type == TokenType.Label
                     Label = firstLabelValue, //!= null (therefore valid)
                     SecondaryLabels = secondaryLabels, //may be empty, but not null (valid)
                     SemicolonToken = closingSemicolon //Type == TokenType.Semicolon
                  }; //i.e., none are null
               }
            }
         }
         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         if (node == null) //if we failed to create the node, we should still successfully create an effectively empty label-declaration-part.
         { //keep in mind that the above line will revert changes to the index; an effectively empty lable-declaration-part retroactively consumed no tokens.
            //all the contents will be null by default, since this AST node comprises nothing.
            node = new LabelDeclarationPart()
            {
               MySource = source,
               FileLocation = firstLabelWordSymbolTok.FileLocation, //even if this token isn't a "label", this node's location still lies here.
               NodeLength = 0,
            };
         }
         return node;
      }
   }

   //unneeded
   //public class Letter : ASTNode
   //{
   //   //content fields (tokens & nodes) go here.
   //   public override ASTNodeType NodeType { get => ASTNodeType.Letter; }
   //   public override string ToString()
   //   {
   //      return $"{GetType().Name}.ToString(): Not yet implemented";
   //      //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
   //   }
   //   public new static Letter? Parse(Source source, ref int index)
   //   {
   //      int tind = index;
   //      Letter? node = null;

   //      //body

   //      //if (fullyValid)
   //      node = new Letter() { MySource = source, FileLocation = -69, NodeLength = 420 };

   //      node?.AssertCorrectStructure();
   //      return node;
   //   }
   //}

   public class MemberDesignator : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.MemberDesignator; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static MemberDesignator? Parse(Source source, ref int index)
      {
         int tind = index;
         MemberDesignator? node = null;

         //body

         //if (fullyValid)
         node = new MemberDesignator() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class MultiplyingOperator : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.MultiplyingOperator; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static MultiplyingOperator? Parse(Source source, ref int index)
      {
         int tind = index;
         MultiplyingOperator? node = null;

         //body

         //if (fullyValid)
         node = new MultiplyingOperator() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class NewOrdinalType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.NewOrdinalType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static NewOrdinalType? Parse(Source source, ref int index)
      {
         int tind = index;
         NewOrdinalType? node = null;

         //body

         //if (fullyValid)
         node = new NewOrdinalType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class NewPointerType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.NewPointerType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static NewPointerType? Parse(Source source, ref int index)
      {
         int tind = index;
         NewPointerType? node = null;

         //body

         //if (fullyValid)
         node = new NewPointerType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class NewStructuredType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.NewStructuredType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static NewStructuredType? Parse(Source source, ref int index)
      {
         int tind = index;
         NewStructuredType? node = null;

         //body

         //if (fullyValid)
         node = new NewStructuredType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class NewType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.NewType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static NewType? Parse(Source source, ref int index)
      {
         int tind = index;
         NewType? node = null;

         //body

         //if (fullyValid)
         node = new NewType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class OrdinalType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.OrdinalType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static OrdinalType? Parse(Source source, ref int index)
      {
         int tind = index;
         OrdinalType? node = null;

         //body

         //if (fullyValid)
         node = new OrdinalType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class OrdinalTypeIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.OrdinalTypeIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static OrdinalTypeIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         OrdinalTypeIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new OrdinalTypeIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class PackedConformantArraySchema : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.PackedConformantArraySchema; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static PackedConformantArraySchema? Parse(Source source, ref int index)
      {
         int tind = index;
         PackedConformantArraySchema? node = null;

         //body

         //if (fullyValid)
         node = new PackedConformantArraySchema() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class PointerType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.PointerType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static PointerType? Parse(Source source, ref int index)
      {
         int tind = index;
         PointerType? node = null;

         //body

         //if (fullyValid)
         node = new PointerType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class PointerTypeIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.PointerTypeIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static PointerTypeIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         PointerTypeIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new PointerTypeIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class PointerVariable : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.PointerVariable; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static PointerVariable? Parse(Source source, ref int index)
      {
         int tind = index;
         PointerVariable? node = null;

         //body

         //if (fullyValid)
         node = new PointerVariable() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ProceduralParameterSpecification : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ProceduralParameterSpecification; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ProceduralParameterSpecification? Parse(Source source, ref int index)
      {
         int tind = index;
         ProceduralParameterSpecification? node = null;

         //body

         //if (fullyValid)
         node = new ProceduralParameterSpecification() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ProcedureAndFunctionDeclarationPart : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ProcedureAndFunctionDeclarationPart; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ProcedureAndFunctionDeclarationPart? Parse(Source source, ref int index)
      {
         int tind = index;
         ProcedureAndFunctionDeclarationPart? node = null;

         //body

         //if (fullyValid)
         node = new ProcedureAndFunctionDeclarationPart() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ProcedureBlock : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ProcedureBlock; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ProcedureBlock? Parse(Source source, ref int index)
      {
         int tind = index;
         ProcedureBlock? node = null;

         //body

         //if (fullyValid)
         node = new ProcedureBlock() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ProcedureDeclaration : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ProcedureDeclaration; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ProcedureDeclaration? Parse(Source source, ref int index)
      {
         int tind = index;
         ProcedureDeclaration? node = null;

         //body

         //if (fullyValid)
         node = new ProcedureDeclaration() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ProcedureHeading : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ProcedureHeading; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ProcedureHeading? Parse(Source source, ref int index)
      {
         int tind = index;
         ProcedureHeading? node = null;

         //body

         //if (fullyValid)
         node = new ProcedureHeading() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ProcedureIdentification : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ProcedureIdentification; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ProcedureIdentification? Parse(Source source, ref int index)
      {
         int tind = index;
         ProcedureIdentification? node = null;

         //body

         //if (fullyValid)
         node = new ProcedureIdentification() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ProcedureIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ProcedureIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ProcedureIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         ProcedureIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new ProcedureIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ProcedureStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ProcedureStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ProcedureStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         ProcedureStatement? node = null;

         //body

         //if (fullyValid)
         node = new ProcedureStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class Program : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.Program; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static Program? Parse(Source source, ref int index)
      {
         int tind = index;
         Program? node = null;

         //body

         //if (fullyValid)
         node = new Program() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ProgramBlock : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ProgramBlock; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ProgramBlock? Parse(Source source, ref int index)
      {
         int tind = index;
         ProgramBlock? node = null;

         //body

         //if (fullyValid)
         node = new ProgramBlock() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ProgramHeading : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ProgramHeading; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ProgramHeading? Parse(Source source, ref int index)
      {
         int tind = index;
         ProgramHeading? node = null;

         //body

         //if (fullyValid)
         node = new ProgramHeading() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ProgramParameterList : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ProgramParameterList; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ProgramParameterList? Parse(Source source, ref int index)
      {
         int tind = index;
         ProgramParameterList? node = null;

         //body

         //if (fullyValid)
         node = new ProgramParameterList() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ReadParameterList : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ReadParameterList; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ReadParameterList? Parse(Source source, ref int index)
      {
         int tind = index;
         ReadParameterList? node = null;

         //body

         //if (fullyValid)
         node = new ReadParameterList() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ReadlnParameterList : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ReadlnParameterList; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ReadlnParameterList? Parse(Source source, ref int index)
      {
         int tind = index;
         ReadlnParameterList? node = null;

         //body

         //if (fullyValid)
         node = new ReadlnParameterList() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class RealTypeIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.RealTypeIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static RealTypeIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         RealTypeIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new RealTypeIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class RecordSection : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.RecordSection; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static RecordSection? Parse(Source source, ref int index)
      {
         int tind = index;
         RecordSection? node = null;

         //body

         //if (fullyValid)
         node = new RecordSection() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class RecordType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.RecordType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static RecordType? Parse(Source source, ref int index)
      {
         int tind = index;
         RecordType? node = null;

         //body

         //if (fullyValid)
         node = new RecordType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class RecordVariable : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.RecordVariable; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static RecordVariable? Parse(Source source, ref int index)
      {
         int tind = index;
         RecordVariable? node = null;

         //body

         //if (fullyValid)
         node = new RecordVariable() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class RecordVariableList : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.RecordVariableList; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static RecordVariableList? Parse(Source source, ref int index)
      {
         int tind = index;
         RecordVariableList? node = null;

         //body

         //if (fullyValid)
         node = new RecordVariableList() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class RelationalOperator : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.RelationalOperator; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static RelationalOperator? Parse(Source source, ref int index)
      {
         int tind = index;
         RelationalOperator? node = null;

         //body

         //if (fullyValid)
         node = new RelationalOperator() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class RepeatStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.RepeatStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static RepeatStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         RepeatStatement? node = null;

         //body

         //if (fullyValid)
         node = new RepeatStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class RepetitiveStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.RepetitiveStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static RepetitiveStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         RepetitiveStatement? node = null;

         //body

         //if (fullyValid)
         node = new RepetitiveStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ResultType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ResultType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ResultType? Parse(Source source, ref int index)
      {
         int tind = index;
         ResultType? node = null;

         //body

         //if (fullyValid)
         node = new ResultType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ScaleFactor : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ScaleFactor; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ScaleFactor? Parse(Source source, ref int index)
      {
         int tind = index;
         ScaleFactor? node = null;

         //body

         //if (fullyValid)
         node = new ScaleFactor() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class SetConstructor : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.SetConstructor; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static SetConstructor? Parse(Source source, ref int index)
      {
         int tind = index;
         SetConstructor? node = null;

         //body

         //if (fullyValid)
         node = new SetConstructor() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class SetType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.SetType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static SetType? Parse(Source source, ref int index)
      {
         int tind = index;
         SetType? node = null;

         //body

         //if (fullyValid)
         node = new SetType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class Sign : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.Sign; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static Sign? Parse(Source source, ref int index)
      {
         int tind = index;
         Sign? node = null;

         //body

         //if (fullyValid)
         node = new Sign() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class SignedInteger : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.SignedInteger; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static SignedInteger? Parse(Source source, ref int index)
      {
         int tind = index;
         SignedInteger? node = null;

         //body

         //if (fullyValid)
         node = new SignedInteger() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class SignedNumber : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.SignedNumber; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static SignedNumber? Parse(Source source, ref int index)
      {
         int tind = index;
         SignedNumber? node = null;

         //body

         //if (fullyValid)
         node = new SignedNumber() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class SignedReal : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.SignedReal; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static SignedReal? Parse(Source source, ref int index)
      {
         int tind = index;
         SignedReal? node = null;

         //body

         //if (fullyValid)
         node = new SignedReal() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class SimpleExpression : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.SimpleExpression; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static SimpleExpression? Parse(Source source, ref int index)
      {
         int tind = index;
         SimpleExpression? node = null;

         //body

         //if (fullyValid)
         node = new SimpleExpression() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class SimpleStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.SimpleStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static SimpleStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         SimpleStatement? node = null;

         //body

         //if (fullyValid)
         node = new SimpleStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class SimpleType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.SimpleType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static SimpleType? Parse(Source source, ref int index)
      {
         int tind = index;
         SimpleType? node = null;

         //body

         //if (fullyValid)
         node = new SimpleType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class SimpleTypeIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.SimpleTypeIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static SimpleTypeIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         SimpleTypeIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new SimpleTypeIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   //unneeded
   //public class SpecialSymbol : ASTNode
   //{
   //   //content fields (tokens & nodes) go here.
   //   public override ASTNodeType NodeType { get => ASTNodeType.SpecialSymbol; }
   //   public override string ToString()
   //   {
   //      return $"{GetType().Name}.ToString(): Not yet implemented";
   //      //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
   //   }
   //   public new static SpecialSymbol? Parse(Source source, ref int index)
   //   {
   //      int tind = index;
   //      SpecialSymbol? node = null;

   //      //body

   //      //if (fullyValid)
   //      node = new SpecialSymbol() { MySource = source, FileLocation = -69, NodeLength = 420 };

   //      node?.AssertCorrectStructure();
   //      return node;
   //   }
   //}

   public class Statement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.Statement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static Statement? Parse(Source source, ref int index)
      {
         int tind = index;
         Statement? node = null;

         //body

         //if (fullyValid)
         node = new Statement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class StatementPart : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.StatementPart; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static StatementPart? Parse(Source source, ref int index)
      {
         int tind = index;
         StatementPart? node = null;

         //body

         //if (fullyValid)
         node = new StatementPart() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class StatementSequence : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.StatementSequence; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static StatementSequence? Parse(Source source, ref int index)
      {
         int tind = index;
         StatementSequence? node = null;

         //body

         //if (fullyValid)
         node = new StatementSequence() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   //unneeded
   //public class StringCharacter : ASTNode
   //{
   //   //content fields (tokens & nodes) go here.
   //   public override ASTNodeType NodeType { get => ASTNodeType.StringCharacter; }
   //   public override string ToString()
   //   {
   //      return $"{GetType().Name}.ToString(): Not yet implemented";
   //      //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
   //   }
   //   public new static StringCharacter? Parse(Source source, ref int index)
   //   {
   //      int tind = index;
   //      StringCharacter? node = null;

   //      //body

   //      //if (fullyValid)
   //      node = new StringCharacter() { MySource = source, FileLocation = -69, NodeLength = 420 };

   //      node?.AssertCorrectStructure();
   //      return node;
   //   }
   //}

   //unneeded
   //public class StringElement : ASTNode
   //{
   //   //content fields (tokens & nodes) go here.
   //   public override ASTNodeType NodeType { get => ASTNodeType.StringElement; }
   //   public override string ToString()
   //   {
   //      return $"{GetType().Name}.ToString(): Not yet implemented";
   //      //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
   //   }
   //   public new static StringElement? Parse(Source source, ref int index)
   //   {
   //      int tind = index;
   //      StringElement? node = null;

   //      //body

   //      //if (fullyValid)
   //      node = new StringElement() { MySource = source, FileLocation = -69, NodeLength = 420 };

   //      node?.AssertCorrectStructure();
   //      return node;
   //   }
   //}

   public class StructuredStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.StructuredStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static StructuredStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         StructuredStatement? node = null;

         //body

         //if (fullyValid)
         node = new StructuredStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class StructuredType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.StructuredType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static StructuredType? Parse(Source source, ref int index)
      {
         int tind = index;
         StructuredType? node = null;

         //body

         //if (fullyValid)
         node = new StructuredType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class StructuredTypeIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.StructuredTypeIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static StructuredTypeIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         StructuredTypeIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new StructuredTypeIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class SubrangeType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.SubrangeType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static SubrangeType? Parse(Source source, ref int index)
      {
         int tind = index;
         SubrangeType? node = null;

         //body

         //if (fullyValid)
         node = new SubrangeType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class TagField : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.TagField; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static TagField? Parse(Source source, ref int index)
      {
         int tind = index;
         TagField? node = null;

         //body

         //if (fullyValid)
         node = new TagField() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class TagType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.TagType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static TagType? Parse(Source source, ref int index)
      {
         int tind = index;
         TagType? node = null;

         //body

         //if (fullyValid)
         node = new TagType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class Term : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.Term; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static Term? Parse(Source source, ref int index)
      {
         int tind = index;
         Term? node = null;

         //body

         //if (fullyValid)
         node = new Term() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class TypeDefinition : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.TypeDefinition; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static TypeDefinition? Parse(Source source, ref int index)
      {
         int tind = index;
         TypeDefinition? node = null;

         //body

         //if (fullyValid)
         node = new TypeDefinition() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class TypeDefinitionPart : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.TypeDefinitionPart; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static TypeDefinitionPart? Parse(Source source, ref int index)
      {
         int tind = index;
         TypeDefinitionPart? node = null;

         //body

         //if (fullyValid)
         node = new TypeDefinitionPart() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class TypeDenoter : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.TypeDenoter; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static TypeDenoter? Parse(Source source, ref int index)
      {
         int tind = index;
         TypeDenoter? node = null;

         //body

         //if (fullyValid)
         node = new TypeDenoter() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class TypeIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.TypeIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static TypeIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         TypeIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new TypeIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class UnpackedConformantArraySchema : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.UnpackedConformantArraySchema; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static UnpackedConformantArraySchema? Parse(Source source, ref int index)
      {
         int tind = index;
         UnpackedConformantArraySchema? node = null;

         //body

         //if (fullyValid)
         node = new UnpackedConformantArraySchema() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class UnpackedStructuredType : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.UnpackedStructuredType; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static UnpackedStructuredType? Parse(Source source, ref int index)
      {
         int tind = index;
         UnpackedStructuredType? node = null;

         //body

         //if (fullyValid)
         node = new UnpackedStructuredType() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class UnsignedConstant : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.UnsignedConstant; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static UnsignedConstant? Parse(Source source, ref int index)
      {
         int tind = index;
         UnsignedConstant? node = null;

         //body

         //if (fullyValid)
         node = new UnsignedConstant() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class UnsignedInteger : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.UnsignedInteger; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static UnsignedInteger? Parse(Source source, ref int index)
      {
         int tind = index;
         UnsignedInteger? node = null;

         //body

         //if (fullyValid)
         node = new UnsignedInteger() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class UnsignedNumber : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.UnsignedNumber; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static UnsignedNumber? Parse(Source source, ref int index)
      {
         int tind = index;
         UnsignedNumber? node = null;

         //body

         //if (fullyValid)
         node = new UnsignedNumber() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class UnsignedReal : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.UnsignedReal; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static UnsignedReal? Parse(Source source, ref int index)
      {
         int tind = index;
         UnsignedReal? node = null;

         //body

         //if (fullyValid)
         node = new UnsignedReal() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ValueConformantArraySpecification : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ValueConformantArraySpecification; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ValueConformantArraySpecification? Parse(Source source, ref int index)
      {
         int tind = index;
         ValueConformantArraySpecification? node = null;

         //body

         //if (fullyValid)
         node = new ValueConformantArraySpecification() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ValueParameterSpecification : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.ValueParameterSpecification; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static ValueParameterSpecification? Parse(Source source, ref int index)
      {
         int tind = index;
         ValueParameterSpecification? node = null;

         //body

         //if (fullyValid)
         node = new ValueParameterSpecification() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class VariableAccess : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.VariableAccess; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static VariableAccess? Parse(Source source, ref int index)
      {
         int tind = index;
         VariableAccess? node = null;

         //body

         //if (fullyValid)
         node = new VariableAccess() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class VariableConformantArraySpecification : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.VariableConformantArraySpecification; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static VariableConformantArraySpecification? Parse(Source source, ref int index)
      {
         int tind = index;
         VariableConformantArraySpecification? node = null;

         //body

         //if (fullyValid)
         node = new VariableConformantArraySpecification() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class VariableDeclaration : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.VariableDeclaration; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static VariableDeclaration? Parse(Source source, ref int index)
      {
         int tind = index;
         VariableDeclaration? node = null;

         //body

         //if (fullyValid)
         node = new VariableDeclaration() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class VariableDeclarationPart : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.VariableDeclarationPart; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static VariableDeclarationPart? Parse(Source source, ref int index)
      {
         int tind = index;
         VariableDeclarationPart? node = null;

         //body

         //if (fullyValid)
         node = new VariableDeclarationPart() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class VariableIdentifier : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.VariableIdentifier; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static VariableIdentifier? Parse(Source source, ref int index)
      {
         int tind = index;
         VariableIdentifier? node = null;

         //body

         //if (fullyValid)
         node = new VariableIdentifier() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class VariableParameterSpecification : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.VariableParameterSpecification; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static VariableParameterSpecification? Parse(Source source, ref int index)
      {
         int tind = index;
         VariableParameterSpecification? node = null;

         //body

         //if (fullyValid)
         node = new VariableParameterSpecification() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class Variant : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.Variant; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static Variant? Parse(Source source, ref int index)
      {
         int tind = index;
         Variant? node = null;

         //body

         //if (fullyValid)
         node = new Variant() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class VariantPart : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.VariantPart; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static VariantPart? Parse(Source source, ref int index)
      {
         int tind = index;
         VariantPart? node = null;

         //body

         //if (fullyValid)
         node = new VariantPart() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class VariantSelector : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.VariantSelector; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static VariantSelector? Parse(Source source, ref int index)
      {
         int tind = index;
         VariantSelector? node = null;

         //body

         //if (fullyValid)
         node = new VariantSelector() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class WhileStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.WhileStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static WhileStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         WhileStatement? node = null;

         //body

         //if (fullyValid)
         node = new WhileStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class WithStatement : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.WithStatement; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static WithStatement? Parse(Source source, ref int index)
      {
         int tind = index;
         WithStatement? node = null;

         //body

         //if (fullyValid)
         node = new WithStatement() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   //unneeded
   //public class WordSymbol : ASTNode
   //{
   //   //content fields (tokens & nodes) go here.
   //   public override ASTNodeType NodeType { get => ASTNodeType.WordSymbol; }
   //   public override string ToString()
   //   {
   //      return $"{GetType().Name}.ToString(): Not yet implemented";
   //      //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
   //   }
   //   public new static WordSymbol? Parse(Source source, ref int index)
   //   {
   //      int tind = index;
   //      WordSymbol? node = null;

   //      //body

   //      //if (fullyValid)
   //      node = new WordSymbol() { MySource = source, FileLocation = -69, NodeLength = 420 };

   //      node?.AssertCorrectStructure();
   //      return node;
   //   }
   //}

   public class WriteParameter : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.WriteParameter; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static WriteParameter? Parse(Source source, ref int index)
      {
         int tind = index;
         WriteParameter? node = null;

         //body

         //if (fullyValid)
         node = new WriteParameter() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class WriteParameterList : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.WriteParameterList; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static WriteParameterList? Parse(Source source, ref int index)
      {
         int tind = index;
         WriteParameterList? node = null;

         //body

         //if (fullyValid)
         node = new WriteParameterList() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

   public class WritelnParameterList : ASTNode
   {
      //content fields (tokens & nodes) go here.
      public override ASTNodeType NodeType { get => ASTNodeType.WritelnParameterList; }
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         //return $"({NodeType} {space} {separated} {content} {nodes/tokens})";
         if (prettyPrint)
            return $"{GetType().Name}.ToString(): Not yet implemented";
         else
            return $"{GetType().Name}.ToString(): Not yet implemented";
      }
      protected override void AssertCorrectStructure()
      {

      }
      public new static WritelnParameterList? Parse(Source source, ref int index)
      {
         int tind = index;
         WritelnParameterList? node = null;

         //body

         //if (fullyValid)
         node = new WritelnParameterList() { MySource = source, FileLocation = -69, NodeLength = 420 };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }







   #region old?
   //need to design parser classes to work well for all sorts of grammar defs. Here
   //are some examples of the unique ones, hopefully these cover all the bases.

   //category-esq:
   //actual-parameter = expression | variable-access | procedure-identifier | function-identifier .

   //ord terminals:
   //digit = '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' .

   //1 or more:
   //directive = letter { letter | digit } .

   //big mix of terminals and non-terminals:
   //for-statement = 'for' control-variable ':=' initial-value ( 'to' | 'downto' ) final-value 'do' statement .

   //entirely optional:
   //readln-parameter-list = [ '(' ( file-variable | variable-access ) { ',' variable-access } ')' ] .
   //label-declaration-part = [ 'label' label { ',' label } ';' ] .


   //hold off on the concept below until experience indicates if it's necessary
   //to deambiguize terminal and non-terminals, let the classname suffix T mean "terminal", and NT mean "nonterminal"

   //hold off on the concept below until experience indicates if it's necessary
   //additionally, to deambiguize static concepts with values (e.g., "label" keyword vs "label" value), let the classname prefix S mean static, and V mean value
   //
   //  iirc, a "grammatical class" type node according to my patterns is any non-terminal symbol, even if it resolves only to one (non)-terminal child symbol.
   //

   //maybe this? https://stackoverflow.com/questions/857705/get-all-derived-types-of-a-type
   //chain the above concept with:
   //Type potentialSub, potentialSuper;
   //bool isDirectSubclassOfSuper = potentialSub.IsSubclassOf(potentialSuper) && potentialSub.BaseType == potentialSuper;
   //public class Block : ASTNode
   //{
   //   public new static Block? Parse(List<Token> tokens, ref int index)
   //   {
   //      Block? result = null; //see 6.2.1
   //      //comprised of (in this order)
   //      //0 or 1: (label-word-symbol "label" 0 or more: (comma-special-symbol "label") semicolon-special-symbol)
   //      //0 or 1: (const-word-symbol "constant-definition" semicolon-special-symbol 0 or more: ("constant-definition" semicolon-special-symbol))
   //      //... see 6.2.1 for the rest of this list
   //      return null; 
   //   }
   //}

   //I think we would only do this if block parts were defined like 
   //block-part = a | b | c | d | e
   //instead of the reality that these nodes are directly used to construct a block.
   //public class BlockPart : ASTNode
   //{
   //   public new static BlockPart? Parse(List<Token> tokens, ref int index)
   //   {
   //      BlockPart? result = null;

   //      return result;
   //   }
   //}

   //TODO: order all of these in the order they appear in the spec

   //public class LabelDeclarationPart : ASTNode
   //{
   //   //items to complete a derived ASTNode:
   //   //Add all relevant ASTNode/Lexer Token data members
   //   //Write the ToString() function
   //   //In parser: need to assign data members & "FileLocation" & "MySource"
   //   //overload FlattenedData()
   //   public Token? FirstLabelWordSymbolToken { get; private set; } = null;
   //   public LabelValue? FirstLabelValue { get; private set; } = null;
   //   public IReadOnlyList<(Token commaToken, LabelValue labelValue)>? SecondaryLabels { get; private set; } = null;
   //   public Token? ClosingSemicolonToken { get; private set; } = null;
   //   public override string ToString()
   //   {
   //      //these should always be ALL null or NONE null (add debug for this?)
   //      if (FirstLabelWordSymbolToken == null || FirstLabelValue == null || SecondaryLabels == null || ClosingSemicolonToken == null)
   //         return $"({NodeType})";
   //      else return $"({NodeType} {FirstLabelWordSymbolToken} {FirstLabelValue} {string.Join(' ', SecondaryLabels.Select(x => $"{x.commaToken} {x.labelValue}"))} {ClosingSemicolonToken})";
   //   }
   //   //This particular parse function contains most of the patterns used. Reference it as an example
   //   public new static LabelDeclarationPart? Parse(Source source, ref int index)
   //   {
   //      //first of all, here is how the spec defines this item (see 6.2.1):
   //      //label-declaration-part = [ 'label' label { ',' label } ';' ] .
   //      //note that the contents of label-declaration-part are all encapsulated in [] (0 or 1 of), which means
   //      //in this case, if parsing fails, you should still successfully create an effectively empty label-declaration-part.
   //      //my probably correct interpretation of the definition says that a label-declaration-part is comprised of:
   //      //0 or 1: (label-word-symbol "label" 0 or more: (comma-special-symbol "label") semicolon-special-symbol)
   //      //where "label" refers to an actual label value 0-9999 (see section 6.1.6)
   //      //sometimes there are *functionally* identical interpretation of the spec in terms of executing the grammar.
   //      //I try to stick to the spec for clairity.

   //      //prelude (most parse functions should have this)
   //      int tind = index;
   //      LabelDeclarationPart? node = null; //matches return type/containing class type
   //      var tokens = source.LexerTokens;
   //      //grab all contiguous simple tokens
   //      Token firstLabelWordSymbolTok = Peek(tokens, tind++);
   //      //if the next item is an ASTNode, then verify your tokens collected so far
   //      if (firstLabelWordSymbolTok.Type == TokenType.Label)
   //      {
   //         //one by one, parse and verify your required ASTNodes, then descend one nested if deeper
   //         LabelValue? firstLabelValue = LabelValue.Parse(tokens, ref tind);
   //         if (firstLabelValue != null)
   //         {
   //            //if at any point you reach a variable number of possible tokens and/or ASTNodes, handle it like this.
   //            //in this case, we need to parse a variable amount of (, label)
   //            List<(Token comma, LabelValue labelValue)> secondaryLabels = new();
   //            Token tempComma = Peek(tokens, tind++);
   //            LabelValue? tempLabelValue = LabelValue.Parse(tokens, ref tind);
   //            while (tempComma.Type == TokenType.Comma && tempLabelValue != null) //we require both a command and a label
   //            {
   //               secondaryLabels.Add((tempComma, tempLabelValue)); //this only adds if the 0th iter was (, label), or if the previous iter was (, label), so it's fine
   //               tempComma = Peek(tokens, tind++);
   //               tempLabelValue = LabelValue.Parse(tokens, ref tind);
   //            }
   //            //secondaryLabels are already verified.
   //            Token closingSemicolon = Peek(tokens, tind++);
   //            if (closingSemicolon.Type == TokenType.Semicolon)
   //            {
   //               //we've met all the requirements for this AST node as per the 6.2.1 definition of label-declaration-part, so create the node
   //               node = new LabelDeclarationPart() 
   //               { //at this point in execution, the following must be true
   //                  FirstLabelWordSymbolToken = firstLabelWordSymbolTok, //Type == TokenType.Label
   //                  FirstLabelValue = firstLabelValue, //!= null (therefore valid)
   //                  SecondaryLabels = secondaryLabels, //may be empty, but not null (valid)
   //                  ClosingSemicolonToken = closingSemicolon //Type == TokenType.Semicolon
   //               }; //i.e., none are null
   //            }
   //         }
   //      }
   //      index = node == null ? index : tind;
   //      if (node == null) //if we failed to create the node, we should still successfully create an effectively empty label-declaration-part.
   //      { //keep in mind that the above line will revert changes to the index; an effectively empty lable-declaration-part retroactively consumed no tokens.
   //         //all the contents will be null by default, since this AST node comprises nothing.
   //         node = new LabelDeclarationPart() { };
   //      }
   //      return node;
   //   }
   //   protected override IEnumerable<MeasurableTokenesq> FlattenedData()
   //   {
   //      if (FirstLabelWordSymbolToken != null)
   //         yield return new MeasurableTokenesq(FirstLabelWordSymbolToken);
   //      if (FirstLabelValue != null)
   //         yield return new MeasurableTokenesq(FirstLabelValue);
   //      if (SecondaryLabels != null)
   //         foreach (var e in SecondaryLabels)
   //         {
   //            yield return new MeasurableTokenesq(e.commaToken);
   //            yield return new MeasurableTokenesq(e.labelValue);
   //         }
   //      if (ClosingSemicolonToken != null)
   //         yield return new MeasurableTokenesq(ClosingSemicolonToken);
   //   }
   //}

   //public class ConstantDefinitionPart : ASTNode //needs verify
   //{
   //   public Token? FirstConstWordSymbolToken { get; private set; } = null;
   //   public ConstantDefinition? FirstConstantDefinition { get; private set; } = null;
   //   public Token? FirstSemicolonToken { get; private set; } = null;
   //   public IReadOnlyList<(ConstantDefinition constantDefinition, Token semicolonToken)>? SecondaryConstantDefinitions { get; private set; } = null;
   //   public override ASTNodeType NodeType { get => ASTNodeType.ConstantDefinitionPart; }
   //   public override string ToString()
   //   {
   //      //these should always be ALL null or NONE null
   //      if (FirstConstWordSymbolToken == null || FirstConstantDefinition == null || FirstSemicolonToken == null || SecondaryConstantDefinitions == null)
   //         return $"({NodeType})";
   //      else return $"({NodeType} {FirstConstWordSymbolToken} {FirstConstantDefinition} {FirstSemicolonToken} {string.Join(string.Empty, SecondaryConstantDefinitions.Select(x => $"{x.constantDefinition} {x.semicolonToken}"))})";
   //   }
   //   public new static ConstantDefinitionPart? Parse(IReadOnlyList<Token> tokens, ref int index)
   //   { //TODOTODOTODOTODOTODOTODO but need to do ConstantDefinition first
   //      int tind = index;
   //      ConstantDefinitionPart? node = null;

   //      Token firstLabelWordSymbolTok = Peek(tokens, tind++);
   //      if (firstLabelWordSymbolTok.Type == TokenType.Label)
   //      {
   //         //one by one, parse and verify your required ASTNodes, then descend one nested if deeper
   //         LabelValue? firstLabelValue = LabelValue.Parse(tokens, ref tind);
   //         if (firstLabelValue != null)
   //         {
   //            //if at any point you reach a variable number of possible tokens and/or ASTNodes, handle it like this.
   //            //in this case, we need to parse a variable amount of (, label)
   //            List<(Token comma, LabelValue labelValue)> secondaryLabels = new();
   //            Token tempComma = Peek(tokens, tind++);
   //            LabelValue? tempLabelValue = LabelValue.Parse(tokens, ref tind);
   //            while (tempComma.Type == TokenType.Comma && tempLabelValue != null) //we require both a command and a label
   //            {
   //               secondaryLabels.Add((tempComma, tempLabelValue)); //this only adds if the 0th iter was (, label), or if the previous iter was (, label), so it's fine
   //               tempComma = Peek(tokens, tind++);
   //               tempLabelValue = LabelValue.Parse(tokens, ref tind);
   //            }
   //            //secondaryLabels are already verified.
   //            Token closingSemicolon = Peek(tokens, tind++);
   //            if (closingSemicolon.Type == TokenType.Semicolon)
   //            {
   //               //we've met all the requirements for this AST node as per the 6.2.1 definition of label-declaration-part, so create the node
   //               node = new ConstantDefinitionPart()
   //               { //at this point in execution, the following must be true
   //                  FirstLabelWordSymbol = firstLabelWordSymbolTok, //Type == TokenType.Label
   //                  FirstLabelValue = firstLabelValue, //!= null (therefore valid)
   //                  SecondaryLabels = secondaryLabels, //may be empty, but not null (valid)
   //                  ClosingSemicolon = closingSemicolon //Type == TokenType.Semicolon
   //               }; //i.e., none are null
   //            }
   //         }
   //      }
   //      index = node == null ? index : tind;
   //      if (node == null) //if we failed to create the node, we should still successfully create an effectively empty label-declaration-part.
   //      { //keep in mind that the above line will revert changes to the index; an effectively empty lable-declaration-part retroactively consumed no tokens.
   //         //all the contents will be null by default, since this AST node comprises nothing.
   //         node = new ConstantDefinitionPart() { };
   //      }
   //      return node;
   //   }
   //}

   ////public class Value : ASTNode
   ////{
   ////   public new static Value? Parse(List<Token> tokens, ref int index)
   ////   {
   ////      Value? result = null;
   ////      //iterate over derivers and call .parse on all of them?
   ////      result ??= LabelValueT.Parse(tokens, ref index);
   ////      return result;
   ////   }
   ////}

   //public class LabelValue : ASTNode //needs verify
   //{
   //   public Token DigitSequenceToken { get; private set; } = null!;
   //   public override ASTNodeType NodeType { get => ASTNodeType.LabelValue; }
   //   public override string ToString()
   //   {
   //      return $"({NodeType} {DigitSequenceToken})";
   //   }
   //   public new static LabelValue? Parse(IReadOnlyList<Token> tokens, ref int index)
   //   {
   //      int tind = index;
   //      LabelValue? node = null;
   //      //regarding label value: see section 6.1.6, simply a digit sequence
   //      Token labelValueTok = Peek(tokens, tind++);
   //      //iirc, label values are 0-9999, should that be enforced here or the typechecker?
   //      //probably here, because label values are *defined* to be 0-9999, and that's enforcable here?
   //      //although, that would require parsing as an int, and that sounds like something more befitting
   //      //the typechecker stage.
   //      if (labelValueTok.Type == TokenType.SEQ_Digits)
   //      {
   //         node = new LabelValue() { DigitSequenceToken = labelValueTok, FileLocation = labelValueTok.FileLocation };
   //      }
   //      index = node == null ? index : tind;
   //      return node;
   //   }
   //}

   //public class ConstantDefinition : ASTNode //needs verify
   //{
   //   public Identifier Identifier { get; private set; } = null!;
   //   public Token EqualsToken { get; private set; } = null!;
   //   public Constant Constant { get; private set; } = null!;
   //   public override string ToString()
   //   {
   //      return $"({NodeType} {Identifier} {EqualsToken} {Constant})";
   //   }
   //   public new static ConstantDefinition? Parse(Source source, ref int index)
   //   {
   //      int tind = index;
   //      ConstantDefinition? node = null;
   //      var tokens = source.LexerTokens;
   //      Identifier? id = Identifier.Parse(source, ref tind);
   //      if (id != null)
   //      {
   //         Token eqToken = Peek(tokens, tind++);
   //         if (eqToken.Type == TokenType.Equals)
   //         {
   //            Constant? constant = Constant.Parse(source, ref tind);
   //            if (constant != null)
   //            {
   //               node = new ConstantDefinition()
   //               {
   //                  Identifier = id,
   //                  EqualsToken = eqToken,
   //                  Constant = constant,
   //                  MySource = source,
   //                  FileLocation = id.FileLocation
   //               };
   //            }
   //         }
   //      }
   //      index = node == null ? index : tind;
   //      return node;
   //   }
   //   protected override IEnumerable<MeasurableTokenesq> FlattenedData()
   //   {
   //      yield return new MeasurableTokenesq(Identifier);
   //      yield return new MeasurableTokenesq(EqualsToken);
   //      yield return new MeasurableTokenesq(Constant);
   //   }
   //}

   //public class Identifier : ASTNode
   //{ //directive should be basically the same
   //   public Token FirstLetterSequence { get; private set; } = null!;
   //   public IReadOnlyList<Token>? LetterAndDigitSequences { get; private set; } = null;
   //   public override string ToString()
   //   {
   //      if (LetterAndDigitSequences == null)
   //         return $"({NodeType} {FirstLetterSequence})";
   //      return $"({NodeType} {FirstLetterSequence} {string.Join(' ', LetterAndDigitSequences)})";
   //   }
   //   public new static Identifier? Parse(Source source, ref int index)
   //   {
   //      int tind = index;
   //      Identifier? node = null;
   //      var tokens = source.LexerTokens;
   //      //spec definition: identifier = letter { letter | digit }.
   //      //should match our functionality of "" = SEQ_Letters { SEQ_Letters | SEQ_Digits }, but this might be worth correcting.
   //      Token firstLetterToken = Peek(tokens, tind++);
   //      if (firstLetterToken.Type == TokenType.SEQ_Letters)
   //      {
   //         List<Token> seqs = new List<Token>();
   //         Token t = Peek(tokens, tind++);
   //         while (t.Type == TokenType.SEQ_Letters || t.Type == TokenType.SEQ_Digits) 
   //         {
   //            seqs.Add(t);
   //            t = Peek(tokens, tind++);
   //         }
   //         node = new Identifier() 
   //         {
   //            FirstLetterSequence = firstLetterToken,
   //            LetterAndDigitSequences = seqs,
   //            FileLocation = firstLetterToken.FileLocation,
   //         };
   //      }
   //      index = node == null ? index : tind;
   //      return node;
   //   }
   //   protected override IEnumerable<MeasurableTokenesq> FlattenedData()
   //   {
   //      yield return new MeasurableTokenesq(FirstLetterSequence);
   //      if (LetterAndDigitSequences != null)
   //         foreach (var e in LetterAndDigitSequences)
   //            yield return new MeasurableTokenesq(e);
   //   }
   //}

   //public class Constant : ASTNode
   //{
   //   public new static Constant? Parse(Source source, ref int index)
   //   {
   //      Constant? result = null;
   //      result ??= NumberConstant.Parse(source, ref index);
   //      result ??= StringConstant.Parse(source, ref index);
   //      return result;
   //   }
   //}

   //public class NumberConstant : Constant
   //{
   //   public Token SignToken { get; private set; } = null!;
   //   public override string ToString()
   //   {
   //      return $"";
   //   }
   //   public new static NumberConstant? Parse(Source source, ref int index)
   //   {
   //      int tind = index;
   //      NumberConstant? node = null;
   //      var tokens = source.LexerTokens;

   //      Token signTok = Peek(tokens, tind++);
   //      if (signTok.Type == TokenType.Plus ||
   //          signTok.Type == TokenType.Minus)
   //      {
   //         //get unsigned-number or constant-identifier
   //      }

   //      index = node == null ? index : tind;
   //      return node;
   //   }
   //   protected override IEnumerable<MeasurableTokenesq> FlattenedData()
   //   {
   //      yield return new MeasurableTokenesq(SignToken);
   //   }
   //}

   //public class StringConstant : Constant
   //{
   //   public Token CharacterString { get; private set; } = null!;
   //   public override string ToString()
   //   {
   //      return $"{NodeType} {CharacterString}";
   //   }
   //   public new static StringConstant? Parse(Source source, ref int index)
   //   {
   //      int tind = index;
   //      StringConstant? node = null;
   //      var tokens = source.LexerTokens;

   //      Token stringTok = Peek(tokens, tind++);
   //      if (stringTok.Type == TokenType.String)
   //      {
   //         node = new StringConstant()
   //         {
   //            CharacterString = stringTok,
   //            FileLocation = stringTok.FileLocation,
   //            MySource = source
   //         };
   //      }

   //      index = node == null ? index : tind;
   //      return node;
   //   }
   //   protected override IEnumerable<MeasurableTokenesq> FlattenedData()
   //   {
   //      yield return new MeasurableTokenesq(CharacterString);
   //   }
   //}
   #endregion
   #endregion

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
            ASTNode? node = ASTNode.Parse(source, ref ind);
            if (node == null/* || node.NodeType == ASTNodeType.UNDEFINED*/)
            {
               source.AppendMessage(new Source.Message(Source.CompilerPhase.Parse, Source.Severity.Error, "No valid candidates for parsing token", source.LexerTokens[ind].FileLocation, true));
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
