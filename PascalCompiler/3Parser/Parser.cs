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

namespace PascalCompiler.Parser
{
   public class ASTNode
   {
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
      private int? nodeLength = null;
      public int NodeLength
      { //this and "Content" *look* like abababab type recursion, but aren't so long as any AST node's components eventually end at an AST node entirely comprised by only lexer tokens.
         get
         {
            if (nodeLength == null)
               nodeLength = FlattenedData().Select(x => x.Length).Aggregate((l, r) => l + r);
            return nodeLength.Value;
         }
      }
      private string? content = null;
      public string Content 
      {
         get 
         {
            if (content == null)
               content = FlattenedData().Select(x => x.Content).Aggregate((l, r) => l + r);
            return content;
         } 
      }
      //adding the below concept as a feature only makes sense if every non-terminal grammar node in the spec
      //was structured in such a way where the representing AST node is either fully "effectively empty"
      //or fully populated, and nothing in between. That probably isn't true.
      ///// <summary>
      ///// Is this AST node effectively empty? When true, the spec usually indicated that the definition of this AST node
      ///// had the entirety of the contents defined as optional. This value is usually false.
      ///// </summary>
      //public bool EffectivelyEmpty { get; protected set; } = false;

      public static ASTNode? Parse(Source source, ref int index)
      {
         ASTNode? result = null;
         //list of:
         //result ??= [ASTNodeCategory].Parse(tokens, ref index);
         //where [ASTNodeCategory] is a type that one or more terminals derive from.
         return result;
      }

      protected static Token Peek(IReadOnlyList<Token> tokens, int index)
      {
         return index < tokens.Count ? tokens[index] : Token.UndefinedToken;
      }

      protected virtual IEnumerable<MeasurableTokenesq> FlattenedData()
      {
         throw new NotImplementedException(); //if this is ever thrown, the compiler has a bug. (I.e., someone didn't implement it!)
      }

      public override string ToString()
      {
         return $"This ASTNode's ToString() function was not overrided by it's deriver! Type: {GetType().Name}";
      }
   }

   #region Node Grammatical Classes
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
   public class Block : ASTNode
   {
      public new static Block? Parse(List<Token> tokens, ref int index)
      {
         Block? result = null; //see 6.2.1
         //comprised of (in this order)
         //0 or 1: (label-word-symbol "label" 0 or more: (comma-special-symbol "label") semicolon-special-symbol)
         //0 or 1: (const-word-symbol "constant-definition" semicolon-special-symbol 0 or more: ("constant-definition" semicolon-special-symbol))
         //... see 6.2.1 for the rest of this list
         return null; 
      }
   }

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

