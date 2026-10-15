//Copyright TheUbMunster, 2024, all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using PascalCompiler.Parser;

namespace PascalCompiler.TypeChecker
{
   /// <summary>
   /// A declaration as the symbol table records it: what an identifier or label denotes at its defining-point (6.2.2), which by 6.2.2.11
   /// is what it denotes at every applied occurrence. The kinds Pascal has are constant (6.3), type (6.4.1), variable (6.5.1, 6.6.3,
   /// 6.10), field (6.4.3.3, 6.8.3.10), procedure and function (6.6.1, 6.6.2, 6.6.3.4, 6.6.3.5, 6.6.5, 6.6.6), bound-identifier
   /// (6.6.3.7) and label (6.2.1). A Scope maps spellings to these per region; the checker also stores the Symbol each identifier
   /// occurrence resolved to on that Identifier node, so later phases need not repeat the lookup.
   /// </summary>
   public abstract class Symbol
   {
      /// <summary>The spelling at the defining-point (compared without regard to case, 6.1.3); for a label its integral value (6.1.6).</summary>
      public string Name { get; }
      /// <summary>The Identifier or Label node of the defining occurrence (6.2.2.8); null for a required identifier (6.2.2.10).</summary>
      public ASTNode? DefiningPoint { get; }
      /// <summary>The region the defining-point is for. Assigned by Scope.TryDeclare; null until then, and for a FieldSymbol reached through a record-variable rather than a with-statement.</summary>
      public Scope? Scope { get; internal set; }
      /// <summary>What the identifier denotes, as a diagnostic would word it: "constant", "variable", "procedure", ...</summary>
      public abstract string KindName { get; }

      protected Symbol(string name, ASTNode? definingPoint)
      {
         Name = name;
         DefiningPoint = definingPoint;
      }

      public override string ToString() => $"{KindName} {Name}";
   }

   /// <summary>A constant-definition (6.3), a constant of an enumerated-type (6.4.2.3), or a required constant: false and true (6.4.2.2) or maxint (6.7.2.2).</summary>
   public class ConstantSymbol : Symbol
   {
      public ConstantValue Value { get; }
      public ResolvedType Type => Value.Type;
      public ConstantSymbol(string name, ASTNode? definingPoint, ConstantValue value) : base(name, definingPoint)
      {
         Value = value;
      }
      public override string KindName => "constant";
      public override string ToString() => $"constant {Name} = {Value}";
   }

   /// <summary>A type-definition (6.4.1) or a required type (6.4.2.2, 6.4.3.5).</summary>
   public class TypeSymbol : Symbol
   {
      public ResolvedType Type { get; }
      public TypeSymbol(string name, ASTNode? definingPoint, ResolvedType type) : base(name, definingPoint)
      {
         Type = type;
      }
      public override string KindName => "type";
      public override string ToString() => string.Equals(Type.Name, Name, StringComparison.OrdinalIgnoreCase) ? $"type {Name}" : $"type {Name} = {Type}";
   }

   public enum VariableKind
   {
      /// <summary>Declared in a variable-declaration-part (6.5.1), or as input or output by the program-parameter-list alone (6.10).</summary>
      Declared,
      /// <summary>A value parameter (6.6.3.2): a variable of the block, initialized from the actual-parameter.</summary>
      ValueParameter,
      /// <summary>A variable parameter (6.6.3.3): denotes the actual-parameter's variable for the whole activation.</summary>
      VariableParameter,
      /// <summary>A value conformant-array parameter (6.6.3.7).</summary>
      ValueConformantArrayParameter,
      /// <summary>A variable conformant-array parameter (6.6.3.7).</summary>
      VariableConformantArrayParameter
   }

   /// <summary>
   /// A variable-identifier: a declared variable (6.5.1), a formal parameter (6.6.3.2, 6.6.3.3, 6.6.3.7) or a program-parameter (6.10).
   /// The kind matters to 6.6.3.3 (what may be passed as a variable parameter), to 6.8.3.9 (a control-variable must be declared in the
   /// variable-declaration-part of the block) and to 6.6.3.7 (a conformant-array parameter has a schema rather than a fixed type).
   /// </summary>
   public class VariableSymbol : Symbol
   {
      public ResolvedType Type { get; }
      public VariableKind Kind { get; }
      /// <summary>The conformant-array-schema of the two conformant kinds, null otherwise. Type is then the array type the schema gives the section's formal parameters (6.6.3.7.1).</summary>
      public ConformantSchema? ConformantSchema { get; }
      /// <summary>Whether the identifier also occurs in the program-parameter-list (6.10), binding the variable to an entity outside the program.</summary>
      public bool IsProgramParameter { get; set; }

