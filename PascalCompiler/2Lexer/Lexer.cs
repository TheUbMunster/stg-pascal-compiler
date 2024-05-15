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
using PascalCompiler.Scanner;

namespace PascalCompiler.Lexer
{
   public sealed class Token
   {
      #region Consts
      public static readonly Token UndefinedToken = new Token() { Type = TokenType.UNDEFINED };
      private static readonly IReadOnlyDictionary<TokenType, Regex> regexes = new Dictionary<TokenType, Regex>()
      {
         //word-symbol (see 6.1.2)
         //what the regex means
         //  \\G(?<=([^A-Za-z])|(^))and(?=([^A-Za-z0-9])|(\\z))
         //  \\G -> demand the regex scan start at the end of the provided index to regex.match, and not at the beginning of the string (beginning of file)
         //  (?<=([^A-Za-z])|(^)) -> immediately before the keyword, it must be the beginning of the string (beginning of the file) or a non-letter character (to prevent subsequences of letters being parsed as keywords)
         //  (?i:and) -> match the keyword in question (case insensitively)
         //  (?=([^A-Za-z0-9])|(\\z)) -> immediately after the keyword, must be a non-letter & non-digit OR the end of the string (end of file).
         { TokenType.And,       new Regex("\\G(?<=([^A-Za-z])|(^))(?i:and)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Array,     new Regex("\\G(?<=([^A-Za-z])|(^))(?i:array)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Begin,     new Regex("\\G(?<=([^A-Za-z])|(^))(?i:begin)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Case,      new Regex("\\G(?<=([^A-Za-z])|(^))(?i:case)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Const,     new Regex("\\G(?<=([^A-Za-z])|(^))(?i:const)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Div,       new Regex("\\G(?<=([^A-Za-z])|(^))(?i:div)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Do,        new Regex("\\G(?<=([^A-Za-z])|(^))(?i:do)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Downto,    new Regex("\\G(?<=([^A-Za-z])|(^))(?i:downto)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Else,      new Regex("\\G(?<=([^A-Za-z])|(^))(?i:else)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.End,       new Regex("\\G(?<=([^A-Za-z])|(^))(?i:end)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.File,      new Regex("\\G(?<=([^A-Za-z])|(^))(?i:file)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.For,       new Regex("\\G(?<=([^A-Za-z])|(^))(?i:for)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Function,  new Regex("\\G(?<=([^A-Za-z])|(^))(?i:function)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Goto,      new Regex("\\G(?<=([^A-Za-z])|(^))(?i:goto)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.If,        new Regex("\\G(?<=([^A-Za-z])|(^))(?i:if)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.In,        new Regex("\\G(?<=([^A-Za-z])|(^))(?i:in)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Label,     new Regex("\\G(?<=([^A-Za-z])|(^))(?i:label)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Mod,       new Regex("\\G(?<=([^A-Za-z])|(^))(?i:mod)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Nil,       new Regex("\\G(?<=([^A-Za-z])|(^))(?i:nil)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Not,       new Regex("\\G(?<=([^A-Za-z])|(^))(?i:not)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Of,        new Regex("\\G(?<=([^A-Za-z])|(^))(?i:of)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Or,        new Regex("\\G(?<=([^A-Za-z])|(^))(?i:or)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Packed,    new Regex("\\G(?<=([^A-Za-z])|(^))(?i:packed)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Procedure, new Regex("\\G(?<=([^A-Za-z])|(^))(?i:procedure)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Program,   new Regex("\\G(?<=([^A-Za-z])|(^))(?i:program)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Record,    new Regex("\\G(?<=([^A-Za-z])|(^))(?i:record)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Repeat,    new Regex("\\G(?<=([^A-Za-z])|(^))(?i:repeat)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Set,       new Regex("\\G(?<=([^A-Za-z])|(^))(?i:set)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Then,      new Regex("\\G(?<=([^A-Za-z])|(^))(?i:then)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.To,        new Regex("\\G(?<=([^A-Za-z])|(^))(?i:to)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Type,      new Regex("\\G(?<=([^A-Za-z])|(^))(?i:type)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Until,     new Regex("\\G(?<=([^A-Za-z])|(^))(?i:until)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.Var,       new Regex("\\G(?<=([^A-Za-z])|(^))(?i:var)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.While,     new Regex("\\G(?<=([^A-Za-z])|(^))(?i:while)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },
         { TokenType.With,      new Regex("\\G(?<=([^A-Za-z])|(^))(?i:with)(?=([^A-Za-z0-9])|(\\z))", RegexOptions.Compiled) },

         //character-strings (see 6.1.7)
         //don't need to worry about crazy illegal chars (e.g., bell), scanner already took care of that.
         { TokenType.CharacterString, new Regex("\\G'[^'\\r\\n]*?'", RegexOptions.Compiled) },

         //digit (see 6.1.1)
         { TokenType.Digit, new Regex("\\G[0-9]", RegexOptions.Compiled) },

         //letter (see 6.1.1)
         { TokenType.Letter, new Regex("\\G[A-Za-z]", RegexOptions.Compiled) },

         //special-symbol (see 6.1.2)
         { TokenType.Plus,                new Regex($"\\G{Regex.Escape("+")}", RegexOptions.Compiled)},
         { TokenType.Minus,               new Regex($"\\G{Regex.Escape("-")}", RegexOptions.Compiled)},
         { TokenType.Asterisk,            new Regex($"\\G{Regex.Escape("*")}", RegexOptions.Compiled)},
         { TokenType.ForwardSlash,        new Regex($"\\G{Regex.Escape("/")}", RegexOptions.Compiled)},
         { TokenType.Equals,              new Regex($"\\G{Regex.Escape("=")}", RegexOptions.Compiled)},
         { TokenType.LessThan,            new Regex($"\\G{Regex.Escape("<")}", RegexOptions.Compiled)},
         { TokenType.GreaterThan,         new Regex($"\\G{Regex.Escape(">")}", RegexOptions.Compiled)},
         { TokenType.LeftSquareBracket,   new Regex($"\\G({Regex.Escape("[")}|{Regex.Escape("(.")})", RegexOptions.Compiled)},
         { TokenType.RightSquareBracket,  new Regex($"\\G({Regex.Escape("]")}|{Regex.Escape(".)")})", RegexOptions.Compiled)},
         { TokenType.Dot,                 new Regex($"\\G{Regex.Escape(".")}", RegexOptions.Compiled)},
         { TokenType.Comma,               new Regex($"\\G{Regex.Escape(",")}", RegexOptions.Compiled)},
         { TokenType.Colon,               new Regex($"\\G{Regex.Escape(":")}", RegexOptions.Compiled)},
         { TokenType.Semicolon,           new Regex($"\\G{Regex.Escape(";")}", RegexOptions.Compiled)},
         { TokenType.UpArrow,             new Regex($"\\G({Regex.Escape("^")}|{Regex.Escape("@")})", RegexOptions.Compiled)},
         { TokenType.OpenParen,           new Regex($"\\G{Regex.Escape("(")}", RegexOptions.Compiled)},
         { TokenType.CloseParen,          new Regex($"\\G{Regex.Escape(")")}", RegexOptions.Compiled)},
         { TokenType.KetPair,             new Regex($"\\G{Regex.Escape("<>")}", RegexOptions.Compiled)},
         { TokenType.LessThanOrEqual,     new Regex($"\\G{Regex.Escape("<=")}", RegexOptions.Compiled)},
         { TokenType.GreaterThanOrEqual,  new Regex($"\\G{Regex.Escape(">=")}", RegexOptions.Compiled)},
         { TokenType.Walrus,              new Regex($"\\G{Regex.Escape(":=")}", RegexOptions.Compiled)},
         { TokenType.DoubleDot,           new Regex($"\\G{Regex.Escape("..")}", RegexOptions.Compiled)},

         //commentary (see 6.1.8)
         //note: if a comment spans multiple lines, that comment fully consumed the linebreaks; i.e., there are no LINEBREAK tokens interdispersed throughout the comment.
         { TokenType.Comment, new Regex("\\G({|\\(\\*)(.|\\n)*?(}|\\*\\))", RegexOptions.Compiled) }, //we don't have to worry about filtering the ., because the scan step already did that

         //not explicit via spec, but useful for this implementation.
         { TokenType.WHITESPACE, new Regex("\\G[ \\t]+", RegexOptions.Compiled) },
         { TokenType.LINEBREAK, new Regex("\\G(\r\n?|\n)", RegexOptions.Compiled) },
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
            t.TokenLength = m.Value.Length; //can only assign length if it was a success
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
      private string? content = null;
      public string Content 
      {
         get
         {
            if (FileLocation < 0 || FileLocation >= MySource.FileContents.Length)
               return string.Empty; //skip caching, (e.g., for TokenType.UNDEFINED)
            if (content == null) 
               content = MySource.FileContents.Substring(FileLocation, TokenLength);
            return content;
         }
      }
      #endregion

      #region Overrides
      public override string ToString()
      {
         return $"{Type}{(string.IsNullOrEmpty(Content) ? "" : $" '{Regex.Replace(Content, @"(\r\n?|\n)", "[line break]")}'")}";
      }
      #endregion
   }

   public static class Lexer
   {
      public static void Lex(Source source)
      {
         TokenType max = TTMax();
         int index = 0;//, newlineCount = 0, lastNewLineIndex = 0;
         TokenType attempt = max;
         while (index < source.FileContents.Length)
         {
            Token? t = Token.TryCreateToken(source, attempt, index);
            if (t != null)
            {
               attempt = max;
               index += t.TokenLength;
               source.AddLexerToken(t);
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
         source.LexingComplete();
      }

      #region Helpers
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
         return (j < 1) ? null : Arr[j]; // < 1 to skip undefined
      }

      /// <summary>
      /// Gets the minimum value of this enum
      /// </summary>
      private static TokenType TTMin()
      {
         return Enum.GetValues<TokenType>().OrderBy(x => (int)x).Skip(1).First();
      }

      /// <summary>
      /// Gets the maximum value of this enum
      /// </summary>
      private static TokenType TTMax()
      {
         return Enum.GetValues<TokenType>().OrderBy(x => (int)x).Last();
      }
      #endregion
   }
}