   public class LabelDeclarationPart : ASTNode
   {
      //items to complete a derived ASTNode:
      //Add all relevant ASTNode/Lexer Token data members
      //Write the ToString() function
      //In parser: need to assign data members & "FileLocation" & "MySource"
      //overload FlattenedData()
      public Token? FirstLabelWordSymbolToken { get; private set; } = null;
      public LabelValue? FirstLabelValue { get; private set; } = null;
      public IReadOnlyList<(Token commaToken, LabelValue labelValue)>? SecondaryLabels { get; private set; } = null;
      public Token? ClosingSemicolonToken { get; private set; } = null;
      public override string ToString()
      {
         //these should always be ALL null or NONE null (add debug for this?)
         if (FirstLabelWordSymbolToken == null || FirstLabelValue == null || SecondaryLabels == null || ClosingSemicolonToken == null)
            return $"({NodeType})";
         else return $"({NodeType} {FirstLabelWordSymbolToken} {FirstLabelValue} {string.Join(' ', SecondaryLabels.Select(x => $"{x.commaToken} {x.labelValue}"))} {ClosingSemicolonToken})";
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
         int tind = index;
         LabelDeclarationPart? node = null; //matches return type/containing class type
         var tokens = source.LexerTokens;
         //grab all contiguous simple tokens
         Token firstLabelWordSymbolTok = Peek(tokens, tind++);
         //if the next item is an ASTNode, then verify your tokens collected so far
         if (firstLabelWordSymbolTok.Type == TokenType.Label)
         {
            //one by one, parse and verify your required ASTNodes, then descend one nested if deeper
            LabelValue? firstLabelValue = LabelValue.Parse(tokens, ref tind);
            if (firstLabelValue != null)
            {
               //if at any point you reach a variable number of possible tokens and/or ASTNodes, handle it like this.
               //in this case, we need to parse a variable amount of (, label)
               List<(Token comma, LabelValue labelValue)> secondaryLabels = new();
               Token tempComma = Peek(tokens, tind++);
               LabelValue? tempLabelValue = LabelValue.Parse(tokens, ref tind);
               while (tempComma.Type == TokenType.Comma && tempLabelValue != null) //we require both a command and a label
               {
                  secondaryLabels.Add((tempComma, tempLabelValue)); //this only adds if the 0th iter was (, label), or if the previous iter was (, label), so it's fine
                  tempComma = Peek(tokens, tind++);
                  tempLabelValue = LabelValue.Parse(tokens, ref tind);
               }
               //secondaryLabels are already verified.
               Token closingSemicolon = Peek(tokens, tind++);
               if (closingSemicolon.Type == TokenType.Semicolon)
               {
                  //we've met all the requirements for this AST node as per the 6.2.1 definition of label-declaration-part, so create the node
                  node = new LabelDeclarationPart() 
                  { //at this point in execution, the following must be true
                     FirstLabelWordSymbolToken = firstLabelWordSymbolTok, //Type == TokenType.Label
                     FirstLabelValue = firstLabelValue, //!= null (therefore valid)
                     SecondaryLabels = secondaryLabels, //may be empty, but not null (valid)
                     ClosingSemicolonToken = closingSemicolon //Type == TokenType.Semicolon
                  }; //i.e., none are null
               }
            }
         }
         index = node == null ? index : tind;
         if (node == null) //if we failed to create the node, we should still successfully create an effectively empty label-declaration-part.
         { //keep in mind that the above line will revert changes to the index; an effectively empty lable-declaration-part retroactively consumed no tokens.
            //all the contents will be null by default, since this AST node comprises nothing.
            node = new LabelDeclarationPart() { };
         }
         return node;
      }
      protected override IEnumerable<MeasurableTokenesq> FlattenedData()
      {
         if (FirstLabelWordSymbolToken != null)
            yield return new MeasurableTokenesq(FirstLabelWordSymbolToken);
         if (FirstLabelValue != null)
            yield return new MeasurableTokenesq(FirstLabelValue);
         if (SecondaryLabels != null)
            foreach (var e in SecondaryLabels)
            {
               yield return new MeasurableTokenesq(e.commaToken);
               yield return new MeasurableTokenesq(e.labelValue);
            }
         if (ClosingSemicolonToken != null)
            yield return new MeasurableTokenesq(ClosingSemicolonToken);
      }
   }

   public class ConstantDefinitionPart : ASTNode //needs verify
   {
      public Token? FirstConstWordSymbolToken { get; private set; } = null;
      public ConstantDefinition? FirstConstantDefinition { get; private set; } = null;
      public Token? FirstSemicolonToken { get; private set; } = null;
      public IReadOnlyList<(ConstantDefinition constantDefinition, Token semicolonToken)>? SecondaryConstantDefinitions { get; private set; } = null;
      public override ASTNodeType NodeType { get => ASTNodeType.ConstantDefinitionPart; }
      public override string ToString()
      {
         //these should always be ALL null or NONE null
         if (FirstConstWordSymbolToken == null || FirstConstantDefinition == null || FirstSemicolonToken == null || SecondaryConstantDefinitions == null)
            return $"({NodeType})";
         else return $"({NodeType} {FirstConstWordSymbolToken} {FirstConstantDefinition} {FirstSemicolonToken} {string.Join(string.Empty, SecondaryConstantDefinitions.Select(x => $"{x.constantDefinition} {x.semicolonToken}"))})";
      }
      public new static ConstantDefinitionPart? Parse(IReadOnlyList<Token> tokens, ref int index)
      { //TODOTODOTODOTODOTODOTODO but need to do ConstantDefinition first
         int tind = index;
         ConstantDefinitionPart? node = null;

         Token firstLabelWordSymbolTok = Peek(tokens, tind++);
         if (firstLabelWordSymbolTok.Type == TokenType.Label)
         {
            //one by one, parse and verify your required ASTNodes, then descend one nested if deeper
            LabelValue? firstLabelValue = LabelValue.Parse(tokens, ref tind);
            if (firstLabelValue != null)
            {
               //if at any point you reach a variable number of possible tokens and/or ASTNodes, handle it like this.
               //in this case, we need to parse a variable amount of (, label)
               List<(Token comma, LabelValue labelValue)> secondaryLabels = new();
               Token tempComma = Peek(tokens, tind++);
               LabelValue? tempLabelValue = LabelValue.Parse(tokens, ref tind);
               while (tempComma.Type == TokenType.Comma && tempLabelValue != null) //we require both a command and a label
               {
                  secondaryLabels.Add((tempComma, tempLabelValue)); //this only adds if the 0th iter was (, label), or if the previous iter was (, label), so it's fine
                  tempComma = Peek(tokens, tind++);
                  tempLabelValue = LabelValue.Parse(tokens, ref tind);
               }
               //secondaryLabels are already verified.
               Token closingSemicolon = Peek(tokens, tind++);
               if (closingSemicolon.Type == TokenType.Semicolon)
               {
                  //we've met all the requirements for this AST node as per the 6.2.1 definition of label-declaration-part, so create the node
                  node = new ConstantDefinitionPart()
                  { //at this point in execution, the following must be true
                     FirstLabelWordSymbol = firstLabelWordSymbolTok, //Type == TokenType.Label
                     FirstLabelValue = firstLabelValue, //!= null (therefore valid)
                     SecondaryLabels = secondaryLabels, //may be empty, but not null (valid)
                     ClosingSemicolon = closingSemicolon //Type == TokenType.Semicolon
                  }; //i.e., none are null
               }
            }
         }
         index = node == null ? index : tind;
         if (node == null) //if we failed to create the node, we should still successfully create an effectively empty label-declaration-part.
         { //keep in mind that the above line will revert changes to the index; an effectively empty lable-declaration-part retroactively consumed no tokens.
            //all the contents will be null by default, since this AST node comprises nothing.
            node = new ConstantDefinitionPart() { };
         }
         return node;
      }
   }