      public VariableSymbol(string name, ASTNode? definingPoint, ResolvedType type, VariableKind kind = VariableKind.Declared) : base(name, definingPoint)
      {
         if (kind is VariableKind.ValueConformantArrayParameter or VariableKind.VariableConformantArrayParameter)
            throw new ArgumentException("A conformant-array parameter is declared from its schema.", nameof(kind));
         Type = type;
         Kind = kind;
      }

      public VariableSymbol(string name, ASTNode? definingPoint, ConformantSchema schema, bool isVariableParameter) : base(name, definingPoint)
      {
         ConformantSchema = schema;
         Type = schema.ArrayType;
         Kind = isVariableParameter ? VariableKind.VariableConformantArrayParameter : VariableKind.ValueConformantArrayParameter;
      }

      public bool IsParameter => Kind != VariableKind.Declared;
      /// <summary>Whether this denotes the actual-parameter's own variable (6.6.3.3), of a fixed type or of a conformant array.</summary>
      public bool IsVariableParameter => Kind is VariableKind.VariableParameter or VariableKind.VariableConformantArrayParameter;
      public override string KindName => IsParameter ? "parameter" : "variable";
      public override string ToString() => $"{KindName} {Name}: {Type}";
   }

   /// <summary>
   /// A field-identifier (6.4.3.3) as an identifier occurrence resolves to it: at a field-specifier, where the record-variable's type
   /// supplies the field (6.5.3.3), or as a field-designator-identifier inside a with-statement, whose Scope holds one of these for every
   /// field of the record-variable (6.8.3.10).
   /// </summary>
   public class FieldSymbol : Symbol
   {
      public RecordResolvedType.Field Field { get; }
      public ResolvedType Type => Field.Type;
      /// <summary>For a with-statement's field-designator-identifier, the record-variable whose field this denotes (6.8.3.10); null at a field-specifier, where the field-designator itself names the record-variable.</summary>
      public RecordVariable? RecordVariable { get; }
      public FieldSymbol(RecordResolvedType.Field field, ASTNode? definingPoint, RecordVariable? recordVariable = null) : base(field.Name, definingPoint)
      {
         Field = field;
         RecordVariable = recordVariable;
      }
      public override string KindName => "field";
      public override string ToString() => $"field {Name}: {Type}";
   }

   public enum RoutineKind
   {
      /// <summary>Declared by a procedure-declaration or function-declaration with a block (6.6.1, 6.6.2).</summary>
      Declared,
      /// <summary>A formal procedural or functional parameter (6.6.3.4, 6.6.3.5): a heading without a block, standing for the actual-parameter.</summary>
      FormalParameter,
      /// <summary>A required procedure (6.6.5, 6.9) or function (6.6.6). Its parameters follow rules of their own, so ParameterSections is empty and the checker goes by Required.</summary>
      Required
   }

   /// <summary>The required procedures: file handling (6.6.5.2), dynamic allocation (6.6.5.3), transfer (6.6.5.4), and input and output (6.9).</summary>
   public enum RequiredProcedure { Rewrite, Put, Reset, Get, New, Dispose, Pack, Unpack, Read, Readln, Write, Writeln, Page }

   /// <summary>The required functions: arithmetic (6.6.6.2), transfer (6.6.6.3), ordinal (6.6.6.4) and Boolean (6.6.6.5).</summary>
   public enum RequiredFunction { Abs, Sqr, Sin, Cos, Exp, Ln, Sqrt, Arctan, Trunc, Round, Ord, Chr, Succ, Pred, Odd, Eof, Eoln }

   /// <summary>A procedure-identifier or function-identifier: what the two share is a formal-parameter-list (6.6.3.1) and the forward state of 6.6.1 and 6.6.2.</summary>
   public abstract class RoutineSymbol : Symbol
   {
      public RoutineKind Kind { get; }
      /// <summary>The formal-parameter-sections in order (6.6.3.1); empty when the heading has no formal-parameter-list, and always for a required routine.</summary>
      public IReadOnlyList<ParameterSection> ParameterSections { get; }
      /// <summary>The formal parameters flat and in order, as actual-parameters match them (6.7.3, 6.8.2.3): each a VariableSymbol, ProcedureSymbol or FunctionSymbol.</summary>
      public IEnumerable<Symbol> Parameters => ParameterSections.SelectMany(section => section.Parameters);
      /// <summary>
      /// True from a heading followed by the directive forward until the procedure-identification or function-identification that supplies
      /// the block (6.6.1, 6.6.2). The checker clears it there; a routine still forward at the end of its declaration-part is an error.
      /// </summary>
      public bool IsForward { get; set; }

