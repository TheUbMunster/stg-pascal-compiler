//Copyright TheUbMunster, 2024, all rights reserved.

using PascalCompiler.TypeChecker;

namespace PascalCompiler.Parser
{
   /// <summary>
   /// A node that has a type: the value nodes of 6.7 (expression, simple-expression, term, factor, function-designator, ...) and the
   /// variable nodes of 6.5 (variable-access, indexed-variable, field-designator, ...). The type checker assigns ResolvedType.
   /// </summary>
   public abstract class TypedNode : ASTNode
   {
      /// <summary>The type of the value or variable this node denotes. Assigned by the type checker; null until it has run.</summary>
      public ResolvedType? ResolvedType { get; set; } = null;
   }

   /// <summary>
   /// A node that denotes a type: the nodes of 6.4 (type-denoter, type-identifier, array-type, subrange-type, ...). The type checker
   /// assigns DenotedType; the member is named differently from TypedNode.ResolvedType because "the type this denotes" and "the type
   /// this has" are different questions and are easily confused in a type checker.
   /// </summary>
   public abstract class TypeDenotingNode : ASTNode
   {
      /// <summary>The type this construct denotes. Assigned by the type checker; null until it has run.</summary>
      public ResolvedType? DenotedType { get; set; } = null;
   }

   /// <summary>
   /// A typed node whose value is known at compile time: constant, case-constant, unsigned-constant, the number nodes and
   /// constant-identifier. Subrange bounds, array bounds and case labels need the value, not just the type.
   /// </summary>
   public abstract class ConstantNode : TypedNode
   {
      /// <summary>The compile-time value. Assigned by the type checker; null until it has run.</summary>
      public ConstantValue? Value { get; set; } = null;
   }
}
