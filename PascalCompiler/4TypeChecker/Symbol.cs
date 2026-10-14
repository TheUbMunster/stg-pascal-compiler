//Copyright TheUbMunster, 2024, all rights reserved.

namespace PascalCompiler.TypeChecker
{
   /// <summary>
   /// A declaration as the symbol table records it: what an identifier denotes at its defining-point (6.2.2). The kinds Pascal needs
   /// are constant (with its value), type, variable (with its defining block and parameter kind), record field, procedure and
   /// function (with their parameters, result type and forward state), bound identifier and label; the required identifiers of
   /// 6.2.2.10 are entered in a scope enclosing the program.
   ///
   /// The symbol table maps names to these per scope while the type checker runs; the checker also stores the Symbol each identifier
   /// occurrence resolved to on that Identifier node, so that later phases need not repeat the scoped lookup. To be filled in along
   /// with the type checker.
   /// </summary>
   public abstract class Symbol
   {
   }
}