   //public class Value : ASTNode
   //{
   //   public new static Value? Parse(List<Token> tokens, ref int index)
   //   {
   //      Value? result = null;
   //      //iterate over derivers and call .parse on all of them?
   //      result ??= LabelValueT.Parse(tokens, ref index);
   //      return result;
   //   }
   //}

   public class LabelValue : ASTNode //needs verify
   {
      public Token DigitSequenceToken { get; private set; } = null!;
      public override ASTNodeType NodeType { get => ASTNodeType.LabelValue; }
      public override string ToString()
      {
         return $"({NodeType} {DigitSequenceToken})";
      }
      public new static LabelValue? Parse(IReadOnlyList<Token> tokens, ref int index)
      {
         int tind = index;
         LabelValue? node = null;
         //regarding label value: see section 6.1.6, simply a digit sequence
         Token labelValueTok = Peek(tokens, tind++);
         //iirc, label values are 0-9999, should that be enforced here or the typechecker?
         //probably here, because label values are *defined* to be 0-9999, and that's enforcable here?
         //although, that would require parsing as an int, and that sounds like something more befitting
         //the typechecker stage.
         if (labelValueTok.Type == TokenType.SEQ_Digits)
         {
            node = new LabelValue() { DigitSequenceToken = labelValueTok, FileLocation = labelValueTok.FileLocation };
         }
         index = node == null ? index : tind;
         return node;
      }
   }

   public class ConstantDefinition : ASTNode //needs verify
   {
      public Identifier Identifier { get; private set; } = null!;
      public Token EqualsToken { get; private set; } = null!;
      public Constant Constant { get; private set; } = null!;
      public override string ToString()
      {
         return $"({NodeType} {Identifier} {EqualsToken} {Constant})";
      }
      public new static ConstantDefinition? Parse(Source source, ref int index)
      {
         int tind = index;
         ConstantDefinition? node = null;
         var tokens = source.LexerTokens;
         Identifier? id = Identifier.Parse(source, ref tind);
         if (id != null)
         {
            Token eqToken = Peek(tokens, tind++);
            if (eqToken.Type == TokenType.Equals)
            {
               Constant? constant = Constant.Parse(source, ref tind);
               if (constant != null)
               {
                  node = new ConstantDefinition()
                  {
                     Identifier = id,
                     EqualsToken = eqToken,
                     Constant = constant,
                     MySource = source,
                     FileLocation = id.FileLocation
                  };
               }
            }
         }
         index = node == null ? index : tind;
         return node;
      }
      protected override IEnumerable<MeasurableTokenesq> FlattenedData()
      {
         yield return new MeasurableTokenesq(Identifier);
         yield return new MeasurableTokenesq(EqualsToken);
         yield return new MeasurableTokenesq(Constant);
      }
   }

