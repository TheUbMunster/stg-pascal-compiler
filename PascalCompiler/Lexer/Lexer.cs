using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Emit;
using System.Drawing;
using System.Reflection.Metadata;

namespace PascalCompiler.Lexer
{
   public sealed class Token
   {
      #region Consts
      private static readonly IReadOnlyDictionary<TokenType, Regex> regexes = new Dictionary<TokenType, Regex>()
      {
         //word-symbol (see 6.1.2)
         { TokenType.And, new Regex("\\Gand", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Array, new Regex("\\Garray", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Begin, new Regex("\\Gbegin", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Case, new Regex("\\Gcase", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Const, new Regex("\\Gconst", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Div, new Regex("\\Gdiv", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Do, new Regex("\\Gdo", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Downto, new Regex("\\Gdownto", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Else, new Regex("\\Gelse", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.End, new Regex("\\Gend", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.File, new Regex("\\Gfile", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.For, new Regex("\\Gfor", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Function, new Regex("\\Gfunction", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Goto, new Regex("\\Ggoto", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.If, new Regex("\\Gif", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.In, new Regex("\\Gin", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Label, new Regex("\\Glabel", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Mod, new Regex("\\Gmod", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Nil, new Regex("\\Gnil", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Not, new Regex("\\Gnot", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Of, new Regex("\\Gof", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Or, new Regex("\\Gor", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Packed, new Regex("\\Gpacked", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Procedure, new Regex("\\Gprocedure", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Program, new Regex("\\Gprogram", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Record, new Regex("\\Grecord", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Repeat, new Regex("\\Grepeat", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Set, new Regex("\\Gset", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Then, new Regex("\\Gthen", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.To, new Regex("\\Gto", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Type, new Regex("\\Gtype", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Until, new Regex("\\Guntil", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.Var, new Regex("\\Gvar", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.While, new Regex("\\Gwhile", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         { TokenType.With, new Regex("\\Gwith", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         //character-strings (see 6.1.7)
         //{ TokenType.String, (new Regex($"\\G'({string.Join('|', LegalCharacterSet.legalChars.Where(c => c != '\'').Select(c => Regex.Escape(c.ToString())))})'", RegexOptions.Compiled | RegexOptions.IgnoreCase), null) },
         { TokenType.String, new Regex("\\G'.*?'", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         //digit (see 6.1.1)
         { TokenType.Digit, new Regex("\\G[0-9]", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
         //letter (see 6.1.1)
         { TokenType.UppercaseLetter, new Regex("\\G[A-Z]", RegexOptions.Compiled) },
         { TokenType.LowercaseLetter, new Regex("\\G[a-z]", RegexOptions.Compiled) },
         //special-symbol (see 6.1.2)
         { TokenType.Plus, new Regex($"\\G{Regex.Escape("+")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.Minus, new Regex($"\\G{Regex.Escape("-")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.Asterisk, new Regex($"\\G{Regex.Escape("*")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.ForwardSlash, new Regex($"\\G{Regex.Escape("/")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.Equals, new Regex($"\\G{Regex.Escape("=")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.LessThan, new Regex($"\\G{Regex.Escape("<")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.GreaterThan, new Regex($"\\G{Regex.Escape(">")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.LeftSquareBracket, new Regex($"\\G({Regex.Escape("[")}|{Regex.Escape("(.")})", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.RightSquareBracket, new Regex($"\\G({Regex.Escape("]")}|{Regex.Escape(".)")})", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.Dot, new Regex($"\\G{Regex.Escape(".")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.Comma, new Regex($"\\G{Regex.Escape(",")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.Colon, new Regex($"\\G{Regex.Escape(":")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.Semicolon, new Regex($"\\G{Regex.Escape(";")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.UpArrow, new Regex($"\\G({Regex.Escape("^")}|{Regex.Escape("@")})", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.OpenParen, new Regex($"\\G{Regex.Escape("(")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.CloseParen, new Regex($"\\G{Regex.Escape(")")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.KetPair, new Regex($"\\G{Regex.Escape("<>")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.LessThanOrEqual, new Regex($"\\G{Regex.Escape("<=")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.GreaterThanOrEqual, new Regex($"\\G{Regex.Escape(">=")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.Walrus, new Regex($"\\G{Regex.Escape(":=")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         { TokenType.DoubleDot, new Regex($"\\G{Regex.Escape("..")}", RegexOptions.Compiled | RegexOptions.IgnoreCase)},
         //commentary (see 6.1.8)
         { TokenType.Comment, new Regex("\\G({|\\(\\*).*?(}|\\*\\))", RegexOptions.Compiled | RegexOptions.IgnoreCase) }, //we don't have to worry about filtering the ., because the scan step already did that
         //not explicit via spec, but useful for this implementation.
         { TokenType.WHITESPACE, new Regex("\\G\\s+", RegexOptions.Compiled) },
         { TokenType.CR, new Regex("\\G\\r", RegexOptions.Compiled) },
         { TokenType.LF, new Regex("\\G\\n", RegexOptions.Compiled) },
      };
      #endregion

      #region Creation
      private Token() { }
      public static Token? TryCreateToken(Source src, TokenType type, int fileLocation)
      {
         Token t = new Token() { Reg = regexes[type], Type = type, MySource = src, FileLocation = fileLocation };
         Match m = t.Reg.Match(src.FileContents, fileLocation);
         if (m.Success)
         {
            t.Content = m.Value;
            t.TokenLength = m.Value.Length;
            return t;
         }
         else return null;
      }
      #endregion


      #region Fields
      private Source MySource { get; init; } = null!;
      public TokenType Type { get; private init; }
      public Regex Reg { get; private init; } = null!;
      //These three are assigned by the lexer.
      public int FileLocation { get; set; } = -1;
      public int TokenLength { get; set; } = -1;
      public string Content { get; set; } = null!; //todo: calculate this via substring of the source using FileLocation and TokenLength + cache it.
      #endregion

      #region Overrides
      public override string ToString()
      {
         return $"{Type}{(string.IsNullOrEmpty(Content) ? "" : $" '{Regex.Replace(Content, @"\r\n?|\n", "[line break]")}'")}";
      }
      #endregion
   }

   public static class Lexer
   {      
      public static void Lex(Source source)
      {
         TokenType max = TTMax();
         int index = 0;//, newlineCount = 0, lastNewLineIndex = 0;
         List<Token> tokens = new List<Token>();
         TokenType attempt = max;
         while (index < source.FileContents.Length)
         {
            Token? t = Token.TryCreateToken(source, attempt, index);
            if (t != null)
            {
               attempt = max;
               index += t.TokenLength;
               tokens.Add(t);
            }
            else
            {
               TokenType? dec = TTDecriment(attempt);
               if (dec.HasValue)
                  attempt = dec.Value;
               else
               {
                  source.AppendMessage(new(Source.CompilerPhase.Lexer, Source.Severity.Error, $"Unable to lex {source.Filename}", index, true));
                  return;
               }
            }
         }
         source.TakeLexerTokens(tokens);
      }

      /// <summary>
      /// Increments an enum to it's next defined value.
      /// </summary>
      /// <returns>Null if the incremented value was the largest value in the enum, the incremented enum otherwise</returns>
      private static TokenType? TTIncrement(TokenType val)
      {
         TokenType[] Arr = Enum.GetValues<TokenType>().OrderBy(x => (int)x).ToArray(); //this should fetch them in sorted order.
         int j = Array.IndexOf(Arr, val) + 1;
         return (j >= Arr.Length) ? null : Arr[j];
      }

      /// <summary>
      /// Decriments an enum to it's previous defined value.
      /// </summary>
      /// <returns>Null if the Decrimented value was the smallest value in the enum, the decrimented enum otherwise</returns>
      private static TokenType? TTDecriment(TokenType val)
      {
         TokenType[] Arr = Enum.GetValues<TokenType>().OrderBy(x => (int)x).ToArray(); //this should fetch them in sorted order.
         int j = Array.IndexOf(Arr, val) - 1;
         return (j < 0) ? null : Arr[j];
      }

      /// <summary>
      /// Gets the minimum value of this enum
      /// </summary>
      private static TokenType TTMin()
      {
         return Enum.GetValues<TokenType>().OrderBy(x => (int)x).First();
      }

      /// <summary>
      /// Gets the maximum value of this enum
      /// </summary>
      private static TokenType TTMax()
      {
         return Enum.GetValues<TokenType>().OrderBy(x => (int)x).Last();
      }
   }
}