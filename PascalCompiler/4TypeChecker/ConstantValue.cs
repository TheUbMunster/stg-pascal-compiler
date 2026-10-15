//Copyright TheUbMunster, 2024, all rights reserved.

using System.Globalization;

namespace PascalCompiler.TypeChecker
{
   /// <summary>
   /// A value known at compile time: what a constant-definition introduces an identifier to denote (6.3), and what the type checker
   /// stores on a ConstantNode, because subrange bounds, array bounds and case-constants need the value itself and not just its type.
   /// A constant is a signed or unsigned number, a constant-identifier or a character-string; an unsigned-constant in an expression may
   /// also be nil (6.7.1).
   /// </summary>
   public abstract class ConstantValue
   {
      public abstract ResolvedType Type { get; }
   }

   /// <summary>A value of an ordinal-type held as its ordinal number (6.4.2.2, 6.4.2.3): an integer, a char, false or true, or an enumerated-type's constant.</summary>
   public class OrdinalConstantValue : ConstantValue
   {
      public override OrdinalResolvedType Type { get; }
      public long Ordinal { get; }
      public OrdinalConstantValue(OrdinalResolvedType type, long ordinal)
      {
         Type = type;
         Ordinal = ordinal;
      }
      public override string ToString() => Type.ValueToString(Ordinal);
   }

   /// <summary>A value of the real-type (6.4.2.2 b).</summary>
   public class RealConstantValue : ConstantValue
   {
      public double Value { get; }
      public RealConstantValue(double value)
      {
         Value = value;
      }
      public override ResolvedType Type => RealResolvedType.RealRType;
      public override string ToString() => Value.ToString("R", CultureInfo.InvariantCulture);
   }

   /// <summary>
   /// A character-string of two or more string-elements, which denotes a value of the string-type of that length (6.1.7). A character-string
   /// of one string-element denotes a char and is an OrdinalConstantValue.
   /// </summary>
   public class StringConstantValue : ConstantValue
   {
      public string Value { get; }
      public StringConstantValue(string value)
      {
         Value = value;
      }
      public override ArrayResolvedType Type => ArrayResolvedType.StringType(Value.Length);
      public override string ToString() => "'" + Value.Replace("'", "''") + "'";
   }

   /// <summary>The nil-value (6.4.4): the one unsigned-constant that is not a constant in the sense of 6.3.</summary>
   public class NilConstantValue : ConstantValue
   {
      public static readonly NilConstantValue Nil = new NilConstantValue();
      private NilConstantValue() { }
      public override ResolvedType Type => NilResolvedType.NilRType;
      public override string ToString() => "nil";
   }
}