   public class Identifier : ASTNode
   { //directive should be basically the same
      public Token FirstLetterSequence { get; private set; } = null!;
      public IReadOnlyList<Token>? LetterAndDigitSequences { get; private set; } = null;
      public override string ToString()
      {
         if (LetterAndDigitSequences == null)
            return $"({NodeType} {FirstLetterSequence})";
         return $"({NodeType} {FirstLetterSequence} {string.Join(' ', LetterAndDigitSequences)})";
      }
      public new static Identifier? Parse(Source source, ref int index)
      {
         int tind = index;
         Identifier? node = null;
         var tokens = source.LexerTokens;
         //spec definition: identifier = letter { letter | digit }.
         //should match our functionality of "" = SEQ_Letters { SEQ_Letters | SEQ_Digits }, but this might be worth correcting.
         Token firstLetterToken = Peek(tokens, tind++);
         if (firstLetterToken.Type == TokenType.SEQ_Letters)
         {
            List<Token> seqs = new List<Token>();
            Token t = Peek(tokens, tind++);
            while (t.Type == TokenType.SEQ_Letters || t.Type == TokenType.SEQ_Digits) 
            {
               seqs.Add(t);
               t = Peek(tokens, tind++);
            }
            node = new Identifier() 
            {
               FirstLetterSequence = firstLetterToken,
               LetterAndDigitSequences = seqs,
               FileLocation = firstLetterToken.FileLocation,
            };
         }
         index = node == null ? index : tind;
         return node;
      }
      protected override IEnumerable<MeasurableTokenesq> FlattenedData()
      {
         yield return new MeasurableTokenesq(FirstLetterSequence);
         if (LetterAndDigitSequences != null)
            foreach (var e in LetterAndDigitSequences)
               yield return new MeasurableTokenesq(e);
      }
   }

   public class Constant : ASTNode
   {
      public new static Constant? Parse(Source source, ref int index)
      {
         Constant? result = null;
         result ??= NumberConstant.Parse(source, ref index);
         result ??= StringConstant.Parse(source, ref index);
         return result;
      }
   }

   public class NumberConstant : Constant
   {
      public Token SignToken { get; private set; } = null!;
      public override string ToString()
      {
         return $"";
      }
      public new static NumberConstant? Parse(Source source, ref int index)
      {
         int tind = index;
         NumberConstant? node = null;
         var tokens = source.LexerTokens;

         Token signTok = Peek(tokens, tind++);
         if (signTok.Type == TokenType.Plus ||
             signTok.Type == TokenType.Minus)
         {
            //get unsigned-number or constant-identifier
         }

         index = node == null ? index : tind;
         return node;
      }
      protected override IEnumerable<MeasurableTokenesq> FlattenedData()
      {
         yield return new MeasurableTokenesq(SignToken);
      }
   }

   public class StringConstant : Constant
   {
      public Token CharacterString { get; private set; } = null!;
      public override string ToString()
      {
         return $"{NodeType} {CharacterString}";
      }
      public new static StringConstant? Parse(Source source, ref int index)
      {
         int tind = index;
         StringConstant? node = null;
         var tokens = source.LexerTokens;

         Token stringTok = Peek(tokens, tind++);
         if (stringTok.Type == TokenType.String)
         {
            node = new StringConstant()
            {
               CharacterString = stringTok,
               FileLocation = stringTok.FileLocation,
               MySource = source
            };
         }

         index = node == null ? index : tind;
         return node;
      }
      protected override IEnumerable<MeasurableTokenesq> FlattenedData()
      {
         yield return new MeasurableTokenesq(CharacterString);
      }
   }
   #endregion

   public static class Parser
   {
      public static void Parse(Source source)
      {
         int ind = 0;
         while (ind < source.LexerTokens.Count/* && source.LexerTokens[ind].Type != TokenType.END_OF_FILE*/)
         {
            ASTNode? node = ASTNode.Parse(source, ref ind);
            if (node == null/* || node.NodeType == ASTNodeType.UNDEFINED*/)
            {
               source.AppendMessage(new Source.Message(Source.CompilerPhase.Parse, Source.Severity.Error, "No valid candidates for parsing token", source.LexerTokens[ind].FileLocation, true));
               throw new Exception(); //todo: put a message in this
            }
            source.AddParserNode(node);
         }
         source.ParsingComplete();
      }
   }
}
