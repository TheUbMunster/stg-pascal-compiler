//Copyright TheUbMunster, 2024, all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler.Parser
{
   public class ParserException : Exception
   {
      public ParserException(string? message) : base(message) { }
      public ParserException(string? message, Exception? innerException) : base(message, innerException) { }
   }
   public enum ASTNodeType : int
   {
      UNDEFINED = int.MinValue,
      ActualParameter = 0,
      ActualParameterList,
      AddingOperator,
      ApostropheImage,
      ArrayType,
      ArrayVariable,
      AssignmentStatement,
      BaseType,
      Block,
      BooleanExpression,
      BoundIdentifier,
      BufferVariable,
      CaseConstant,
      CaseConstantList,
      CaseIndex,
      CaseListElement,
      CaseStatement,
      CharacterString,
      ComponentType,
      ComponentVariable,
      CompoundStatement,
      ConditionalStatement,
      ConformantArrayParameterSpecification,
      ConformantArraySchema,
      Constant,
      ConstantDefinition,
      ConstantDefinitionPart,
      ConstantIdentifier,
      ControlVariable,
      Digit,
      DigitSequence,
      Directive,
      DomainType,
      ElsePart,
      EmptyStatement,
      EntireVariable,
      EnumeratedType,
      Expression,
      Factor,
      FieldDesignator,
      FieldDesignatorIdentifier,
      FieldIdentifier,
      FieldList,
      FieldSpecifier,
      FileType,
      FileVariable,
      FinalValue,
      FixedPart,
      ForStatement,
      FormalParameterList,
      FormalParameterSection,
      FractionalPart,
      FunctionBlock,
      FunctionDeclaration,
      FunctionDesignator,
      FunctionHeading,
      FunctionIdentification,
      FunctionIdentifier,
      FunctionalParameterSpecification,
      GotoStatement,
      IdentifiedVariable,
      Identifier,
      IdentifierList,
      IfStatement,
      IndexExpression,
      IndexType,
      IndexTypeSpecification,
      IndexedVariable,
      InitialValue,
      Label,
      LabelDeclarationPart,
      Letter,
      MemberDesignator,
      MultiplyingOperator,
      NewOrdinalType,
      NewPointerType,
      NewStructuredType,
      NewType,
      OrdinalType,
      OrdinalTypeIdentifier,
      PackedConformantArraySchema,
      PointerType,
      PointerTypeIdentifier,
      PointerVariable,
      ProceduralParameterSpecification,
      ProcedureAndFunctionDeclarationPart,
      ProcedureBlock,
      ProcedureDeclaration,
      ProcedureHeading,
      ProcedureIdentification,
      ProcedureIdentifier,
      ProcedureStatement,
      Program,
      ProgramBlock,
      ProgramHeading,
      ProgramParameterList,
      ReadParameterList,
      ReadlnParameterList,
      RealTypeIdentifier,
      RecordSection,
      RecordType,
      RecordVariable,
      RecordVariableList,
      RelationalOperator,
      RepeatStatement,
      RepetitiveStatement,
      ResultType,
      ScaleFactor,
      SetConstructor,
      SetType,
      Sign,
      SignedInteger,
      SignedNumber,
      SignedReal,
      SimpleExpression,
      SimpleStatement,
      SimpleType,
      SimpleTypeIdentifier,
      SpecialSymbol,
      Statement,
      StatementPart,
      StatementSequence,
      StringCharacter,
      StringElement,
      StructuredStatement,
      StructuredType,
      StructuredTypeIdentifier,
      SubrangeType,
      TagField,
      TagType,
      Term,
      TypeDefinition,
      TypeDefinitionPart,
      TypeDenoter,
      TypeIdentifier,
      UnpackedConformantArraySchema,
      UnpackedStructuredType,
      UnsignedConstant,
      UnsignedInteger,
      UnsignedNumber,
      UnsignedReal,
      ValueConformantArraySpecification,
      ValueParameterSpecification,
      VariableAccess,
      VariableConformantArraySpecification,
      VariableDeclaration,
      VariableDeclarationPart,
      VariableIdentifier,
      VariableParameterSpecification,
      Variant,
      VariantPart,
      VariantSelector,
      WhileStatement,
      WithStatement,
      WordSymbol,
      WriteParameter,
      WriteParameterList,
      WritelnParameterList,
   }
   /// <summary>
   /// Declares that exactly one of the named members of the node is set: the node's rule is an alternation (e.g. type-denoter =
   /// type-identifier | new-type). Checked by ASTNode.AssertCorrectStructure, together with the members' nullability annotations.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
   public sealed class OneOfAttribute : Attribute
   {
      public IReadOnlyList<string> Members { get; }
      public OneOfAttribute(params string[] members) { Members = members; }
   }

   /// <summary>
   /// Declares that the named members are either all set or all unset: they form an optional group of the node's rule
   /// (e.g. the [ 'label' label { ',' label } ';' ] of label-declaration-part). Checked by ASTNode.AssertCorrectStructure.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
   public sealed class TogetherAttribute : Attribute
   {
      public IReadOnlyList<string> Members { get; }
      public TogetherAttribute(params string[] members) { Members = members; }
   }
}
