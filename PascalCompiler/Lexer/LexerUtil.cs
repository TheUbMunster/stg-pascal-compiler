using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler.Lexer
{
   public class LexerException : Exception
   {
      public LexerException(string? message) : base(message) { }
      public LexerException(string? message, Exception? innerException) : base(message, innerException) { }
   }
   public record struct FilePosition(int line, int character)
   {
      public string ToString(bool clean = false)
      {
         if (clean)
            return $"{line} {character}";
         else
            return $"Line: {line}, Character: {character}";
      }
      public override string ToString() => ToString(false);
   }
   public enum TokenType : uint
   { //any commented out entries represent concepts that were likely migrated to the parser.
      //*direct* explicit usage for "digits" and "letters" according to 6.1.1 is unclear, commented if/until needed.
      //rebuttal for above: https://stackoverflow.com/questions/3192619/design-guidelines-for-parser-and-lexer
      //character-strings (see 6.1.7)
      StringCharacter, // [_-\(\)] (not including digits or letters)
      ApostropheImage, // "
      //digit (see 6.1.1)
      _0, _1, _2, _3, _4, _5, _6, _7, _8, _9,
      //letter (see 6.1.1)
      A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
      a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z,
      //identifier (see 6.1.3)
      ///*DYNAMIC*/ Identifier, //one letter followed by zero or more letters and/or digits.
      //directive (see 6.1.4)
      ///*DYNAMIC*/ Directive, //one letter followed by zero or more letters and/or digits.
      //word-symbol (see 6.1.2)
      And, // and
      Array, // array
      Begin, // begin
      Case, // case
      Const, // const
      Div, // div
      Do, // do
      Downto, // downto
      Else, // else
      End, // end
      File, // file
      For, // for
      Function, // function
      Goto, // goto
      If, // if
      In, // in
      Label, // label
      Mod, // mod
      Nil, // nil
      Not, // not
      Of, // of
      Or, // or
      Packed, // packed
      Procedure, // procedure
      Program, // program
      Record, // record
      Repeat, // repeat
      Set, // set
      Then, // then
      To, // to
      Type, // type
      Until, // until
      Var, // var
      While, // while
      With, // with
      //special-symbol (see 6.1.2)
      Plus, // +
      Minus, // -
      Asterisk, // *
      ForwardSlash, // /
      Equals, // =
      LessThan, // <
      GreaterThan, // >
      /*DYNAMIC*/ LeftSquareBracket, // [ *OR* (. (see 6.1.9)
      /*DYNAMIC*/ RightSquareBracket, // ] *OR* .) (see 6.1.9)
      Dot, // .
      Comma, // ,
      Colon, // :
      Semicolon, // ;
      /*DYNAMIC*/ UpArrow, // ↑/^ *OR* @ (see 6.1.9)
      OpenParen, // (
      CloseParen, // )
      KetPair, // <>
      LessThanOrEqual, // <=
      GreaterThanOrEqual, // >=
      Walrus, // :=
      DoubleDot, // ..
      //numbers (see 6.1.5)
      //some entries omitted, as elements corresponding to these types are disambiguated and constructed from other tokens in a later step.
      //namely, any qualifying token can be implicitly upcast to signed-number or unsigned-number
      ///*DYNAMIC*/ SignedReal, // optionally a sign followed by an unsigned-real
      ///*DYNAMIC*/ SignedInteger, // optionally a sign followed by an unsigned-integer
      ///*DYNAMIC*/ Sign, // + *OR* -
      ///*DYNAMIC*/ UnsignedReal, // a digit-sequence followed by a dot followed by a fractional-part followed optionally by (e followed by a scale-factor), *OR* a digit-sequence followed by e followed by a scale-factor
      ///*DYNAMIC*/ UnsignedInteger, // a digit-sequence
      ///*DYNAMIC*/ FractionalPart, // a digit-sequence
      ///*DYNAMIC*/ ScaleFactor, //optionally a sign followed by a digit-sequence
      ///*DYNAMIC*/ DigitSequence, // a digit followed by zero or more digits
      //labels (see 6.1.6)
      ///*DYNAMIC*/ Label, // a digit sequence
      //token separators (commentary) (see 6.1.8)
      /*DYNAMIC*/ OpenComment, // { *OR* (* (see 6.1.8)
      /*DYNAMIC*/ CloseComment, // } *OR* *) (see 6.1.8)
      //not explicit in the spec, but useful for this implementation
      WHITESPACE = 65536u, //not including cr or lf
      CR,
      LF
   }
}
