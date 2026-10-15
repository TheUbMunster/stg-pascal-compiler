//Copyright TheUbMunster, 2024, all rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PascalCompiler.TypeChecker
{
   /// <summary>Which kind of region (6.2.2.2) a scope stands for.</summary>
   public enum ScopeKind
   {
      /// <summary>The region enclosing the program in which the required identifiers have their defining-points (6.2.2.10).</summary>
      Required,
      /// <summary>The program-block: its program-parameters (6.10) and the declarations of its block (6.2.1).</summary>
      Program,
      /// <summary>A procedure or function: its formal parameters (6.6.3.1) and the declarations of its block (6.2.1) share the one region.</summary>
      Routine,
      /// <summary>The formal-parameter-list of a procedural or functional parameter, which has no block of its own (6.6.3.1 note 2).</summary>
      FormalParameterList,
      /// <summary>The statement of a with-statement, in which the fields of one record-variable are field-designator-identifiers (6.8.3.10).</summary>
      With
   }

   /// <summary>
   /// The symbol table of one region (6.2.2.2): the identifiers and labels that have a defining-point there, and the enclosing region.
   /// Lookup walks outward, so an inner defining-point hides an outer one of the same spelling (6.2.2.4, 6.2.2.5); a spelling may have
   /// only one defining-point per region (6.2.2.7); spellings compare without regard to case (6.1.3). The fields of a record are not
   /// here but in its RecordResolvedType (6.2.2.6), except as a with-statement brings them into scope.
   /// </summary>
   public class Scope
   {
      private readonly Dictionary<string, Symbol> symbols = new(StringComparer.OrdinalIgnoreCase);
      private readonly List<Symbol> declared = new();

      public Scope? Parent { get; }
      public ScopeKind Kind { get; }
      /// <summary>For a Routine scope, the procedure or function whose formal parameters and block this is; null otherwise. 6.8.2.2 lets the function-identifier be assigned within it.</summary>
      public RoutineSymbol? Routine { get; }
      /// <summary>The symbols declared in this region, in declaration order.</summary>
      public IReadOnlyList<Symbol> Declared => declared;

      public Scope(ScopeKind kind, Scope? parent, RoutineSymbol? routine = null)
      {
         Kind = kind;
         Parent = parent;
         Routine = routine;
      }

      /// <summary>Opens the scope of a region enclosed by this one; leaving it is returning to Parent.</summary>
      public Scope Enter(ScopeKind kind, RoutineSymbol? routine = null) => new Scope(kind, this, routine);

      /// <summary>
      /// The scope of the block closest-containing this region: this scope, or the nearest enclosing Program or Routine scope. The
      /// for-statement needs it, since its control-variable must be declared in that block's variable-declaration-part (6.8.3.9).
      /// </summary>
      public Scope EnclosingBlock
      {
         get
         {
            Scope scope = this;
            while (scope.Kind is not (ScopeKind.Program or ScopeKind.Routine) && scope.Parent != null)
               scope = scope.Parent;
            return scope;
         }
      }

      /// <summary>
      /// Gives the symbol its defining-point in this region and records the region on it. Declares nothing and fails when the spelling
      /// already has a defining-point here (6.2.2.7), handing back that earlier symbol for the error message.
      /// </summary>
      public bool TryDeclare(Symbol symbol, [NotNullWhen(false)] out Symbol? existing)
      {
         if (symbols.TryGetValue(symbol.Name, out existing))
            return false;
         symbols.Add(symbol.Name, symbol);
         declared.Add(symbol);
         symbol.Scope = this;
         return true;
      }

      /// <summary>The symbol with this spelling declared in this region itself, or null.</summary>
      public Symbol? LookupLocal(string spelling) => symbols.TryGetValue(spelling, out Symbol? symbol) ? symbol : null;

      /// <summary>The symbol an applied occurrence of this spelling denotes here: the innermost defining-point in this or an enclosing region (6.2.2.4, 6.2.2.5), or null when there is none.</summary>
      public Symbol? Lookup(string spelling)
      {
         for (Scope? scope = this; scope != null; scope = scope.Parent)
            if (scope.symbols.TryGetValue(spelling, out Symbol? symbol))
               return symbol;
         return null;
      }

      /// <summary>
      /// The region enclosing the program (6.2.2.10), holding the required constants (6.4.2.2, 6.7.2.2), types (6.4.2.2, 6.4.3.5),
      /// procedures (6.6.5, 6.9) and functions (6.6.6). input and output are not among them: they are variables, declared only by a
      /// program-parameter-list (6.10).
      /// </summary>
      public static Scope CreateRequired()
      {
         Scope scope = new Scope(ScopeKind.Required, null);
         void Declare(Symbol symbol)
         {
            if (!scope.TryDeclare(symbol, out _))
               throw new InvalidOperationException($"required identifier {symbol.Name} declared twice");
         }
         Declare(new ConstantSymbol("false", null, new OrdinalConstantValue(BooleanResolvedType.BooleanRType, 0)));
         Declare(new ConstantSymbol("true", null, new OrdinalConstantValue(BooleanResolvedType.BooleanRType, 1)));
         Declare(new ConstantSymbol("maxint", null, new OrdinalConstantValue(IntResolvedType.IntRType, IntResolvedType.MaxInt)));
         foreach (ResolvedType type in new ResolvedType[] { IntResolvedType.IntRType, RealResolvedType.RealRType, BooleanResolvedType.BooleanRType, CharResolvedType.CharRType, TextResolvedType.TextRType })
            Declare(new TypeSymbol(type.Name!, null, type));
         foreach (RequiredProcedure procedure in Enum.GetValues<RequiredProcedure>())
            Declare(new ProcedureSymbol(procedure));
         foreach (RequiredFunction function in Enum.GetValues<RequiredFunction>())
            Declare(new FunctionSymbol(function));
         return scope;
      }
   }
}
