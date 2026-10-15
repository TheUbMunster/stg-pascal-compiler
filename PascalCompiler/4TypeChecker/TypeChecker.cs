//Copyright TheUbMunster, 2024, all rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PascalCompiler.Parser;
using PascalCompiler.Scanner;
using static PascalCompiler.TypeChecker.ResolvedType;

namespace PascalCompiler.TypeChecker
{
    /// <summary>
    /// The type checker: one walk over the parse tree in source order that resolves every identifier against the Scope it is in, gives
    /// every TypedNode its ResolvedType, every TypeDenotingNode its DenotedType and every ConstantNode its Value, and reports each
    /// violation of clause 6 as a Typecheck message. Walking in source order is what makes 6.2.2.9 (declare before use) hold on its own:
    /// when an applied occurrence is reached, its defining-point, if it has one, has already been declared.
    ///
    /// The methods follow the clauses of the spec, one per construct that has rules of its own; a wrapper node without rules (Statement
    /// over SimpleStatement, UnsignedNumber over UnsignedInteger, ...) is handled inside its parent's method. Where the parser packed
    /// several readings into an X.Ambiguous placeholder, the method that reaches it decides by lookup which reading is meant and assigns
    /// Resolved; Check runs Ambiguity.Collapse afterwards so that nothing downstream ever sees a placeholder.
    /// </summary>
    public class TypeChecker
    {
        private readonly Source source;
        /// <summary>The region the walk is currently in. Enter and Parent move it as blocks, formal-parameter-lists and with-statements open and close.</summary>
        private Scope scope;
        /// <summary>6.2.2.9: the new-pointer-types of the type-definition-part being checked whose domain type-identifier is defined later in it; null outside one.</summary>
        private List<(PointerResolvedType pointer, TypeIdentifier domain)>? forwardDomains;
        /// <summary>6.2.2.9: the identifiers the type-definition-part being checked defines, so that a pointer domain naming one of them waits for it instead of finding an outer type.</summary>
        private HashSet<string>? typeDefinitionNames;
        /// <summary>6.8.3.9: the control-variables of the for-statements whose bodies are being checked; a statement threatening one is an error.</summary>
        private readonly HashSet<VariableSymbol> activeControlVariables = new();
        /// <summary>6.8.3.9: every statement that threatened a declared variable, with the region it was in, for the check against the block's routines.</summary>
        private readonly List<(VariableSymbol variable, ASTNode at, Scope scope)> threats = new();
        /// <summary>6.8.1: the statements and statement-sequences enclosing the one being checked, which decide where a goto may lead.</summary>
        private readonly Stack<Statement> enclosingStatements = new();
        private readonly Stack<StatementSequence> enclosingSequences = new();
        /// <summary>6.8.1 c): the statement-sequence of each block's statement-part, by the block's scope.</summary>
        private readonly Dictionary<Scope, StatementSequence> blockSequences = new();
        /// <summary>6.8.1: goto-statements whose placement is checked once the labeled statement is known, in the sweep of the block that declares the label.</summary>
        private readonly List<(LabelSymbol label, GotoStatement at, Statement[] statements, StatementSequence[] sequences)> pendingGotos = new();
        /// <summary>6.9, 6.6.6.5: the implied textfiles (input, output) already reported as missing from the program-parameter-list; each is reported once.</summary>
        private readonly HashSet<string> missingImpliedFiles = new(StringComparer.OrdinalIgnoreCase);

        private TypeChecker(Source source)
        {
            this.source = source;
            scope = Scope.CreateRequired();
        }

        /// <summary>Type checks each parser node of the source (one Program) and, if no error was reported, collapses the ambiguity placeholders.</summary>
        public static void Check(Source source)
        {
            foreach (ASTNode node in source.ParserNodes)
            {
                new TypeChecker(source).CheckProgram((Parser.Program)node);
                if (!source.ErrorHasOccurred)
                    Ambiguity.Collapse(node);
            }
        }

        #region Messages
        private void Error(ASTNode at, string message) =>
           source.AppendMessage(new Source.Message(Source.CompilerPhase.Typecheck, Source.Severity.Error, message, at.FileLocation, true));
        private void Warning(ASTNode at, string message) =>
           source.AppendMessage(new Source.Message(Source.CompilerPhase.Typecheck, Source.Severity.Warning, message, at.FileLocation, false));
        private void Error(TokenNode at, string message) =>
           source.AppendMessage(new Source.Message(Source.CompilerPhase.Typecheck, Source.Severity.Error, message, at.FileLocation, true));
        /// <summary>
        /// Gives the symbol its defining-point in the current region and records it on the defining occurrence (6.2.2.8); a spelling that already has a
        /// defining-point there is reported (6.2.2.7) and nothing is declared. Returns whether it was declared.
        /// </summary>
        private bool Declare(Symbol symbol, Identifier identifier)
        {
            if (scope.TryDeclare(symbol, out Symbol? existing))
            {
                identifier.Symbol = symbol;
                return true;
            }
            Error(identifier, $"'{identifier.Name}' is already declared in this scope as a {existing.KindName} (6.2.2.7).");
            return false;
        }
        /// <summary>The same for a label, which is a digit-sequence rather than an identifier and has a Symbol slot of its own.</summary>
        private bool Declare(LabelSymbol symbol, Label label)
        {
            if (scope.TryDeclare(symbol, out _))
            {
                label.Symbol = symbol;
                return true;
            }
            Error(label, $"Label {symbol.Name} is declared more than once in this block (6.2.2.7).");
            return false;
        }
        /// <summary>6.2.2.8: the symbol an applied occurrence denotes, recorded on it; null and an error when the identifier has no defining-point in scope (6.2.2.1).</summary>
        private Symbol? Lookup(Identifier identifier)
        {
            Symbol? symbol = scope.Lookup(identifier.Name);
            if (symbol == null)
                Error(identifier, $"'{identifier.Name}' is not declared here (6.2.2.1, 6.2.2.9).");
            else
                identifier.Symbol = symbol;
            return symbol;
        }
        /// <summary>Lookup, and the symbol shall be of the given kind, named as the message words it ("a type", "a variable"); null and an error otherwise.</summary>
        private T? Resolve<T>(Identifier identifier, string role) where T : Symbol
        {
            Symbol? symbol = Lookup(identifier);
            if (symbol == null)
                return null;
            if (symbol is T wanted)
                return wanted;
            Error(identifier, $"'{identifier.Name}' denotes a {symbol.KindName}, but {role} is required here.");
            return null;
        }
        #endregion

        #region Type predicates
        //6.7.1: a factor whose type is a subrange of T is treated as being of type T, so these look through subranges to the host
        private static bool IsInteger(ResolvedType type) => type is OrdinalResolvedType { Host: IntResolvedType };
        private static bool IsBoolean(ResolvedType type) => type is OrdinalResolvedType { Host: BooleanResolvedType };
        private static bool IsChar(ResolvedType type) => type is OrdinalResolvedType { Host: CharResolvedType };
        private static bool IsNumeric(ResolvedType type) => IsInteger(type) || type is RealResolvedType;
        private static bool IsSet(ResolvedType type) => type is SetResolvedType or EmptySetResolvedType;
        private static bool IsString(ResolvedType type) => type is ArrayResolvedType { IsStringType: true };
        /// <summary>
        /// 6.7.1: the canonical set type a set operator's result has, given two compatible set operands: the canonical set of the base's host,
        /// packed as whichever operand has a packedness of its own, or left to the context when neither does. [] with [] stays [].
        /// </summary>
        private static ResolvedType CanonicalSetOf(ResolvedType left, ResolvedType right)
        {
            SetResolvedType? a = left as SetResolvedType, b = right as SetResolvedType;
            SetResolvedType? representative = a ?? b;
            if (representative == null)
                return EmptySetResolvedType.EmptySetRType;
            bool? packed = a is { PackednessFromContext: false } ? a.IsPacked : b is { PackednessFromContext: false } ? b.IsPacked : null;
            return representative.Base.CanonicalSet(packed);
        }
        #endregion

        #region Node helpers
        /// <summary>The identifiers of an identifier-list in source order.</summary>
        private static IEnumerable<Identifier> Identifiers(IdentifierList list) => list.SecondaryIdentifiers.Select(x => x.identifier).Prepend(list.Identifier);
        private static IEnumerable<Statement> Statements(StatementSequence sequence) => sequence.SecondaryStatements.Select(x => x.statement).Prepend(sequence.Statement);
        private static List<ActualParameter> Actuals(ActualParameterList? list) =>
            list == null ? new() : list.SecondaryActualParameters.Select(x => x.actualParameter).Prepend(list.ActualParameter).ToList();
        private static string Spelling(Directive directive) => directive.Letter.Token.Content + string.Concat(directive.SecondaryCharacters.Select(x => x.Token.Content));
        private static bool IsWithin(Scope inner, Scope outer)
        {
            for (Scope? s = inner; s != null; s = s.Parent)
                if (s == outer)
                    return true;
            return false;
        }
        /// <summary>
        /// The reading of an actual-parameter the formal asks for: a bare identifier is packed as every reading it could be (6.7.3), and the first candidate with the
        /// wanted slot set becomes Resolved. Null when the actual cannot be read that way, which the caller reports.
        /// </summary>
        private static T? Reading<T>(ActualParameter actual, Func<ActualParameter, T?> slot) where T : ASTNode
        {
            if (actual is ActualParameter.Ambiguous ambiguous)
            {
                ActualParameter? candidate = ambiguous.Candidates.FirstOrDefault(c => slot(c) != null);
                if (candidate == null)
                    return null;
                ambiguous.Resolved = candidate;
                return slot(candidate);
            }
            return slot(actual);
        }
        /// <summary>The declared variable an access denotes as a whole (an entire-variable), or null for a component, identified or buffer variable (6.8.3.9).</summary>
        private static VariableSymbol? EntireVariableOf(VariableAccess access) =>
            access is IAmbiguous { Resolved: null } ? null
            : Ambiguity.Resolution(access) is VariableAccess { EntireVariable: { VariableIdentifier: { Identifier: { Symbol: VariableSymbol variable } } } } ? variable : null;
        /// <summary>
        /// 6.8.3.9: records that a statement threatens a variable (assigns it, passes it as a variable parameter, or reads into it), which is an error inside a
        /// for-statement that it controls; the check against the block's routines happens at the for-statement from the record.
        /// </summary>
        private void Threaten(VariableSymbol variable, ASTNode at)
        {
            if (variable.Kind != VariableKind.Declared)
                return;
            if (activeControlVariables.Contains(variable))
                Error(at, $"This statement threatens '{variable.Name}', the control-variable of a for-statement containing it (6.8.3.9).");
            threats.Add((variable, at, scope));
        }
        /// <summary>6.9, 6.6.6.5, 6.10: input or output used implicitly, which requires the program-parameter-list to contain it.</summary>
        private void RequireProgramParameter(string name, ASTNode at, string clause)
        {
            if (scope.Lookup(name) is not VariableSymbol { IsProgramParameter: true, Type: TextResolvedType } && missingImpliedFiles.Add(name))
                Error(at, $"The implied textfile {name} requires the program-parameter-list to contain an identifier spelled '{name}' ({clause}, 6.10).");
        }
        #endregion

        #region 6.10 Programs, 6.2 Blocks
        /// <summary>6.10: the program-parameters are variables of the program-block; input and output of type text, any other declared in its variable-declaration-part.</summary>
        private void CheckProgram(Parser.Program program)
        {
            scope = scope.Enter(ScopeKind.Program);
            List<Identifier> pending = new();
            HashSet<string> listed = new(StringComparer.OrdinalIgnoreCase);
            IdentifierList? il = program.ProgramHeading.ProgramParameterList?.IdentifierList;
            if (il != null)
            {
                foreach (Identifier identifier in Identifiers(il))
                {
                    if (!listed.Add(identifier.Name))
                    {
                        Error(identifier, $"Program parameter '{identifier.Name}' is listed more than once; the identifiers of a program-parameter-list shall be distinct (6.10).");
                        continue;
                    }
                    if (identifier.Name.ToLowerInvariant() is "input" or "output")
                        Declare(new VariableSymbol(identifier.Name, identifier, TextResolvedType.TextRType) { IsProgramParameter = true }, identifier); //6.10: declared by the heading alone, of type text
                    else
                        pending.Add(identifier);
                }
            }
            CheckBlock(program.ProgramBlock.Block);
            foreach (Identifier identifier in pending)
            {
                if (scope.LookupLocal(identifier.Name) is VariableSymbol variable)
                {
                    variable.IsProgramParameter = true;
                    identifier.Symbol = variable;
                }
                else
                    Error(identifier, $"Program parameter '{identifier.Name}' is not declared as a variable in the program-block (6.10).");
            }
            scope = scope.Parent!;
        }
        /// <summary>6.2.1: the six parts in order, in a scope of their own; afterwards every label sits on exactly one statement and every forward routine has its block (6.6.1).</summary>
        private void CheckBlock(Block block)
        {
            CheckLabelDeclarationPart(block.LabelDeclarationPart);
            CheckConstantDefinitionPart(block.ConstantDefinitionPart);
            CheckTypeDefinitionPart(block.TypeDefinitionPart);
            CheckVariableDeclarationPart(block.VariableDeclarationPart);
            CheckProcedureAndFunctionDeclarationPart(block.ProcedureAndFunctionDeclarationPart);
            blockSequences[scope] = block.StatementPart.CompoundStatement.StatementSequence;
            CheckCompoundStatement(block.StatementPart.CompoundStatement);
            foreach (LabelSymbol label in scope.Declared.OfType<LabelSymbol>())
            {
                if (label.LabeledStatement == null)
                    Error(label.DefiningPoint!, $"Label {label.Name} is declared but no statement of this block is prefixed by it (6.2.1).");
            }
            foreach ((LabelSymbol label, GotoStatement at, Statement[] statements, StatementSequence[] sequences) in pendingGotos.Where(g => g.label.Scope == scope).ToList())
            {
                //6.8.1: the labeled statement S shall a) contain the goto, b) be a statement of a statement-sequence containing it, or c) be a statement of the
                //statement-part of a block containing it
                if (label.LabeledStatement is Statement target &&
                    !statements.Contains(target) && !sequences.Any(sequence => Statements(sequence).Contains(target)) && !Statements(blockSequences[scope]).Contains(target))
                    Error(at, $"The statement labeled {label.Name} neither contains this goto, nor is a statement of a statement-sequence containing it, nor is at the outermost level of a block containing it (6.8.1).");
            }
            pendingGotos.RemoveAll(g => g.label.Scope == scope);
            foreach (RoutineSymbol rs in scope.Declared.OfType<RoutineSymbol>())
            {
                if (rs.IsForward)
                    Error(rs.DefiningPoint!, $"{rs.KindName} '{rs.Name}' is declared forward but no {rs.KindName}-declaration in this declaration-part supplies its block (6.6.1, 6.6.2).");
            }
            if (scope.Routine is FunctionSymbol f && !f.IsResultAssigned)
                Error(f.DefiningPoint!, $"Function '{f.Name}' never assigns its result; a function-block shall contain at least one assignment to the function-identifier (6.6.2).");
        }
        /// <summary>6.2.1, 6.1.6: labels are 0..9999, distinct by value, and declared for the block.</summary>
        private void CheckLabelDeclarationPart(LabelDeclarationPart part)
        {
            if (part.Label == null)
                return;
            foreach (Label l in part.SecondaryLabels!.Select(x => x.label).Prepend(part.Label))
            {
                long? ll = l.DigitSequence.AsLong;
                if (ll == null || ll > 9999) //digit sequence cannot be negative; null means it did not even fit in a long.
                {
                    Error(l, $"Label {l.DigitSequence.Digits} is outside 0..9999, the closed interval labels shall lie in (6.1.6).");
                    continue;
                }
                Declare(new LabelSymbol((int)ll.Value, l), l);
            }
        }
        #endregion

