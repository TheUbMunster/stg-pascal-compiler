//Copyright TheUbMunster, 2024, all rights reserved.

namespace PascalCompiler.TypeChecker
{
   /// <summary>
   /// A value known at compile time: an integer, a real, a character, a string, a Boolean, an enumeration constant, or nil. Stored on
   /// ConstantNode by the type checker, because subrange bounds, array bounds and case labels must be evaluated during checking.
   /// To be filled in along with the type checker.
   /// </summary>
   public abstract class ConstantValue
   {
   }
}
