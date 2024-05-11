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
   /// <summary>
   /// The types of tokens that our lexer is looking for. It's entries are heavily influenced by the entirety
   /// of section 6.1, but you'll notice some entries are missing, and some were added. This is because section
   /// 6.1 describes a grammar structure, much of the representing logic is present in the parser instead. The
   /// lexer is purely concerned about the grammar's terminals (nonexpandable members of the grammar, i.e., "atoms").
   /// 
   /// The numerical values of each of the enum entries represent their precedence, the first attempted to be lexed
   /// are the large (positive) values, the last attempted to be lexed are the small (negative) values.
   /// </summary>
   public enum TokenType : int
   { //any commented out entries represent concepts that were likely migrated to the parser.
      //*direct* explicit usage for "digits" and "letters" according to 6.1.1 is unclear, commented if/until needed.
      //rebuttal for above: https://stackoverflow.com/questions/3192619/design-guidelines-for-parser-and-lexer
      //character-strings (see 6.1.7)
      String = -50, // every letter in LegalCharacterSet.legalChars except ', includes the wrapping ''
      //ApostropheImage, // " (unnecessary)
      //digit (see 6.1.1)
      ///*DYNAMIC*/ Digit = -102, //any single digit (see 6.1.1)
      //_0, _1, _2, _3, _4, _5, _6, _7, _8, _9,
      //letter (see 6.1.1)

      //the concepts backing letters & numbers in section 6.1 belong to the parser, not the lexer.
      //this does NOT correlate to the digit-sequence concept outlined in 6.1.5
      /*DYNAMIC*/ SEQ_Digits = -101, //any amount of contiguous digits [0-9]
      //this does NOT correlate to any part of the spec in 6.1
      /*DYNAMIC*/ SEQ_Letters = -100, //any amount of contiguous letters of any case [A-Za-z]

      ///*DYNAMIC*/ UppercaseLetter = -101, //any capitol letter (see 6.1.1)
      //A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
      ///*DYNAMIC*/ LowercaseLetter = -100, //any lowercase letter (see 6.1.1)
      //a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z,
      //identifier (see 6.1.3)
      ///*DYNAMIC*/ Identifier, //one letter followed by zero or more letters and/or digits.
      //directive (see 6.1.4)
      ///*DYNAMIC*/ Directive, //one letter followed by zero or more letters and/or digits.
      //word-symbol (see 6.1.2)
      And         = 0, // and
      Array       = 1, // array
      Begin       = 2, // begin
      Case        = 3, // case
      Const       = 4, // const
      Div         = 5, // div
      Do          = 6, // do
      Downto      = 7, // downto
      Else        = 8, // else
      End         = 9, // end
      File        = 10, // file
      For         = 11, // for
      Function    = 12, // function
      Goto        = 13, // goto
      If          = 14, // if
      In          = 15, // in
      Label       = 16, // label
      Mod         = 17, // mod
      Nil         = 18, // nil
      Not         = 19, // not
      Of          = 20, // of
      Or          = 21, // or
      Packed      = 22, // packed
      Procedure   = 23, // procedure
      Program     = 24, // program
      Record      = 25, // record
      Repeat      = 26, // repeat
      Set         = 27, // set
      Then        = 28, // then
      To          = 29, // to
      Type        = 30, // type
      Until       = 31, // until
      Var         = 32, // var
      While       = 33, // while
      With        = 34, // with
      //special-symbol (see 6.1.2)
      Plus        = 35, // +
      Minus       = 36, // -
      Asterisk    = 37, // *
      ForwardSlash= 38, // /
      Equals      = 39, // =
      LessThan    = 40, // <
      GreaterThan = 41, // >
      //needs to be higher than parens
      /*DYNAMIC*/ LeftSquareBracket = 100, // [ *OR* (. (see 6.1.9)
      //needs to be higher than parens
      /*DYNAMIC*/ RightSquareBracket = 101, // ] *OR* .) (see 6.1.9)
      Dot = 42, // .
      Comma = 43, // ,
      Colon = 44, // :
      Semicolon = 45, // ;
      /*DYNAMIC*/ UpArrow = 46, // ↑/^ *OR* @ (see 6.1.9)
      OpenParen = 47, // (
      CloseParen = 48, // )
      //needs to be higher than lt or gt
      KetPair = 49, // <>
      //needs to be higher than lt or gt
      LessThanOrEqual = 50, // <=
      //needs to be higher than lt or gt
      GreaterThanOrEqual = 51, // >=
      //needs to be higher than colon
      Walrus = 52, // :=
      //needs to be higher than single dot
      DoubleDot = 53, // ..
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
      ///*DYNAMIC*/ OpenComment, // { *OR* (* (see 6.1.8)
      ///*DYNAMIC*/ CloseComment, // } *OR* *) (see 6.1.8)
      //needs to be higher than parens
      /*DYNAMIC*/ Comment = 1000, // (see 6.1.8)
      //not explicit in the spec, but useful for this implementation
      WHITESPACE = 65536, //not including cr or lf
      //needs to be higher than whsp
      LINEBREAK = 65537,
   }
}