        #region 6.3 Constants
        /// <summary>6.3: each constant-definition declares a ConstantSymbol; the constant may not refer to the identifier being defined.</summary>
        private void CheckConstantDefinitionPart(ConstantDefinitionPart part)
        {
            if (part.ConstantDefinition == null)
                return;
            foreach (ConstantDefinition definition in (part.SecondaryConstantDefinitions ?? []).Select(x => x.constantDefinition).Prepend(part.ConstantDefinition))
            {
                ConstantValue? value = EvaluateConstant(definition.Constant); //evaluated before declaring, so the constant cannot refer to the identifier it defines (6.3)
                if (value != null) //otherwise reported inside; later uses of the identifier are reported as undeclared
                    Declare(new ConstantSymbol(definition.Identifier.Name, definition.Identifier, value), definition.Identifier);
            }
        }
        /// <summary>6.3: a signed or unsigned number, a constant-identifier (which must denote a constant, a numeric one when signed) or a character-string.</summary>
        private ConstantValue? EvaluateConstant(Constant constant)
        {
            ConstantValue? value = constant switch
            {
                { UnsignedNumber: { } number } => EvaluateUnsignedNumber(number),
                { ConstantIdentifier: { } identifier } => EvaluateConstantIdentifier(identifier),
                { CharacterStringToken: { } text } => EvaluateCharacterString(text),
                _ => throw new InvalidOperationException($"Constant at file position {constant.FileLocation} has no alternative set (compiler bug).")
            };
            if (constant.Sign != null && value != null)
            {
                bool negate = constant.Sign.MinusToken != null;
                switch (value)
                {
                    case OrdinalConstantValue { Type: IntResolvedType } integer:
                        if (negate)
                            value = new OrdinalConstantValue(IntResolvedType.IntRType, -integer.Ordinal);
                        break;
                    case RealConstantValue real:
                        if (negate)
                            value = new RealConstantValue(-real.Value);
                        break;
                    default:
                        Error(constant.Sign, $"A sign applies to a constant of integer-type or real-type, not to {value} of type {value.Type} (6.3).");
                        value = null;
                        break;
                }
            }
            constant.Value = value;
            constant.ResolvedType = value?.Type;
            return value;
        }
        /// <summary>6.7.1: the unsigned-constant of a factor; as EvaluateConstant without the sign, plus nil.</summary>
        private ConstantValue? EvaluateUnsignedConstant(UnsignedConstant constant)
        {
            ConstantValue? value = constant switch
            {
                { UnsignedNumber: { } number } => EvaluateUnsignedNumber(number),
                { ConstantIdentifier: { } identifier } => EvaluateConstantIdentifier(identifier),
                { CharacterStringToken: { } text } => EvaluateCharacterString(text),
                { NilToken: not null } => NilConstantValue.Nil, //6.4.4
                _ => throw new InvalidOperationException($"Unsigned-constant at file position {constant.FileLocation} has no alternative set (compiler bug).")
            };
            constant.Value = value;
            constant.ResolvedType = value?.Type;
            return value;
        }
        /// <summary>6.1.5: an unsigned-integer denotes an integer value, an unsigned-real a real one.</summary>
        private ConstantValue? EvaluateUnsignedNumber(UnsignedNumber number)
        {
            ConstantValue? value;
            if (number.UnsignedInteger is UnsignedInteger integer)
            {
                long? digits = integer.DigitSequence.AsLong;
                if (digits == null || digits > IntResolvedType.MaxInt)
                {
                    Error(integer, $"The integer {integer.DigitSequence.Digits} exceeds maxint, which is {IntResolvedType.MaxInt} (6.1.5, 6.7.2.2).");
                    value = null;
                }
                else
                    value = new OrdinalConstantValue(IntResolvedType.IntRType, digits.Value);
                integer.Value = value;
                integer.ResolvedType = value?.Type;
            }
            else
            {
                UnsignedReal real = number.UnsignedReal!;
                string text = real.DigitSequence.Digits;
                if (real.FractionalPart != null)
                    text += "." + real.FractionalPart.DigitSequence.Digits;
                if (real.ScaleFactor != null)
                    text += "e" + (real.ScaleFactor.Sign?.MinusToken != null ? "-" : string.Empty) + real.ScaleFactor.DigitSequence.Digits;
                if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsed) && double.IsFinite(parsed))
                    value = new RealConstantValue(parsed);
                else
                {
                    Error(real, $"The real {text} is outside the values this implementation represents (6.1.5, 6.4.2.2).");
                    value = null;
                }
                real.Value = value;
                real.ResolvedType = value?.Type;
            }
            number.Value = value;
            number.ResolvedType = value?.Type;
            return value;
        }
        /// <summary>6.3: a constant-identifier denotes the value of the constant-definition (or required constant) it refers to.</summary>
        private ConstantValue? EvaluateConstantIdentifier(ConstantIdentifier identifier)
        {
            ConstantSymbol? symbol = Resolve<ConstantSymbol>(identifier.Identifier, "a constant");
            identifier.Value = symbol?.Value;
            identifier.ResolvedType = symbol?.Type;
            return symbol?.Value;
        }
        /// <summary>6.1.7: a character-string of one string-element denotes a char, of more a value of the string-type of that length.</summary>
        private ConstantValue? EvaluateCharacterString(TokenNode token)
        {
            string content = token.Token.Content;
            string text = content.Substring(1, content.Length - 2).Replace("''", "'"); //the token keeps its apostrophes; an apostrophe-image is two
            if (text.Length == 0)
            {
                Error(token, "A character-string shall contain at least one string-element (6.1.7).");
                return null;
            }
            return text.Length == 1 ? new OrdinalConstantValue(CharResolvedType.CharRType, text[0]) : new StringConstantValue(text);
        }
        #endregion

        #region 6.4 Types
        /// <summary>6.4.1: each type-definition declares a TypeSymbol and names the type if it was anonymous; at the end, every pointer domain written ahead of its definition is filled in (6.2.2.9).</summary>
        private void CheckTypeDefinitionPart(TypeDefinitionPart part)
        {
            if (part.TypeDefinition == null)
                return;
            List<TypeDefinition> definitions = (part.SecondaryTypeDefinitions ?? []).Select(x => x.typeDefinition).Prepend(part.TypeDefinition).ToList();
            forwardDomains = new();
            typeDefinitionNames = new HashSet<string>(definitions.Select(d => d.Identifier.Name), StringComparer.OrdinalIgnoreCase);
            foreach (TypeDefinition definition in definitions)
            {
                ResolvedType type = DenoteType(definition.TypeDenoter);
                if (Declare(new TypeSymbol(definition.Identifier.Name, definition.Identifier, type), definition.Identifier) && type is not ErrorResolvedType)
                    type.Name ??= definition.Identifier.Name; //6.4.1: the first type-definition to denote a type names it; "type t = u" leaves u's name
            }
            foreach ((PointerResolvedType pointer, TypeIdentifier domain) in forwardDomains)
            {
                //6.2.2.9: the domain's defining-point is later in this very part, so it is looked up now that the part is complete
                if (scope.LookupLocal(domain.Identifier.Name) is TypeSymbol symbol)
                {
                    domain.Identifier.Symbol = symbol;
                    domain.DenotedType = symbol.Type;
                    pointer.Domain = symbol.Type;
                }
                else
                    Error(domain.Identifier, $"'{domain.Identifier.Name}', the domain of a pointer-type, is not defined as a type in this type-definition-part (6.2.2.9, 6.4.4).");
            }
            forwardDomains = null;
            typeDefinitionNames = null;
        }
        /// <summary>6.4.1: a type-identifier denotes the type it was defined as; a new-type denotes a fresh type distinct from every other.</summary>
        private ResolvedType DenoteType(TypeDenoter denoter)
        {
            ResolvedType type = denoter switch
            {
                { TypeIdentifier: { } identifier } => DenoteTypeIdentifier(identifier),
                { NewType: { NewOrdinalType: { } ordinal } } => DenoteNewOrdinalType(ordinal),
                { NewType: { NewStructuredType: { } structured } } => DenoteNewStructuredType(structured),
                { NewType: { NewPointerType: { } pointer } } => DenoteNewPointerType(pointer),
                _ => throw new InvalidOperationException($"Type-denoter at file position {denoter.FileLocation} has no alternative set (compiler bug).")
            };
            if (denoter.NewType != null)
                denoter.NewType.DenotedType = type;
            denoter.DenotedType = type;
            return type;
        }
        /// <summary>6.4.1: the identifier must denote a TypeSymbol; ErrorRType otherwise.</summary>
        private ResolvedType DenoteTypeIdentifier(TypeIdentifier identifier)
        {
            TypeSymbol? symbol = Resolve<TypeSymbol>(identifier.Identifier, "a type");
            ResolvedType type = symbol?.Type ?? ErrorResolvedType.ErrorRType;
            identifier.DenotedType = type;
            return type;
        }
        /// <summary>6.4.2: an ordinal-type-identifier, an enumerated-type (6.4.2.3, declaring its constants in the enclosing block) or a subrange-type (6.4.2.4, bounds of one ordinal type, the first not above the second).</summary>
        private OrdinalResolvedType DenoteOrdinalType(OrdinalType type)
        {
            OrdinalResolvedType result = type switch
            {
                { NewOrdinalType: { } ordinal } => DenoteNewOrdinalType(ordinal),
                { OrdinalTypeIdentifier: { } identifier } => DenoteOrdinalTypeIdentifier(identifier),
                _ => throw new InvalidOperationException($"Ordinal-type at file position {type.FileLocation} has no alternative set (compiler bug).")
            };
            type.DenotedType = result;
            return result;
        }
        /// <summary>6.4.2.1: a type-identifier that shall denote an ordinal-type. There is no ordinal error type, so after reporting one the recovery type is integer.</summary>
        private OrdinalResolvedType DenoteOrdinalTypeIdentifier(OrdinalTypeIdentifier identifier)
        {
            ResolvedType type = DenoteTypeIdentifier(identifier.TypeIdentifier);
            OrdinalResolvedType ordinal;
            if (type is OrdinalResolvedType found)
                ordinal = found;
            else
            {
                if (type is not ErrorResolvedType)
                    Error(identifier, $"'{identifier.TypeIdentifier.Identifier.Name}' denotes {type}, where an ordinal-type is required (6.4.2.1).");
                ordinal = IntResolvedType.IntRType;
            }
            identifier.DenotedType = ordinal;
            return ordinal;
        }
        /// <summary>6.4.2.3, 6.4.2.4: an enumerated-type or a subrange-type, each a new type.</summary>
        private OrdinalResolvedType DenoteNewOrdinalType(NewOrdinalType type)
        {
            OrdinalResolvedType result;
            if (type.EnumeratedType is EnumeratedType enumerated)
            {
                //6.4.2.3: the constants are declared in the enclosing block, in order, with ordinal numbers from zero
                List<Identifier> identifiers = Identifiers(enumerated.IdentifierList).ToList();
                EnumeratedResolvedType enumeration = new EnumeratedResolvedType(identifiers.Select(x => x.Name));
                for (int i = 0; i < identifiers.Count; i++)
                    Declare(new ConstantSymbol(identifiers[i].Name, identifiers[i], new OrdinalConstantValue(enumeration, i)), identifiers[i]);
                enumerated.DenotedType = enumeration;
                result = enumeration;
            }
            else
            {
                SubrangeType subrange = type.SubrangeType!;
                ConstantValue? low = EvaluateConstant(subrange.Constant), high = EvaluateConstant(subrange.SecondConstant);
                result = IntResolvedType.IntRType; //recovery when the bounds are wrong
                if (low is OrdinalConstantValue lo && high is OrdinalConstantValue hi)
                {
                    if (!Compatible(lo.Type, hi.Type))
                        Error(subrange, $"The bounds of a subrange-type shall be of one ordinal-type; these are of {lo.Type} and {hi.Type} (6.4.2.4).");
                    else if (lo.Ordinal > hi.Ordinal)
                        Error(subrange, $"The first bound of a subrange-type shall not exceed the second, but {lo} > {hi} (6.4.2.4).");
                    else
                        result = new SubrangeResolvedType(lo.Type.Host, lo.Ordinal, hi.Ordinal);
                }
                else if (low != null && high != null)
                    Error(subrange, $"The bounds of a subrange-type shall be of an ordinal-type, not {low.Type} and {high.Type} (6.4.2.4).");
                subrange.DenotedType = result;
            }
            type.DenotedType = result;
            return result;
        }
        /// <summary>6.4.3: array (6.4.3.2, nested per index), record (6.4.3.3), set of an ordinal base (6.4.3.4) or file of a permissible component (6.4.3.5), packed or not.</summary>
        private StructuredResolvedType DenoteNewStructuredType(NewStructuredType type)
        {
            bool packed = type.PackedToken != null;
            StructuredResolvedType result;
            switch (type.UnpackedStructuredType)
            {
                case { ArrayType: { } array }:
                {
                    //6.4.3.2: array[a, b] of T is array[a] of array[b] of T, every level packed when the whole is
                    List<IndexType> indices = array.SecondaryIndexTypes.Select(x => x.indexType).Prepend(array.IndexType).ToList();
                    List<OrdinalResolvedType> indexTypes = new();
                    foreach (IndexType index in indices)
                    {
                        OrdinalResolvedType indexType = DenoteOrdinalType(index.OrdinalType);
                        index.DenotedType = indexType;
                        indexTypes.Add(indexType);
                    }
                    ResolvedType component = DenoteType(array.ComponentType.TypeDenoter);
                    array.ComponentType.DenotedType = component;
                    ArrayResolvedType arrayType = new ArrayResolvedType(indexTypes[^1], component, packed);
                    for (int i = indexTypes.Count - 2; i >= 0; i--)
                        arrayType = new ArrayResolvedType(indexTypes[i], arrayType, packed);
                    array.DenotedType = arrayType;
                    result = arrayType;
                    break;
                }
                case { RecordType: { } record }:
                    result = new RecordResolvedType(DenoteFieldList(record.FieldList), packed); //6.4.3.3
                    record.DenotedType = result;
                    break;
                case { SetType: { } set }:
                {
                    OrdinalResolvedType baseType = DenoteOrdinalType(set.BaseType.OrdinalType); //6.4.3.4
                    set.BaseType.DenotedType = baseType;
                    result = new SetResolvedType(baseType, packed);
                    set.DenotedType = result;
                    break;
                }
                case { FileType: { } file }:
                {
                    ResolvedType component = DenoteType(file.ComponentType.TypeDenoter);
                    file.ComponentType.DenotedType = component;
                    if (component.ContainsFileType)
                        Error(file.ComponentType, $"{component} is not permissible as the component-type of a file-type, since it is or contains a file-type (6.4.3.5).");
                    result = new FileResolvedType(component, packed);
                    file.DenotedType = result;
                    break;
                }
                default:
                    throw new InvalidOperationException($"Unpacked-structured-type at file position {type.FileLocation} has no alternative set (compiler bug).");
            }
            type.UnpackedStructuredType.DenotedType = result;
            type.DenotedType = result;
            return result;
        }
        /// <summary>6.4.3.3: field-identifiers distinct across the whole record; case-constants compatible with the tag-type, distinct, and together covering it.</summary>
        private RecordResolvedType.FieldList DenoteFieldList(FieldList fieldList) => DenoteFieldList(fieldList, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
        /// <summary>The recursion over nested variants, carrying the field-identifiers declared so far: 6.4.3.3 makes the whole record one region, so a field-identifier may occur once in all of it.</summary>
        private RecordResolvedType.FieldList DenoteFieldList(FieldList fieldList, HashSet<string> declaredFields)
        {
            List<RecordResolvedType.Field> fixedPart = new();
            if (fieldList.FixedPart is FixedPart fixedPartNode)
            {
                foreach (RecordSection section in fixedPartNode.SecondaryRecordSections.Select(x => x.recordSection).Prepend(fixedPartNode.RecordSection))
                {
                    ResolvedType type = DenoteType(section.TypeDenoter);
                    foreach (Identifier identifier in Identifiers(section.IdentifierList))
                    {
                        if (!declaredFields.Add(identifier.Name))
                        {
                            Error(identifier, $"Field '{identifier.Name}' is already declared in this record; a field-identifier has one defining-point per record-type (6.4.3.3, 6.2.2.7).");
                            continue;
                        }
                        RecordResolvedType.Field field = new RecordResolvedType.Field(identifier.Name, type);
                        fixedPart.Add(field);
                        identifier.Symbol = new FieldSymbol(field, identifier);
                    }
                }
            }
            RecordResolvedType.VariantPart? variantPart = null;
            if (fieldList.VariantPart is VariantPart variantPartNode)
            {
                VariantSelector selector = variantPartNode.VariantSelector;
                OrdinalResolvedType tagType = DenoteOrdinalTypeIdentifier(selector.TagType.OrdinalTypeIdentifier);
                selector.TagType.DenotedType = tagType;
                bool tagTypeKnown = selector.TagType.OrdinalTypeIdentifier.TypeIdentifier.DenotedType is not ErrorResolvedType;
                string? tagField = null;
                if (selector.TagField is TagField tag)
                {
                    if (declaredFields.Add(tag.Identifier.Name))
                        tagField = tag.Identifier.Name;
                    else
                        Error(tag.Identifier, $"Tag-field '{tag.Identifier.Name}' is already declared in this record (6.4.3.3, 6.2.2.7).");
                }
                HashSet<long> covered = new();
                List<RecordResolvedType.Variant> variants = new();
                foreach (Variant variantNode in variantPartNode.SecondaryVariants.Select(x => x.variant).Prepend(variantPartNode.Variant))
                {
                    List<long> constants = new();
                    foreach (CaseConstant caseConstant in variantNode.CaseConstantList.SecondaryCaseConstants.Select(x => x.caseConstant).Prepend(variantNode.CaseConstantList.CaseConstant))
                    {
                        ConstantValue? value = EvaluateConstant(caseConstant.Constant);
                        caseConstant.Value = value;
                        caseConstant.ResolvedType = value?.Type;
                        if (value is OrdinalConstantValue ordinal && Compatible(ordinal.Type, tagType))
                        {
                            if (covered.Add(ordinal.Ordinal))
                                constants.Add(ordinal.Ordinal);
                            else
                                Error(caseConstant, $"Case-constant {ordinal} occurs more than once in this variant-part (6.4.3.3).");
                        }
                        else if (value != null && tagTypeKnown)
                            Error(caseConstant, $"Case-constant {value} is not of a type compatible with the tag-type {tagType} (6.4.3.3).");
                    }
                    variants.Add(new RecordResolvedType.Variant(constants, DenoteFieldList(variantNode.FieldList, declaredFields)));
                }
                if (tagTypeKnown && covered.Count != tagType.Cardinality)
                    Error(variantPartNode, $"The case-constants of a variant-part shall together be every value of the tag-type; these cover {covered.Count} of the {tagType.Cardinality} values of {tagType} (6.4.3.3).");
                variantPart = new RecordResolvedType.VariantPart(tagField, tagType, variants);
                if (variantPart.TagField != null)
                    selector.TagField!.Identifier.Symbol = new FieldSymbol(variantPart.TagField, selector.TagField);
            }
            return new RecordResolvedType.FieldList(fixedPart, variantPart);
        }
        /// <summary>6.4.4: the domain is a type-identifier; one defined later in the same type-definition-part is allowed (6.2.2.9) and filled in at its end.</summary>
        private PointerResolvedType DenoteNewPointerType(NewPointerType type)
        {
            PointerResolvedType pointer = new PointerResolvedType();
            TypeIdentifier domain = type.DomainType.TypeIdentifier;
            if (forwardDomains != null && typeDefinitionNames!.Contains(domain.Identifier.Name) && scope.LookupLocal(domain.Identifier.Name) == null)
                forwardDomains.Add((pointer, domain)); //6.2.2.9: its definition is still to come in this part
            else
            {
                ResolvedType domainType = DenoteTypeIdentifier(domain);
                if (domainType is not ErrorResolvedType)
                    pointer.Domain = domainType;
            }
            type.DomainType.DenotedType = pointer.Domain;
            type.DenotedType = pointer;
            return pointer;
        }
        #endregion

        #region 6.5 Variables
        /// <summary>6.5.1: each identifier of each variable-declaration declares a VariableSymbol of the denoted type.</summary>
        private void CheckVariableDeclarationPart(VariableDeclarationPart part)
        {
            if (part.VariableDeclaration == null)
                return;
            foreach (VariableDeclaration declaration in (part.SecondaryVariableDeclarations ?? []).Select(x => x.variableDeclaration).Prepend(part.VariableDeclaration))
            {
                ResolvedType type = DenoteType(declaration.TypeDenoter);
                foreach (Identifier identifier in Identifiers(declaration.IdentifierList))
                    Declare(new VariableSymbol(identifier.Name, identifier, type), identifier);
            }
        }
        /// <summary>6.5: the base is an entire-variable or, under a with-statement, a field-designator-identifier (that ambiguity); then indexed-variables (6.5.3.2), field-designators (6.5.3.3), identified-variables (6.5.4) and buffer-variables (6.5.5), the last two being the ^ ambiguity.</summary>
        private ResolvedType TypeOfVariableAccess(VariableAccess access)
        {
            if (access.ResolvedType != null)
                return access.ResolvedType; //readings share their inner nodes, so one may already have been typed
            if (access is VariableAccess.Ambiguous ambiguous)
            {
                VariableAccess? chosen;
                VariableAccess? entire = ambiguous.Candidates.FirstOrDefault(c => c.EntireVariable != null);
                if (entire != null)
                {
                    //a bare identifier: an entire-variable, or a field-designator-identifier of an enclosing with-statement's record-variable (6.8.3.10)
                    chosen = scope.Lookup(entire.EntireVariable!.VariableIdentifier.Identifier.Name) is FieldSymbol
                        ? ambiguous.Candidates.FirstOrDefault(c => c.ComponentVariable?.FieldDesignator?.FieldDesignatorIdentifier != null)
                        : entire;
                }
                else
                {
                    //"x^": an identified-variable or a buffer-variable, decided by the type of the x both readings share
                    VariableAccess? identified = ambiguous.Candidates.FirstOrDefault(c => c.IdentifiedVariable != null);
                    VariableAccess? buffer = ambiguous.Candidates.FirstOrDefault(c => c.BufferVariable != null);
                    ResolvedType inner = TypeOfVariableAccess(identified?.IdentifiedVariable?.PointerVariable.VariableAccess ?? buffer!.BufferVariable!.FileVariable.VariableAccess);
                    chosen = inner is FileResolvedType ? buffer : identified;
                }
                if (chosen == null)
                    throw new InvalidOperationException($"Ambiguous variable-access at file position {access.FileLocation} has no reading of the expected shape (compiler bug).");
                ambiguous.Resolved = chosen;
                access.ResolvedType = TypeOfVariableAccess(chosen);
                return access.ResolvedType;
            }
            ResolvedType result;
            switch (access)
            {
                case { EntireVariable: { } entire }:
                {
                    //6.5.2
                    VariableSymbol? variable = Resolve<VariableSymbol>(entire.VariableIdentifier.Identifier, "a variable");
                    result = variable?.Type ?? ErrorResolvedType.ErrorRType;
                    entire.VariableIdentifier.ResolvedType = result;
                    entire.ResolvedType = result;
                    break;
                }
                case { ComponentVariable: { IndexedVariable: { } indexed } component }:
                {
                    //6.5.3.2: one array level per index-expression (6.4.3.2), each index-expression assignment-compatible with that level's index-type
                    ResolvedType current = TypeOfVariableAccess(indexed.ArrayVariable.VariableAccess);
                    indexed.ArrayVariable.ResolvedType = current;
                    foreach (IndexExpression indexExpression in indexed.SecondaryIndexExpressions.Select(x => x.indexExpression).Prepend(indexed.IndexExpression))
                    {
                        ResolvedType index = TypeOfExpression(indexExpression.Expression);
                        indexExpression.ResolvedType = index;
                        if (current is ArrayResolvedType array)
                        {
                            if (!AssignmentCompatible(array.Index, index))
                                Error(indexExpression, $"An index-expression of type {index} is not assignment-compatible with the index-type {array.Index} (6.5.3.2).");
                            current = array.Component;
                        }
                        else
                        {
                            if (current is not ErrorResolvedType)
                                Error(indexExpression, $"Only a variable of an array-type can be indexed, not one of type {current} (6.5.3.2).");
                            current = ErrorResolvedType.ErrorRType;
                        }
                    }
                    result = current;
                    indexed.ResolvedType = result;
                    component.ResolvedType = result;
                    break;
                }
                case { ComponentVariable: { FieldDesignator: { RecordVariable: { } recordVariable, FieldSpecifier: { } specifier } designator } component }:
                {
                    //6.5.3.3: the record-variable's type supplies the field
                    ResolvedType recordType = TypeOfVariableAccess(recordVariable.VariableAccess);
                    recordVariable.ResolvedType = recordType;
                    Identifier identifier = specifier.FieldIdentifier.Identifier;
                    result = ErrorResolvedType.ErrorRType;
                    if (recordType is RecordResolvedType record)
                    {
                        if (record.FindField(identifier.Name) is RecordResolvedType.Field field)
                        {
                            identifier.Symbol = new FieldSymbol(field, identifier);
                            result = field.Type;
                        }
                        else
                            Error(identifier, $"{record} has no field '{identifier.Name}' (6.5.3.3).");
                    }
                    else if (recordType is not ErrorResolvedType)
                        Error(recordVariable, $"Only a variable of a record-type has fields; this one is of type {recordType} (6.5.3.3).");
                    specifier.FieldIdentifier.ResolvedType = result;
                    specifier.ResolvedType = result;
                    designator.ResolvedType = result;
                    component.ResolvedType = result;
                    break;
                }
                case { ComponentVariable: { FieldDesignator: { FieldDesignatorIdentifier: { } fieldDesignator } designator } component }:
                {
                    //6.8.3.10: a field of an enclosing with-statement's record-variable
                    FieldSymbol? field = Resolve<FieldSymbol>(fieldDesignator.Identifier, "a field of an enclosing with-statement's record-variable");
                    result = field?.Type ?? ErrorResolvedType.ErrorRType;
                    fieldDesignator.ResolvedType = result;
                    designator.ResolvedType = result;
                    component.ResolvedType = result;
                    break;
                }
                case { IdentifiedVariable: { } identified }:
                {
                    //6.5.4: the variable the pointer identifies, of the pointer-type's domain-type
                    ResolvedType pointer = TypeOfVariableAccess(identified.PointerVariable.VariableAccess);
                    identified.PointerVariable.ResolvedType = pointer;
                    if (pointer is PointerResolvedType { Domain: { } domain })
                        result = domain;
                    else
                    {
                        if (pointer is not ErrorResolvedType and not PointerResolvedType)
                            Error(identified, $"Only a variable of a pointer-type identifies a variable; this one is of type {pointer} (6.5.4).");
                        result = ErrorResolvedType.ErrorRType;
                    }
                    identified.ResolvedType = result;
                    break;
                }
                case { BufferVariable: { } buffer }:
                {
                    //6.5.5: the buffer-variable of a file, of its component-type
                    ResolvedType file = TypeOfVariableAccess(buffer.FileVariable.VariableAccess);
                    buffer.FileVariable.ResolvedType = file;
                    if (file is FileResolvedType fileType)
                        result = fileType.Component;
                    else
                    {
                        if (file is not ErrorResolvedType)
                            Error(buffer, $"Only a variable of a file-type has a buffer-variable; this one is of type {file} (6.5.5).");
                        result = ErrorResolvedType.ErrorRType;
                    }
                    buffer.ResolvedType = result;
                    break;
                }
                default:
                    throw new InvalidOperationException($"Variable-access at file position {access.FileLocation} has no alternative set (compiler bug).");
            }
            access.ResolvedType = result;
            return result;
        }
        #endregion

        #region 6.6 Procedures and functions
        /// <summary>6.6.1, 6.6.2: declares each routine in the block's scope, then checks its block in a Routine scope.</summary>
        private void CheckProcedureAndFunctionDeclarationPart(ProcedureAndFunctionDeclarationPart part)
        {
            foreach ((ProcedureDeclaration? procedure, FunctionDeclaration? function, TokenNode _) in part.ProcedureAndFunctionDeclarationParts)
            {
                if (procedure != null)
                    CheckProcedureDeclaration(procedure);
                else
                    CheckFunctionDeclaration(function!);
            }
        }
        /// <summary>6.6.1: "procedure p;" followed by a block declares a new procedure unless p is forward, which settles the heading/identification ambiguity; a forward routine gets exactly one block, in the same declaration-part.</summary>
        private void CheckProcedureDeclaration(ProcedureDeclaration declaration)
        {
            if (declaration is ProcedureDeclaration.Ambiguous ambiguous)
            {
                ProcedureDeclaration heading = ambiguous.Candidates.First(c => c.ProcedureHeading != null);
                ProcedureDeclaration identification = ambiguous.Candidates.First(c => c.ProcedureIdentification != null);
                declaration = scope.LookupLocal(heading.ProcedureHeading!.Identifier.Name) is ProcedureSymbol { IsForward: true } ? identification : heading;
                ambiguous.Resolved = declaration;
            }
            switch (declaration)
            {
                case { ProcedureHeading: { } heading, Directive: { } directive }:
                {
                    ProcedureSymbol symbol = new ProcedureSymbol(heading.Identifier.Name, heading.Identifier, RoutineKind.Declared, DeclareFormalParameterList(heading.FormalParameterList));
                    CheckDirective(directive, symbol);
                    Declare(symbol, heading.Identifier);
                    break;
                }
                case { ProcedureHeading: { } heading, ProcedureBlock: { } block }:
                {
                    ProcedureSymbol symbol = new ProcedureSymbol(heading.Identifier.Name, heading.Identifier, RoutineKind.Declared, DeclareFormalParameterList(heading.FormalParameterList));
                    Declare(symbol, heading.Identifier);
                    CheckRoutineBlock(symbol, block.Block);
                    break;
                }
                case { ProcedureIdentification: { } identification, ProcedureBlock: { } block }:
                {
                    Identifier identifier = identification.ProcedureIdentifier.Identifier;
                    if (scope.LookupLocal(identifier.Name) is ProcedureSymbol { IsForward: true } symbol)
                    {
                        identifier.Symbol = symbol;
                        symbol.IsForward = false;
                        CheckRoutineBlock(symbol, block.Block);
                    }
                    else
                        Error(identifier, $"'{identifier.Name}' is not a procedure declared forward in this declaration-part, which a procedure-identification requires (6.6.1).");
                    break;
                }
                default:
                    throw new InvalidOperationException($"Procedure-declaration at file position {declaration.FileLocation} has no alternative set (compiler bug).");
            }
        }
        /// <summary>6.6.2: as for procedures, plus a result-type that is a simple or pointer type-identifier and a block that assigns the function-identifier at least once.</summary>
        private void CheckFunctionDeclaration(FunctionDeclaration declaration)
        {
            switch (declaration)
            {
                case { FunctionHeading: { } heading, Directive: { } directive }:
                {
                    FunctionSymbol symbol = new FunctionSymbol(heading.Identifier.Name, heading.Identifier, RoutineKind.Declared, DeclareFormalParameterList(heading.FormalParameterList), DenoteResultType(heading.ResultType));
                    CheckDirective(directive, symbol);
                    Declare(symbol, heading.Identifier);
                    break;
                }
                case { FunctionHeading: { } heading, FunctionBlock: { } block }:
                {
                    FunctionSymbol symbol = new FunctionSymbol(heading.Identifier.Name, heading.Identifier, RoutineKind.Declared, DeclareFormalParameterList(heading.FormalParameterList), DenoteResultType(heading.ResultType));
                    Declare(symbol, heading.Identifier);
                    CheckRoutineBlock(symbol, block.Block);
                    break;
                }
                case { FunctionIdentification: { } identification, FunctionBlock: { } block }:
                {
                    Identifier identifier = identification.FunctionIdentifier.Identifier;
                    if (scope.LookupLocal(identifier.Name) is FunctionSymbol { IsForward: true } symbol)
                    {
                        identifier.Symbol = symbol;
                        symbol.IsForward = false;
                        CheckRoutineBlock(symbol, block.Block);
                    }
                    else
                        Error(identifier, $"'{identifier.Name}' is not a function declared forward in this declaration-part, which a function-identification requires (6.6.2).");
                    break;
                }
                default:
                    throw new InvalidOperationException($"Function-declaration at file position {declaration.FileLocation} has no alternative set (compiler bug).");
            }
        }
        /// <summary>6.1.4, 6.6.1, 6.6.2: forward is the one directive the standard defines; it marks the routine as awaiting its block.</summary>
        private void CheckDirective(Directive directive, RoutineSymbol symbol)
        {
            if (Spelling(directive).Equals("forward", StringComparison.OrdinalIgnoreCase))
                symbol.IsForward = true;
            else
                Error(directive, $"The directive '{Spelling(directive)}' is not forward, the only directive this implementation defines (6.1.4).");
        }
        /// <summary>
        /// 6.6.1, 6.6.2, 6.6.3.1: enters the routine's scope, declares its formal parameters there (they share the block's region) with the bound-identifiers of
        /// conformant parameters (6.6.3.7.1), checks the block, and leaves. A clash here repeats one already reported within the formal-parameter-list.
        /// </summary>
        private void CheckRoutineBlock(RoutineSymbol routine, Block block)
        {
            scope = scope.Enter(ScopeKind.Routine, routine);
            foreach (Symbol parameter in routine.Parameters)
            {
                scope.TryDeclare(parameter, out _);
                if (parameter is VariableSymbol { ConformantSchema: { } schema })
                {
                    for (ConformantSchema? level = schema; level != null; level = level.ComponentSchema)
                    {
                        scope.TryDeclare(level.LowBound, out _);
                        scope.TryDeclare(level.HighBound, out _);
                    }
                }
            }
            CheckBlock(block);
            scope = scope.Parent!;
        }
        /// <summary>6.6.2: the result-type is a type-identifier denoting a simple-type or a pointer-type; the parser packs both readings, and the type found decides.</summary>
        private ResolvedType DenoteResultType(ResultType resultType)
        {
            ResultType simple = resultType, pointer = resultType;
            if (resultType is ResultType.Ambiguous ambiguous)
            {
                simple = ambiguous.Candidates.First(c => c.SimpleTypeIdentifier != null);
                pointer = ambiguous.Candidates.First(c => c.PointerTypeIdentifier != null);
            }
            TypeIdentifier identifier = simple.SimpleTypeIdentifier?.TypeIdentifier ?? pointer.PointerTypeIdentifier!.TypeIdentifier;
            ResolvedType type = DenoteTypeIdentifier(identifier);
            ResultType chosen = simple;
            switch (type)
            {
                case OrdinalResolvedType or RealResolvedType:
                    if (simple.SimpleTypeIdentifier != null)
                        simple.SimpleTypeIdentifier.DenotedType = type;
                    break;
                case PointerResolvedType:
                    chosen = pointer;
                    if (pointer.PointerTypeIdentifier != null)
                    {
                        pointer.PointerTypeIdentifier.TypeIdentifier.Identifier.Symbol = identifier.Identifier.Symbol;
                        pointer.PointerTypeIdentifier.TypeIdentifier.DenotedType = type;
                        pointer.PointerTypeIdentifier.DenotedType = type;
                    }
                    break;
                default:
                    if (type is not ErrorResolvedType)
                        Error(identifier, $"A result-type shall be a simple-type or a pointer-type, not {type} (6.6.2).");
                    type = ErrorResolvedType.ErrorRType;
                    break;
            }
            if (resultType is ResultType.Ambiguous placeholder)
                placeholder.Resolved = chosen;
            chosen.DenotedType = type;
            resultType.DenotedType = type;
            return type;
        }
        /// <summary>6.6.3.1 to 6.6.3.7: declares the parameters of each section in a formal-parameter-list scope of their own (where a repeated name is caught) and returns the sections for the RoutineSymbol; CheckRoutineBlock declares them again in the block's region.</summary>
        private IReadOnlyList<ParameterSection> DeclareFormalParameterList(FormalParameterList? list)
        {
            List<ParameterSection> sections = new();
            if (list == null)
                return sections;
            scope = scope.Enter(ScopeKind.FormalParameterList);
            foreach (FormalParameterSection section in list.SecondaryFormalParameterSections.Select(x => x.formalParameterSection).Prepend(list.FormalParameterSection))
            {
                ParameterSection built;
                switch (section)
                {
                    case { ValueParameterSpecification: { } value }:
                    {
                        //6.6.3.2
                        ResolvedType type = DenoteTypeIdentifier(value.TypeIdentifier);
                        if (type.ContainsFileType)
                            Error(value.TypeIdentifier, $"A value parameter shall be of a type permissible as a file component; {type} is or contains a file-type (6.6.3.2, 6.4.3.5).");
                        built = new ParameterSection(ParameterSectionKind.Value, DeclareParameters(value.IdentifierList, type, VariableKind.ValueParameter));
                        break;
                    }
                    case { VariableParameterSpecification: { } variable }:
                        //6.6.3.3
                        built = new ParameterSection(ParameterSectionKind.Variable, DeclareParameters(variable.IdentifierList, DenoteTypeIdentifier(variable.TypeIdentifier), VariableKind.VariableParameter));
                        break;
                    case { ProceduralParameterSpecification: { ProcedureHeading: { } heading } }:
                    {
                        //6.6.3.4
                        ProcedureSymbol procedure = new ProcedureSymbol(heading.Identifier.Name, heading.Identifier, RoutineKind.FormalParameter, DeclareFormalParameterList(heading.FormalParameterList));
                        Declare(procedure, heading.Identifier);
                        built = new ParameterSection(ParameterSectionKind.Procedural, new[] { procedure });
                        break;
                    }
                    case { FunctionalParameterSpecification: { FunctionHeading: { } heading } }:
                    {
                        //6.6.3.5
                        FunctionSymbol function = new FunctionSymbol(heading.Identifier.Name, heading.Identifier, RoutineKind.FormalParameter, DeclareFormalParameterList(heading.FormalParameterList), DenoteResultType(heading.ResultType));
                        Declare(function, heading.Identifier);
                        built = new ParameterSection(ParameterSectionKind.Functional, new[] { function });
                        break;
                    }
                    case { ConformantArrayParameterSpecification: { } conformant }:
                    {
                        //6.6.3.7
                        bool isVariable = conformant.VariableConformantArraySpecification != null;
                        IdentifierList identifiers = conformant.ValueConformantArraySpecification?.IdentifierList ?? conformant.VariableConformantArraySpecification!.IdentifierList;
                        ConformantArraySchema schemaNode = conformant.ValueConformantArraySpecification?.ConformantArraySchema ?? conformant.VariableConformantArraySpecification!.ConformantArraySchema;
                        ConformantSchema schema = BuildConformantSchema(schemaNode);
                        List<Symbol> parameters = new();
                        foreach (Identifier identifier in Identifiers(identifiers))
                        {
                            VariableSymbol parameter = new VariableSymbol(identifier.Name, identifier, schema, isVariable);
                            Declare(parameter, identifier);
                            parameters.Add(parameter);
                        }
                        built = new ParameterSection(isVariable ? ParameterSectionKind.VariableConformantArray : ParameterSectionKind.ValueConformantArray, parameters);
                        break;
                    }
                    default:
                        throw new InvalidOperationException($"Formal-parameter-section at file position {section.FileLocation} has no alternative set (compiler bug).");
                }
                sections.Add(built);
            }
            scope = scope.Parent!;
            return sections;
        }
        /// <summary>6.6.3.2, 6.6.3.3: one VariableSymbol of the given kind and type per identifier of a parameter specification, each declared in the current scope.</summary>
        private List<Symbol> DeclareParameters(IdentifierList list, ResolvedType type, VariableKind kind)
        {
            List<Symbol> parameters = new();
            foreach (Identifier identifier in Identifiers(list))
            {
                VariableSymbol parameter = new VariableSymbol(identifier.Name, identifier, type, kind);
                Declare(parameter, identifier);
                parameters.Add(parameter);
            }
            return parameters;
        }
        /// <summary>
        /// 6.6.3.7.1: builds the schema, one ConformantSchema per index-type-specification with the abbreviated form unfolded, declaring each bound-identifier in the
        /// current (formal-parameter-list) scope as a BoundIdentifierSymbol of the index type.
        /// </summary>
        private ConformantSchema BuildConformantSchema(ConformantArraySchema schema)
        {
            ConformantSchema built;
            if (schema.PackedConformantArraySchema is PackedConformantArraySchema packed)
            {
                (BoundIdentifierSymbol low, BoundIdentifierSymbol high, OrdinalResolvedType index) = DeclareIndexTypeSpecification(packed.IndexTypeSpecification);
                built = new ConformantSchema(low, high, index, true, DenoteTypeIdentifier(packed.TypeIdentifier));
                packed.DenotedType = built.ArrayType;
            }
            else
            {
                UnpackedConformantArraySchema unpacked = schema.UnpackedConformantArraySchema!;
                List<(BoundIdentifierSymbol low, BoundIdentifierSymbol high, OrdinalResolvedType index)> bounds =
                    unpacked.SecondaryIndexTypeSpecifications.Select(x => x.indexTypeSpecification).Prepend(unpacked.IndexTypeSpecification).Select(DeclareIndexTypeSpecification).ToList();
                int last = bounds.Count - 1;
                built = unpacked.ConformantArraySchema != null
                    ? new ConformantSchema(bounds[last].low, bounds[last].high, bounds[last].index, false, BuildConformantSchema(unpacked.ConformantArraySchema))
                    : new ConformantSchema(bounds[last].low, bounds[last].high, bounds[last].index, false, DenoteTypeIdentifier(unpacked.TypeIdentifier!));
                for (int i = last - 1; i >= 0; i--)
                    built = new ConformantSchema(bounds[i].low, bounds[i].high, bounds[i].index, false, built); //the abbreviated form: one level per index-type-specification
                unpacked.DenotedType = built.ArrayType;
            }
            schema.DenotedType = built.ArrayType;
            return built;
        }
        /// <summary>6.6.3.7.1: an index-type-specification "low..high: T" declares its two bound-identifiers, of the ordinal type T.</summary>
        private (BoundIdentifierSymbol low, BoundIdentifierSymbol high, OrdinalResolvedType index) DeclareIndexTypeSpecification(IndexTypeSpecification specification)
        {
            OrdinalResolvedType index = DenoteOrdinalTypeIdentifier(specification.OrdinalTypeIdentifier);
            BoundIdentifierSymbol low = new BoundIdentifierSymbol(specification.FirstIdentifier.Name, specification.FirstIdentifier, index, false);
            BoundIdentifierSymbol high = new BoundIdentifierSymbol(specification.SecondIdentifier.Name, specification.SecondIdentifier, index, true);
            Declare(low, specification.FirstIdentifier);
            Declare(high, specification.SecondIdentifier);
            return (low, high, index);
        }
        /// <summary>6.7.3, 6.8.2.3: as many actuals as formals; value parameters assignment-compatible, variable parameters of the same type and neither a packed component nor a tag-field (6.6.3.3), routines congruous (6.6.3.4 to 6.6.3.6), conformant arrays conformable (6.6.3.8).</summary>
        private void CheckActualParameters(RoutineSymbol routine, ActualParameterList? actuals, ASTNode at)
        {
            List<ActualParameter> actualList = Actuals(actuals);
            List<Symbol> formals = routine.Parameters.ToList();
            if (actualList.Count != formals.Count)
            {
                Error(at, $"{routine.KindName} '{routine.Name}' has {formals.Count} formal parameter(s) but is given {actualList.Count} actual(s) (6.7.3, 6.8.2.3).");
                foreach (ActualParameter actual in actualList)
                    if (Reading(actual, a => a.Expression) is Expression expression)
                        TypeOfExpression(expression); //still typed, for their own errors and the annotations
                return;
            }
            Dictionary<ConformantSchema, ResolvedType> sectionTypes = new(); //6.6.3.7.1: the actuals of one conformant section share one type
            foreach ((ActualParameter actual, Symbol formal) in actualList.Zip(formals))
            {
                switch (formal)
                {
                    case VariableSymbol { IsVariableParameter: true } variable:
                    {
                        VariableAccess? access = Reading(actual, a => a.VariableAccess);
                        if (access == null)
                        {
                            Error(actual, $"The variable parameter '{variable.Name}' requires a variable-access as its actual-parameter (6.6.3.3).");
                            break;
                        }
                        ResolvedType type = TypeOfVariableAccess(access);
                        if (type is ErrorResolvedType)
                            break;
                        if (variable.ConformantSchema is ConformantSchema schema)
                            CheckConformantActual(actual, type, schema, sectionTypes);
                        else if (!ReferenceEquals(type, variable.Type))
                            Error(actual, $"The actual for the variable parameter '{variable.Name}' shall be of its type {variable.Type}, not {type} (6.6.3.3).");
                        CheckVariableParameterAccess(access, actual);
                        break;
                    }
                    case VariableSymbol value:
                    {
                        Expression? expression = Reading(actual, a => a.Expression);
                        if (expression == null)
                        {
                            Error(actual, $"The value parameter '{value.Name}' requires an expression as its actual-parameter (6.6.3.2).");
                            break;
                        }
                        ResolvedType type = TypeOfExpression(expression);
                        if (type is ErrorResolvedType)
                            break;
                        if (value.ConformantSchema is ConformantSchema schema)
                            CheckConformantActual(actual, type, schema, sectionTypes);
                        else if (!AssignmentCompatible(value.Type, type))
                            Error(actual, $"The actual for the value parameter '{value.Name}' shall be assignment-compatible with {value.Type}; {type} is not (6.6.3.2, 6.4.6).");
                        break;
                    }
                    case ProcedureSymbol procedure:
                    {
                        ProcedureIdentifier? identifier = Reading(actual, a => a.ProcedureIdentifier);
                        if (identifier == null)
                        {
                            Error(actual, $"The procedural parameter '{procedure.Name}' requires a procedure-identifier as its actual-parameter (6.6.3.4).");
                            break;
                        }
                        if (Resolve<ProcedureSymbol>(identifier.Identifier, "a procedure") is ProcedureSymbol actualProcedure)
                        {
                            if (actualProcedure.Kind == RoutineKind.Required)
                                Error(identifier, $"The required procedure '{actualProcedure.Name}' cannot be an actual parameter; one with a defining-point in the program is required (6.6.3.4).");
                            else if (!ParameterSection.Congruous(actualProcedure.ParameterSections, procedure.ParameterSections))
                                Error(identifier, $"The formal-parameter-list of '{actualProcedure.Name}' is not congruous with that of the procedural parameter '{procedure.Name}' (6.6.3.4, 6.6.3.6).");
                        }
                        break;
                    }
                    case FunctionSymbol function:
                    {
                        FunctionIdentifier? identifier = Reading(actual, a => a.FunctionIdentifier);
                        if (identifier == null)
                        {
                            Error(actual, $"The functional parameter '{function.Name}' requires a function-identifier as its actual-parameter (6.6.3.5).");
                            break;
                        }
                        if (Resolve<FunctionSymbol>(identifier.Identifier, "a function") is FunctionSymbol actualFunction)
                        {
                            if (actualFunction.Kind == RoutineKind.Required)
                                Error(identifier, $"The required function '{actualFunction.Name}' cannot be an actual parameter; one with a defining-point in the program is required (6.6.3.5).");
                            else if (!ParameterSection.Congruous(actualFunction.ParameterSections, function.ParameterSections))
                                Error(identifier, $"The formal-parameter-list of '{actualFunction.Name}' is not congruous with that of the functional parameter '{function.Name}' (6.6.3.5, 6.6.3.6).");
                            else if (!ReferenceEquals(actualFunction.ResultType, function.ResultType))
                                Error(identifier, $"The result type of '{actualFunction.Name}', {actualFunction.ResultType}, is not that of the functional parameter '{function.Name}', {function.ResultType} (6.6.3.5).");
                        }
                        break;
                    }
                }
            }
        }
        /// <summary>6.6.3.7.1, 6.6.3.8: an actual for a conformant-array parameter conforms to the schema, and every actual of the section has one type.</summary>
        private void CheckConformantActual(ActualParameter actual, ResolvedType type, ConformantSchema schema, Dictionary<ConformantSchema, ResolvedType> sectionTypes)
        {
            if (!Conformable(type, schema))
                Error(actual, $"An actual of type {type} is not conformable with the conformant-array-schema {schema} (6.6.3.8).");
            else if (sectionTypes.TryGetValue(schema, out ResolvedType? first) && !ReferenceEquals(first, type))
                Error(actual, $"The actuals of one conformant-array-parameter-specification shall all be of the same type; this one is {type} where an earlier one was {first} (6.6.3.7.1).");
            else
                sectionTypes[schema] = type;
        }
        /// <summary>6.6.3.3: an actual variable parameter is neither a component of a variable of a packed type nor a tag-field; passing it threatens the variable (6.8.3.9 b).</summary>
        private void CheckVariableParameterAccess(VariableAccess access, ASTNode at)
        {
            VariableAccess resolved = (VariableAccess)Ambiguity.Resolution(access);
            switch (resolved)
            {
                case { ComponentVariable: { IndexedVariable: { ArrayVariable: { ResolvedType: StructuredResolvedType { IsPacked: true } } } } }:
                case { ComponentVariable: { FieldDesignator: { RecordVariable: { ResolvedType: StructuredResolvedType { IsPacked: true } } } } }:
                case { ComponentVariable: { FieldDesignator: { FieldDesignatorIdentifier: { Identifier: { Symbol: FieldSymbol { RecordVariable: { ResolvedType: StructuredResolvedType { IsPacked: true } } } } } } } }:
                    Error(at, "An actual variable parameter shall not be a component of a variable whose type is designated packed (6.6.3.3).");
                    break;
                case { ComponentVariable: { FieldDesignator: { FieldSpecifier: { FieldIdentifier: { Identifier: { Symbol: FieldSymbol { Field: { IsTagField: true } } } } } } } }:
                case { ComponentVariable: { FieldDesignator: { FieldDesignatorIdentifier: { Identifier: { Symbol: FieldSymbol { Field: { IsTagField: true } } } } } } }:
                    Error(at, "An actual variable parameter shall not be the tag-field of a variant-part (6.6.3.3).");
                    break;
            }
            if (EntireVariableOf(resolved) is VariableSymbol variable)
                Threaten(variable, at);
        }
        /// <summary>6.6.3.8: an array type conforms to a schema if a) its index-type is compatible with the schema's, b) its bounds lie within it, c) its component is the schema's type or conforms to its component schema, and d) both or neither are packed.</summary>
        private bool Conformable(ResolvedType actual, ConformantSchema schema) =>
            actual is ArrayResolvedType array
            && Compatible(array.Index, schema.IndexType)
            && schema.IndexType.Includes(array.Index.Low) && schema.IndexType.Includes(array.Index.High)
            && (schema.ComponentSchema != null ? Conformable(array.Component, schema.ComponentSchema) : ReferenceEquals(array.Component, schema.ComponentType))
            && array.IsPacked == schema.IsPacked;
        #endregion

        #region 6.7 Expressions
        /// <summary>6.7.1, 6.7.2.5: a simple-expression, or two compared by a relational-operator (table 6) yielding Boolean.</summary>
        private ResolvedType TypeOfExpression(Expression expression)
        {
            if (expression.ResolvedType != null)
                return expression.ResolvedType; //readings of an ambiguous construct can share it, and it is typed once
            ResolvedType left = TypeOfSimpleExpression(expression.SimpleExpression);
            ResolvedType result = left; //without a relational-operator the expression is its simple-expression
            if (expression.RelationalOperator is RelationalOperator op)
            {
                ResolvedType right = TypeOfSimpleExpression(expression.SecondSimpleExpression!);
                result = BooleanResolvedType.BooleanRType;
                if (left is not ErrorResolvedType && right is not ErrorResolvedType)
                {
                    string spelling = (op.EqualsToken ?? op.KetPairToken ?? op.LessThanToken ?? op.GreaterThanToken ?? op.LessThanOrEqualToken ?? op.GreaterThanOrEqualToken ?? op.InToken)!.Token.Content;
                    if (op.InToken != null)
                    {
                        //table 6: the left operand is of any ordinal-type T and the right of a set-type whose base is compatible with T; [] belongs to every set-type (6.7.1)
                        if (left is not OrdinalResolvedType || !(right is EmptySetResolvedType || (right is SetResolvedType set && Compatible(left, set.Base))))
                            Error(op, $"'in' takes an ordinal value and a set of a compatible base type, not {left} and {right} (6.7.2.5).");
                    }
                    else if (!Compatible(left, right) && !(left is RealResolvedType && right is IntResolvedType) && !(left is IntResolvedType && right is RealResolvedType))
                    {
                        Error(op, $"The operands of '{spelling}' shall be of compatible types, or one of real-type and the other of integer-type; {left} and {right} are neither (6.7.2.5).");
                    }
                    else
                    {
                        //table 6, by the kind the operands share; nil and [] take their kind from the other side
                        ResolvedType kind = left is NilResolvedType or EmptySetResolvedType ? right : left;
                        bool isEquality = op.EqualsToken != null || op.KetPairToken != null;
                        bool isStrictOrdering = op.LessThanToken != null || op.GreaterThanToken != null;
                        bool allowed = kind switch
                        {
                            OrdinalResolvedType or RealResolvedType => true,              //any simple-type, under every operator
                            ArrayResolvedType { IsStringType: true } => true,             //string-types, under every operator (lexicographic)
                            SetResolvedType or EmptySetResolvedType => !isStrictOrdering, //sets: = and <>, and <= and >= as inclusion
                            PointerResolvedType or NilResolvedType => isEquality,         //pointers: = and <> only
                            _ => false                                                    //records, other arrays and files are never compared
                        };
                        if (!allowed)
                            Error(op, $"'{spelling}' cannot be applied to operands of type {kind} (6.7.2.5, table 6).");
                    }
                }
            }
            expression.ResolvedType = result;
            return result;
        }
        /// <summary>6.7.2.2 to 6.7.2.4: an optional sign and terms joined by adding-operators (tables 3 and 4: arithmetic, Boolean or, set union and difference).</summary>
        private ResolvedType TypeOfSimpleExpression(SimpleExpression expression)
        {
            ResolvedType result = TypeOfTerm(expression.Term);
            if (expression.Sign != null && result is not ErrorResolvedType)
            {
                //6.7.2.2, table 4: a sign applies to an integer or real operand and leaves its type unchanged; it binds the first term only
                if (IsInteger(result))
                    result = IntResolvedType.IntRType;
                else if (result is not RealResolvedType)
                {
                    Error(expression.Sign, $"A sign applies to an integer or real operand, not to {result} (6.7.2.2).");
                    result = ErrorResolvedType.ErrorRType;
                }
            }
            foreach ((AddingOperator op, Term term) in expression.SecondaryTerms)
            {
                ResolvedType right = TypeOfTerm(term);
                if (result is ErrorResolvedType || right is ErrorResolvedType)
                {
                    result = ErrorResolvedType.ErrorRType; //already reported inside an operand; keep walking so the remaining terms are checked
                    continue;
                }
                string spelling = (op.PlusToken ?? op.MinusToken ?? op.OrToken)!.Token.Content;
                if (op.OrToken != null)
                {
                    //6.7.2.3: both operands Boolean, result Boolean
                    if (!IsBoolean(result) || !IsBoolean(right))
                        Error(op, $"The operands of 'or' shall be Boolean, not {result} and {right} (6.7.2.3).");
                    result = BooleanResolvedType.BooleanRType;
                }
                else if (IsNumeric(result) && IsNumeric(right))
                {
                    //6.7.2.2, table 3: integer if both operands are integer, real otherwise
                    result = IsInteger(result) && IsInteger(right) ? IntResolvedType.IntRType : RealResolvedType.RealRType;
                }
                else if (IsSet(result) && IsSet(right))
                {
                    //6.7.2.4: union and difference of compatible sets; the result is the canonical set type of their base (6.7.1)
                    if (Compatible(result, right))
                        result = CanonicalSetOf(result, right);
                    else
                    {
                        Error(op, $"The operands of '{spelling}' are not compatible sets: {result} and {right} need compatible base types and the same packedness (6.4.5, 6.7.2.4).");
                        result = ErrorResolvedType.ErrorRType;
                    }
                }
                else
                {
                    Error(op, $"'{spelling}' applies to two integer or real operands or to two sets, not to {result} and {right} (6.7.2.2, 6.7.2.4).");
                    result = ErrorResolvedType.ErrorRType;
                }
            }
            expression.ResolvedType = result;
            return result;
        }
        /// <summary>6.7.2.2 to 6.7.2.4: factors joined by multiplying-operators (arithmetic, div and mod on integers, Boolean and, set intersection).</summary>
        private ResolvedType TypeOfTerm(Term term)
        {
            ResolvedType result = TypeOfFactor(term.Factor);
            foreach ((MultiplyingOperator op, Factor factor) in term.SecondaryFactors)
            {
                ResolvedType right = TypeOfFactor(factor);
                if (result is ErrorResolvedType || right is ErrorResolvedType)
                {
                    result = ErrorResolvedType.ErrorRType; //already reported inside an operand; keep walking so the remaining factors are checked
                    continue;
                }
                string spelling = (op.AsteriskToken ?? op.ForwardSlashToken ?? op.DivToken ?? op.ModToken ?? op.AndToken)!.Token.Content;
                switch (op)
                {
                    case { AndToken: not null }:
                        //6.7.2.3: both operands Boolean, result Boolean
                        if (!IsBoolean(result) || !IsBoolean(right))
                            Error(op, $"The operands of 'and' shall be Boolean, not {result} and {right} (6.7.2.3).");
                        result = BooleanResolvedType.BooleanRType;
                        break;
                    case { AsteriskToken: not null }:
                        if (IsNumeric(result) && IsNumeric(right))
                            result = IsInteger(result) && IsInteger(right) ? IntResolvedType.IntRType : RealResolvedType.RealRType; //table 3
                        else if (IsSet(result) && IsSet(right))
                        {
                            //6.7.2.4: intersection of compatible sets
                            if (Compatible(result, right))
                                result = CanonicalSetOf(result, right);
                            else
                            {
                                Error(op, $"The operands of '*' are not compatible sets: {result} and {right} need compatible base types and the same packedness (6.4.5, 6.7.2.4).");
                                result = ErrorResolvedType.ErrorRType;
                            }
                        }
                        else
                        {
                            Error(op, $"'*' applies to two integer or real operands or to two sets, not to {result} and {right} (6.7.2.2, 6.7.2.4).");
                            result = ErrorResolvedType.ErrorRType;
                        }
                        break;
                    case { ForwardSlashToken: not null }:
                        //table 3: integer or real operands, the result always real
                        if (!IsNumeric(result) || !IsNumeric(right))
                            Error(op, $"The operands of '/' shall be integer or real, not {result} and {right} (6.7.2.2).");
                        result = RealResolvedType.RealRType;
                        break;
                    case { DivToken: not null }:
                    case { ModToken: not null }:
                        //table 3: both operands integer, result integer
                        if (!IsInteger(result) || !IsInteger(right))
                            Error(op, $"The operands of '{spelling}' shall be integer, not {result} and {right} (6.7.2.2).");
                        result = IntResolvedType.IntRType;
                        break;
                    default:
                        throw new InvalidOperationException($"Multiplying-operator at file position {op.FileLocation} has no alternative set (compiler bug).");
                }
            }
            term.ResolvedType = result;
            return result;
        }
        /// <summary>The identifier a factor's identifier readings share, from whichever reading carries it plainly.</summary>
        private static Identifier? FactorIdentifier(Factor factor) =>
            factor.BoundIdentifier?.Identifier ?? factor.UnsignedConstant?.ConstantIdentifier?.Identifier ?? factor.FunctionDesignator?.FunctionIdentifier.Identifier;
        /// <summary>6.7.1: an identifier factor is a bound-identifier, a variable-access, a constant-identifier or a parameterless function-designator according to what it denotes (that ambiguity); otherwise an unsigned-constant, a function-designator, a set-constructor, a parenthesized expression or not.</summary>
        private ResolvedType TypeOfFactor(Factor factor)
        {
            if (factor is Factor.Ambiguous ambiguous)
            {
                Identifier identifier = ambiguous.Candidates.Select(FactorIdentifier).First(i => i != null)!;
                Symbol? symbol = Lookup(identifier);
                Factor? chosen = symbol switch
                {
                    BoundIdentifierSymbol => ambiguous.Candidates.FirstOrDefault(c => c.BoundIdentifier != null),
                    VariableSymbol or FieldSymbol => ambiguous.Candidates.FirstOrDefault(c => c.VariableAccess != null),
                    ConstantSymbol => ambiguous.Candidates.FirstOrDefault(c => c.UnsignedConstant != null),
                    FunctionSymbol => ambiguous.Candidates.FirstOrDefault(c => c.FunctionDesignator != null),
                    _ => null
                };
                if (chosen == null)
                {
                    if (symbol != null)
                        Error(identifier, $"'{identifier.Name}' denotes a {symbol.KindName}, which cannot stand as a factor (6.7.1).");
                    factor.ResolvedType = ErrorResolvedType.ErrorRType;
                    return factor.ResolvedType;
                }
                ambiguous.Resolved = chosen;
                factor = chosen;
            }
            ResolvedType result;
            switch (factor)
            {
                case { BoundIdentifier: { } bound }:
                {
                    //6.6.3.7.1
                    BoundIdentifierSymbol? symbol = Resolve<BoundIdentifierSymbol>(bound.Identifier, "a bound-identifier");
                    result = symbol != null ? symbol.Type : ErrorResolvedType.ErrorRType;
                    bound.ResolvedType = result;
                    break;
                }
                case { VariableAccess: { } access }:
                    result = TypeOfVariableAccess(access);
                    break;
                case { UnsignedConstant: { } constant }:
                    result = EvaluateUnsignedConstant(constant)?.Type ?? ErrorResolvedType.ErrorRType;
                    break;
                case { FunctionDesignator: { } call }:
                    result = TypeOfFunctionDesignator(call);
                    break;
                case { SetConstructor: { } set }:
                    result = TypeOfSetConstructor(set);
                    break;
                case { Expression: { } inner }:
                    result = TypeOfExpression(inner); //6.7.1: a parenthesized expression has the type of the expression
                    break;
                case { NottedFactor: { } operand }:
                {
                    //6.7.2.3: the operand shall be Boolean, the result is Boolean
                    ResolvedType inner = TypeOfFactor(operand);
                    if (inner is not ErrorResolvedType && !IsBoolean(inner))
                        Error(factor, $"The operand of 'not' shall be Boolean, not {inner} (6.7.2.3).");
                    result = BooleanResolvedType.BooleanRType;
                    break;
                }
                default:
                    throw new InvalidOperationException($"Factor at file position {factor.FileLocation} has no alternative set (compiler bug).");
            }
            factor.ResolvedType = result;
            return result;
        }
        /// <summary>6.7.3: the identifier denotes a function; its actual-parameters are checked and its result type is the factor's type (for abs, sqr, succ and pred that of the argument).</summary>
        private ResolvedType TypeOfFunctionDesignator(FunctionDesignator designator)
        {
            FunctionSymbol? function = Resolve<FunctionSymbol>(designator.FunctionIdentifier.Identifier, "a function");
            ResolvedType result;
            if (function == null)
                result = ErrorResolvedType.ErrorRType;
            else if (function.Required != null)
                result = TypeOfRequiredFunction(function, designator);
            else
            {
                CheckActualParameters(function, designator.ActualParameterList, designator);
                result = function.ResultType ?? ErrorResolvedType.ErrorRType;
            }
            designator.ResolvedType = result;
            return result;
        }
        /// <summary>6.6.6: the required functions, whose parameters follow rules of their own rather than a formal-parameter-list.</summary>
        private ResolvedType TypeOfRequiredFunction(FunctionSymbol function, FunctionDesignator designator)
        {
            RequiredFunction required = function.Required!.Value;
            List<ActualParameter> actuals = Actuals(designator.ActualParameterList);
            if (required is RequiredFunction.Eof or RequiredFunction.Eoln)
            {
                //6.6.6.5: an optional file-variable (a textfile for eoln); input when absent
                if (actuals.Count == 0)
                    RequireProgramParameter("input", designator, "6.6.6.5");
                else if (actuals.Count > 1)
                    Error(designator, $"{function.Name} takes at most one parameter, a file-variable (6.6.6.5).");
                else if (Reading(actuals[0], a => a.VariableAccess) is VariableAccess file)
                {
                    ResolvedType type = TypeOfVariableAccess(file);
                    bool acceptable = required == RequiredFunction.Eoln ? type is TextResolvedType : type is FileResolvedType;
                    if (!acceptable && type is not ErrorResolvedType)
                        Error(actuals[0], $"The parameter of {function.Name} shall be a {(required == RequiredFunction.Eoln ? "textfile" : "file-variable")}, not a variable of type {type} (6.6.6.5).");
                }
                else
                    Error(actuals[0], $"The parameter of {function.Name} shall be a file-variable (6.6.6.5).");
                return BooleanResolvedType.BooleanRType;
            }
            ResolvedType fallback = function.ResultType ?? ErrorResolvedType.ErrorRType;
            if (actuals.Count != 1)
            {
                Error(designator, $"{function.Name} takes exactly one parameter (6.6.6).");
                return fallback;
            }
            Expression? expression = Reading(actuals[0], a => a.Expression);
            if (expression == null)
            {
                Error(actuals[0], $"The parameter of {function.Name} shall be an expression (6.6.6).");
                return fallback;
            }
            ResolvedType argument = TypeOfExpression(expression);
            if (argument is ErrorResolvedType)
                return fallback;
            switch (required)
            {
                case RequiredFunction.Abs:
                case RequiredFunction.Sqr:
                    //6.6.6.2: the result has the type of the parameter
                    if (IsNumeric(argument))
                        return IsInteger(argument) ? IntResolvedType.IntRType : RealResolvedType.RealRType;
                    Error(expression, $"The parameter of {function.Name} shall be of integer-type or real-type, not {argument} (6.6.6.2).");
                    return ErrorResolvedType.ErrorRType;
                case RequiredFunction.Sin:
                case RequiredFunction.Cos:
                case RequiredFunction.Exp:
                case RequiredFunction.Ln:
                case RequiredFunction.Sqrt:
                case RequiredFunction.Arctan:
                    if (!IsNumeric(argument))
                        Error(expression, $"The parameter of {function.Name} shall be of integer-type or real-type, not {argument} (6.6.6.2).");
                    return RealResolvedType.RealRType;
                case RequiredFunction.Trunc:
                case RequiredFunction.Round:
                    if (argument is not RealResolvedType)
                        Error(expression, $"The parameter of {function.Name} shall be of real-type, not {argument} (6.6.6.3).");
                    return IntResolvedType.IntRType;
                case RequiredFunction.Ord:
                    if (argument is not OrdinalResolvedType)
                        Error(expression, $"The parameter of ord shall be of an ordinal-type, not {argument} (6.6.6.4).");
                    return IntResolvedType.IntRType;
                case RequiredFunction.Chr:
                    if (!IsInteger(argument))
                        Error(expression, $"The parameter of chr shall be of integer-type, not {argument} (6.6.6.4).");
                    return CharResolvedType.CharRType;
                case RequiredFunction.Succ:
                case RequiredFunction.Pred:
                    //6.6.6.4: the result is of the parameter's type, which 6.7.1 takes as the host of a subrange
                    if (argument is OrdinalResolvedType ordinal)
                        return ordinal.Host;
                    Error(expression, $"The parameter of {function.Name} shall be of an ordinal-type, not {argument} (6.6.6.4).");
                    return ErrorResolvedType.ErrorRType;
                case RequiredFunction.Odd:
                    if (!IsInteger(argument))
                        Error(expression, $"The parameter of odd shall be of integer-type, not {argument} (6.6.6.5).");
                    return BooleanResolvedType.BooleanRType;
                default:
                    throw new InvalidOperationException($"'{function.Name}' is not a required function (compiler bug).");
            }
        }
        /// <summary>6.7.1: [] has EmptySetResolvedType; otherwise the expressions of every member-designator share one ordinal type T and the constructor has T's canonical set type.</summary>
        private ResolvedType TypeOfSetConstructor(SetConstructor constructor)
        {
            ResolvedType result;
            if (constructor.MemberDesignator == null)
                result = EmptySetResolvedType.EmptySetRType; //6.7.1: [] is a value of every set-type
            else
            {
                OrdinalResolvedType? baseType = null;
                bool failed = false;
                foreach (MemberDesignator member in (constructor.SecondaryMemberDesignators ?? []).Select(x => x.memberDesignator).Prepend(constructor.MemberDesignator))
                {
                    IEnumerable<Expression> expressions = member.SecondExpression == null ? new[] { member.Expression } : new[] { member.Expression, member.SecondExpression };
                    foreach (Expression expression in expressions)
                    {
                        ResolvedType type = TypeOfExpression(expression);
                        if (type is ErrorResolvedType)
                            failed = true;
                        else if (type is not OrdinalResolvedType ordinal)
                        {
                            Error(expression, $"The members of a set-constructor shall be of an ordinal-type, not {type} (6.7.1).");
                            failed = true;
                        }
                        else if (baseType == null)
                            baseType = ordinal.Host;
                        else if (!Compatible(baseType, ordinal))
                        {
                            Error(expression, $"The members of a set-constructor shall all be of one type; {type} is not compatible with {baseType} (6.7.1).");
                            failed = true;
                        }
                    }
                    member.ResolvedType = baseType;
                }
                result = failed || baseType == null ? ErrorResolvedType.ErrorRType : baseType.CanonicalSet(null); //packed or not as the context requires
            }
            constructor.ResolvedType = result;
            return result;
        }
        #endregion

        #region 6.8 Statements
        /// <summary>6.8.1: a label prefix places the label's statement (6.2.1); then the simple or structured statement.</summary>
        private void CheckStatement(Statement statement)
        {
            if (statement.Label != null)
            {
                long? value = statement.Label.DigitSequence.AsLong;
                if (value != null && scope.EnclosingBlock.LookupLocal(value.Value.ToString()) is LabelSymbol label)
                {
                    if (label.LabeledStatement == null)
                        label.LabeledStatement = statement;
                    else
                        Error(statement.Label, $"Label {label.Name} prefixes more than one statement of its block (6.2.1).");
                    statement.Label.Symbol = label;
                }
                else
                    Error(statement.Label, $"Label {statement.Label.DigitSequence.Digits} is not declared in the label-declaration-part of this block (6.2.1).");
            }
            enclosingStatements.Push(statement);
            switch (statement)
            {
                case { SimpleStatement: { EmptyStatement: not null } }: break; //6.8.2.1: nothing to check
                case { SimpleStatement: { AssignmentStatement: { } assignment } }: CheckAssignmentStatement(assignment); break;
                case { SimpleStatement: { ProcedureStatement: { } call } }: CheckProcedureStatement(call); break;
                case { SimpleStatement: { GotoStatement: { } jump } }: CheckGotoStatement(jump); break;
                case { StructuredStatement: { CompoundStatement: { } compound } }: CheckCompoundStatement(compound); break;
                case { StructuredStatement: { ConditionalStatement: { IfStatement: { } conditional } } }: CheckIfStatement(conditional); break;
                case { StructuredStatement: { ConditionalStatement: { CaseStatement: { } selection } } }: CheckCaseStatement(selection); break;
                case { StructuredStatement: { RepetitiveStatement: { RepeatStatement: { } loop } } }: CheckRepeatStatement(loop); break;
                case { StructuredStatement: { RepetitiveStatement: { WhileStatement: { } loop } } }: CheckWhileStatement(loop); break;
                case { StructuredStatement: { RepetitiveStatement: { ForStatement: { } loop } } }: CheckForStatement(loop); break;
                case { StructuredStatement: { WithStatement: { } with } }: CheckWithStatement(with); break;
                default: throw new InvalidOperationException($"Statement at file position {statement.FileLocation} has no alternative set (compiler bug).");
            }
            enclosingStatements.Pop();
        }
        /// <summary>6.8.3.2: each statement of the statement-sequence.</summary>
        private void CheckCompoundStatement(CompoundStatement statement)
        {
            CheckStatementSequence(statement.StatementSequence);
        }
        /// <summary>6.8.3.1: each statement of a statement-sequence in order; compound-statements and repeat-statements are built from one.</summary>
        private void CheckStatementSequence(StatementSequence sequence)
        {
            enclosingSequences.Push(sequence);
            foreach (Statement statement in Statements(sequence))
                CheckStatement(statement);
            enclosingSequences.Pop();
        }
        /// <summary>The Routine scope of the given routine among those enclosing the current one, or null: whether we are inside its block.</summary>
        private Scope? EnclosingRoutineScope(RoutineSymbol routine)
        {
            for (Scope? s = scope; s != null; s = s.Parent)
                if (s.Kind == ScopeKind.Routine && s.Routine == routine)
                    return s;
            return null;
        }
        /// <summary>6.8.2.2: the target is a variable-access or the identifier of the function being defined (that ambiguity); the expression is assignment-compatible with it.</summary>
        private void CheckAssignmentStatement(AssignmentStatement statement)
        {
            if (statement is AssignmentStatement.Ambiguous ambiguous)
            {
                //"f := e" with a bare identifier: the function reading only when f is a function whose block contains this statement
                AssignmentStatement variableReading = ambiguous.Candidates.First(c => c.VariableAccess != null);
                AssignmentStatement functionReading = ambiguous.Candidates.First(c => c.FunctionIdentifier != null);
                statement = scope.Lookup(functionReading.FunctionIdentifier!.Identifier.Name) is FunctionSymbol function && EnclosingRoutineScope(function) != null ? functionReading : variableReading;
                ambiguous.Resolved = statement;
            }
            ResolvedType target;
            if (statement.VariableAccess is VariableAccess access)
            {
                target = TypeOfVariableAccess(access);
                if (EntireVariableOf(access) is VariableSymbol variable)
                    Threaten(variable, statement); //6.8.3.9 a)
            }
            else
            {
                //6.8.2.2: the function-identifier denotes a function whose function-block contains this statement
                Identifier identifier = statement.FunctionIdentifier!.Identifier;
                FunctionSymbol? function = Resolve<FunctionSymbol>(identifier, "a function");
                if (function != null && EnclosingRoutineScope(function) == null)
                {
                    Error(identifier, $"The function '{function.Name}' can only be assigned its result within its own function-block (6.8.2.2).");
                    function = null;
                }
                if (function != null)
                    function.IsResultAssigned = true;
                target = function?.ResultType ?? ErrorResolvedType.ErrorRType;
            }
            ResolvedType value = TypeOfExpression(statement.Expression);
            if (target is not ErrorResolvedType && value is not ErrorResolvedType && !AssignmentCompatible(target, value))
                Error(statement, $"A value of type {value} is not assignment-compatible with the target of type {target} (6.8.2.2, 6.4.6).");
        }
        /// <summary>6.8.2.3: the identifier denotes a procedure; a declared one takes an actual-parameter-list, a required one follows its own rules, which also settles the parameter-list ambiguity.</summary>
        private void CheckProcedureStatement(ProcedureStatement statement)
        {
            Identifier procedureIdentifier = (statement is ProcedureStatement.Ambiguous packed ? packed.Candidates[0] : statement).ProcedureIdentifier.Identifier; //a placeholder's own slots are unset
            ProcedureSymbol? procedure = Resolve<ProcedureSymbol>(procedureIdentifier, "a procedure");
            if (statement is ProcedureStatement.Ambiguous ambiguous)
            {
                //the readings differ in which parameter list they parsed; the procedure decides which applies
                Func<ProcedureStatement, bool> wanted = procedure?.Required switch
                {
                    RequiredProcedure.Read => c => c.ReadParameterList != null,
                    RequiredProcedure.Readln => c => c.ReadlnParameterList != null,
                    RequiredProcedure.Write => c => c.WriteParameterList != null,
                    RequiredProcedure.Writeln => c => c.WritelnParameterList != null,
                    _ => c => c.ReadParameterList == null && c.ReadlnParameterList == null && c.WriteParameterList == null && c.WritelnParameterList == null
                };
                ProcedureStatement? chosen = ambiguous.Candidates.FirstOrDefault(wanted);
                if (chosen == null)
                {
                    if (procedure != null)
                        Error(statement, $"The parameter list of this statement is not one that '{procedure.Name}' accepts (6.8.2.3, 6.9).");
                    return;
                }
                ambiguous.Resolved = chosen;
                statement = chosen;
            }
            if (procedure == null)
                return;
            statement.ProcedureIdentifier.Identifier.Symbol = procedure;
            if (procedure.Required != null)
                CheckRequiredProcedureStatement(procedure, statement);
            else
                CheckActualParameters(procedure, statement.ActualParameterList, statement);
        }
        /// <summary>6.6.5, 6.9: the required procedures; read and readln take a file and variables its components can be read into, write and writeln take write-parameters whose widths need a textfile, page takes a textfile.</summary>
        private void CheckRequiredProcedureStatement(ProcedureSymbol procedure, ProcedureStatement statement)
        {
            List<ActualParameter> actuals = Actuals(statement.ActualParameterList);
            switch (procedure.Required)
            {
                case RequiredProcedure.Rewrite:
                case RequiredProcedure.Put:
                case RequiredProcedure.Reset:
                case RequiredProcedure.Get:
                    //6.6.5.2
                    if (actuals.Count != 1)
                        Error(statement, $"{procedure.Name} takes exactly one parameter, a file-variable (6.6.5.2).");
                    else
                        TypeOfFileActual(actuals[0], procedure.Name, "6.6.5.2");
                    break;
                case RequiredProcedure.New:
                case RequiredProcedure.Dispose:
                    CheckAllocation(procedure, statement, actuals);
                    break;
                case RequiredProcedure.Pack:
                case RequiredProcedure.Unpack:
                    CheckTransfer(procedure, statement, actuals);
                    break;
                case RequiredProcedure.Read:
                case RequiredProcedure.Readln:
                    CheckReadParameters(procedure, statement);
                    break;
                case RequiredProcedure.Write:
                case RequiredProcedure.Writeln:
                    CheckWriteParameters(procedure, statement);
                    break;
                case RequiredProcedure.Page:
                    //6.9.5: no parameter (output) or one textfile
                    if (actuals.Count == 0)
                        RequireProgramParameter("output", statement, "6.9.5");
                    else if (actuals.Count > 1)
                        Error(statement, "page takes at most one parameter, a textfile (6.9.5).");
                    else if (TypeOfFileActual(actuals[0], procedure.Name, "6.9.5") is FileResolvedType and not TextResolvedType)
                        Error(actuals[0], "The parameter of page shall be a textfile (6.9.5).");
                    break;
                default:
                    throw new InvalidOperationException($"'{procedure.Name}' is not a required procedure (compiler bug).");
            }
        }
        /// <summary>An actual that shall be a file-variable: its type, or ErrorRType after a message.</summary>
        private ResolvedType TypeOfFileActual(ActualParameter actual, string procedure, string clause)
        {
            if (Reading(actual, a => a.VariableAccess) is not VariableAccess access)
            {
                Error(actual, $"The parameter of {procedure} shall be a file-variable ({clause}).");
                return ErrorResolvedType.ErrorRType;
            }
            ResolvedType type = TypeOfVariableAccess(access);
            if (type is FileResolvedType or ErrorResolvedType)
                return type;
            Error(actual, $"The parameter of {procedure} shall be a file-variable, not a variable of type {type} ({clause}).");
            return ErrorResolvedType.ErrorRType;
        }
        /// <summary>The constant an expression plainly is (a literal or constant-identifier factor, possibly signed), or null; what new and dispose need of their case-constants (6.6.5.3).</summary>
        private static ConstantValue? ConstantValueOf(Expression expression)
        {
            if (expression is not { RelationalOperator: null, SimpleExpression: { SecondaryTerms.Count: 0, Term: { SecondaryFactors.Count: 0 } term } simple })
                return null;
            if (Ambiguity.Resolution(term.Factor) is not Factor { UnsignedConstant: { Value: { } value } })
                return null;
            if (simple.Sign?.MinusToken == null)
                return value;
            return value switch
            {
                OrdinalConstantValue { Type: IntResolvedType } integer => new OrdinalConstantValue(IntResolvedType.IntRType, -integer.Ordinal),
                RealConstantValue real => new RealConstantValue(-real.Value),
                _ => null
            };
        }
        /// <summary>6.6.5.3: new(p) and dispose(q) take a pointer; with case-constants they select successively nested variants of the record the pointer's domain is.</summary>
        private void CheckAllocation(ProcedureSymbol procedure, ProcedureStatement statement, List<ActualParameter> actuals)
        {
            if (actuals.Count == 0)
            {
                Error(statement, $"{procedure.Name} takes a pointer parameter, optionally followed by case-constants (6.6.5.3).");
                return;
            }
            ResolvedType pointerType;
            if (procedure.Required == RequiredProcedure.New)
            {
                if (Reading(actuals[0], a => a.VariableAccess) is not VariableAccess access)
                {
                    Error(actuals[0], "The first parameter of new shall be a variable-access of a pointer-type (6.6.5.3).");
                    return;
                }
                pointerType = TypeOfVariableAccess(access);
                if (EntireVariableOf(access) is VariableSymbol variable)
                    Threaten(variable, statement); //new attributes a value to it, as an assignment would
            }
            else
            {
                if (Reading(actuals[0], a => a.Expression) is not Expression expression)
                {
                    Error(actuals[0], "The first parameter of dispose shall be an expression of a pointer-type (6.6.5.3).");
                    return;
                }
                pointerType = TypeOfExpression(expression);
            }
            if (pointerType is ErrorResolvedType)
                return;
            if (pointerType is not PointerResolvedType pointer)
            {
                Error(actuals[0], $"The first parameter of {procedure.Name} shall be of a pointer-type, not {pointerType} (6.6.5.3).");
                return;
            }
            if (actuals.Count == 1)
                return;
            RecordResolvedType.VariantPart? variantPart = (pointer.Domain as RecordResolvedType)?.Fields.VariantPart;
            if (variantPart == null)
            {
                Error(actuals[1], $"{procedure.Name} with case-constants requires a pointer to a record-type with a variant-part; the domain here is {pointer.Domain} (6.6.5.3).");
                return;
            }
            foreach (ActualParameter actual in actuals.Skip(1))
            {
                if (variantPart == null)
                {
                    Error(actual, "More case-constants are given than there are nested variant-parts to select (6.6.5.3).");
                    break;
                }
                Expression? expression = Reading(actual, a => a.Expression);
                ConstantValue? value = expression == null ? null : (TypeOfExpression(expression) is ErrorResolvedType ? null : ConstantValueOf(expression));
                if (expression != null && expression.ResolvedType is ErrorResolvedType)
                    break;
                if (value is not OrdinalConstantValue ordinal || !Compatible(ordinal.Type, variantPart.TagType))
                {
                    Error(actual, $"A case-constant of {procedure.Name} shall be a constant of a type compatible with the tag-type {variantPart.TagType} (6.6.5.3).");
                    break;
                }
                RecordResolvedType.Variant? variant = variantPart.Variants.FirstOrDefault(v => v.CaseConstants.Contains(ordinal.Ordinal));
                if (variant == null)
                {
                    Error(actual, $"No variant has the case-constant {ordinal} (6.6.5.3).");
                    break;
                }
                variantPart = variant.FieldList.VariantPart;
            }
        }
        /// <summary>6.6.5.4: pack(a, i, z) and unpack(z, a, i): a of an unpacked array-type, z of a packed one with the same component-type, i assignment-compatible with a's index-type.</summary>
        private void CheckTransfer(ProcedureSymbol procedure, ProcedureStatement statement, List<ActualParameter> actuals)
        {
            if (actuals.Count != 3)
            {
                Error(statement, $"{procedure.Name} takes exactly three parameters (6.6.5.4).");
                return;
            }
            bool isPack = procedure.Required == RequiredProcedure.Pack;
            ActualParameter unpackedActual = isPack ? actuals[0] : actuals[1], indexActual = isPack ? actuals[1] : actuals[2], packedActual = isPack ? actuals[2] : actuals[0];
            ResolvedType unpackedType = Reading(unpackedActual, a => a.VariableAccess) is VariableAccess ua ? TypeOfVariableAccess(ua) : ErrorResolvedType.ErrorRType;
            ResolvedType packedType = Reading(packedActual, a => a.VariableAccess) is VariableAccess pa ? TypeOfVariableAccess(pa) : ErrorResolvedType.ErrorRType;
            ResolvedType indexType = Reading(indexActual, a => a.Expression) is Expression ie ? TypeOfExpression(ie) : ErrorResolvedType.ErrorRType;
            if (isPack && Reading(packedActual, a => a.VariableAccess) is VariableAccess target && EntireVariableOf(target) is VariableSymbol packedVariable)
                Threaten(packedVariable, statement);
            if (!isPack && Reading(unpackedActual, a => a.VariableAccess) is VariableAccess target2 && EntireVariableOf(target2) is VariableSymbol unpackedVariable)
                Threaten(unpackedVariable, statement);
            ArrayResolvedType? unpacked = unpackedType as ArrayResolvedType, packed = packedType as ArrayResolvedType;
            if (unpackedType is not ErrorResolvedType && unpacked is not { IsPacked: false })
                Error(unpackedActual, $"This parameter of {procedure.Name} shall be a variable of an array-type not designated packed, not {unpackedType} (6.6.5.4).");
            if (packedType is not ErrorResolvedType && packed is not { IsPacked: true })
                Error(packedActual, $"This parameter of {procedure.Name} shall be a variable of an array-type designated packed, not {packedType} (6.6.5.4).");
            if (unpacked is { IsPacked: false } && packed is { IsPacked: true })
            {
                if (!ReferenceEquals(unpacked.Component, packed.Component))
                    Error(statement, $"The two arrays of {procedure.Name} shall have the same component-type; these have {unpacked.Component} and {packed.Component} (6.6.5.4).");
                if (packed.ComponentCount > unpacked.ComponentCount)
                    Error(statement, $"The packed array of {procedure.Name} shall not have more components than the unpacked one (6.6.5.4).");
                if (indexType is not ErrorResolvedType && !AssignmentCompatible(unpacked.Index, indexType))
                    Error(indexActual, $"The index parameter of {procedure.Name} shall be assignment-compatible with the index-type {unpacked.Index}, not of type {indexType} (6.6.5.4).");
            }
        }
        /// <summary>6.9.1, 6.9.2: the read-parameter-list, with the leading file-variable ambiguity settled by whether the first item is a file.</summary>
        private void CheckReadParameters(ProcedureSymbol procedure, ProcedureStatement statement)
        {
            bool isReadln = procedure.Required == RequiredProcedure.Readln;
            string clause = isReadln ? "6.9.2" : "6.9.1";
            FileVariable? fileVariable;
            List<VariableAccess> variables;
            if (statement.ReadParameterList is ReadParameterList readList)
            {
                if (readList is ReadParameterList.Ambiguous ambiguous)
                {
                    ReadParameterList without = ambiguous.Candidates.First(c => c.FileVariable == null);
                    readList = TypeOfVariableAccess(without.VariableAccess) is FileResolvedType ? ambiguous.Candidates.First(c => c.FileVariable != null) : without;
                    ambiguous.Resolved = readList;
                }
                fileVariable = readList.FileVariable;
                variables = readList.SecondaryVariableAccesses.Select(x => x.variableAccess).Prepend(readList.VariableAccess).ToList();
            }
            else if (statement.ReadlnParameterList is ReadlnParameterList readlnList)
            {
                if (readlnList is ReadlnParameterList.Ambiguous ambiguous)
                {
                    ReadlnParameterList without = ambiguous.Candidates.First(c => c.FileVariable == null);
                    readlnList = without.VariableAccess != null && TypeOfVariableAccess(without.VariableAccess) is FileResolvedType ? ambiguous.Candidates.First(c => c.FileVariable != null) : without;
                    ambiguous.Resolved = readlnList;
                }
                fileVariable = readlnList.FileVariable;
                //readln-parameter-list = [ '(' ( file-variable | variable-access ) { ',' variable-access } ')' ]: after a file-variable, every variable is a secondary one
                IEnumerable<VariableAccess> rest = (readlnList.SecondaryVariableAccesses ?? []).Select(x => x.variableAccess);
                variables = (readlnList.VariableAccess == null ? rest : rest.Prepend(readlnList.VariableAccess)).ToList();
            }
            else
            {
                Error(statement, $"{procedure.Name} requires a read-parameter-list ({clause}).");
                return;
            }
            ResolvedType file = TextResolvedType.TextRType;
            if (fileVariable == null)
                RequireProgramParameter("input", statement, clause);
            else
            {
                file = TypeOfVariableAccess(fileVariable.VariableAccess);
                fileVariable.ResolvedType = file;
                if (file is not FileResolvedType and not ErrorResolvedType)
                {
                    Error(fileVariable, $"The first parameter of {procedure.Name} shall be a file-variable when it is not read into; this one is of type {file} ({clause}).");
                    file = ErrorResolvedType.ErrorRType;
                }
                else if (isReadln && file is not TextResolvedType and not ErrorResolvedType)
                    Error(fileVariable, "readln applies only to textfiles (6.9.2).");
            }
            foreach (VariableAccess access in variables)
            {
                ResolvedType type = TypeOfVariableAccess(access);
                if (EntireVariableOf(access) is VariableSymbol variable)
                    Threaten(variable, access); //6.8.3.9 c)
                if (type is ErrorResolvedType || file is ErrorResolvedType)
                    continue;
                if (file is TextResolvedType)
                {
                    if (!(type is RealResolvedType || IsString(type) || IsChar(type) || IsInteger(type)))
                        Error(access, $"A variable read from a textfile shall be of real-type, of a string-type, or compatible with char or integer, not of type {type} (6.9.1).");
                }
                else if (file is FileResolvedType other && !AssignmentCompatible(type, other.Component))
                    Error(access, $"The file's component-type {other.Component} is not assignment-compatible with a variable of type {type} (6.6.5.2, 6.9.1).");
            }
        }
        /// <summary>6.9.3, 6.9.4: the write-parameter-list, with the leading file-variable ambiguity settled by whether the first item is a file.</summary>
        private void CheckWriteParameters(ProcedureSymbol procedure, ProcedureStatement statement)
        {
            bool isWriteln = procedure.Required == RequiredProcedure.Writeln;
            string clause = isWriteln ? "6.9.4" : "6.9.3";
            FileVariable? fileVariable;
            List<WriteParameter> parameters;
            if (statement.WriteParameterList is WriteParameterList writeList)
            {
                if (writeList is WriteParameterList.Ambiguous ambiguous)
                {
                    WriteParameterList without = ambiguous.Candidates.First(c => c.FileVariable == null);
                    writeList = TypeOfExpression(without.WriteParameter.Expression) is FileResolvedType ? ambiguous.Candidates.First(c => c.FileVariable != null) : without;
                    ambiguous.Resolved = writeList;
                }
                fileVariable = writeList.FileVariable;
                parameters = writeList.SecondaryWriteParameters.Select(x => x.writeParameter).Prepend(writeList.WriteParameter).ToList();
            }
            else if (statement.WritelnParameterList is WritelnParameterList writelnList)
            {
                if (writelnList is WritelnParameterList.Ambiguous ambiguous)
                {
                    WritelnParameterList without = ambiguous.Candidates.First(c => c.FileVariable == null);
                    writelnList = without.WriteParameter != null && TypeOfExpression(without.WriteParameter.Expression) is FileResolvedType ? ambiguous.Candidates.First(c => c.FileVariable != null) : without;
                    ambiguous.Resolved = writelnList;
                }
                fileVariable = writelnList.FileVariable;
                //writeln-parameter-list = [ '(' ( file-variable | write-parameter ) { ',' write-parameter } ')' ]: after a file-variable, every write-parameter is a secondary one
                IEnumerable<WriteParameter> rest = (writelnList.SecondaryWriteParameters ?? []).Select(x => x.writeParameter);
                parameters = (writelnList.WriteParameter == null ? rest : rest.Prepend(writelnList.WriteParameter)).ToList();
            }
            else
            {
                Error(statement, $"{procedure.Name} requires a write-parameter-list ({clause}).");
                return;
            }
            ResolvedType file = TextResolvedType.TextRType;
            if (fileVariable == null)
                RequireProgramParameter("output", statement, clause);
            else
            {
                file = TypeOfVariableAccess(fileVariable.VariableAccess);
                fileVariable.ResolvedType = file;
                if (file is not FileResolvedType and not ErrorResolvedType)
                {
                    Error(fileVariable, $"The first parameter of {procedure.Name} shall be a file-variable when it is not written; this one is of type {file} ({clause}).");
                    file = ErrorResolvedType.ErrorRType;
                }
                else if (isWriteln && file is not TextResolvedType and not ErrorResolvedType)
                    Error(fileVariable, "writeln applies only to textfiles (6.9.4).");
            }
            foreach (WriteParameter parameter in parameters)
            {
                ResolvedType type = TypeOfExpression(parameter.Expression);
                ResolvedType? width = parameter.SecondExpression == null ? null : TypeOfExpression(parameter.SecondExpression);
                ResolvedType? fraction = parameter.ThirdExpression == null ? null : TypeOfExpression(parameter.ThirdExpression);
                if (type is ErrorResolvedType || file is ErrorResolvedType)
                    continue;
                if (file is TextResolvedType)
                {
                    //6.9.3.1: e, e:TotalWidth or e:TotalWidth:FracDigits
                    if (!(IsInteger(type) || type is RealResolvedType || IsChar(type) || IsBoolean(type) || IsString(type)))
                        Error(parameter, $"A value written to a textfile shall be of integer-type, real-type, char-type, Boolean-type or a string-type, not {type} (6.9.3.1).");
                    if (width is not null and not ErrorResolvedType && !IsInteger(width))
                        Error(parameter.SecondExpression!, $"TotalWidth shall be of integer-type, not {width} (6.9.3.1).");
                    if (fraction != null)
                    {
                        if (type is not RealResolvedType)
                            Error(parameter.ThirdExpression!, "FracDigits may only be given for a value of real-type (6.9.3.1).");
                        else if (fraction is not ErrorResolvedType && !IsInteger(fraction))
                            Error(parameter.ThirdExpression!, $"FracDigits shall be of integer-type, not {fraction} (6.9.3.1).");
                    }
                }
                else if (file is FileResolvedType other)
                {
                    if (width != null)
                        Error(parameter, "Field widths apply only when writing to a textfile (6.9.3).");
                    if (!AssignmentCompatible(other.Component, type))
                        Error(parameter, $"A value of type {type} is not assignment-compatible with the file's component-type {other.Component} (6.6.5.2, 6.9.3).");
                }
            }
        }
        /// <summary>6.8.2.4: the label is declared in this or an enclosing block; where the jump may land (6.8.1) is checked in that block's sweep, once the labeled statement is known.</summary>
        private void CheckGotoStatement(GotoStatement statement)
        {
            long? value = statement.Label.DigitSequence.AsLong;
            if (value != null && scope.Lookup(value.Value.ToString()) is LabelSymbol label)
            {
                statement.Label.Symbol = label;
                pendingGotos.Add((label, statement, enclosingStatements.ToArray(), enclosingSequences.ToArray()));
            }
            else
                Error(statement.Label, $"Label {statement.Label.DigitSequence.Digits} is not declared in this block or an enclosing one (6.8.2.4, 6.2.1).");
        }
        /// <summary>6.8.3.4: a Boolean-expression and one or two statements.</summary>
        private void CheckIfStatement(IfStatement statement)
        {
            CheckBooleanExpression(statement.BooleanExpression);
            CheckStatement(statement.Statement);
            if (statement.ElsePart != null)
                CheckStatement(statement.ElsePart.Statement);
        }
        /// <summary>6.7.1: a Boolean-expression is an expression of Boolean-type; if, while and repeat are controlled by one (6.8.3.4, 6.8.3.7, 6.8.3.8).</summary>
        private void CheckBooleanExpression(BooleanExpression expression)
        {
            ResolvedType type = TypeOfExpression(expression.Expression);
            expression.ResolvedType = type;
            if (type is not ErrorResolvedType && !IsBoolean(type))
                Error(expression, $"The controlling expression shall be of Boolean-type, not {type} (6.7.1, 6.8.3).");
        }
        /// <summary>6.8.3.5: an ordinal case-index; case-constants of a compatible type, distinct across the whole statement.</summary>
        private void CheckCaseStatement(CaseStatement statement)
        {
            ResolvedType index = TypeOfExpression(statement.CaseIndex.Expression);
            statement.CaseIndex.ResolvedType = index;
            OrdinalResolvedType? ordinalIndex = index as OrdinalResolvedType;
            if (ordinalIndex == null && index is not ErrorResolvedType)
                Error(statement.CaseIndex, $"The case-index shall be of an ordinal-type, not {index} (6.8.3.5).");
            HashSet<long> seen = new();
            foreach (CaseListElement element in statement.SecondaryCaseListElements.Select(x => x.caseListElement).Prepend(statement.CaseListElement))
            {
                foreach (CaseConstant caseConstant in element.CaseConstantList.SecondaryCaseConstants.Select(x => x.caseConstant).Prepend(element.CaseConstantList.CaseConstant))
                {
                    ConstantValue? value = EvaluateConstant(caseConstant.Constant);
                    caseConstant.Value = value;
                    caseConstant.ResolvedType = value?.Type;
                    if (value is OrdinalConstantValue ordinal)
                    {
                        if (ordinalIndex != null && !Compatible(ordinal.Type, ordinalIndex))
                            Error(caseConstant, $"Case-constant {ordinal} is not of a type compatible with the case-index type {index} (6.8.3.5).");
                        else if (!seen.Add(ordinal.Ordinal))
                            Error(caseConstant, $"Case-constant {ordinal} occurs more than once in this case-statement (6.8.3.5).");
                    }
                    else if (value != null)
                        Error(caseConstant, $"A case-constant shall be of an ordinal-type, not {value.Type} (6.8.3.5).");
                }
                CheckStatement(element.Statement);
            }
        }
        /// <summary>6.8.3.7: a Boolean-expression after the statement-sequence.</summary>
        private void CheckRepeatStatement(RepeatStatement statement)
        {
            CheckStatementSequence(statement.StatementSequence);
            CheckBooleanExpression(statement.BooleanExpression);
        }
        /// <summary>6.8.3.8: a Boolean-expression before the statement.</summary>
        private void CheckWhileStatement(WhileStatement statement)
        {
            CheckBooleanExpression(statement.BooleanExpression);
            CheckStatement(statement.Statement);
        }
        /// <summary>6.8.3.9: the control-variable is an ordinal variable declared in this block's variable-declaration-part, the bounds are compatible with it, and nothing in the loop or in the block's routines threatens it.</summary>
        private void CheckForStatement(ForStatement statement)
        {
            Identifier identifier = statement.ControlVariable.EntireVariable.VariableIdentifier.Identifier;
            VariableSymbol? control = Resolve<VariableSymbol>(identifier, "a variable");
            ResolvedType controlType = control?.Type ?? ErrorResolvedType.ErrorRType;
            statement.ControlVariable.EntireVariable.VariableIdentifier.ResolvedType = controlType;
            statement.ControlVariable.EntireVariable.ResolvedType = controlType;
            statement.ControlVariable.ResolvedType = controlType;
            if (control != null)
            {
                if (control.Kind != VariableKind.Declared || control.Scope != scope.EnclosingBlock)
                    Error(identifier, $"The control-variable '{control.Name}' shall be declared in the variable-declaration-part of the block closest-containing the for-statement (6.8.3.9).");
                else if (controlType is not OrdinalResolvedType)
                    Error(identifier, $"The control-variable shall be of an ordinal-type, not {controlType} (6.8.3.9).");
                Threaten(control, statement); //the for-statement itself assigns its control-variable (6.8.3.9 d), so one nested in a loop over the same variable is caught here
            }
            ResolvedType initial = TypeOfExpression(statement.InitialValue.Expression);
            ResolvedType final = TypeOfExpression(statement.FinalValue.Expression);
            statement.InitialValue.ResolvedType = initial;
            statement.FinalValue.ResolvedType = final;
            if (controlType is OrdinalResolvedType)
            {
                if (initial is not ErrorResolvedType && !Compatible(controlType, initial))
                    Error(statement.InitialValue, $"The initial-value of type {initial} is not compatible with the control-variable's type {controlType} (6.8.3.9).");
                if (final is not ErrorResolvedType && !Compatible(controlType, final))
                    Error(statement.FinalValue, $"The final-value of type {final} is not compatible with the control-variable's type {controlType} (6.8.3.9).");
            }
            bool activated = control != null && activeControlVariables.Add(control);
            CheckStatement(statement.Statement);
            if (activated)
                activeControlVariables.Remove(control!);
            if (control != null)
            {
                //6.8.3.9: nor may any statement of the block's procedure-and-function-declaration-part threaten the control-variable; those were checked before this statement
                Scope block = scope.EnclosingBlock;
                foreach ((VariableSymbol variable, ASTNode at, Scope where) in threats)
                    if (variable == control && where.EnclosingBlock != block && IsWithin(where, block))
                        Error(at, $"This statement threatens '{control.Name}', the control-variable of a for-statement in the enclosing block (6.8.3.9).");
            }
        }
        /// <summary>6.8.3.10: each record-variable opens a With scope holding a FieldSymbol per field of its record type; the statement is checked inside the innermost.</summary>
        private void CheckWithStatement(WithStatement statement)
        {
            //"with v1, v2 do s" is "with v1 do with v2 do s", so each record-variable opens a scope the next is looked up in
            int opened = 0;
            foreach (RecordVariable recordVariable in statement.RecordVariableList.SecondaryRecordVariables.Select(x => x.recordVariable).Prepend(statement.RecordVariableList.RecordVariable))
            {
                ResolvedType type = TypeOfVariableAccess(recordVariable.VariableAccess);
                recordVariable.ResolvedType = type;
                scope = scope.Enter(ScopeKind.With);
                opened++;
                if (type is RecordResolvedType record)
                {
                    foreach (RecordResolvedType.Field field in record.AllFields)
                        scope.TryDeclare(new FieldSymbol(field, recordVariable, recordVariable), out _); //fields of one record are distinct, so this cannot fail
                }
                else if (type is not ErrorResolvedType)
                    Error(recordVariable, $"The record-variable of a with-statement shall be of a record-type, not {type} (6.8.3.10).");
            }
            CheckStatement(statement.Statement);
            for (; opened > 0; opened--)
                scope = scope.Parent!;
        }
        #endregion
    }
}