      protected RoutineSymbol(string name, ASTNode? definingPoint, RoutineKind kind, IEnumerable<ParameterSection> parameterSections) : base(name, definingPoint)
      {
         Kind = kind;
         ParameterSections = parameterSections.ToArray();
      }
   }

   public class ProcedureSymbol : RoutineSymbol
   {
      /// <summary>Which required procedure this is; null unless Kind is Required.</summary>
      public RequiredProcedure? Required { get; }
      public ProcedureSymbol(string name, ASTNode? definingPoint, RoutineKind kind, IEnumerable<ParameterSection> parameterSections) : base(name, definingPoint, kind, parameterSections) { }
      public ProcedureSymbol(RequiredProcedure required) : base(required.ToString().ToLowerInvariant(), null, RoutineKind.Required, Enumerable.Empty<ParameterSection>())
      {
         Required = required;
      }
      public override string KindName => "procedure";
   }

   public class FunctionSymbol : RoutineSymbol
   {
      /// <summary>The result type (6.6.2). Null for abs, sqr, succ and pred, whose result has the type of their argument (6.6.6.2, 6.6.6.4).</summary>
      public ResolvedType? ResultType { get; }
      /// <summary>Which required function this is; null unless Kind is Required.</summary>
      public RequiredFunction? Required { get; }
      /// <summary>Whether the function-block has assigned to the function-identifier yet; 6.6.2 requires at least one such assignment-statement.</summary>
      public bool IsResultAssigned { get; set; }

      public FunctionSymbol(string name, ASTNode? definingPoint, RoutineKind kind, IEnumerable<ParameterSection> parameterSections, ResolvedType resultType) : base(name, definingPoint, kind, parameterSections)
      {
         ResultType = resultType;
      }

      public FunctionSymbol(RequiredFunction required) : base(required.ToString().ToLowerInvariant(), null, RoutineKind.Required, Enumerable.Empty<ParameterSection>())
      {
         Required = required;
         ResultType = required switch
         {
            RequiredFunction.Sin or RequiredFunction.Cos or RequiredFunction.Exp or RequiredFunction.Ln or RequiredFunction.Sqrt or RequiredFunction.Arctan => RealResolvedType.RealRType,
            RequiredFunction.Trunc or RequiredFunction.Round or RequiredFunction.Ord => IntResolvedType.IntRType,
            RequiredFunction.Chr => CharResolvedType.CharRType,
            RequiredFunction.Odd or RequiredFunction.Eof or RequiredFunction.Eoln => BooleanResolvedType.BooleanRType,
            _ => null
         };
      }

      public override string KindName => "function";
      public override string ToString() => ResultType == null ? $"function {Name}" : $"function {Name}: {ResultType}";
   }

   /// <summary>A label of a label-declaration-part (6.2.1): it prefixes exactly one statement of the block, which goto-statements in its scope denote (6.8.2.4).</summary>
   public class LabelSymbol : Symbol
   {
      public int Value { get; }
      /// <summary>The statement the label prefixes; set when the checker meets it. 6.2.1 requires exactly one in the block.</summary>
      public Statement? LabeledStatement { get; set; }
      public LabelSymbol(int value, ASTNode definingPoint) : base(value.ToString(), definingPoint)
      {
         Value = value;
      }
      public override string KindName => "label";
   }

   /// <summary>A bound-identifier of an index-type-specification (6.6.3.7.1): in the block it denotes the smallest or largest index value of the actual conformant array, "neither a constant nor a variable" (note 2).</summary>
   public class BoundIdentifierSymbol : Symbol
   {
      public OrdinalResolvedType Type { get; }
      public bool IsHighBound { get; }
      public BoundIdentifierSymbol(string name, ASTNode? definingPoint, OrdinalResolvedType type, bool isHighBound) : base(name, definingPoint)
      {
         Type = type;
         IsHighBound = isHighBound;
      }
      public override string KindName => "bound identifier";
      public override string ToString() => $"{(IsHighBound ? "high" : "low")} bound {Name}: {Type}";
   }

   public enum ParameterSectionKind { Value, Variable, Procedural, Functional, ValueConformantArray, VariableConformantArray }

   /// <summary>
   /// One formal-parameter-section (6.6.3.1, 6.6.3.7.1) and the symbols it declares, kept together because congruity (6.6.3.6) compares
   /// lists section by section. A value, variable or conformant-array section declares one VariableSymbol per identifier, all of one
   /// type; a procedural or functional section declares a single ProcedureSymbol or FunctionSymbol.
   /// </summary>
   public class ParameterSection
   {
      public ParameterSectionKind Kind { get; }
      public IReadOnlyList<Symbol> Parameters { get; }
      public ParameterSection(ParameterSectionKind kind, IEnumerable<Symbol> parameters)
      {
         Kind = kind;
         Parameters = parameters.ToArray();
      }

