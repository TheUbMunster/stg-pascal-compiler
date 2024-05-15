using PascalCompiler.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler.Lexer
{
   public class MeasurableTokenesq
   { //paramless ctor is hidden, one of these has to be non-null
      Token? tok;
      ASTNode? node;
      public MeasurableTokenesq(Token t) 
      {
         tok = t;
      }
      public MeasurableTokenesq(ASTNode n)
      {
         node = n;
      }
      public int FileLocation { get => tok == null ? node!.FileLocation : tok.FileLocation; }
      public int Length { get => Content.Length; }
      public string Content { get => tok == null ? node!.Content : tok.Content; } //Token & ASTNode will handle calc/cache
   }
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
   {
      UNDEFINED = int.MinValue, //adding this may break things, see if there's a way to make this unecessary?
      
      //character-strings (see 6.1.7), this simplifies the 4 nonterminals, which is fine because the other three nonterminals
      //(string-element, apostrophe-image, string-character) are all only used in each other or in character-string
      CharacterString = -50, // every letter in LegalCharacterSet.legalChars except ', includes the wrapping ''

      //letter (see 6.1.1)
      /*DYNAMIC*/ Letter = -100, //any uppercase or lowercase letter [A-Za-z]

      //digit (see 6.1.1)
      /*DYNAMIC*/ Digit = -101, //any digit [0-9]

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

      //comment needs to be higher than parens for (* *)'s sake
      /*DYNAMIC*/ Comment = 1000, // (see 6.1.8)
      //not explicit in the spec, but useful for this implementation
      WHITESPACE = 65536, //not including cr or lf
      //needs to be higher than whsp
      LINEBREAK = 65537,
   }
}
