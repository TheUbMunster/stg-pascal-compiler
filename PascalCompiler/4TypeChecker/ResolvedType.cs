//Copyright TheUbMunster, 2024, all rights reserved.

namespace PascalCompiler.TypeChecker
{
   /// <summary>
   /// A type as the type checker understands it: the semantic counterpart of a type-denoter in the source (integer, a particular
   /// record type, a subrange, a set of some ordinal type, ...). Stored on the parse tree by the type checker (see TypedNode and
   /// TypeDenotingNode). The hierarchy is to be filled in along with the type checker; 6.4 of the spec describes the kinds, and
   /// 6.4.5/6.4.6 the compatibility and assignment-compatibility relations between them.
   ///
   /// Identity is by reference: ISO Pascal uses name equivalence (6.4.1, "each occurrence of a new-type shall denote a type that is
   /// distinct from any other new-type"), and a type-definition makes its identifier denote the same type object as its denoter.
   /// </summary>
   public abstract class ResolvedType
   {
   }
}
