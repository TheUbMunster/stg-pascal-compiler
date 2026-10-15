//Copyright TheUbMunster, 2024, all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace PascalCompiler.TypeChecker
{
    /// <summary>
    /// A type as the type checker understands it: the semantic counterpart of a type-denoter in the source (integer, a particular
    /// record type, a subrange, a set of some ordinal type, ...). Stored on the parse tree by the type checker (see TypedNode and
    /// TypeDenotingNode). 6.4 of the spec describes the kinds; 6.4.5 and 6.4.6 define the compatibility and assignment-compatibility
    /// relations, which are the two questions the checker asks instead of equality.
    ///
    /// Identity is by reference: ISO Pascal uses name equivalence (6.4.1, "each occurrence of a new-type shall denote a type that is
    /// distinct from any other new-type"), and a type-definition makes its identifier denote the same type object as its denoter, so
    /// nothing here overrides Equals. The parser owns the plain names (ArrayType, SubrangeType, ...), hence the ResolvedType suffix.
    /// </summary>
    public abstract class ResolvedType
    {
        /// <summary>
        /// The identifier of the first type-definition that denoted this type, or the required type-identifier; null while the type has
        /// only appeared anonymously. The checker sets it at a type-definition when it is still null; a later "type t = u" leaves "u" in
        /// place because t denotes the same type (6.4.1). ToString and diagnostics prefer it to the structure.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Whether this is a file-type, or a structured type with a file-type somewhere in its components. Such a type is not permissible
        /// as the component-type of a file-type (6.4.3.5), and its values cannot be assigned (6.4.6 a).
        /// </summary>
        public virtual bool ContainsFileType => false;

        /// <summary>The type written out structurally, as a type-denoter would; ToString falls back to it when there is no Name.</summary>
        protected abstract string Structure { get; }

        public override string ToString() => Name ?? Structure;

        /// <summary>
        /// 6.4.5: T1 and T2 are compatible if a) they are the same type, b) one is a subrange of the other or both are subranges of the
        /// same host-type, c) they are set-types of compatible base-types and both or neither is packed, or d) they are string-types with
        /// the same number of components. nil and [] do not have a type of their own but "assume a suitable type to satisfy ... the
        /// compatibility rules for operators" (6.4.4 note 2, 6.7.1 note 3), so they are compatible with every pointer-type and set-type.
        /// </summary>
        public static bool Compatible(ResolvedType t1, ResolvedType t2)
        {
            if (t1 is ErrorResolvedType || t2 is ErrorResolvedType)
                return true;
            if (ReferenceEquals(t1, t2))
                return true; //a)
            if (t1 is OrdinalResolvedType o1 && t2 is OrdinalResolvedType o2)
                return ReferenceEquals(o1.Host, o2.Host); //b): a type that is not a subrange is its own host, which covers both halves of the rule
            if (t1 is SetResolvedType s1 && t2 is SetResolvedType s2)
                return Compatible(s1.Base, s2.Base) && (s1.PackednessFromContext || s2.PackednessFromContext || s1.IsPacked == s2.IsPacked); //c)
            if (t1 is ArrayResolvedType a1 && t2 is ArrayResolvedType a2)
                return a1.IsStringType && a2.IsStringType && a1.ComponentCount == a2.ComponentCount; //d)
            if (t1 is EmptySetResolvedType || t2 is EmptySetResolvedType)
                return (t1 is SetResolvedType or EmptySetResolvedType) && (t2 is SetResolvedType or EmptySetResolvedType);
            if (t1 is NilResolvedType || t2 is NilResolvedType)
                return (t1 is PointerResolvedType or NilResolvedType) && (t2 is PointerResolvedType or NilResolvedType);
            return false;
        }

        /// <summary>
        /// 6.4.6: a value of type T2 is assignment-compatible with type T1 if a) they are the same type and it is permissible as the
        /// component-type of a file-type, b) T1 is real and T2 is integer, c) they are compatible ordinal-types, d) they are compatible
        /// set-types, or e) they are compatible string-types. Rules c) and d) also require the value to lie within T1 (within its base-type
        /// for d); that is a property of the value, which the checker tests for constants and the runtime for everything else, so it is
        /// not part of this predicate. nil is assignable to every pointer-type (6.4.4) and [] to every set-type (6.7.1).
        /// </summary>
        public static bool AssignmentCompatible(ResolvedType t1, ResolvedType t2)
        {
            if (t1 is ErrorResolvedType || t2 is ErrorResolvedType)
                return true;
            if (ReferenceEquals(t1, t2))
                return !t1.ContainsFileType; //a)
            if (t1 is RealResolvedType && t2 is IntResolvedType)
                return true; //b)
            if (t1 is OrdinalResolvedType && t2 is OrdinalResolvedType)
                return Compatible(t1, t2); //c)
            if (t1 is SetResolvedType && (t2 is SetResolvedType or EmptySetResolvedType))
                return Compatible(t1, t2); //d)
            if (t1 is ArrayResolvedType && t2 is ArrayResolvedType)
                return Compatible(t1, t2); //e): two array types are only ever compatible as string-types
            if (t1 is PointerResolvedType && t2 is NilResolvedType)
                return true;
            return false;
        }
    }

    /// <summary>
    /// An ordinal-type (6.4.2.1): integer, Boolean, char, an enumerated-type, or a subrange of one of those. Every value has an ordinal
    /// number (6.4.2.2, 6.4.2.3), and the type is the closed interval Low..High of those numbers.
    /// </summary>
    public abstract class OrdinalResolvedType : ResolvedType
    {
        private readonly SetResolvedType?[] canonicalSets = new SetResolvedType?[3];

        /// <summary>Ordinal number of the smallest value.</summary>
        public abstract long Low { get; }
        /// <summary>Ordinal number of the largest value.</summary>
        public abstract long High { get; }
        /// <summary>Number of values; the k of 6.4.3.2.</summary>
        public long Cardinality => High - Low + 1;
        public bool Includes(long ordinal) => Low <= ordinal && ordinal <= High;

        /// <summary>
        /// The host-type (6.4.2.4) of a subrange, the type itself otherwise. 6.7.1 treats a factor of a subrange type as being of its host,
        /// and 6.4.5 b) makes two ordinal-types compatible exactly when their hosts are the same type.
        /// </summary>
        public virtual OrdinalResolvedType Host => this;

        /// <summary>The source spelling of the value with this ordinal number: "65" for integer, "'A'" for char, the constant's identifier for an enumerated type.</summary>
        public abstract string ValueToString(long ordinal);

        /// <summary>
        /// The canonical set-of-T type of 6.4.3.4 for this type T (for its host, if this is a subrange): the unpacked one, the packed one,
        /// or, for null, the one 6.7.1 gives a set-constructor, which takes either packedness from its context. Set-constructors and the
        /// results of set operators have these types; a declared "set of T" is a distinct type that is merely compatible with them (6.4.5 c).
        /// </summary>
        public virtual SetResolvedType CanonicalSet(bool? packed)
        {
            int which = packed switch { null => 0, false => 1, true => 2 };
            return canonicalSets[which] ??= new SetResolvedType(this, packed ?? false, packednessFromContext: packed == null);
        }
    }

    /// <summary>
    /// integer (6.4.2.2 a): the ordinal number of a value is the value itself. maxint is implementation-defined; it is int.MaxValue here so
    /// that arithmetic on ordinal numbers, which are held as long, cannot overflow.
    /// </summary>
    public class IntResolvedType : OrdinalResolvedType
    {
        public const long MaxInt = int.MaxValue;
        public static readonly IntResolvedType IntRType = new IntResolvedType();
        private IntResolvedType() { Name = "integer"; }
        public override long Low => -MaxInt;
        public override long High => MaxInt;
        public override string ValueToString(long ordinal) => ordinal.ToString();
        protected override string Structure => "integer";
    }

    /// <summary>real (6.4.2.2 b): the only simple-type that is not an ordinal-type.</summary>
    public class RealResolvedType : ResolvedType
    {
        public static readonly RealResolvedType RealRType = new RealResolvedType();
        private RealResolvedType() { Name = "real"; }
        protected override string Structure => "real";
    }

    /// <summary>Boolean (6.4.2.2 c): false and true, with ordinal numbers 0 and 1.</summary>
    public class BooleanResolvedType : OrdinalResolvedType
    {
        public static readonly BooleanResolvedType BooleanRType = new BooleanResolvedType();
        private BooleanResolvedType() { Name = "Boolean"; }
        public override long Low => 0;
        public override long High => 1;
        public override string ValueToString(long ordinal) => ordinal == 0 ? "false" : "true";
        protected override string Structure => "Boolean";
    }

    /// <summary>
    /// char (6.4.2.2 d): an implementation-defined set of characters with consecutive ordinal numbers from zero. The set is taken to be
    /// every UTF-16 code unit, which keeps ord and chr the plain casts.
    /// </summary>
    public class CharResolvedType : OrdinalResolvedType
    {
        public static readonly CharResolvedType CharRType = new CharResolvedType();
        private CharResolvedType() { Name = "char"; }
        public override long Low => 0;
        public override long High => char.MaxValue;
        public override string ValueToString(long ordinal) => (char)ordinal == '\'' ? "''''" : $"'{(char)ordinal}'";
        protected override string Structure => "char";
    }

    /// <summary>
    /// An enumerated-type (6.4.2.3): its constants in order, with ordinal numbers 0, 1, ... in that order. The checker declares each
    /// constant as a symbol of this type in the enclosing block; the type keeps the identifiers for ToString and ValueToString.
    /// </summary>
    public class EnumeratedResolvedType : OrdinalResolvedType
    {
        public IReadOnlyList<string> Constants { get; }
        public EnumeratedResolvedType(IEnumerable<string> constants)
        {
            Constants = constants.ToArray();
        }
        public override long Low => 0;
        public override long High => Constants.Count - 1;
        public override string ValueToString(long ordinal) => Constants[(int)ordinal];
        protected override string Structure => $"({string.Join(", ", Constants)})";
    }

    /// <summary>
    /// A subrange-type (6.4.2.4): the values of its host-type whose ordinal numbers lie in Low..High. The bounds are constants, which never
    /// have a subrange type themselves, so a subrange given a subrange as host is normalized to a subrange of that host. That the first
    /// bound does not exceed the second and that both have one ordinal type is for the checker to report before constructing this.
    /// </summary>
    public class SubrangeResolvedType : OrdinalResolvedType
    {
        public override OrdinalResolvedType Host { get; }
        public override long Low { get; }
        public override long High { get; }
        public SubrangeResolvedType(OrdinalResolvedType host, long low, long high)
        {
            Host = host.Host;
            Low = low;
            High = high;
        }
        public override string ValueToString(long ordinal) => Host.ValueToString(ordinal);
        public override SetResolvedType CanonicalSet(bool? packed) => Host.CanonicalSet(packed);
        protected override string Structure => $"{Host.ValueToString(Low)}..{Host.ValueToString(High)}";
    }

    /// <summary>
    /// A structured-type (6.4.3): array, record, set or file, optionally designated packed. Packing only asks the processor to economize
    /// storage (6.4.3.1), but it is part of the type: set compatibility depends on it (6.4.5 c), a string-type must be packed (6.4.3.2),
    /// and no component of a packed variable may be passed as a variable parameter (6.6.3.3).
    /// </summary>
    public abstract class StructuredResolvedType : ResolvedType
    {
        public bool IsPacked { get; }
        protected StructuredResolvedType(bool isPacked)
        {
            IsPacked = isPacked;
        }
        protected string PackedPrefix => IsPacked ? "packed " : string.Empty;
    }

    /// <summary>
    /// An array-type (6.4.3.2): a mapping from each value of Index to a component of type Component. "array[a, b] of T" abbreviates
    /// "array[a] of array[b] of T", with packed propagating inward, so the checker builds nested instances. A packed array with index
    /// 1..n (n > 1) and component char is a string-type.
    /// </summary>
    public class ArrayResolvedType : StructuredResolvedType
    {
        private static readonly Dictionary<long, ArrayResolvedType> stringTypes = new();

        public OrdinalResolvedType Index { get; }
        public ResolvedType Component { get; }
        public ArrayResolvedType(OrdinalResolvedType index, ResolvedType component, bool isPacked) : base(isPacked)
        {
            Index = index;
            Component = component;
        }
        /// <summary>Number of components; the k of 6.4.3.2.</summary>
        public long ComponentCount => Index.Cardinality;
        public bool IsStringType => IsPacked && Component is CharResolvedType && Index is SubrangeResolvedType { Host: IntResolvedType, Low: 1, High: > 1 };
        public override bool ContainsFileType => Component.ContainsFileType;
        protected override string Structure => $"{PackedPrefix}array[{Index}] of {Component}";

        /// <summary>
        /// The string-type of a character-string with the given number of components (more than one; a single string-element is a char):
        /// packed array[1..componentCount] of char. 6.1.7 makes all character-strings of one length possess the same string-type, so the
        /// instance is created once per length.
        /// </summary>
        public static ArrayResolvedType StringType(long componentCount)
        {
            lock (stringTypes)
            {
                if (!stringTypes.TryGetValue(componentCount, out ArrayResolvedType? stringType))
                    stringTypes[componentCount] = stringType = new ArrayResolvedType(new SubrangeResolvedType(IntResolvedType.IntRType, 1, componentCount), CharResolvedType.CharRType, true);
                return stringType;
            }
        }
    }

    /// <summary>
    /// A record-type (6.4.3.3): the structure of its field-list. Every field-identifier of a record, whether in the fixed-part, a tag-field
    /// or inside a variant however deep, has the whole record-type as its region, so AllFields enumerates them flat and FindField looks
    /// them up by name without regard to case (6.1.3).
    /// </summary>
    public class RecordResolvedType : StructuredResolvedType
    {
        public FieldList Fields { get; }
        public RecordResolvedType(FieldList fields, bool isPacked) : base(isPacked)
        {
            Fields = fields;
        }
        public IEnumerable<Field> AllFields => Fields.AllFields;
        public Field? FindField(string name) => AllFields.FirstOrDefault(field => string.Equals(field.Name, name, StringComparison.OrdinalIgnoreCase));
        public override bool ContainsFileType => AllFields.Any(field => field.Type.ContainsFileType);
        protected override string Structure => Fields.FixedPart.Count == 0 && Fields.VariantPart == null ? $"{PackedPrefix}record end" : $"{PackedPrefix}record {Fields} end";

        /// <summary>A field: a component named by a field-identifier. A tag-field is the selector of its variant-part (6.4.3.3), which 6.6.3.3 restricts.</summary>
        public class Field
        {
            public string Name { get; }
            public ResolvedType Type { get; }
            public bool IsTagField { get; }
            public Field(string name, ResolvedType type, bool isTagField = false)
            {
                Name = name;
                Type = type;
                IsTagField = isTagField;
            }
            public override string ToString() => $"{Name}: {Type}";
        }

        /// <summary>A field-list (6.4.3.3): the fields of a fixed-part followed by an optional variant-part; empty when it has neither.</summary>
        public class FieldList
        {
            public IReadOnlyList<Field> FixedPart { get; }
            public VariantPart? VariantPart { get; }
            public FieldList(IEnumerable<Field> fixedPart, VariantPart? variantPart)
            {
                FixedPart = fixedPart.ToArray();
                VariantPart = variantPart;
            }
            /// <summary>Every field of this field-list and, recursively, of its variants, tag-fields included, in source order.</summary>
            public IEnumerable<Field> AllFields
            {
                get
                {
                    foreach (Field field in FixedPart)
                        yield return field;
                    if (VariantPart != null)
                    {
                        if (VariantPart.TagField != null)
                            yield return VariantPart.TagField;
                        foreach (Variant variant in VariantPart.Variants)
                            foreach (Field field in variant.FieldList.AllFields)
                                yield return field;
                    }
                }
            }
            public override string ToString() => string.Join("; ", FixedPart.Select(field => field.ToString()).Concat(VariantPart == null ? Enumerable.Empty<string>() : new[] { VariantPart.ToString() }));
        }

        /// <summary>
        /// A variant-part (6.4.3.3): a selector of TagType, which is a field named TagField when the variant-selector has a tag-field, and
        /// the variants it chooses between. 6.4.3.3 requires the case-constants of all the variants to be distinct and to cover every value
        /// of the tag-type; the checker verifies that from CaseConstants.
        /// </summary>
        public class VariantPart
        {
            public Field? TagField { get; }
            public OrdinalResolvedType TagType { get; }
            public IReadOnlyList<Variant> Variants { get; }
            public VariantPart(string? tagField, OrdinalResolvedType tagType, IEnumerable<Variant> variants)
            {
                TagField = tagField == null ? null : new Field(tagField, tagType, isTagField: true);
                TagType = tagType;
                Variants = variants.ToArray();
            }
            public override string ToString() => $"case {(TagField == null ? string.Empty : TagField.Name + ": ")}{TagType} of {string.Join("; ", Variants.Select(variant => $"{string.Join(", ", variant.CaseConstants.Select(TagType.ValueToString))}: ({variant.FieldList})"))}";
        }

        /// <summary>A variant (6.4.3.3): the field-list that is active while the selector holds one of CaseConstants, given as ordinal numbers of the tag-type.</summary>
        public class Variant
        {
            public IReadOnlyList<long> CaseConstants { get; }
            public FieldList FieldList { get; }
            public Variant(IEnumerable<long> caseConstants, FieldList fieldList)
            {
                CaseConstants = caseConstants.ToArray();
                FieldList = fieldList;
            }
        }
    }

    /// <summary>
    /// A set-type (6.4.3.4): the powerset of the ordinal Base. Two set-types are compatible when their base-types are and their packedness
    /// agrees (6.4.5 c); the type of a set-constructor is exempt from the second half, see PackednessFromContext.
    /// </summary>
    public class SetResolvedType : StructuredResolvedType
    {
        public OrdinalResolvedType Base { get; }
        /// <summary>
        /// True only for the canonical set type of a set-constructor, which 6.7.1 lets be packed or unpacked as its context requires:
        /// IsPacked is then false and Compatible ignores it. Obtain such types from OrdinalResolvedType.CanonicalSet.
        /// </summary>
        public bool PackednessFromContext { get; }
        public SetResolvedType(OrdinalResolvedType baseType, bool isPacked) : this(baseType, isPacked, packednessFromContext: false) { }
        internal SetResolvedType(OrdinalResolvedType baseType, bool isPacked, bool packednessFromContext) : base(isPacked)
        {
            Base = baseType;
            PackednessFromContext = packednessFromContext;
        }
        protected override string Structure => $"{PackedPrefix}set of {Base}";
    }

    /// <summary>
    /// A file-type (6.4.3.5): sequences of Component values with a current position and a mode. Component may neither be nor contain a
    /// file-type; the checker reports that against the component-type's denoter. Every file-type ContainsFileType, which also bars it from
    /// assignment (6.4.6 a).
    /// </summary>
    public class FileResolvedType : StructuredResolvedType
    {
        public ResolvedType Component { get; }
        public FileResolvedType(ResolvedType component, bool isPacked) : base(isPacked)
        {
            Component = component;
        }
        public override bool ContainsFileType => true;
        protected override string Structure => $"{PackedPrefix}file of {Component}";
    }

    /// <summary>
    /// text (6.4.3.5): the required file-type with components of type char and an additional line structure. Everything that accepts a
    /// variable of type "file of char" accepts a textfile (6.4.3.5 note 6), and only a textfile admits the operations of 6.6.6.5 and 6.9.
    /// </summary>
    public class TextResolvedType : FileResolvedType
    {
        public static readonly TextResolvedType TextRType = new TextResolvedType();
        private TextResolvedType() : base(CharResolvedType.CharRType, false) { Name = "text"; }
        protected override string Structure => "text";
    }

    /// <summary>
    /// A pointer-type (6.4.4): the nil-value plus identifying-values of variables of the Domain type. Domain is assigned late because
    /// 6.2.2.9 lets "^t" precede the type-definition of t within one type-definition-part; the checker fills it in when t is defined and
    /// reports a Domain still null at the end of that part.
    /// </summary>
    public class PointerResolvedType : ResolvedType
    {
        public ResolvedType? Domain { get; set; }
        public PointerResolvedType(ResolvedType? domain = null)
        {
            Domain = domain;
        }
        protected override string Structure => "^" + (Domain?.ToString() ?? "?");
    }

    /// <summary>
    /// The type of the factor nil, which "does not have a single type, but assumes a suitable pointer-type" (6.4.4 note 2): compatible
    /// with and assignable to every pointer-type.
    /// </summary>
    public class NilResolvedType : ResolvedType
    {
        public static readonly NilResolvedType NilRType = new NilResolvedType();
        private NilResolvedType() { }
        protected override string Structure => "nil";
    }

    /// <summary>The type of the set-constructor [], which likewise assumes a suitable set-type (6.7.1 note 3): compatible with and assignable to every set-type.</summary>
    public class EmptySetResolvedType : ResolvedType
    {
        public static readonly EmptySetResolvedType EmptySetRType = new EmptySetResolvedType();
        private EmptySetResolvedType() { }
        protected override string Structure => "[]";
    }

    /// <summary>
    /// Not in the spec: the type the checker gives a construct it has already reported an error for. It is compatible with, and assignable
    /// to and from, everything, so one mistake yields one message instead of a cascade.
    /// </summary>
    public class ErrorResolvedType : ResolvedType
    {
        public static readonly ErrorResolvedType ErrorRType = new ErrorResolvedType();
        private ErrorResolvedType() { }
        protected override string Structure => "<error>";
    }
}
