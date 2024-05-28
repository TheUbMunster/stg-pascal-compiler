using PascalCompiler.Lexer;
using PascalCompiler.Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler.Parser
{
   #region Node Grammatical Classes
   //regex for capturing all the comments
   /*
   (the entirety of the below line, including the leading //)

//The comment below belongs to the ISO/IEC 7185:1990 spec, and is here as a reference for assisting implementation development.\r\n   //I believe this is covered by U.S. fair use \(under the intent of scholarship\), if not, ISO; contact me and I will immediately remove it.\r\n   //.+?\r\n

    */

      public class ActualParameter : ASTNode
   {
      public Expression? Expression { get; private init; } = null;
      public VariableAccess? VariableAccess { get; private init; } = null;
      public ProcedureIdentifier? ProcedureIdentifier { get; private init; } = null;
      public FunctionIdentifier? FunctionIdentifier { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            Expression? expression = Expression.Parse(source, ref tind);
            if (expression != null)
            {
               node = new ActualParameter()
               {
                  MySource = source,
                  FileLocation = expression.FileLocation,

                  Expression = expression
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            VariableAccess? variableAccess = VariableAccess.Parse(source, ref tind);
            if (variableAccess != null)
            {
               node = new ActualParameter()
               {
                  MySource = source,
                  FileLocation = variableAccess.FileLocation,

                  VariableAccess = variableAccess
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            ProcedureIdentifier? procedureIdentifier = ProcedureIdentifier.Parse(source, ref tind);
            if (procedureIdentifier != null)
            {
               node = new ActualParameter()
               {
                  MySource = source,
                  FileLocation = procedureIdentifier.FileLocation,

                  ProcedureIdentifier = procedureIdentifier
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            FunctionIdentifier? functionIdentifier = FunctionIdentifier.Parse(source, ref tind);
            if (functionIdentifier != null)
            {
               node = new ActualParameter()
               {
                  MySource = source,
                  FileLocation = functionIdentifier.FileLocation,

                  FunctionIdentifier = functionIdentifier
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ActualParameterList : ASTNode
   {
      public TokenNode OpenParenToken { get; private init; } = null!;
      public ActualParameter ActualParameter { get; private init; } = null!;
      public IReadOnlyList<(TokenNode commaToken, ActualParameter actualParameter)> SecondaryActualParameters { get; private init; } = null!;
      public TokenNode CloseParenToken { get; private init; } = null!;
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
         int tind = index;
         ActualParameterList? node = null;

         TokenNode openParenTok = PopToken(source, ref tind);
         if (openParenTok.Type == TokenType.OpenParen)
         {
            ActualParameter? firstActualParameter = ActualParameter.Parse(source, ref tind);
            if (firstActualParameter != null)
            {
               List<(TokenNode commaToken, ActualParameter actualParameter)> secondaryActualParameters = new();
               {
                  while (true)
                  {
                     int pretind = tind;
                     TokenNode tempComma = PopToken(source, ref tind);
                     ActualParameter? tempActualParameter = ActualParameter.Parse(source, ref tind);
                     if (tempComma.Type == TokenType.Comma && tempActualParameter != null)
                     { //both exist (add to collection and continue)
                        secondaryActualParameters.Add((tempComma, tempActualParameter));
                     }
                     else //in any other situation
                     {
                        tind = pretind;
                        break;
                     }
                  }
               }
               TokenNode closingParenTok = PopToken(source, ref tind);
               if (closingParenTok.Type == TokenType.CloseParen)
               {
                  node = new ActualParameterList()
                  {
                     MySource = source, FileLocation = openParenTok.FileLocation,

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
      public TokenNode? PlusToken { get; private init; } = null;
      public TokenNode? MinusToken { get; private init; } = null;
      public TokenNode? OrToken { get; private init; } = null;
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         if (prettyPrint)
            return $"{new string('\t', indentLevel)}({NodeType}{Environment.NewLine}" +
                   PlusToken != null ? $"{new string('\t', indentLevel + 1)}{PlusToken}{Environment.NewLine}" : string.Empty +
                   MinusToken != null ? $"{new string('\t', indentLevel + 1)}{MinusToken}{Environment.NewLine}" : string.Empty +
                   OrToken != null ? $"{new string('\t', indentLevel + 1)}{OrToken}{Environment.NewLine}" : string.Empty +
                   $"{new string('\t', indentLevel)})";
         else
            return $"({NodeType}" +
                   PlusToken != null ? $"{PlusToken}" : string.Empty +
                   MinusToken != null ? $"{MinusToken}" : string.Empty +
                   OrToken != null ? $"{OrToken}" : string.Empty +
                   $")";
      }
      protected override void AssertCorrectStructure()
      {
         int totalNonNull = 0;
         totalNonNull += PlusToken != null ? 1 : 0;
         totalNonNull += MinusToken != null ? 1 : 0;
         totalNonNull += OrToken != null ? 1 : 0;
         //should be exactly one non-null
         if (totalNonNull != 1)
         {
            throw new InvalidOperationException($"Compiler parse error in {GetType().Name} (compiler bug): invalid object state.");
         }
      }
      public new static AddingOperator? Parse(Source source, ref int index)
      {
         int tind = index;
         AddingOperator? node = null;

         TokenNode tok = PopToken(source, ref tind);

         if (tok.Type == TokenType.Plus ||
             tok.Type == TokenType.Minus ||
             tok.Type == TokenType.Or)
         {
            node = new AddingOperator()
            {
               MySource = source, FileLocation = tok.FileLocation,

               PlusToken = tok.Type == TokenType.Plus ? tok : null!,
               MinusToken = tok.Type == TokenType.Minus ? tok : null!,
               OrToken = tok.Type == TokenType.Or ? tok : null!,
            };
         }

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
      public TokenNode ArrayToken { get; private init; } = null!;
      public TokenNode LeftSquareBracketToken { get; private init; } = null!;
      public IndexType IndexType { get; private init; } = null!;
      public IReadOnlyList<(TokenNode commaToken, IndexType indexType)> SecondaryIndexTypes { get; private set; } = null!;
      public TokenNode RightSquareBracketToken { get; private init; } = null!;
      public TokenNode OfToken { get; private init; } = null!;
      public ComponentType ComponentType { get; private init; } = null!;
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

         TokenNode arrayToken = PopToken(source, ref tind);
         TokenNode leftSquareBracketToken = PopToken(source, ref tind);
         if (arrayToken.Type == TokenType.Array &&
             leftSquareBracketToken.Type == TokenType.LeftSquareBracket)
         {
            IndexType? indexType = IndexType.Parse(source, ref tind);
            if (indexType != null)
            {
               List<(TokenNode commaToken, IndexType indexType)> secondaryIndexTypes = new();
               {
                  while (true)
                  {
                     int pretind = tind;
                     TokenNode tempComma = PopToken(source, ref tind);
                     IndexType? tempIndexType = IndexType.Parse(source, ref tind);
                     if (tempComma.Type == TokenType.Comma && tempIndexType != null)
                     { //both exist (add to collection and continue)
                        secondaryIndexTypes.Add((tempComma, tempIndexType));
                     }
                     else //in any other situation
                     {
                        tind = pretind;
                        break;
                     }
                  }
               }
               TokenNode rightSquareBracketToken = PopToken(source, ref tind);
               TokenNode ofToken = PopToken(source, ref tind);
               if (rightSquareBracketToken.Type == TokenType.RightSquareBracket &&
                   ofToken.Type == TokenType.Of)
               {
                  ComponentType? componentType = ComponentType.Parse(source, ref tind);
                  if (componentType != null)
                  {
                     node = new ArrayType() 
                     {
                        MySource = source, FileLocation = arrayToken.FileLocation,

                        ArrayToken = arrayToken,
                        LeftSquareBracketToken = leftSquareBracketToken,
                        IndexType = indexType,
                        SecondaryIndexTypes = secondaryIndexTypes,
                        RightSquareBracketToken = rightSquareBracketToken,
                        OfToken = ofToken,
                        ComponentType = componentType
                     };
                  }
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ArrayVariable : ASTNode
   {
      public VariableAccess VariableAccess { get; private init; } = null!;
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

         VariableAccess? variableAccess = VariableAccess.Parse(source, ref tind);

         if (variableAccess != null)
         {
            node = new ArrayVariable() 
            { 
               MySource = source, FileLocation = variableAccess.FileLocation,
               
               VariableAccess = variableAccess
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class AssignmentStatement : ASTNode
   {
      public VariableAccess? VariableAccess { get; private init; } = null;
      public FunctionIdentifier? FunctionIdentifier { get; private init; } = null;
      public TokenNode WalrusToken { get; private init; } = null!;
      public Expression Expression { get; private init; } = null!;
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

         VariableAccess? variableAccess = VariableAccess.Parse(source, ref tind);
         FunctionIdentifier? functionIdentifier = null;

         if (variableAccess == null)
         {
            functionIdentifier = FunctionIdentifier.Parse(source, ref tind); //we only want to attempt to parse if the previous one failed.
         }

         ASTNode? content = ((ASTNode?)variableAccess ?? functionIdentifier);
         if (content != null)
         {
            TokenNode walrusToken = PopToken(source, ref tind);
            if (walrusToken.Type == TokenType.Walrus)
            {
               Expression? expression = Expression.Parse(source, ref tind);
               if (expression != null)
               {
                  node = new AssignmentStatement()
                  {
                     MySource = source, FileLocation = content.FileLocation,

                     VariableAccess = variableAccess,
                     FunctionIdentifier = functionIdentifier,
                     WalrusToken = walrusToken,
                     Expression = expression
                  };
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class BaseType : ASTNode
   {
      public OrdinalType OrdinalType { get; private init; } = null!;
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

         OrdinalType? ordinalType = OrdinalType.Parse(source, ref tind);
         if (ordinalType != null)
         {
            node = new BaseType() 
            {
               MySource = source, FileLocation = ordinalType.FileLocation,

               OrdinalType = ordinalType,
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class Block : ASTNode
   {
      public LabelDeclarationPart LabelDeclarationPart { get; private init; } = null!;
      public ConstantDefinitionPart ConstantDefinitionPart { get; private init; } = null!;
      public TypeDefinitionPart TypeDefinitionPart { get; private init; } = null!;
      public VariableDeclarationPart VariableDeclarationPart { get; private init; } = null!;
      public ProcedureAndFunctionDeclarationPart ProcedureAndFunctionDeclarationPart { get; private init; } = null!;
      public StatementPart StatementPart { get; private init; } = null!;
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

         LabelDeclarationPart? labelDeclarationPart = LabelDeclarationPart.Parse(source, ref tind);
         ConstantDefinitionPart? constantDefinitionPart = ConstantDefinitionPart.Parse(source, ref tind);
         TypeDefinitionPart? typeDefinitionPart = TypeDefinitionPart.Parse(source, ref tind);
         VariableDeclarationPart? variableDeclarationPart = VariableDeclarationPart.Parse(source, ref tind);
         ProcedureAndFunctionDeclarationPart? procedureAndFunctionDeclarationPart = ProcedureAndFunctionDeclarationPart.Parse(source, ref tind);
         StatementPart? statementPart = StatementPart.Parse(source, ref tind);

         if (labelDeclarationPart != null)
         {
            if (constantDefinitionPart != null)
            {
               if (typeDefinitionPart != null)
               {
                  if (variableDeclarationPart != null)
                  {
                     if (procedureAndFunctionDeclarationPart != null)
                     {
                        if (statementPart != null)
                        {
                           node = new Block() 
                           {
                              MySource = source, FileLocation = labelDeclarationPart!.FileLocation,

                              LabelDeclarationPart = labelDeclarationPart,
                              ConstantDefinitionPart = constantDefinitionPart,
                              TypeDefinitionPart = typeDefinitionPart,
                              VariableDeclarationPart = variableDeclarationPart,
                              ProcedureAndFunctionDeclarationPart = procedureAndFunctionDeclarationPart,
                              StatementPart = statementPart
                           };
                        }
                     }
                  }
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class BooleanExpression : ASTNode
   {
      public Expression Expression { get; private init; } = null!;
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

         Expression? expression = Expression.Parse(source, ref tind);
         if (expression != null)
         {
            node = new BooleanExpression() 
            {
               MySource = source, FileLocation = expression.FileLocation,

               Expression = expression
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class BoundIdentifier : ASTNode
   {
      public Identifier Identifier { get; private init; } = null!;
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

         Identifier? identifier = Identifier.Parse(source, ref tind);
         if (identifier != null)
         {
            node = new BoundIdentifier()
            {
               MySource = source, FileLocation = identifier.FileLocation,

               Identifier = identifier
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class BufferVariable : ASTNode
   {
      public FileVariable FileVariable { get; private init; } = null!;
      public TokenNode UpArrowToken { get; private init; } = null!;
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

         FileVariable? fileVariable = FileVariable.Parse(source, ref tind);
         if (fileVariable != null)
         {
            TokenNode upArrowToken = PopToken(source, ref tind);
            if (upArrowToken.Type == TokenType.UpArrow)
            {
               node = new BufferVariable() 
               {
                  MySource = source, FileLocation = fileVariable.FileLocation,
                  
                  FileVariable = fileVariable,
                  UpArrowToken = upArrowToken
               };
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class CaseConstant : ASTNode
   {
      public Constant Constant { get; private init; } = null!;
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

         Constant? constant = Constant.Parse(source, ref tind);
         if (constant != null)
         {
            node = new CaseConstant() 
            {
               MySource = source, FileLocation = constant.FileLocation,

               Constant = constant
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class CaseConstantList : ASTNode
   {
      public CaseConstant CaseConstant { get; private init; } = null!;
      public IReadOnlyList<(TokenNode commaToken, CaseConstant caseConstant)> SecondaryCaseConstants { get; private init; } = null!;
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

         CaseConstant? caseConstant = CaseConstant.Parse(source, ref tind);
         if (caseConstant != null)
         {
            List<(TokenNode commaToken, CaseConstant caseConstant)> secondaryCaseConstants = new();
            {
               while (true)
               {
                  int pretind = tind;
                  TokenNode tempComma = PopToken(source, ref tind);
                  CaseConstant? tempCaseConstant = CaseConstant.Parse(source, ref tind);
                  if (tempComma.Type == TokenType.Comma && tempCaseConstant != null)
                  { //both exist (add to collection and continue)
                     secondaryCaseConstants.Add((tempComma, tempCaseConstant));
                  }
                  else //in any other situation
                  {
                     tind = pretind;
                     break;
                  }
               }
            }
            node = new CaseConstantList() 
            {
               MySource = source, FileLocation = caseConstant.FileLocation,

               CaseConstant = caseConstant,
               SecondaryCaseConstants = secondaryCaseConstants
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class CaseIndex : ASTNode
   {
      public Expression Expression { get; private init; } = null!;
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

         Expression? expression = Expression.Parse(source, ref tind);
         if (expression != null)
         {
            node = new CaseIndex() 
            {
               MySource = source, FileLocation = expression.FileLocation,
            
               Expression = expression
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class CaseListElement : ASTNode
   {
      public CaseConstantList CaseConstantList { get; private init; } = null!;
      public TokenNode ColonToken { get; private init; } = null!;
      public Statement Statement { get; private init; } = null!;
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

         CaseConstantList? caseConstantList = CaseConstantList.Parse(source, ref tind);
         if (caseConstantList != null)
         {
            TokenNode colonToken = PopToken(source, ref tind);
            if (colonToken.Type == TokenType.Colon)
            {
               Statement? statement = Statement.Parse(source, ref tind);
               if (statement != null)
               {
                  node = new CaseListElement() 
                  {
                     MySource = source, FileLocation = caseConstantList.FileLocation,

                     CaseConstantList = caseConstantList,
                     ColonToken = colonToken,
                     Statement = statement
                  };
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class CaseStatement : ASTNode
   {
      public TokenNode CaseToken { get; private init; } = null!;
      public CaseIndex CaseIndex { get; private init; } = null!;
      public TokenNode OfToken { get; private init; } = null!;
      public CaseListElement CaseListElement { get; private init; } = null!;
      public IReadOnlyList<(TokenNode semicolonToken, CaseListElement caseListElement)> SecondaryCaseListElement { get; private init; } = null!;
      public TokenNode? SemicolonToken { get; private init; } = null!;
      public TokenNode EndToken { get; private init; } = null!;
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

         TokenNode caseToken = PopToken(source, ref tind);
         if (caseToken.Type == TokenType.Case)
         {
            CaseIndex? caseIndex = CaseIndex.Parse(source, ref tind);
            if (caseIndex != null)
            {
               TokenNode ofToken = PopToken(source, ref tind);
               if (ofToken.Type == TokenType.Of)
               {
                  CaseListElement? caseListElement = CaseListElement.Parse(source, ref tind);
                  if (caseListElement != null)
                  {
                     List<(TokenNode semicolonToken, CaseListElement caseListElement)> secondaryCaseListElement = new();
                     {
                        while (true)
                        {
                           int pretind = tind;
                           TokenNode tempSemicolon = PopToken(source, ref tind);
                           CaseListElement? tempCaseListElement = CaseListElement.Parse(source, ref tind);
                           if (tempSemicolon.Type == TokenType.Semicolon && tempCaseListElement != null)
                           { //both exist (add to collection and continue)
                              secondaryCaseListElement.Add((tempSemicolon, tempCaseListElement));
                           }
                           else //in any other situation
                           {
                              tind = pretind;
                              break;
                           }
                        }
                     }
                     int pretind2 = tind;
                     TokenNode? semicolonToken = PopToken(source, ref tind);
                     if (semicolonToken.Type != TokenType.Semicolon)
                     {
                        tind = pretind2;
                        semicolonToken = null;
                     }
                     TokenNode endToken = PopToken(source, ref tind);
                     if (endToken.Type == TokenType.End)
                     {
                        node = new CaseStatement() 
                        {
                           MySource = source, FileLocation = caseToken.FileLocation,

                           CaseToken = caseToken,
                           CaseIndex = caseIndex,
                           OfToken = ofToken,
                           CaseListElement = caseListElement,
                           SecondaryCaseListElement = secondaryCaseListElement,
                           SemicolonToken = semicolonToken,
                           EndToken = endToken
                        };
                     }
                  }
               }
            }
         }

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
      public TypeDenoter TypeDenoter { get; private init; } = null!;
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

         TypeDenoter? typeDenoter = TypeDenoter.Parse(source, ref tind);
         if (typeDenoter != null)
         {
            node = new ComponentType() 
            {
               MySource = source, FileLocation = typeDenoter.FileLocation,
            
               TypeDenoter = typeDenoter
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ComponentVariable : ASTNode
   {
      public IndexedVariable? IndexedVariable { get; private init; } = null!;
      public FieldDesignator? FieldDesignator { get; private init; } = null!;
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

         if (node == null)
         {
            int pretind = tind;
            IndexedVariable? indexedVariable = IndexedVariable.Parse(source, ref tind);
            if (indexedVariable != null)
            {
               node = new ComponentVariable()
               {
                  MySource = source,
                  FileLocation = indexedVariable.FileLocation,

                  IndexedVariable = indexedVariable
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            FieldDesignator? fieldDesignator = FieldDesignator.Parse(source, ref tind);
            if (fieldDesignator != null)
            {
               node = new ComponentVariable()
               {
                  MySource = source,
                  FileLocation = fieldDesignator.FileLocation,

                  FieldDesignator = fieldDesignator
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class CompoundStatement : ASTNode
   {
      public TokenNode BeginToken { get; private init; } = null!;
      public StatementSequence StatementSequence { get; private init; } = null!;
      public TokenNode EndToken { get; private init; } = null!;
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

         TokenNode beginToken = PopToken(source, ref tind);
         if (beginToken.Type == TokenType.Begin)
         {
            StatementSequence? statementSequence = StatementSequence.Parse(source, ref tind);
            if (statementSequence != null)
            {
               TokenNode endToken = PopToken(source, ref tind);
               if (endToken.Type == TokenType.End)
               {
                  node = new CompoundStatement() 
                  {
                     MySource = source, FileLocation = beginToken.FileLocation, 
                     
                     BeginToken = beginToken,
                     StatementSequence = statementSequence,
                     EndToken = endToken
                  };
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ConditionalStatement : ASTNode
   {
      public IfStatement? IfStatement { get; private init; } = null;
      public CaseStatement? CaseStatement { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            IfStatement? ifStatement = IfStatement.Parse(source, ref tind);
            if (ifStatement != null)
            {
               node = new ConditionalStatement()
               {
                  MySource = source,
                  FileLocation = ifStatement.FileLocation,

                  IfStatement = ifStatement
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            CaseStatement? caseStatement = CaseStatement.Parse(source, ref tind);
            if (caseStatement != null)
            {
               node = new ConditionalStatement()
               {
                  MySource = source,
                  FileLocation = caseStatement.FileLocation,

                  CaseStatement = caseStatement
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ConformantArrayParameterSpecification : ASTNode
   {
      public ValueConformantArraySpecification? ValueConformantArraySpecification { get; private init; } = null;
      public VariableConformantArraySpecification? VariableConformantArraySpecification { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            ValueConformantArraySpecification? valueConformantArraySpecification = ValueConformantArraySpecification.Parse(source, ref tind);
            if (valueConformantArraySpecification != null)
            {
               node = new ConformantArrayParameterSpecification()
               {
                  MySource = source,
                  FileLocation = valueConformantArraySpecification.FileLocation,

                  ValueConformantArraySpecification = valueConformantArraySpecification
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            VariableConformantArraySpecification? variableConformantArraySpecification = VariableConformantArraySpecification.Parse(source, ref tind);
            if (variableConformantArraySpecification != null)
            {
               node = new ConformantArrayParameterSpecification()
               {
                  MySource = source,
                  FileLocation = variableConformantArraySpecification.FileLocation,

                  VariableConformantArraySpecification = variableConformantArraySpecification
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ConformantArraySchema : ASTNode
   {
      public PackedConformantArraySchema? PackedConformantArraySchema { get; private init; } = null;
      public UnpackedConformantArraySchema? UnpackedConformantArraySchema { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            PackedConformantArraySchema? packedConformantArraySchema = PackedConformantArraySchema.Parse(source, ref tind);
            if (packedConformantArraySchema != null)
            {
               node = new ConformantArraySchema()
               {
                  MySource = source,
                  FileLocation = packedConformantArraySchema.FileLocation,

                  PackedConformantArraySchema = packedConformantArraySchema
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            UnpackedConformantArraySchema? unpackedConformantArraySchema = UnpackedConformantArraySchema.Parse(source, ref tind);
            if (unpackedConformantArraySchema != null)
            {
               node = new ConformantArraySchema()
               {
                  MySource = source,
                  FileLocation = unpackedConformantArraySchema.FileLocation,

                  UnpackedConformantArraySchema = unpackedConformantArraySchema
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class Constant : ASTNode
   {
      public Sign? Sign { get; private init; } = null;
      public UnsignedNumber? UnsignedNumber { get; private init; } = null;
      public ConstantIdentifier? ConstantIdentifier { get; private init; } = null;
      public TokenNode? CharacterStringToken { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            Sign? sign = Sign.Parse(source, ref tind);
            UnsignedNumber? unsignedNumber = UnsignedNumber.Parse(source, ref tind);
            ConstantIdentifier? constantIdentifier = null;
            if (unsignedNumber == null)
            {
               constantIdentifier = ConstantIdentifier.Parse(source, ref tind);
            }

            ASTNode content = ((ASTNode?)unsignedNumber ?? constantIdentifier)!;
            if (content != null)
            {
               node = new Constant()
               {
                  MySource = source,
                  FileLocation = sign?.FileLocation ?? content.FileLocation,

                  Sign = sign,
                  UnsignedNumber = unsignedNumber,
                  ConstantIdentifier = constantIdentifier
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode characterString = PopToken(source, ref tind);
            if (characterString.Type == TokenType.CharacterString)
            {
               node = new Constant()
               {
                  MySource = source, FileLocation = characterString.FileLocation,

                  CharacterStringToken = characterString
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ConstantDefinition : ASTNode
   {
      public Identifier Identifier { get; private init; } = null!;
      public TokenNode EqualsToken { get; private init; } = null!;
      public Constant Constant { get; private init; } = null!;
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

         Identifier? identifier = Identifier.Parse(source, ref tind);
         if (identifier != null)
         {
            TokenNode equalsToken = PopToken(source, ref tind);
            if (equalsToken.Type == TokenType.Equals)
            {
               Constant? constant = Constant.Parse(source, ref tind);
               if (constant != null)
               {
                  node = new ConstantDefinition() 
                  {
                     MySource = source, FileLocation = identifier.FileLocation,

                     Identifier = identifier,
                     EqualsToken = equalsToken,
                     Constant = constant
                  };
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ConstantDefinitionPart : ASTNode
   {
      public TokenNode? ConstToken { get; private init; } = null;
      public ConstantDefinition? ConstantDefinition { get; private init; } = null;
      public TokenNode? SemicolonToken { get; private init; } = null;
      public IReadOnlyList<(ConstantDefinition constantDefinition, TokenNode semicolonToken)>? SecondaryConstantDefinitions { get; private init; } = null;
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

         TokenNode constToken = PopToken(source, ref tind);
         if (constToken.Type == TokenType.Const)
         {
            ConstantDefinition? constantDefinition = ConstantDefinition.Parse(source, ref tind);
            if (constantDefinition != null)
            {
               TokenNode semicolonToken = PopToken(source, ref tind);
               if (semicolonToken.Type == TokenType.Semicolon)
               {
                  List<(ConstantDefinition constantDefinition, TokenNode semicolonToken)> secondaryConstantDefinitions = new();
                  {
                     while (true)
                     {
                        int pretind = tind;
                        ConstantDefinition? tempConstantDefinition = ConstantDefinition.Parse(source, ref tind);
                        TokenNode tempSemicolon = PopToken(source, ref tind);
                        if (tempConstantDefinition != null && tempSemicolon.Type == TokenType.Semicolon)
                        { //both exist (add to collection and continue)
                           secondaryConstantDefinitions.Add((tempConstantDefinition, tempSemicolon));
                        }
                        else //in any other situation
                        {
                           tind = pretind;
                           break;
                        }
                     }
                  }

                  node = new ConstantDefinitionPart() 
                  {
                     MySource = source, FileLocation = constToken.FileLocation,

                     ConstToken = constToken,
                     ConstantDefinition = constantDefinition,
                     SemicolonToken = semicolonToken,
                     SecondaryConstantDefinitions = secondaryConstantDefinitions
                  };
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         int ttind = index;
         node ??= new ConstantDefinitionPart()
         {
            MySource = source, FileLocation = PopToken(source, ref ttind).FileLocation, //just need the location of the first token, Pop has no lasting effect.
         };
         return node;
      }
   }

      public class ConstantIdentifier : ASTNode
   {
      public Identifier Identifier { get; private init; } = null!;
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

         Identifier? identifier = Identifier.Parse(source, ref tind);
         if (identifier != null)
         {
            node = new ConstantIdentifier() 
            {
               MySource = source, FileLocation = identifier.FileLocation,

               Identifier = identifier
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ControlVariable : ASTNode
   {
      public EntireVariable EntireVariable { get; private init; } = null!;
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

         EntireVariable? entireVariable = EntireVariable.Parse(source, ref tind);
         if (entireVariable != null)
         {
            node = new ControlVariable()
            {
               MySource = source,
               FileLocation = entireVariable.FileLocation,

               EntireVariable = entireVariable
            };
         }

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
      public TokenNode Digit { get; private init; } = null!;
      public IReadOnlyList<TokenNode> SecondaryDigits { get; private init; } = null!;
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

         TokenNode digit = PopToken(source, ref tind);
         if (digit.Type == TokenType.Digit)
         {
            List<TokenNode> secondaryDigits = new();
            {
               while (true)
               {
                  int pretind = tind;
                  TokenNode tempDigit = PopToken(source, ref tind);
                  if (tempDigit.Type == TokenType.Digit)
                  {
                     secondaryDigits.Add(tempDigit);
                  }
                  else //in any other situation
                  {
                     tind = pretind;
                     break;
                  }
               }
            }
            node = new DigitSequence() 
            {
               MySource = source, FileLocation = digit.FileLocation,

               Digit = digit,
               SecondaryDigits = secondaryDigits
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class Directive : ASTNode
   {
      public TokenNode Letter { get; private init; } = null!;
      public IReadOnlyList<TokenNode> SecondaryCharacters { get; private init; } = null!; //should this be a list of (TokenNode? number, TokenNode? letter)?
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

         TokenNode letter = PopToken(source, ref tind);
         if (letter.Type == TokenType.Letter)
         {
            List<TokenNode> secondaryChars = new();
            {
               while (true)
               {
                  int pretind = tind;
                  TokenNode tempChar = PopToken(source, ref tind);
                  if (tempChar.Type == TokenType.Digit ||
                      tempChar.Type == TokenType.Letter)
                  {
                     secondaryChars.Add(tempChar);
                  }
                  else //in any other situation
                  {
                     tind = pretind;
                     break;
                  }
               }
            }
            node = new Directive()
            {
               MySource = source,
               FileLocation = letter.FileLocation,

               Letter = letter,
               SecondaryCharacters = secondaryChars
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class DomainType : ASTNode
   {
      public TypeIdentifier TypeIdentifier { get; private init; } = null!;
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

         TypeIdentifier? typeIdentifier = TypeIdentifier.Parse(source, ref tind);
         if (typeIdentifier != null)
         {
            node = new DomainType()
            {
               MySource = source,
               FileLocation = typeIdentifier.FileLocation,

               TypeIdentifier = typeIdentifier
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ElsePart : ASTNode
   {
      public TokenNode ElseToken { get; private init; } = null!;
      public Statement Statement { get; private init; } = null!;
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

         TokenNode elseToken = PopToken(source, ref tind);
         if (elseToken.Type == TokenType.Else)
         {
            Statement? statement = Statement.Parse(source, ref tind);
            if (statement != null)
            {
               node = new ElsePart() 
               {
                  MySource = source, FileLocation = elseToken.FileLocation,
                  
                  ElseToken = elseToken,
                  Statement = statement
               };
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class EmptyStatement : ASTNode
   {
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

         int ttind = index;
         TokenNode whatever = PopToken(source, ref ttind); //no lasting changes, just need the file location.
         node = new EmptyStatement() { MySource = source, FileLocation = whatever.FileLocation };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class EntireVariable : ASTNode
   {
      public VariableIdentifier VariableIdentifier { get; private init; } = null!;
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

         VariableIdentifier? variableIdentifier = VariableIdentifier.Parse(source, ref tind);
         if (variableIdentifier != null)
         {
            node = new EntireVariable()
            {
               MySource = source,
               FileLocation = variableIdentifier.FileLocation,

               VariableIdentifier = variableIdentifier
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class EnumeratedType : ASTNode
   {
      public TokenNode OpenParenToken { get; private init; } = null!;
      public IdentifierList IdentifierList { get; private init; } = null!;
      public TokenNode CloseParenToken { get; private init; } = null!;
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

         TokenNode openParenToken = PopToken(source, ref tind);
         if (openParenToken.Type == TokenType.OpenParen)
         {
            IdentifierList? identifierList = IdentifierList.Parse(source, ref tind);
            if (identifierList != null)
            {
               TokenNode closeParenToken = PopToken(source, ref tind);
               if (closeParenToken.Type == TokenType.CloseParen)
               {
                  node = new EnumeratedType() 
                  {
                     MySource = source, FileLocation = openParenToken.FileLocation,

                     OpenParenToken = openParenToken,
                     IdentifierList = identifierList,
                     CloseParenToken = closeParenToken
                  };
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class Expression : ASTNode
   {
      public SimpleExpression SimpleExpression { get; private init; } = null!;
      public RelationalOperator? RelationalOperator { get; private init; } = null!;
      public SimpleExpression? SecondSimpleExpression { get; private init; } = null!;
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

         SimpleExpression? simpleExpression = SimpleExpression.Parse(source, ref tind);
         if (simpleExpression != null)
         {
            //either stop here, or add relational operator and simple expression.
            int pretind = tind;
            RelationalOperator? relationalOperator = RelationalOperator.Parse(source, ref tind);
            if (relationalOperator != null)
            {
               SimpleExpression? secondSimpleExpression = SimpleExpression.Parse(source, ref tind);
               if (secondSimpleExpression != null)
               {
                  node = new Expression()
                  {
                     MySource = source,
                     FileLocation = simpleExpression.FileLocation,

                     SimpleExpression = simpleExpression,
                     RelationalOperator = relationalOperator,
                     SecondSimpleExpression = secondSimpleExpression
                  };
               }
            }
            if (node == null)
            {
               tind = pretind;
               node = new Expression() 
               {
                  MySource = source, FileLocation = simpleExpression.FileLocation,

                  SimpleExpression = simpleExpression
               };
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class Factor : ASTNode
   {
      public BoundIdentifier? BoundIdentifier { get; private init; } = null;
      public VariableAccess? VariableAccess { get; private init; } = null;
      public UnsignedConstant? UnsignedConstant { get; private init; } = null;
      public FunctionDesignator? FunctionDesignator { get; private init; } = null;
      public SetConstructor? SetConstructor { get; private init; } = null;
      public TokenNode? OpenParenToken { get; private init; } = null;
      public Expression? Expression { get; private init; } = null;
      public TokenNode? CloseParenToken { get; private init; } = null;
      public TokenNode? NotToken { get; private init; } = null;
      public Factor? NottedFactor { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            BoundIdentifier? boundIdentifier = BoundIdentifier.Parse(source, ref tind);
            if (boundIdentifier != null)
            {
               node = new Factor()
               {
                  MySource = source, FileLocation = boundIdentifier.FileLocation,

                  BoundIdentifier = boundIdentifier
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            VariableAccess? variableAccess = VariableAccess.Parse(source, ref tind);
            if (variableAccess != null)
            {
               node = new Factor()
               {
                  MySource = source,
                  FileLocation = variableAccess.FileLocation,

                  VariableAccess = variableAccess
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            UnsignedConstant? unsignedConstant = UnsignedConstant.Parse(source, ref tind);
            if (unsignedConstant != null)
            {
               node = new Factor()
               {
                  MySource = source,
                  FileLocation = unsignedConstant.FileLocation,

                  UnsignedConstant = unsignedConstant
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            FunctionDesignator? functionDesignator = FunctionDesignator.Parse(source, ref tind);
            if (functionDesignator != null)
            {
               node = new Factor()
               {
                  MySource = source,
                  FileLocation = functionDesignator.FileLocation,

                  FunctionDesignator = functionDesignator
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            SetConstructor? setConstructor = SetConstructor.Parse(source, ref tind);
            if (setConstructor != null)
            {
               node = new Factor()
               {
                  MySource = source,
                  FileLocation = setConstructor.FileLocation,

                  SetConstructor = setConstructor
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode openParenToken = PopToken(source, ref tind);
            if (openParenToken.Type == TokenType.OpenParen)
            {
               Expression? expression = Expression.Parse(source, ref tind);
               if (expression != null)
               {
                  TokenNode closeParenToken = PopToken(source, ref tind);
                  if (closeParenToken.Type == TokenType.CloseParen)
                  {
                     node = new Factor()
                     {
                        MySource = source,
                        FileLocation = openParenToken.FileLocation,

                        OpenParenToken = openParenToken,
                        Expression = expression,
                        CloseParenToken = closeParenToken
                     };
                  }
               }
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode notToken = PopToken(source, ref tind);
            if (notToken.Type == TokenType.Not)
            {
               Factor? nottedFactor = Factor.Parse(source, ref tind);
               if (nottedFactor != null)
               {
                  node = new Factor()
                  {
                     MySource = source,
                     FileLocation = notToken.FileLocation,

                     NotToken = notToken,
                     NottedFactor = nottedFactor
                  };
               }
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FieldDesignator : ASTNode
   {
      public RecordVariable? RecordVariable { get; private init; } = null;
      public TokenNode? DotToken { get; private init; } = null;
      public FieldSpecifier? FieldSpecifier { get; private init; } = null;
      public FieldDesignatorIdentifier? FieldDesignatorIdentifier { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            RecordVariable? recordVariable = RecordVariable.Parse(source, ref tind);
            if (recordVariable != null)
            {
               TokenNode dotToken = PopToken(source, ref tind);
               if (dotToken.Type == TokenType.Dot)
               {
                  FieldSpecifier? fieldSpecifier = FieldSpecifier.Parse(source, ref tind);
                  if (fieldSpecifier != null)
                  {
                     node = new FieldDesignator()
                     {
                        MySource = source, FileLocation = recordVariable.FileLocation,

                        RecordVariable = recordVariable,
                        DotToken = dotToken,
                        FieldSpecifier = fieldSpecifier
                     };
                  }
               }
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            FieldDesignatorIdentifier? fieldDesignatorIdentifier = FieldDesignatorIdentifier.Parse(source, ref tind);
            if (fieldDesignatorIdentifier != null)
            {
               node = new FieldDesignator()
               {
                  MySource = source, FileLocation = fieldDesignatorIdentifier.FileLocation,

                  FieldDesignatorIdentifier = fieldDesignatorIdentifier
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FieldDesignatorIdentifier : ASTNode
   {
      public Identifier Identifier { get; private init; } = null!;
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

         Identifier? identifier = Identifier.Parse(source, ref tind);
         if (identifier != null)
         {
            node = new FieldDesignatorIdentifier() 
            {
               MySource = source, FileLocation = identifier.FileLocation,
            
               Identifier = identifier
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FieldIdentifier : ASTNode
   {
      public Identifier Identifier { get; private init; } = null!;
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

         Identifier? identifier = Identifier.Parse(source, ref tind);
         if (identifier != null)
         {
            node = new FieldIdentifier()
            {
               MySource = source,
               FileLocation = identifier.FileLocation,

               Identifier = identifier
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FieldList : ASTNode
   {
      public FixedPart? FixedPart { get; private init; } = null;
      public TokenNode? Semicolon { get; private init; } = null;
      public VariantPart? VariantPart { get; private init; } = null;
      public TokenNode? TrailingSemicolon { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            FixedPart? fixedPart = FixedPart.Parse(source, ref tind);
            if (fixedPart != null)
            {
               TokenNode semicolon = PopToken(source, ref tind);
               if (semicolon.Type == TokenType.Semicolon)
               {
                  VariantPart? variantPart = VariantPart.Parse(source, ref tind);
                  if (variantPart != null)
                  {
                     TokenNode trailingSemicolon = PopToken(source, ref tind);
                     node = new FieldList()
                     {
                        MySource = source, FileLocation = fixedPart.FileLocation,

                        FixedPart = fixedPart,
                        Semicolon = semicolon,
                        VariantPart = variantPart,
                        TrailingSemicolon = trailingSemicolon.Type == TokenType.Semicolon ? trailingSemicolon : null
                     };
                  }
               }
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            FixedPart? fixedPart = FixedPart.Parse(source, ref tind);
            if (fixedPart != null)
            {
               TokenNode trailingSemicolon = PopToken(source, ref tind);
               node = new FieldList()
               {
                  MySource = source,
                  FileLocation = fixedPart.FileLocation,

                  FixedPart = fixedPart,
                  TrailingSemicolon = trailingSemicolon.Type == TokenType.Semicolon ? trailingSemicolon : null
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            VariantPart? variantPart = VariantPart.Parse(source, ref tind);
            if (variantPart != null)
            {
               TokenNode trailingSemicolon = PopToken(source, ref tind);
               node = new FieldList()
               {
                  MySource = source,
                  FileLocation = variantPart.FileLocation,

                  VariantPart = variantPart,
                  TrailingSemicolon = trailingSemicolon.Type == TokenType.Semicolon ? trailingSemicolon : null
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         int ttind = index;
         node ??= new FieldList()
         {
            MySource = source, FileLocation = PopToken(source, ref ttind).FileLocation
         };
         return node;
      }
   }

      public class FieldSpecifier : ASTNode
   {
      public FieldIdentifier FieldIdentifier { get; private init; } = null!;
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

         FieldIdentifier? fieldIdentifier = FieldIdentifier.Parse(source, ref tind);
         if (fieldIdentifier != null)
         {
            node = new FieldSpecifier() 
            {
               MySource = source, FileLocation = fieldIdentifier.FileLocation,

               FieldIdentifier = fieldIdentifier
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FileType : ASTNode
   {
      public TokenNode FileToken { get; private init; } = null!;
      public TokenNode OfToken { get; private init; } = null!;
      public ComponentType ComponentType { get; private init; } = null!;
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

         TokenNode fileToken = PopToken(source, ref tind), ofToken = PopToken(source, ref tind);
         if (fileToken.Type == TokenType.File &&
             ofToken.Type == TokenType.Of)
         {
            ComponentType? componentType = ComponentType.Parse(source, ref tind);
            if (componentType != null)
            {
               node = new FileType() 
               {
                  MySource = source, FileLocation = fileToken.FileLocation,

                  FileToken = fileToken,
                  OfToken = ofToken,
                  ComponentType = componentType
               };
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FileVariable : ASTNode
   {
      public VariableAccess VariableAccess { get; private init; } = null!;
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

         VariableAccess? variableAccess = VariableAccess.Parse(source, ref tind);
         if (variableAccess != null)
         {
            node = new FileVariable() 
            {
               MySource = source, FileLocation = variableAccess.FileLocation,

               VariableAccess = variableAccess
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FinalValue : ASTNode
   {
      public Expression Expression { get; private init; } = null!;
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

         Expression? expression = Expression.Parse(source, ref tind);
         if (expression != null)
         {
            node = new FinalValue()
            {
               MySource = source,
               FileLocation = expression.FileLocation,

               Expression = expression
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FixedPart : ASTNode
   {
      public RecordSection RecordSection { get; private init; } = null!;
      public IReadOnlyList<(TokenNode semicolonToken, RecordSection recordSection)> SecondaryRecordSections { get; private init; } = null!;
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

         RecordSection? recordSection = RecordSection.Parse(source, ref tind);
         if (recordSection != null)
         {
            List<(TokenNode semicolonToken, RecordSection recordSection)> secondaryRecordSections = new();
            {
               while (true)
               {
                  int pretind = tind;
                  TokenNode tempSemicolon = PopToken(source, ref tind);
                  RecordSection? tempRecordSection = RecordSection.Parse(source, ref tind);
                  if (tempSemicolon.Type == TokenType.Semicolon && tempRecordSection != null)
                  { //both exist (add to collection and continue)
                     secondaryRecordSections.Add((tempSemicolon, tempRecordSection));
                  }
                  else //in any other situation
                  {
                     tind = pretind;
                     break;
                  }
               }
            }
            node = new FixedPart()
            {
               MySource = source, FileLocation = recordSection.FileLocation,

               RecordSection = recordSection,
               SecondaryRecordSections = secondaryRecordSections
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ForStatement : ASTNode
   {
      public TokenNode ForToken { get; private init; } = null!;
      public ControlVariable ControlVariable { get; private init; } = null!;
      public TokenNode WalrusToken { get; private init; } = null!;
      public InitialValue InitialValue { get; private init; } = null!;
      public TokenNode? ToToken { get; private init; } = null;
      public TokenNode? DownToToken { get; private init; } = null;
      public FinalValue FinalValue { get; private init; } = null!;
      public TokenNode DoToken { get; private init; } = null!;
      public Statement Statement { get; private init; } = null!;
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

         TokenNode forToken = PopToken(source, ref tind);
         if (forToken.Type == TokenType.For)
         {
            ControlVariable? controlVariable = ControlVariable.Parse(source, ref tind);
            if (controlVariable != null)
            {
               TokenNode walrusToken = PopToken(source, ref tind);
               if (walrusToken.Type == TokenType.Walrus)
               {
                  InitialValue? initialValue = InitialValue.Parse(source, ref tind);
                  if (initialValue != null)
                  {
                     TokenNode toOrDownToToken = PopToken(source, ref tind);
                     if (toOrDownToToken.Type == TokenType.To ||
                         toOrDownToToken.Type == TokenType.Downto)
                     {
                        FinalValue? finalValue = FinalValue.Parse(source, ref tind);
                        if (finalValue != null)
                        {
                           TokenNode doToken = PopToken(source, ref tind);
                           if (doToken.Type == TokenType.Do)
                           {
                              Statement? statement = Statement.Parse(source, ref tind);
                              if (statement != null)
                              {
                                 node = new ForStatement() 
                                 {
                                    MySource = source, FileLocation = forToken.FileLocation,

                                    ForToken = forToken,
                                    ControlVariable = controlVariable,
                                    WalrusToken = walrusToken,
                                    InitialValue = initialValue,
                                    ToToken = toOrDownToToken.Type == TokenType.To ? toOrDownToToken : null,
                                    DownToToken = toOrDownToToken.Type == TokenType.Downto ? toOrDownToToken : null,
                                    FinalValue = finalValue,
                                    DoToken = doToken,
                                    Statement = statement
                                 };
                              }
                           }
                        }
                     }
                  }
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FormalParameterList : ASTNode
   {
      public TokenNode OpenParenToken { get; private init; } = null!;
      public FormalParameterSection FormalParameterSection { get; private init; } = null!;
      public IReadOnlyList<(TokenNode semicolonToken, FormalParameterSection formalParameterSection)> SecondaryFormalParameterSections { get; private init; } = null!;
      public TokenNode CloseParenToken { get; private init; } = null!;
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

         TokenNode openParenToken = PopToken(source, ref tind);
         if (openParenToken.Type == TokenType.OpenParen)
         {
            FormalParameterSection? formalParameterSection = FormalParameterSection.Parse(source, ref tind);
            if (formalParameterSection != null)
            {
               List<(TokenNode semicolonToken, FormalParameterSection formalParameterSection)> secondaryFormalParameterSections = new();
               {
                  while (true)
                  {
                     int pretind = tind;
                     TokenNode tempSemicolon = PopToken(source, ref tind);
                     FormalParameterSection? tempFormalParameterSection = FormalParameterSection.Parse(source, ref tind);
                     if (tempSemicolon.Type == TokenType.Semicolon && tempFormalParameterSection != null)
                     { //both exist (add to collection and continue)
                        secondaryFormalParameterSections.Add((tempSemicolon, tempFormalParameterSection));
                     }
                     else //in any other situation
                     {
                        tind = pretind;
                        break;
                     }
                  }
               }
               TokenNode closeParenToken = PopToken(source, ref tind);
               if (closeParenToken.Type == TokenType.CloseParen)
               {
                  node = new FormalParameterList()
                  {
                     MySource = source, FileLocation = openParenToken.FileLocation,

                     OpenParenToken = openParenToken,
                     FormalParameterSection = formalParameterSection,
                     SecondaryFormalParameterSections = secondaryFormalParameterSections,
                     CloseParenToken = closeParenToken
                  };
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FormalParameterSection : ASTNode
   {
      public ValueParameterSpecification? ValueParameterSpecification { get; private init; } = null;
      public VariableParameterSpecification? VariableParameterSpecification { get; private init; } = null;
      public ProceduralParameterSpecification? ProceduralParameterSpecification { get; private init; } = null;
      public FunctionalParameterSpecification? FunctionalParameterSpecification { get; private init; } = null;
      public ConformantArrayParameterSpecification? ConformantArrayParameterSpecification { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            ValueParameterSpecification? valueParameterSpecification = ValueParameterSpecification.Parse(source, ref tind);
            if (valueParameterSpecification != null)
            {
               node = new FormalParameterSection()
               {
                  MySource = source, FileLocation = valueParameterSpecification.FileLocation,

                  ValueParameterSpecification = valueParameterSpecification
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            VariableParameterSpecification? variableParameterSpecification = VariableParameterSpecification.Parse(source, ref tind);
            if (variableParameterSpecification != null)
            {
               node = new FormalParameterSection()
               {
                  MySource = source, FileLocation = variableParameterSpecification.FileLocation,

                  VariableParameterSpecification = variableParameterSpecification
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            ProceduralParameterSpecification? proceduralParameterSpecification = ProceduralParameterSpecification.Parse(source, ref tind);
            if (proceduralParameterSpecification != null)
            {
               node = new FormalParameterSection()
               {
                  MySource = source, FileLocation = proceduralParameterSpecification.FileLocation,

                  ProceduralParameterSpecification = proceduralParameterSpecification
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            FunctionalParameterSpecification? functionalParameterSpecification = FunctionalParameterSpecification.Parse(source, ref tind);
            if (functionalParameterSpecification != null)
            {
               node = new FormalParameterSection()
               {
                  MySource = source, FileLocation = functionalParameterSpecification.FileLocation,

                  FunctionalParameterSpecification = functionalParameterSpecification
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            ConformantArrayParameterSpecification? conformantArrayParameterSpecification = ConformantArrayParameterSpecification.Parse(source, ref tind);
            if (conformantArrayParameterSpecification != null)
            {
               node = new FormalParameterSection()
               {
                  MySource = source, FileLocation = conformantArrayParameterSpecification.FileLocation,

                  ConformantArrayParameterSpecification = conformantArrayParameterSpecification
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FractionalPart : ASTNode
   {
      public DigitSequence DigitSequence { get; private init; } = null!;
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

         DigitSequence? digitSequence = DigitSequence.Parse(source, ref tind);
         if (digitSequence != null)
         {
            node = new FractionalPart() 
            {
               MySource = source, FileLocation = digitSequence.FileLocation,

               DigitSequence = digitSequence
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FunctionBlock : ASTNode
   {
      public Block Block { get; private init; } = null!;
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

         Block? block = Block.Parse(source, ref tind);
         if (block != null)
         {
            node = new FunctionBlock() 
            {
               MySource = source, FileLocation = block.FileLocation,

               Block = block
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FunctionDeclaration : ASTNode
   {
      public FunctionHeading? FunctionHeading { get; private init; } = null;
      public Directive? Directive { get; private init; } = null;
      public FunctionIdentification? FunctionIdentification { get; private init; } = null;
      public FunctionBlock? FunctionBlock { get; private init; } = null;
      public TokenNode? SemicolonToken { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            FunctionHeading? functionHeading = FunctionHeading.Parse(source, ref tind);
            if (functionHeading != null)
            {
               TokenNode semicolonToken = PopToken(source, ref tind);
               if (semicolonToken.Type == TokenType.Semicolon)
               {
                  Directive? directive = Directive.Parse(source, ref tind);
                  if (directive != null)
                  {
                     node = new FunctionDeclaration()
                     { 
                        MySource = source, FileLocation = functionHeading.FileLocation,

                        FunctionHeading = functionHeading,
                        SemicolonToken = semicolonToken,
                        Directive = directive
                     };
                  }
               }
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            FunctionIdentification? functionIdentification = FunctionIdentification.Parse(source, ref tind);
            if (functionIdentification != null)
            {
               TokenNode semicolonToken = PopToken(source, ref tind);
               if (semicolonToken.Type == TokenType.Semicolon)
               {
                  FunctionBlock? functionBlock = FunctionBlock.Parse(source, ref tind);
                  if (functionBlock != null)
                  {
                     node = new FunctionDeclaration()
                     {
                        MySource = source, FileLocation = functionIdentification.FileLocation,

                        FunctionIdentification = functionIdentification,
                        SemicolonToken = semicolonToken,
                        FunctionBlock = functionBlock
                     };
                  }
               }
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            FunctionHeading? functionHeading = FunctionHeading.Parse(source, ref tind);
            if (functionHeading != null)
            {
               TokenNode semicolonToken = PopToken(source, ref tind);
               if (semicolonToken.Type == TokenType.Semicolon)
               {
                  FunctionBlock? functionBlock = FunctionBlock.Parse(source, ref tind);
                  if (functionBlock != null)
                  {
                     node = new FunctionDeclaration()
                     {
                        MySource = source, FileLocation = functionHeading.FileLocation,

                        FunctionHeading = functionHeading,
                        SemicolonToken = semicolonToken,
                        FunctionBlock = functionBlock
                     };
                  }
               }
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FunctionDesignator : ASTNode
   {
      public FunctionIdentifier FunctionIdentifier { get; private init; } = null!;
      public ActualParameterList? ActualParameterList { get; private init; } = null;
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

         FunctionIdentifier? functionIdentifier = FunctionIdentifier.Parse(source, ref tind);
         if (functionIdentifier != null)
         {
            ActualParameterList? actualParameterList = ActualParameterList.Parse(source, ref tind); //optional; no null check
            node = new FunctionDesignator() 
            {
               MySource = source, FileLocation = functionIdentifier.FileLocation,

               FunctionIdentifier = functionIdentifier,
               ActualParameterList = actualParameterList
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FunctionHeading : ASTNode
   {
      public TokenNode FunctionToken { get; private init; } = null!;
      public Identifier Identifier { get; private init; } = null!;
      public FormalParameterList? FormalParameterList { get; private init; } = null;
      public TokenNode ColonToken { get; private init; } = null!;
      public ResultType ResultType { get; private init; } = null!;
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

         TokenNode functionToken = PopToken(source, ref tind);
         if (functionToken.Type == TokenType.Function)
         {
            Identifier? identifier = Identifier.Parse(source, ref tind);
            if (identifier != null)
            {
               FormalParameterList? formalParameterList = FormalParameterList.Parse(source, ref tind);
               TokenNode colonToken = PopToken(source, ref tind);
               if (colonToken.Type == TokenType.Colon)
               {
                  ResultType? resultType = ResultType.Parse(source, ref tind);
                  if (resultType != null)
                  {
                     node = new FunctionHeading() 
                     {
                        MySource = source, FileLocation = functionToken.FileLocation,

                        FunctionToken = functionToken,
                        Identifier = identifier,
                        FormalParameterList = formalParameterList,
                        ColonToken = colonToken,
                        ResultType = resultType
                     };
                  }
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FunctionIdentification : ASTNode
   {
      public TokenNode FunctionToken { get; private init; } = null!;
      public FunctionIdentifier FunctionIdentifier { get; private init; } = null!;
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

         TokenNode functionToken = PopToken(source, ref tind);
         if (functionToken.Type == TokenType.Function)
         {
            FunctionIdentifier? functionIdentifier = FunctionIdentifier.Parse(source, ref tind);
            if (functionIdentifier != null)
            {
               node = new FunctionIdentification() 
               {
                  MySource = source, FileLocation = functionToken.FileLocation,
               
                  FunctionToken = functionToken,
                  FunctionIdentifier = functionIdentifier
               };
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FunctionIdentifier : ASTNode
   {
      public Identifier Identifier { get; private init; } = null!;
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

         Identifier? identifier = Identifier.Parse(source, ref tind);
         if (identifier != null)
         {
            node = new FunctionIdentifier() 
            {
               MySource = source, FileLocation = identifier.FileLocation,

               Identifier = identifier
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class FunctionalParameterSpecification : ASTNode
   {
      public FunctionHeading FunctionHeading { get; private init; } = null!;
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

         FunctionHeading? functionHeading = FunctionHeading.Parse(source, ref tind);
         if (functionHeading != null)
         {
            node = new FunctionalParameterSpecification()
            {
               MySource = source, FileLocation = functionHeading.FileLocation,

               FunctionHeading = functionHeading
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class GotoStatement : ASTNode
   {
      public TokenNode GotoToken { get; private init; } = null!;
      public Label Label { get; private init; } = null!;
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

         TokenNode gotoToken = PopToken(source, ref tind);
         if (gotoToken.Type == TokenType.Goto)
         {
            Label? label = Label.Parse(source, ref tind);
            if (label != null)
            {
               node = new GotoStatement() 
               {
                  MySource = source, FileLocation = gotoToken.FileLocation,
               
                  GotoToken = gotoToken,
                  Label = label
               };
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class IdentifiedVariable : ASTNode
   {
      public PointerVariable PointerVariable { get; private init; } = null!;
      public TokenNode UpArrowToken { get; private init; } = null!;
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

         PointerVariable? pointerVariable = PointerVariable.Parse(source, ref tind);
         if (pointerVariable != null)
         {
            TokenNode upArrowToken = PopToken(source, ref tind);
            if (upArrowToken.Type == TokenType.UpArrow)
            {
               node = new IdentifiedVariable() 
               {
                  MySource = source, FileLocation = pointerVariable.FileLocation,

                  PointerVariable = pointerVariable,
                  UpArrowToken = upArrowToken
               };
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class Identifier : ASTNode
   {
      public TokenNode Letter { get; private init; } = null!;
      public IReadOnlyList<TokenNode> SecondaryCharacters { get; private init; } = null!;
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

         TokenNode letter = PopToken(source, ref tind);
         if (letter.Type == TokenType.Letter)
         {
            List<TokenNode> secondaryChars = new();
            {
               while (true)
               {
                  int pretind = tind;
                  TokenNode tempChar = PopToken(source, ref tind);
                  if (tempChar.Type == TokenType.Digit ||
                      tempChar.Type == TokenType.Letter)
                  {
                     secondaryChars.Add(tempChar);
                  }
                  else //in any other situation
                  {
                     tind = pretind;
                     break;
                  }
               }
            }
            node = new Identifier()
            {
               MySource = source, FileLocation = letter.FileLocation,

               Letter = letter,
               SecondaryCharacters = secondaryChars
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class IdentifierList : ASTNode
   {
      public Identifier Identifier { get; private init; } = null!;
      public IReadOnlyList<(TokenNode commaToken, Identifier identifier)> SecondaryIdentifiers { get; private init; } = null!;
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

         Identifier? identifier = Identifier.Parse(source, ref tind);
         if (identifier != null)
         {
            List<(TokenNode commaToken, Identifier identifier)> secondaryIdentifiers = new();
            {
               while (true)
               {
                  int pretind = tind;
                  TokenNode tempComma = PopToken(source, ref tind);
                  Identifier? tempIdentifier = Identifier.Parse(source, ref tind);
                  if (tempComma.Type == TokenType.Comma && tempIdentifier != null)
                  { //both exist (add to collection and continue)
                     secondaryIdentifiers.Add((tempComma, tempIdentifier));
                  }
                  else //in any other situation
                  {
                     tind = pretind;
                     break;
                  }
               }
            }
            node = new IdentifierList()
            {
               MySource = source, FileLocation = identifier.FileLocation,

               Identifier = identifier,
               SecondaryIdentifiers = secondaryIdentifiers
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class IfStatement : ASTNode
   {
      public TokenNode IfToken { get; private init; } = null!;
      public BooleanExpression BooleanExpression { get; private init; } = null!;
      public TokenNode ThenToken { get; private init; } = null!;
      public Statement Statement { get; private init; } = null!;
      public ElsePart? ElsePart { get; private init; } = null!;
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

         TokenNode ifToken = PopToken(source, ref tind);
         if (ifToken.Type == TokenType.If)
         {
            BooleanExpression? booleanExpression = BooleanExpression.Parse(source, ref tind);
            if (booleanExpression != null)
            {
               TokenNode thenToken = PopToken(source, ref tind);
               if (thenToken.Type == TokenType.Then)
               {
                  Statement? statement = Statement.Parse(source, ref tind);
                  if (statement != null)
                  {
                     ElsePart? elsePart = ElsePart.Parse(source, ref tind);
                     node = new IfStatement() 
                     {
                        MySource = source, FileLocation = ifToken.FileLocation,

                        IfToken = ifToken,
                        BooleanExpression = booleanExpression,
                        ThenToken = thenToken,
                        Statement = statement,
                        ElsePart = elsePart
                     };
                  }
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class IndexExpression : ASTNode
   {
      public Expression Expression { get; private init; } = null!;
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

         Expression? expression = Expression.Parse(source, ref tind);
         if (expression != null)
         {
            node = new IndexExpression() 
            {
               MySource = source, FileLocation = expression.FileLocation,

               Expression = expression
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class IndexType : ASTNode
   {
      public OrdinalType OrdinalType { get; private init; } = null!;
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

         OrdinalType? ordinalType = OrdinalType.Parse(source, ref tind);
         if (ordinalType != null)
         {
            node = new IndexType() 
            {
               MySource = source, FileLocation = ordinalType.FileLocation,

               OrdinalType = ordinalType
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class IndexTypeSpecification : ASTNode
   {
      public Identifier FirstIdentifier { get; private init; } = null!;
      public TokenNode DoubleDotToken { get; private init; } = null!;
      public Identifier SecondIdentifier { get; private init; } = null!;
      public TokenNode ColonToken { get; private init; } = null!;
      public OrdinalTypeIdentifier OrdinalTypeIdentifier { get; private init; } = null!;
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

         Identifier? firstIdentifier = Identifier.Parse(source, ref tind);
         if (firstIdentifier != null)
         {
            TokenNode doubleDotToken = PopToken(source, ref tind);
            if (doubleDotToken.Type == TokenType.DoubleDot)
            {
               Identifier? secondIdentifier = Identifier.Parse(source, ref tind);
               if (secondIdentifier != null)
               {
                  TokenNode colonToken = PopToken(source, ref tind);
                  if (colonToken.Type == TokenType.Colon)
                  {
                     OrdinalTypeIdentifier? ordinalTypeIdentifier = OrdinalTypeIdentifier.Parse(source, ref tind);
                     if (ordinalTypeIdentifier != null)
                     {
                        node = new IndexTypeSpecification() 
                        {
                           MySource = source, FileLocation = firstIdentifier.FileLocation,

                           FirstIdentifier = firstIdentifier,
                           DoubleDotToken = doubleDotToken,
                           SecondIdentifier = secondIdentifier,
                           ColonToken = colonToken,
                           OrdinalTypeIdentifier = ordinalTypeIdentifier
                        };
                     }
                  }
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class IndexedVariable : ASTNode
   {
      public ArrayVariable ArrayVariable { get; private init; } = null!;
      public TokenNode LeftSquareBracketToken { get; private init; } = null!;
      public IndexExpression IndexExpression { get; private init; } = null!;
      public IReadOnlyList<(TokenNode commaToken, IndexExpression indexExpression)> SecondaryIndexExpressions { get; private init; } = null!;
      public TokenNode RightSquareBracketToken { get; private init; } = null!;
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

         ArrayVariable? arrayVariable = ArrayVariable.Parse(source, ref tind);
         if (arrayVariable != null)
         {
            TokenNode leftSquareBracketToken = PopToken(source, ref tind);
            if (leftSquareBracketToken.Type == TokenType.LeftSquareBracket)
            {
               IndexExpression? indexExpression = IndexExpression.Parse(source, ref tind);
               if (indexExpression != null)
               {
                  List<(TokenNode commaToken, IndexExpression indexExpressions)> secondaryIndexExpressions = new();
                  {
                     while (true)
                     {
                        int pretind = tind;
                        TokenNode tempComma = PopToken(source, ref tind);
                        IndexExpression? tempIndexExpression = IndexExpression.Parse(source, ref tind);
                        if (tempComma.Type == TokenType.Comma && tempIndexExpression != null)
                        { //both exist (add to collection and continue)
                           secondaryIndexExpressions.Add((tempComma, tempIndexExpression));
                        }
                        else //in any other situation
                        {
                           tind = pretind;
                           break;
                        }
                     }
                  }
                  TokenNode rightSquareBracketToken = PopToken(source, ref tind);
                  if (rightSquareBracketToken.Type == TokenType.RightSquareBracket)
                  {
                     node = new IndexedVariable() 
                     {
                        MySource = source, FileLocation = arrayVariable.FileLocation,

                        ArrayVariable = arrayVariable,
                        LeftSquareBracketToken = leftSquareBracketToken,
                        IndexExpression = indexExpression,
                        SecondaryIndexExpressions = secondaryIndexExpressions,
                        RightSquareBracketToken = rightSquareBracketToken
                     };
                  }
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class InitialValue : ASTNode
   {
      public Expression Expression { get; private init; } = null!;
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

         Expression? expression = Expression.Parse(source, ref tind);
         if (expression != null)
         {
            node = new InitialValue() 
            {
               MySource = source, FileLocation = expression.FileLocation,

               Expression = expression
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class Label : ASTNode
   {
      public DigitSequence DigitSequence { get; private init; } = null!;
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

         DigitSequence? digitSequence = DigitSequence.Parse(source, ref tind);
         if (digitSequence != null)
         {
            node = new Label()
            {
               MySource = source, FileLocation = digitSequence.FileLocation,

               DigitSequence = digitSequence
            };
         }

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
      //In parser: need to assign data members & "FileLocation" & "MySource"
      public TokenNode? LabelToken { get; private set; } = null;
      public Label? Label { get; private set; } = null;
      public IReadOnlyList<(TokenNode commaToken, Label label)>? SecondaryLabels { get; private set; } = null;
      public TokenNode? SemicolonToken { get; private set; } = null;

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
      protected internal override IEnumerable<Token> FlattenedView()
      {
         if (LabelToken != null)
         {
            foreach (Token e in LabelToken!.AllTokens)
               yield return e;
            foreach (Token e in Label!.FlattenedView())
               yield return e;
            foreach (var e in SecondaryLabels!)
            {
               foreach (Token ee in e.commaToken.AllTokens)
                  yield return ee;
               foreach (Token ee in e.label.FlattenedView())
                  yield return ee;
            }
            foreach (Token e in SemicolonToken!.AllTokens)
               yield return e;
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
         int tind = index;
         LabelDeclarationPart? node = null; //matches return type/containing class type
         //grab all contiguous simple tokens
         TokenNode firstLabelWordSymbolTok = PopToken(source, ref tind);
         //if the next item is an ASTNode, then verify your tokens collected so far
         if (firstLabelWordSymbolTok.Type == TokenType.Label)
         {
            //one by one, parse and verify your required ASTNodes, then descend one nested if deeper
            Label? firstLabelValue = Label.Parse(source, ref tind);
            if (firstLabelValue != null)
            {
               List<(TokenNode comma, Label labelValue)> secondaryLabels = new();
               {
                  while (true) //we require both a comma and and a label
                  {
                     int pretind = tind;
                     TokenNode tempComma = PopToken(source, ref tind);
                     Label? tempLabelValue = Label.Parse(source, ref tind);
                     if (tempComma.Type == TokenType.Comma && tempLabelValue != null)
                     { //both exist (add to collection and continue)
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
               TokenNode closingSemicolon = PopToken(source, ref tind);
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
         int ttind = index;
         node ??= new LabelDeclarationPart() //if we failed to create the node, we should still successfully create an effectively empty label-declaration-part.
         {  //keep in mind that the above line will revert changes to the index; an effectively empty lable-declaration-part retroactively consumed no tokens.
            //all the contents will be null by default, since this AST node comprises nothing.
            MySource = source,
            FileLocation = PopToken(source, ref ttind).FileLocation, //even if this token isn't a "label", this node's location still lies here.
         };
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
      public Expression Expression { get; private init; } = null!;
      public TokenNode? DoubleDotToken { get; private init; } = null;
      public Expression? SecondExpression { get; private init; } = null;
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

         Expression? expression = Expression.Parse(source, ref tind);
         if (expression != null)
         {
            int pretind = tind;
            TokenNode doubleDotToken = PopToken(source, ref tind);
            if (doubleDotToken.Type == TokenType.DoubleDot)
            {
               Expression? secondExpression = Expression.Parse(source, ref tind);
               if (secondExpression != null)
               {
                  node = new MemberDesignator() 
                  {
                     MySource = source, FileLocation = expression.FileLocation,

                     Expression = expression,
                     DoubleDotToken = doubleDotToken,
                     SecondExpression = secondExpression
                  };
               }
            }
            tind = node == null ? pretind : tind;
            node ??= new MemberDesignator()
            {
               MySource = source, FileLocation = expression.FileLocation,

               Expression = expression
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class MultiplyingOperator : ASTNode
   {
      public TokenNode? AsteriskToken { get; private init; } = null;
      public TokenNode? ForwardSlashToken { get; private init; } = null;
      public TokenNode? DivToken { get; private init; } = null;
      public TokenNode? ModToken { get; private init; } = null;
      public TokenNode? AndToken { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            TokenNode asteriskToken = PopToken(source, ref tind);
            if (asteriskToken.Type == TokenType.Asterisk)
            {
               node = new MultiplyingOperator()
               {
                  MySource = source, FileLocation = asteriskToken.FileLocation,

                  AsteriskToken = asteriskToken
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode forwardSlashToken = PopToken(source, ref tind);
            if (forwardSlashToken.Type == TokenType.ForwardSlash)
            {
               node = new MultiplyingOperator()
               {
                  MySource = source, FileLocation = forwardSlashToken.FileLocation,

                  ForwardSlashToken = forwardSlashToken
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode divToken = PopToken(source, ref tind);
            if (divToken.Type == TokenType.Div)
            {
               node = new MultiplyingOperator()
               {
                  MySource = source, FileLocation = divToken.FileLocation,

                  DivToken = divToken
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode modToken = PopToken(source, ref tind);
            if (modToken.Type == TokenType.Mod)
            {
               node = new MultiplyingOperator()
               {
                  MySource = source, FileLocation = modToken.FileLocation,

                  ModToken = modToken
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode andToken = PopToken(source, ref tind);
            if (andToken.Type == TokenType.And)
            {
               node = new MultiplyingOperator()
               {
                  MySource = source, FileLocation = andToken.FileLocation,

                  AndToken = andToken
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class NewOrdinalType : ASTNode
   {
      public EnumeratedType? EnumeratedType { get; private init; } = null;
      public SubrangeType? SubrangeType { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            EnumeratedType? enumeratedType = EnumeratedType.Parse(source, ref tind);
            if (enumeratedType != null)
            {
               node = new NewOrdinalType()
               {
                  MySource = source, FileLocation = enumeratedType.FileLocation,

                  EnumeratedType = enumeratedType
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            SubrangeType? subrangeType = SubrangeType.Parse(source, ref tind);
            if (subrangeType != null)
            {
               node = new NewOrdinalType()
               {
                  MySource = source, FileLocation = subrangeType.FileLocation,

                  SubrangeType = subrangeType
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class NewPointerType : ASTNode
   {
      public TokenNode UpArrowToken { get; private init; } = null!;
      public DomainType DomainType { get; private init; } = null!;
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

         TokenNode upArrowToken = PopToken(source, ref tind);
         if (upArrowToken.Type == TokenType.UpArrow)
         {
            DomainType? domainType = DomainType.Parse(source, ref tind);
            if (domainType != null)
            {
               node = new NewPointerType() 
               {
                  MySource = source, FileLocation = upArrowToken.FileLocation,
               
                  UpArrowToken = upArrowToken,
                  DomainType = domainType
               };
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class NewStructuredType : ASTNode
   { //treated like: new-structured-type = 'packed' unpacked-structured-type | unpacked-structured-type .
      public TokenNode? PackedToken { get; private init; } = null;
      public UnpackedStructuredType UnpackedStructuredType { get; private init; } = null!;
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

         if (node == null)
         {
            int pretind = tind;
            TokenNode packedToken = PopToken(source, ref tind);
            if (packedToken.Type == TokenType.Packed)
            {
               UnpackedStructuredType? unpackedStructuredType = UnpackedStructuredType.Parse(source, ref tind);
               if (unpackedStructuredType != null)
               {
                  node = new NewStructuredType()
                  {
                     MySource = source, FileLocation = packedToken.FileLocation,

                     PackedToken = packedToken,
                     UnpackedStructuredType = unpackedStructuredType
                  };
               }
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            UnpackedStructuredType? unpackedStructuredType = UnpackedStructuredType.Parse(source, ref tind);
            if (unpackedStructuredType != null)
            {
               node = new NewStructuredType()
               {
                  MySource = source, FileLocation = unpackedStructuredType.FileLocation,

                  UnpackedStructuredType = unpackedStructuredType
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class NewType : ASTNode
   {
      public NewOrdinalType? NewOrdinalType { get; private init; } = null;
      public NewStructuredType? NewStructuredType { get; private init; } = null;
      public NewPointerType? NewPointerType { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            NewOrdinalType? newOrdinalType = NewOrdinalType.Parse(source, ref tind);
            if (newOrdinalType != null)
            {
               node = new NewType()
               {
                  MySource = source, FileLocation = newOrdinalType.FileLocation,

                  NewOrdinalType = newOrdinalType
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            NewStructuredType? newStructuredType = NewStructuredType.Parse(source, ref tind);
            if (newStructuredType != null)
            {
               node = new NewType()
               {
                  MySource = source, FileLocation = newStructuredType.FileLocation,

                  NewStructuredType = newStructuredType
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            NewPointerType? newPointerType = NewPointerType.Parse(source, ref tind);
            if (newPointerType != null)
            {
               node = new NewType()
               {
                  MySource = source, FileLocation = newPointerType.FileLocation,

                  NewPointerType = newPointerType
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class OrdinalType : ASTNode
   {
      public NewOrdinalType? NewOrdinalType { get; private init; } = null;
      public OrdinalTypeIdentifier? OrdinalTypeIdentifier { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            NewOrdinalType? newOrdinalType = NewOrdinalType.Parse(source, ref tind);
            if (newOrdinalType != null)
            {
               node = new OrdinalType()
               {
                  MySource = source, FileLocation = newOrdinalType.FileLocation,

                  NewOrdinalType = newOrdinalType
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            OrdinalTypeIdentifier? ordinalTypeIdentifier = OrdinalTypeIdentifier.Parse(source, ref tind);
            if (ordinalTypeIdentifier != null)
            {
               node = new OrdinalType()
               {
                  MySource = source, FileLocation = ordinalTypeIdentifier.FileLocation,

                  OrdinalTypeIdentifier = ordinalTypeIdentifier
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class OrdinalTypeIdentifier : ASTNode
   {
      public TypeIdentifier TypeIdentifier { get; private init; } = null!;
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

         TypeIdentifier? typeIdentifier = TypeIdentifier.Parse(source, ref tind);
         if (typeIdentifier != null)
         {
            node = new OrdinalTypeIdentifier() 
            {
               MySource = source, FileLocation = typeIdentifier.FileLocation,

               TypeIdentifier = typeIdentifier
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class PackedConformantArraySchema : ASTNode
   {
      public TokenNode PackedToken { get; private init; } = null!;
      public TokenNode ArrayToken { get; private init; } = null!;
      public TokenNode LeftSquareBracketToken { get; private init; } = null!;
      public IndexTypeSpecification IndexTypeSpecification { get; private init; } = null!;
      public TokenNode RightSquareBracketToken { get; private init; } = null!;
      public TokenNode OfToken { get; private init; } = null!;
      public TypeIdentifier TypeIdentifier { get; private init; } = null!;
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

         TokenNode packedToken = PopToken(source, ref tind);
         if (packedToken.Type == TokenType.Packed)
         {
            TokenNode arrayToken = PopToken(source, ref tind);
            if (arrayToken.Type == TokenType.Array)
            {
               TokenNode leftSquareBracketToken = PopToken(source, ref tind);
               if (leftSquareBracketToken.Type == TokenType.LeftSquareBracket)
               {
                  IndexTypeSpecification? indexTypeSpecification = IndexTypeSpecification.Parse(source, ref tind);
                  if (indexTypeSpecification != null)
                  {
                     TokenNode rightSquareBracketToken = PopToken(source, ref tind);
                     if (rightSquareBracketToken.Type == TokenType.RightSquareBracket)
                     {
                        TokenNode ofToken = PopToken(source, ref tind);
                        if (ofToken.Type == TokenType.Of)
                        {
                           TypeIdentifier? typeIdentifier = TypeIdentifier.Parse(source, ref tind);
                           if (typeIdentifier != null)
                           {
                              node = new PackedConformantArraySchema() 
                              {
                                 MySource = source, FileLocation = packedToken.FileLocation,

                                 PackedToken = packedToken,
                                 ArrayToken = arrayToken,
                                 LeftSquareBracketToken = leftSquareBracketToken,
                                 IndexTypeSpecification = indexTypeSpecification,
                                 RightSquareBracketToken = rightSquareBracketToken,
                                 OfToken = ofToken,
                                 TypeIdentifier = typeIdentifier
                              };
                           }
                        }
                     }
                  }
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class PointerType : ASTNode
   {
      public NewPointerType? NewPointerType { get; private init; } = null;
      public PointerTypeIdentifier? PointerTypeIdentifier { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            NewPointerType? newPointerType = NewPointerType.Parse(source, ref tind);
            if (newPointerType != null)
            {
               node = new PointerType()
               {
                  MySource = source, FileLocation = newPointerType.FileLocation,

                  NewPointerType = newPointerType
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            PointerTypeIdentifier? pointerTypeIdentifier = PointerTypeIdentifier.Parse(source, ref tind);
            if (pointerTypeIdentifier != null)
            {
               node = new PointerType()
               {
                  MySource = source,
                  FileLocation = pointerTypeIdentifier.FileLocation,

                  PointerTypeIdentifier = pointerTypeIdentifier
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class PointerTypeIdentifier : ASTNode
   {
      public TypeIdentifier TypeIdentifier { get; private init; } = null!;
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

         TypeIdentifier? typeIdentifier = TypeIdentifier.Parse(source, ref tind);
         if (typeIdentifier != null)
         {
            node = new PointerTypeIdentifier() 
            {
               MySource = source, FileLocation = typeIdentifier.FileLocation,

               TypeIdentifier = typeIdentifier
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class PointerVariable : ASTNode
   {
      public VariableAccess VariableAccess { get; private init; } = null!;
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

         VariableAccess? variableAccess = VariableAccess.Parse(source, ref tind);
         if (variableAccess != null)
         {
            node = new PointerVariable()
            {
               MySource = source, FileLocation = variableAccess.FileLocation,

               VariableAccess = variableAccess
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ProceduralParameterSpecification : ASTNode
   {
      public ProcedureHeading ProcedureHeading { get; private init; } = null!;
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

         ProcedureHeading? procedureHeading = ProcedureHeading.Parse(source, ref tind);
         if (procedureHeading != null)
         {
            node = new ProceduralParameterSpecification()
            {
               MySource = source, FileLocation = procedureHeading.FileLocation,

               ProcedureHeading = procedureHeading
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ProcedureAndFunctionDeclarationPart : ASTNode
   {
      public IReadOnlyList<(ProcedureDeclaration? procedureDeclaration, FunctionDeclaration? functionDeclaration, TokenNode semicolonToken)> ProcedureAndFunctionDeclarationParts { get; private init; } = null!;
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

         int ttind = tind;
         TokenNode firstLocation = PopToken(source, ref ttind);
         List<(ProcedureDeclaration? procedureDeclaration, FunctionDeclaration? functionDeclaration, TokenNode semicolonToken)> procedureAndFunctionDeclarationParts = new();
         {
            while (true)
            {
               int pretind = tind;
               ProcedureDeclaration? tempProcedureDeclaration = ProcedureDeclaration.Parse(source, ref tind);
               FunctionDeclaration? tempFunctionDeclaration = null;
               if (tempProcedureDeclaration == null)
                  tempFunctionDeclaration = FunctionDeclaration.Parse(source, ref tind);
               TokenNode tempSemicolon = PopToken(source, ref tind);
               if (tempSemicolon.Type == TokenType.Semicolon && (tempFunctionDeclaration != null || tempProcedureDeclaration != null))
               { //both exist (add to collection and continue)
                  procedureAndFunctionDeclarationParts.Add((tempProcedureDeclaration, tempFunctionDeclaration, tempSemicolon));
               }
               else //in any other situation
               {
                  tind = pretind;
                  break;
               }
            }
         }
         node = new ProcedureAndFunctionDeclarationPart() 
         {
            MySource = source, FileLocation = firstLocation.FileLocation,

            ProcedureAndFunctionDeclarationParts = procedureAndFunctionDeclarationParts
         };

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ProcedureBlock : ASTNode
   {
      public Block Block { get; private init; } = null!;
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

         Block? block = Block.Parse(source, ref tind);
         if (block != null)
         {
            node = new ProcedureBlock() 
            {
               MySource = source, FileLocation = block.FileLocation,
            
               Block = block
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ProcedureDeclaration : ASTNode
   {
      public ProcedureHeading? ProcedureHeading { get; private init; } = null;
      public Directive? Directive { get; private init; } = null;
      public ProcedureIdentification? ProcedureIdentification { get; private init; } = null;
      public ProcedureBlock? ProcedureBlock { get; private init; } = null;
      public TokenNode SemicolonToken { get; private init; } = null!;
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

         if (node == null)
         {
            int pretind = tind;
            ProcedureHeading? procedureHeading = ProcedureHeading.Parse(source, ref tind);
            if (procedureHeading != null)
            {
               TokenNode semicolonToken = PopToken(source, ref tind);
               if (semicolonToken.Type == TokenType.Semicolon)
               {
                  Directive? directive = Directive.Parse(source, ref tind);
                  if (directive != null)
                  {
                     node = new ProcedureDeclaration()
                     {
                        MySource = source, FileLocation = procedureHeading.FileLocation,

                        ProcedureHeading = procedureHeading,
                        SemicolonToken = semicolonToken,
                        Directive = directive
                     };
                  }
               }
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            ProcedureIdentification? procedureIdentification = ProcedureIdentification.Parse(source, ref tind);
            if (procedureIdentification != null)
            {
               TokenNode semicolonToken = PopToken(source, ref tind);
               if (semicolonToken.Type == TokenType.Semicolon)
               {
                  ProcedureBlock? procedureBlock = ProcedureBlock.Parse(source, ref tind);
                  if (procedureBlock != null)
                  {
                     node = new ProcedureDeclaration()
                     {
                        MySource = source, FileLocation = procedureIdentification.FileLocation,

                        ProcedureIdentification = procedureIdentification,
                        SemicolonToken = semicolonToken,
                        ProcedureBlock = procedureBlock
                     };
                  }
               }
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            ProcedureHeading? procedureHeading = ProcedureHeading.Parse(source, ref tind);
            if (procedureHeading != null)
            {
               TokenNode semicolonToken = PopToken(source, ref tind);
               if (semicolonToken.Type == TokenType.Semicolon)
               {
                  ProcedureBlock? procedureBlock = ProcedureBlock.Parse(source, ref tind);
                  if (procedureBlock != null)
                  {
                     node = new ProcedureDeclaration()
                     {
                        MySource = source, FileLocation = procedureHeading.FileLocation,

                        ProcedureHeading = procedureHeading,
                        SemicolonToken = semicolonToken,
                        ProcedureBlock = procedureBlock
                     };
                  }
               }
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ProcedureHeading : ASTNode
   {
      public TokenNode ProcedureToken { get; private init; } = null!;
      public Identifier Identifier { get; private init; } = null!;
      public FormalParameterList? FormalParameterList { get; private init; } = null;
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

         TokenNode procedureToken = PopToken(source, ref tind);
         if (procedureToken.Type == TokenType.Procedure)
         {
            Identifier? identifier = Identifier.Parse(source, ref tind);
            if (identifier != null)
            {
               FormalParameterList? formalParameterList = FormalParameterList.Parse(source, ref tind);
               node = new ProcedureHeading() 
               {
                  MySource = source, FileLocation = procedureToken.FileLocation,

                  ProcedureToken = procedureToken,
                  Identifier = identifier,
                  FormalParameterList = formalParameterList
               };
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ProcedureIdentification : ASTNode
   {
      public TokenNode ProcedureToken { get; private init; } = null!;
      public ProcedureIdentifier ProcedureIdentifier { get; private init; } = null!;
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

         TokenNode procedureToken = PopToken(source, ref tind);
         if (procedureToken.Type == TokenType.Procedure)
         {
            ProcedureIdentifier? procedureIdentifier = ProcedureIdentifier.Parse(source, ref tind);
            if (procedureIdentifier != null)
            {
               node = new ProcedureIdentification() 
               {
                  MySource = source, FileLocation = procedureToken.FileLocation,

                  ProcedureToken = procedureToken,
                  ProcedureIdentifier = procedureIdentifier
               };
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ProcedureIdentifier : ASTNode
   {
      public Identifier Identifier { get; private init; } = null!;
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

         Identifier? identifier = Identifier.Parse(source, ref tind);
         if (identifier != null)
         {
            node = new ProcedureIdentifier() 
            {
               MySource = source, FileLocation = identifier.FileLocation,

               Identifier = identifier
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ProcedureStatement : ASTNode
   {
      public ProcedureIdentifier ProcedureIdentifier { get; private init; } = null!;
      public ActualParameterList? ActualParameterList { get; private init; } = null;
      public ReadParameterList? ReadParameterList { get; private init; } = null;
      public ReadlnParameterList? ReadlnParameterList { get; private init; } = null;
      public WriteParameterList? WriteParameterList { get; private init; } = null;
      public WritelnParameterList? WritelnParameterList { get; private init; } = null;
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

         ProcedureIdentifier? procedureIdentifier = ProcedureIdentifier.Parse(source, ref tind);
         if (procedureIdentifier != null)
         {
            if (node == null)
            {
               int pretind = tind;
               ReadParameterList? readParameterList = ReadParameterList.Parse(source, ref tind);
               if (readParameterList != null)
               {
                  node = new ProcedureStatement() 
                  {
                     MySource = source, FileLocation = procedureIdentifier.FileLocation,

                     ProcedureIdentifier = procedureIdentifier,
                     ReadParameterList = readParameterList
                  };
               }
               tind = node == null ? pretind : tind;
            }
            if (node == null)
            {
               int pretind = tind;
               ReadlnParameterList? readlnParameterList = ReadlnParameterList.Parse(source, ref tind);
               if (readlnParameterList != null)
               {
                  node = new ProcedureStatement()
                  {
                     MySource = source, FileLocation = procedureIdentifier.FileLocation,

                     ProcedureIdentifier = procedureIdentifier,
                     ReadlnParameterList = readlnParameterList
                  };
               }
               tind = node == null ? pretind : tind;
            }
            if (node == null)
            {
               int pretind = tind;
               WriteParameterList? writeParameterList = WriteParameterList.Parse(source, ref tind);
               if (writeParameterList != null)
               {
                  node = new ProcedureStatement()
                  {
                     MySource = source, FileLocation = procedureIdentifier.FileLocation,

                     ProcedureIdentifier = procedureIdentifier,
                     WriteParameterList = writeParameterList
                  };
               }
               tind = node == null ? pretind : tind;
            }
            if (node == null)
            {
               int pretind = tind;
               WritelnParameterList? writelnParameterList = WritelnParameterList.Parse(source, ref tind);
               if (writelnParameterList != null)
               {
                  node = new ProcedureStatement()
                  {
                     MySource = source, FileLocation = procedureIdentifier.FileLocation,

                     ProcedureIdentifier = procedureIdentifier,
                     WritelnParameterList = writelnParameterList
                  };
               }
               tind = node == null ? pretind : tind;
            }
            if (node == null)
            { //actual-parameter-list needs to be last, because it's optional (no null check), so this will always pass.
               int pretind = tind;
               ActualParameterList? actualParameterList = ActualParameterList.Parse(source, ref tind);
               node = new ProcedureStatement()
               {
                  MySource = source, FileLocation = procedureIdentifier.FileLocation,

                  ProcedureIdentifier = procedureIdentifier,
                  ActualParameterList = actualParameterList
               };
               tind = node == null ? pretind : tind;
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class Program : ASTNode
   {
      public ProgramHeading ProgramHeading { get; private init; } = null!;
      public TokenNode SemicolonToken { get; private init; } = null!;
      public ProgramBlock ProgramBlock { get; private init; } = null!;
      public TokenNode DotToken { get; private init; } = null!;
      public override string ToString(int indentLevel = 0, bool prettyPrint = true)
      {
         if (prettyPrint)
            return $"{new string('\t', indentLevel)}({NodeType}{Environment.NewLine}" +
                   ProgramHeading != null ? $"{new string('\t', indentLevel + 1)}{ProgramHeading!.ToString(indentLevel + 1, prettyPrint)}{Environment.NewLine}" : string.Empty +
                   SemicolonToken != null ? $"{new string('\t', indentLevel + 1)}{SemicolonToken}{Environment.NewLine}" : string.Empty +
                   ProgramBlock != null ? $"{new string('\t', indentLevel + 1)}{ProgramBlock!.ToString(indentLevel + 1, prettyPrint)}{Environment.NewLine}" : string.Empty +
                   DotToken != null ? $"{new string('\t', indentLevel + 1)}{DotToken}{Environment.NewLine}" : string.Empty +
                   $"{new string('\t', indentLevel)})";
         else
            return $"({NodeType}" +
                   ProgramHeading != null ? $"{ProgramHeading!.ToString(indentLevel + 1, prettyPrint)}" : string.Empty +
                   SemicolonToken != null ? $"{SemicolonToken}" : string.Empty +
                   ProgramBlock != null ? $"{ProgramBlock!.ToString(indentLevel + 1, prettyPrint)}" : string.Empty +
                   DotToken != null ? $"{DotToken}" : string.Empty +
                   $")";
      }
      protected override void AssertCorrectStructure()
      {
         if (ProgramHeading == null ||
             SemicolonToken == null ||
             ProgramBlock == null ||
             DotToken == null)
            throw new InvalidOperationException($"Compiler parse error in {GetType().Name} (compiler bug): invalid object state.");
      }
      public new static Program? Parse(Source source, ref int index)
      {
         int tind = index;
         Program? node = null;

         ProgramHeading? ph = ProgramHeading.Parse(source, ref tind);
         if (ph != null)
         {
            TokenNode semicolon = PopToken(source, ref tind);
            if (semicolon.Type == TokenType.Semicolon)
            {
               ProgramBlock? pb = ProgramBlock.Parse(source, ref tind);
               if (pb != null)
               {
                  TokenNode dot = PopToken(source, ref tind);
                  if (dot.Type == TokenType.Dot)
                  {
                     node = new Program()
                     {
                        MySource = source, FileLocation = ph.FileLocation,

                        ProgramHeading = ph,
                        SemicolonToken = semicolon,
                        ProgramBlock = pb,
                        DotToken = dot
                     };
                  }
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ProgramBlock : ASTNode
   {
      public Block Block { get; private init; } = null!;
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

         Block? block = Block.Parse(source, ref tind);
         if (block != null)
         {
            node = new ProgramBlock() 
            {
               MySource = source, FileLocation = block.FileLocation ,
            
               Block = block
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ProgramHeading : ASTNode
   {
      public TokenNode ProgramToken { get; private init; } = null!;
      public Identifier Identifier { get; private init; } = null!;
      public TokenNode? OpenParenToken { get; private init; } = null;
      public ProgramParameterList? ProgramParameterList { get; private init; } = null;
      public TokenNode? CloseParenToken { get; private init; } = null;
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

         TokenNode programToken = PopToken(source, ref tind);
         if (programToken.Type == TokenType.Program)
         {
            Identifier? identifier = Identifier.Parse(source, ref tind);
            if (identifier != null)
            {
               if (node == null)
               {
                  int pretind = tind;
                  TokenNode openParenToken = PopToken(source, ref tind);
                  if (openParenToken.Type == TokenType.OpenParen)
                  {
                     ProgramParameterList? programParameterList = ProgramParameterList.Parse(source, ref tind);
                     if (programParameterList != null)
                     {
                        TokenNode closeParenToken = PopToken(source, ref tind);
                        if (closeParenToken.Type == TokenType.CloseParen)
                        {
                           node = new ProgramHeading()
                           {
                              MySource = source,
                              FileLocation = programToken.FileLocation,

                              ProgramToken = programToken,
                              Identifier = identifier,
                              OpenParenToken = openParenToken,
                              ProgramParameterList = programParameterList,
                              CloseParenToken = closeParenToken
                           };
                        }
                     }
                  }
                  tind = node == null ? pretind : tind;
               }
               if (node == null)
               {
                  int pretind = tind;
                  node = new ProgramHeading()
                  {
                     MySource = source, FileLocation = programToken.FileLocation,

                     ProgramToken = programToken,
                     Identifier = identifier
                  };
                  tind = node == null ? pretind : tind;
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ProgramParameterList : ASTNode
   {
      public IdentifierList IdentifierList { get; private init; } = null!;
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

         IdentifierList? identifierList = IdentifierList.Parse(source, ref tind);
         if (identifierList != null)
         {
            node = new ProgramParameterList() 
            {
               MySource = source, FileLocation = identifierList.FileLocation,

               IdentifierList = identifierList
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ReadParameterList : ASTNode
   {
      public TokenNode OpenParenToken { get; private init; } = null!;
      public FileVariable? FileVariable { get; private init; } = null;
      public TokenNode? CommaToken { get; private init; } = null;
      public VariableAccess VariableAccess { get; private init; } = null!;
      public IReadOnlyList<(TokenNode commaToken, VariableAccess variableAccess)> SecondaryVariableAccesses { get; private init; } = null!;
      public TokenNode CloseParenToken { get; private init; } = null!;
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

         TokenNode openParenToken = PopToken(source, ref tind);
         if (openParenToken.Type == TokenType.OpenParen)
         {
            FileVariable? fileVariable = null;
            TokenNode? commaToken = null;
            {
               int pretind = tind;
               fileVariable = FileVariable.Parse(source, ref tind);
               if (fileVariable != null)
               {
                  commaToken = PopToken(source, ref tind);
               }
               if (fileVariable == null || commaToken?.Type != TokenType.Comma)
                  tind = pretind;
            }
            VariableAccess? variableAccess = VariableAccess.Parse(source, ref tind);
            if (variableAccess != null)
            {
               List<(TokenNode commaToken, VariableAccess variableAccess)> secondaryVariableAccesses = new();
               {
                  while (true)
                  {
                     int pretind = tind;
                     TokenNode tempComma = PopToken(source, ref tind);
                     VariableAccess? tempVariableAccess = VariableAccess.Parse(source, ref tind);
                     if (tempComma.Type == TokenType.Comma && tempVariableAccess != null)
                     { //both exist (add to collection and continue)
                        secondaryVariableAccesses.Add((tempComma, tempVariableAccess));
                     }
                     else //in any other situation
                     {
                        tind = pretind;
                        break;
                     }
                  }
               }
               TokenNode closeParenToken = PopToken(source, ref tind);
               if (closeParenToken.Type == TokenType.CloseParen)
               {
                  node = new ReadParameterList() 
                  {
                     MySource = source, FileLocation = openParenToken.FileLocation,
               
                     OpenParenToken = openParenToken,
                     FileVariable = fileVariable,
                     CommaToken = commaToken,
                     VariableAccess = variableAccess,
                     SecondaryVariableAccesses = secondaryVariableAccesses,
                     CloseParenToken = closeParenToken
                  };
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ReadlnParameterList : ASTNode
   {
      public TokenNode? OpenParenToken { get; private init; } = null;
      public FileVariable? FileVariable { get; private init; } = null;
      public VariableAccess? VariableAccess { get; private init; } = null;
      public IReadOnlyList<(TokenNode commaToken, VariableAccess variableAccess)>? SecondaryVariableAccesses { get; private init; } = null;
      public TokenNode? CloseParenToken { get; private init; } = null;
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

         TokenNode openParenToken = PopToken(source, ref tind);
         if (openParenToken.Type == TokenType.OpenParen)
         {
            FileVariable? fileVariable = FileVariable.Parse(source, ref tind);
            VariableAccess? variableAccess = null;
            if (fileVariable != null)
               variableAccess = VariableAccess.Parse(source, ref tind);
            List<(TokenNode commaToken, VariableAccess variableAccess)> secondaryVariableAccesses = new();
            {
               while (true)
               {
                  int pretind = tind;
                  TokenNode tempComma = PopToken(source, ref tind);
                  VariableAccess? tempVariableAccess = VariableAccess.Parse(source, ref tind);
                  if (tempComma.Type == TokenType.Comma && tempVariableAccess != null)
                  { //both exist (add to collection and continue)
                     secondaryVariableAccesses.Add((tempComma, tempVariableAccess));
                  }
                  else //in any other situation
                  {
                     tind = pretind;
                     break;
                  }
               }
            }
            TokenNode closeParenToken = PopToken(source, ref tind);
            if (closeParenToken.Type == TokenType.CloseParen)
            {
               node = new ReadlnParameterList()
               {
                  MySource = source, FileLocation = openParenToken.FileLocation,

                  OpenParenToken = openParenToken,
                  FileVariable = fileVariable,
                  VariableAccess = variableAccess,
                  SecondaryVariableAccesses = secondaryVariableAccesses,
                  CloseParenToken = closeParenToken
               };
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class RealTypeIdentifier : ASTNode
   {
      public TypeIdentifier TypeIdentifier { get; private init; } = null!;
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

         TypeIdentifier? typeIdentifier = TypeIdentifier.Parse(source, ref tind);
         if (typeIdentifier != null)
         {
            node = new RealTypeIdentifier() 
            {
               MySource = source, FileLocation = typeIdentifier.FileLocation,

               TypeIdentifier = typeIdentifier
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class RecordSection : ASTNode
   {
      public IdentifierList IdentifierList { get; private init; } = null!;
      public TokenNode ColonToken { get; private init; } = null!;
      public TypeDenoter TypeDenoter { get; private init; } = null!;
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

         IdentifierList? identifierList = IdentifierList.Parse(source, ref tind);
         if (identifierList != null)
         {
            TokenNode colonToken = PopToken(source, ref tind);
            if (colonToken.Type == TokenType.Colon)
            {
               TypeDenoter? typeDenoter = TypeDenoter.Parse(source, ref tind);
               if (typeDenoter != null)
               {
                  node = new RecordSection() 
                  {
                     MySource = source, FileLocation = identifierList.FileLocation,

                     IdentifierList = identifierList,
                     ColonToken = colonToken,
                     TypeDenoter = typeDenoter
                  };
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class RecordType : ASTNode
   {
      public TokenNode RecordToken { get; private init; } = null!;
      public FieldList FieldList { get; private init; } = null!;
      public TokenNode EndToken { get; private init; } = null!;
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

         TokenNode recordToken = PopToken(source, ref tind);
         if (recordToken.Type == TokenType.Record)
         {
            FieldList? fieldList = FieldList.Parse(source, ref tind);
            if (fieldList != null)
            {
               TokenNode endToken = PopToken(source, ref tind);
               if (endToken.Type == TokenType.End)
               {
                  node = new RecordType() 
                  {
                     MySource = source, FileLocation = recordToken.FileLocation,

                     RecordToken = recordToken,
                     FieldList = fieldList,
                     EndToken = endToken
                  };
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class RecordVariable : ASTNode
   {
      public VariableAccess VariableAccess { get; private init; } = null!;
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

         VariableAccess? variableAccess = VariableAccess.Parse(source, ref tind);
         if (variableAccess != null)
         {
            node = new RecordVariable() 
            {
               MySource = source, FileLocation = variableAccess.FileLocation,

               VariableAccess = variableAccess
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class RecordVariableList : ASTNode
   {
      public RecordVariable RecordVariable { get; private init; } = null!;
      public IReadOnlyList<(TokenNode commaToken, RecordVariable recordVariable)> SecondaryRecordVariables { get; private init; } = null!;
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

         RecordVariable? recordVariable = RecordVariable.Parse(source, ref tind);
         if (recordVariable != null)
         {
            List<(TokenNode commaToken, RecordVariable recordVariable)> secondaryRecordVariables = new();
            {
               while (true)
               {
                  int pretind = tind;
                  TokenNode tempComma = PopToken(source, ref tind);
                  RecordVariable? tempRecordVariable = RecordVariable.Parse(source, ref tind);
                  if (tempComma.Type == TokenType.Comma && tempRecordVariable != null)
                  { //both exist (add to collection and continue)
                     secondaryRecordVariables.Add((tempComma, tempRecordVariable));
                  }
                  else //in any other situation
                  {
                     tind = pretind;
                     break;
                  }
               }
            }
            node = new RecordVariableList() 
            {
               MySource = source, FileLocation = recordVariable.FileLocation,

               RecordVariable = recordVariable,
               SecondaryRecordVariables = secondaryRecordVariables
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class RelationalOperator : ASTNode
   {
      public TokenNode? EqualsToken { get; private init; } = null;
      public TokenNode? KetPairToken { get; private init; } = null;
      public TokenNode? LessThanToken { get; private init; } = null;
      public TokenNode? GreaterThanToken { get; private init; } = null;
      public TokenNode? LessThanOrEqualToken { get; private init; } = null;
      public TokenNode? GreaterThanOrEqualToken { get; private init; } = null;
      public TokenNode? InToken { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            TokenNode equalsToken = PopToken(source, ref tind);
            if (equalsToken.Type == TokenType.Equals)
            {
               node = new RelationalOperator() 
               {
                  MySource = source, FileLocation = equalsToken.FileLocation,
               
                  EqualsToken = equalsToken
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode ketPairToken = PopToken(source, ref tind);
            if (ketPairToken.Type == TokenType.KetPair)
            {
               node = new RelationalOperator()
               {
                  MySource = source, FileLocation = ketPairToken.FileLocation,

                  KetPairToken = ketPairToken
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode lessThanToken = PopToken(source, ref tind);
            if (lessThanToken.Type == TokenType.LessThan)
            {
               node = new RelationalOperator()
               {
                  MySource = source,
                  FileLocation = lessThanToken.FileLocation,

                  LessThanToken = lessThanToken
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode greaterThanToken = PopToken(source, ref tind);
            if (greaterThanToken.Type == TokenType.GreaterThan)
            {
               node = new RelationalOperator()
               {
                  MySource = source,
                  FileLocation = greaterThanToken.FileLocation,

                  GreaterThanToken = greaterThanToken
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode lessThanOrEqualToken = PopToken(source, ref tind);
            if (lessThanOrEqualToken.Type == TokenType.LessThanOrEqual)
            {
               node = new RelationalOperator()
               {
                  MySource = source,
                  FileLocation = lessThanOrEqualToken.FileLocation,

                  LessThanOrEqualToken = lessThanOrEqualToken
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode greaterThanOrEqualToken = PopToken(source, ref tind);
            if (greaterThanOrEqualToken.Type == TokenType.GreaterThanOrEqual)
            {
               node = new RelationalOperator()
               {
                  MySource = source,
                  FileLocation = greaterThanOrEqualToken.FileLocation,

                  GreaterThanOrEqualToken = greaterThanOrEqualToken
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode inToken = PopToken(source, ref tind);
            if (inToken.Type == TokenType.In)
            {
               node = new RelationalOperator()
               {
                  MySource = source,
                  FileLocation = inToken.FileLocation,

                  InToken = inToken
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class RepeatStatement : ASTNode
   {
      public TokenNode RepeatToken { get; private init; } = null!;
      public StatementSequence StatementSequence { get; private init; } = null!;
      public TokenNode UntilToken { get; private init; } = null!;
      public BooleanExpression BooleanExpression { get; private init; } = null!;
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

         TokenNode repeatToken = PopToken(source, ref tind);
         if (repeatToken.Type == TokenType.Repeat)
         {
            StatementSequence? statementSequence = StatementSequence.Parse(source, ref tind);
            if (statementSequence != null)
            {
               TokenNode untilToken = PopToken(source, ref tind);
               if (untilToken.Type == TokenType.Until)
               {
                  BooleanExpression? booleanExpression = BooleanExpression.Parse(source, ref tind);
                  if (booleanExpression != null)
                  {
                     node = new RepeatStatement() 
                     {
                        MySource = source, FileLocation = repeatToken.FileLocation,
                     
                        RepeatToken = repeatToken,
                        StatementSequence = statementSequence,
                        UntilToken = untilToken,
                        BooleanExpression = booleanExpression
                     };
                  }
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class RepetitiveStatement : ASTNode
   {
      public RepeatStatement? RepeatStatement { get; private init; } = null;
      public WhileStatement? WhileStatement { get; private init; } = null;
      public ForStatement? ForStatement { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            RepeatStatement? repeatStatement = RepeatStatement.Parse(source, ref tind);
            if (repeatStatement != null)
            {
               node = new RepetitiveStatement() 
               {
                  MySource = source, FileLocation = repeatStatement.FileLocation,

                  RepeatStatement = repeatStatement
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            WhileStatement? whileStatement = WhileStatement.Parse(source, ref tind);
            if (whileStatement != null)
            {
               node = new RepetitiveStatement()
               {
                  MySource = source, FileLocation = whileStatement.FileLocation,

                  WhileStatement = whileStatement
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            ForStatement? forStatement = ForStatement.Parse(source, ref tind);
            if (forStatement != null)
            {
               node = new RepetitiveStatement()
               {
                  MySource = source, FileLocation = forStatement.FileLocation,

                  ForStatement = forStatement
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ResultType : ASTNode
   {
      public SimpleTypeIdentifier? SimpleTypeIdentifier { get; private init; } = null;
      public PointerTypeIdentifier? PointerTypeIdentifier { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            SimpleTypeIdentifier? simpleTypeIdentifier = SimpleTypeIdentifier.Parse(source, ref tind);
            if (simpleTypeIdentifier != null)
            {
               node = new ResultType()
               {
                  MySource = source, FileLocation = simpleTypeIdentifier.FileLocation,

                  SimpleTypeIdentifier = simpleTypeIdentifier
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            PointerTypeIdentifier? pointerTypeIdentifier = PointerTypeIdentifier.Parse(source, ref tind);
            if (pointerTypeIdentifier != null)
            {
               node = new ResultType()
               {
                  MySource = source, FileLocation = pointerTypeIdentifier.FileLocation,

                  PointerTypeIdentifier = pointerTypeIdentifier
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class ScaleFactor : ASTNode
   {
      public Sign? Sign { get; private init; } = null;
      public DigitSequence DigitSequence { get; private init; } = null!;
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

         Sign? sign = Sign.Parse(source, ref tind);
         DigitSequence? digitSequence = DigitSequence.Parse(source, ref tind);
         if (digitSequence != null)
         {
            node = new ScaleFactor() 
            {
               MySource = source, FileLocation = sign?.FileLocation ?? digitSequence.FileLocation,

               Sign = sign, 
               DigitSequence = digitSequence
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class SetConstructor : ASTNode
   {
      public TokenNode LeftSquareBracketToken { get; private init; } = null!;
      public MemberDesignator? MemberDesignator { get; private init; } = null;
      public IReadOnlyList<(TokenNode commaToken, MemberDesignator memberDesignator)>? SecondaryMemberDesignators { get; private init; } = null;
      public TokenNode RightSquareBracketToken { get; private init; } = null!;
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

         if (node == null)
         {
            int ppretind = tind;
            TokenNode leftSquareBracketToken = PopToken(source, ref tind);
            if (leftSquareBracketToken.Type == TokenType.LeftSquareBracket)
            {
               MemberDesignator? memberDesignator = MemberDesignator.Parse(source, ref tind);
               if (memberDesignator != null)
               {
                  List<(TokenNode commaToken, MemberDesignator memberDesignator)> secondaryMemberDesignators = new();
                  {
                     while (true)
                     {
                        int pretind = tind;
                        TokenNode tempComma = PopToken(source, ref tind);
                        MemberDesignator? tempMemberDesignator = MemberDesignator.Parse(source, ref tind);
                        if (tempComma.Type == TokenType.Comma && tempMemberDesignator != null)
                        { //both exist (add to collection and continue)
                           secondaryMemberDesignators.Add((tempComma, tempMemberDesignator));
                        }
                        else //in any other situation
                        {
                           tind = pretind;
                           break;
                        }
                     }
                  }
                  TokenNode rightSquareBracketToken = PopToken(source, ref tind);
                  if (rightSquareBracketToken.Type == TokenType.RightSquareBracket)
                  {
                     node = new SetConstructor() 
                     {
                        MySource = source, FileLocation = leftSquareBracketToken.FileLocation,
               
                        LeftSquareBracketToken = leftSquareBracketToken,
                        MemberDesignator = memberDesignator,
                        SecondaryMemberDesignators = secondaryMemberDesignators,
                        RightSquareBracketToken = rightSquareBracketToken
                     };
                  }
               }
            }
            tind = node == null ? ppretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode leftSquareBracketToken = PopToken(source, ref tind);
            if (leftSquareBracketToken.Type == TokenType.LeftSquareBracket)
            {
               TokenNode rightSquareBracketToken = PopToken(source, ref tind);
               if (rightSquareBracketToken.Type == TokenType.RightSquareBracket)
               {
                  node = new SetConstructor()
                  {
                     MySource = source,
                     FileLocation = leftSquareBracketToken.FileLocation,

                     LeftSquareBracketToken = leftSquareBracketToken,
                     RightSquareBracketToken = rightSquareBracketToken
                  };
               }
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class SetType : ASTNode
   {
      public TokenNode SetToken { get; private init; } = null!;
      public TokenNode OfToken { get; private init; } = null!;
      public BaseType BaseType { get; private init; } = null!;
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

         TokenNode setToken = PopToken(source, ref tind);
         if (setToken.Type == TokenType.Set)
         {
            TokenNode ofToken = PopToken(source, ref tind);
            if (ofToken.Type == TokenType.Of)
            {
               BaseType? baseType = BaseType.Parse(source, ref tind);
               if (baseType != null)
               {
                  node = new SetType() 
                  {
                     MySource = source, FileLocation = setToken.FileLocation,

                     SetToken = setToken,
                     OfToken = ofToken,
                     BaseType = baseType
                  };
               }
            }
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class Sign : ASTNode
   {
      public TokenNode? PlusToken { get; private init; } = null;
      public TokenNode? MinusToken { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            TokenNode plusToken = PopToken(source, ref tind);
            if (plusToken.Type == TokenType.Plus)
            {
               node = new Sign() 
               {
                  MySource = source, FileLocation = plusToken.FileLocation,

                  PlusToken = plusToken
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            TokenNode minusToken = PopToken(source, ref tind);
            if (minusToken.Type == TokenType.Minus)
            {
               node = new Sign()
               {
                  MySource = source, FileLocation = minusToken.FileLocation,

                  MinusToken = minusToken
               };
            }
            tind = node == null ? pretind : tind;
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class SignedInteger : ASTNode
   {
      public Sign? Sign { get; private init; } = null;
      public UnsignedInteger UnsignedInteger { get; private init; } = null!;
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

         Sign? sign = Sign.Parse(source, ref tind);
         UnsignedInteger? unsignedInteger = UnsignedInteger.Parse(source, ref tind);
         if (unsignedInteger != null)
         {
            node = new SignedInteger() 
            {
               MySource = source, FileLocation = sign?.FileLocation ?? unsignedInteger.FileLocation,

               Sign = sign,
               UnsignedInteger = unsignedInteger
            };
         }

         node?.AssertCorrectStructure();
         index = node == null ? index : tind;
         return node;
      }
   }

      public class SignedNumber : ASTNode
   {
      public SignedInteger? SignedInteger { get; private init; } = null;
      public SignedReal? SignedReal { get; private init; } = null;
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

         if (node == null)
         {
            int pretind = tind;
            SignedInteger? signedInteger = SignedInteger.Parse(source, ref tind);
            if (signedInteger != null)
            {
               node = new SignedNumber()
               {
                  MySource = source, FileLocation = signedInteger.FileLocation,

                  SignedInteger = signedInteger
               };
            }
            tind = node == null ? pretind : tind;
         }
         if (node == null)
         {
            int pretind = tind;
            SignedReal? signedReal = SignedReal.Parse(source, ref tind);
            if (signedReal != null)
            {
               node = new SignedNumber()
               {
                  MySource = source, FileLocation = signedReal.FileLocation,

                  SignedReal = signedReal
               };
            }
            tind = node == null ? pretind : tind;
         }

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
}