      /// <summary>6.6.3.6: two formal-parameter-lists are congruous when they have the same number of sections and corresponding sections match.</summary>
      public static bool Congruous(IReadOnlyList<ParameterSection> first, IReadOnlyList<ParameterSection> second) =>
         first.Count == second.Count && first.Zip(second, (a, b) => a.Matches(b)).All(match => match);

      /// <summary>6.6.3.6 a) to e): the same kind and number of parameters, and the same type, congruous parameter lists (with the same result type for functions), or equivalent schemas.</summary>
      public bool Matches(ParameterSection other)
      {
         if (Kind != other.Kind || Parameters.Count != other.Parameters.Count)
            return false;
         switch (Kind)
         {
            case ParameterSectionKind.Value:
            case ParameterSectionKind.Variable:
               return ReferenceEquals(((VariableSymbol)Parameters[0]).Type, ((VariableSymbol)other.Parameters[0]).Type);
            case ParameterSectionKind.Procedural:
               return Congruous(((RoutineSymbol)Parameters[0]).ParameterSections, ((RoutineSymbol)other.Parameters[0]).ParameterSections);
            case ParameterSectionKind.Functional:
               FunctionSymbol function = (FunctionSymbol)Parameters[0], otherFunction = (FunctionSymbol)other.Parameters[0];
               return Congruous(function.ParameterSections, otherFunction.ParameterSections) && ReferenceEquals(function.ResultType, otherFunction.ResultType);
            default:
               return ConformantSchema.Equivalent(((VariableSymbol)Parameters[0]).ConformantSchema!, ((VariableSymbol)other.Parameters[0]).ConformantSchema!);
         }
      }
   }

   /// <summary>
   /// A conformant-array-schema (6.6.3.7.1) with one index-type-specification; the abbreviated multi-index form is the nesting of these,
   /// one per index, as with array types. The formal parameters of its section share one array type, distinct from every other type, whose
   /// index type is the one the ordinal-type-identifier names and whose component is the fixed-component-type or the component schema's type.
   /// </summary>
   public class ConformantSchema
   {
      private ArrayResolvedType? arrayType;

      public BoundIdentifierSymbol LowBound { get; }
      public BoundIdentifierSymbol HighBound { get; }
      public OrdinalResolvedType IndexType { get; }
      public bool IsPacked { get; }
      /// <summary>The component type-identifier's type; null when the component is itself a schema.</summary>
      public ResolvedType? ComponentType { get; }
      public ConformantSchema? ComponentSchema { get; }

      public ConformantSchema(BoundIdentifierSymbol lowBound, BoundIdentifierSymbol highBound, OrdinalResolvedType indexType, bool isPacked, ResolvedType componentType)
      {
         LowBound = lowBound;
         HighBound = highBound;
         IndexType = indexType;
         IsPacked = isPacked;
         ComponentType = componentType;
      }

      public ConformantSchema(BoundIdentifierSymbol lowBound, BoundIdentifierSymbol highBound, OrdinalResolvedType indexType, bool isPacked, ConformantSchema componentSchema)
      {
         LowBound = lowBound;
         HighBound = highBound;
         IndexType = indexType;
         IsPacked = isPacked;
         ComponentSchema = componentSchema;
      }

      /// <summary>The fixed-component-type (6.6.3.7.1): the type named at the innermost level.</summary>
      public ResolvedType FixedComponentType => ComponentSchema?.FixedComponentType ?? ComponentType!;
      /// <summary>The array type possessed by the section's formal parameters, created once so that it is distinct from every other type (6.6.3.7.1).</summary>
      public ArrayResolvedType ArrayType => arrayType ??= new ArrayResolvedType(IndexType, ComponentSchema?.ArrayType ?? ComponentType!, IsPacked);

      /// <summary>6.6.3.6 e): one index each, the same index type, equivalent component schemas or the same component type, and the same packedness.</summary>
      public static bool Equivalent(ConformantSchema first, ConformantSchema second) =>
         ReferenceEquals(first.IndexType, second.IndexType) && first.IsPacked == second.IsPacked &&
         (first.ComponentSchema != null && second.ComponentSchema != null
            ? Equivalent(first.ComponentSchema, second.ComponentSchema)
            : ReferenceEquals(first.ComponentType, second.ComponentType));

      public override string ToString() => $"{(IsPacked ? "packed " : string.Empty)}array[{LowBound.Name}..{HighBound.Name}: {IndexType}] of {ComponentSchema?.ToString() ?? ComponentType!.ToString()}";
   }
}
