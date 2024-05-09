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
   public enum TokenType
   {
      //*direct* explicit usage for "digits" and "letters" according to 6.1.1 is unclear, commented if/until needed.
      //digit (see 6.1.1)
      //_0, _1, _2, _3, _4, _5, _6, _7, _8, _9,
      //letter (see 6.1.1)
      //A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,


      //identifier (see 6.1.3)
      Identifier, //one letter followed by one or more letters and/or digits.
      //directive (see 6.1.4)
      Directive, //one letter followed by one or more letters and/or digits.
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
      LeftSquareBracket, // [
      RightSquareBracket, // ]
      Dot, // .
      Comma, // ,
      Colon, // :
      Semicolon, // ;
      UpArrow, // ↑
      OpenParen, // (
      CloseParen, // )
      KetPair, // <>
      LessThanOrEqual, // <=
      GreaterThanOrEqual, // >=
      Walrus, // :=
      DoubleDot, // ..
      //not explicit in the spec, but useful for this implementation
      WHITESPACE, //not including cr or lf
      CR,
      LF
   }
}
