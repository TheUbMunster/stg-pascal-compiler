//Copyright TheUbMunster, 2024, all rights reserved.

using PascalCompiler.Lexer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PascalCompiler.NewParser
{
   /// <summary>
   /// The ISO 7185 grammar (Annex A) in the spec's metalanguage, one rule per line, as the CNFifier and the CYK parser consume it.
   /// The rule text itself is not part of the repository: it is read at runtime from two local files in this directory,
   /// AnnexA.grammar (the rules the parser uses) and AnnexA.lexical.grammar (the lexical rules of 6.1, kept for grammar analysis),
   /// which are gitignored and copied to the output directory when present. Without them the compiler still builds; only the
   /// CYK path fails, with a message saying which file to supply, when it runs.
   /// </summary>
   public static class PascalGrammar
   {
      public const string GrammarFile = "AnnexA.grammar";
      public const string LexicalGrammarFile = "AnnexA.lexical.grammar";
      private static readonly Lazy<string[]> grammar = new Lazy<string[]>(() => Load(GrammarFile));
      private static readonly Lazy<string[]> lexicalGrammar = new Lazy<string[]>(() => Load(LexicalGrammarFile));

      public static string[] grammarSource { get => grammar.Value; }
      //these belong to annex A as well, but the parser doesn't use them because they're terminals, or for other design reasons.
      public static string[] extraGrammarSource { get => lexicalGrammar.Value; }
      public static string[] fullGrammarSource { get => grammarSource.Concat(extraGrammarSource).ToArray(); }
      public static readonly string startingNonterminal = "program";

      private static string[] Load(string fileName)
      {
         string path = Path.Combine(AppContext.BaseDirectory, "3NewParser", fileName);
         if (!File.Exists(path))
            throw new FileNotFoundException($"The Pascal grammar is not part of the repository; supply it in {path} (one rule per line in the ISO 7185 metalanguage).", path);
         return File.ReadAllLines(path).Select(x => x.Trim()).Where(x => x.Length > 0 && !x.StartsWith('#')).ToArray();
      }
      //TODO: turn this into a double dictionary
      //maps TokenType to the terminal string from the grammar (not including the wrapping '')
      public static readonly IReadOnlyDictionary<TokenType, string> grammarTokenTypeMapping = new Dictionary<TokenType, string>()
      {
         //TODO:
         { TokenType.CharacterString, "character-string" }, //TODO: need to modify grammar def to make this appear as a terminal
         { TokenType.Letter, "letter" }, //TODO: need to modify grammar def to make this appear as a terminal
         { TokenType.Digit, "digit" }, //TODO: need to modify grammar def to make this appear as a terminal

         { TokenType.And, "and" }, //although a member of "word-symbol", I don't believe "word-symbol" appears on the RHS of a production, productions use this terminal directly.
         { TokenType.Array, "array" }, //"" ""
         { TokenType.Begin, "begin" }, //"" ""
         { TokenType.Case, "case" }, //"" ""
         { TokenType.Const, "const" }, //etc. for all word-symbols
         { TokenType.Div, "div" },
         { TokenType.Do, "do" },
         { TokenType.Downto, "downto" },
         { TokenType.Else, "else" },
         { TokenType.End, "end" },
         { TokenType.File, "file" },
         { TokenType.For, "for" },
         { TokenType.Function, "function" },
         { TokenType.Goto, "goto" },
         { TokenType.If, "if" },
         { TokenType.In, "in" },
         //NOTE: since the grammar got changed (e.g., "CharacterString" is a TokenType and therefore is also a terminal
         //this means that all grammar definitions that utilize it need to be changed accordingly to represent that it's
         //a terminal and not a nonterminal (E.g., everywehre the nonterminal "character-string" shows up, replace it
         //with the terminal "'character-string'".

         //NOTE: since extraGrammarSource is not explicitly included in this mapping, it's good to note that there are
         //a few implicit exclusions as well. Since "Comment", "WHITESPACE", "LINEBREAK" are all absorbable, that means
         //we don't need to include them in the mapping.

         //NOTE: the grammar probably needs to be modified to include "EOF" in a/the starting production.
      };
      public static readonly IReadOnlyDictionary<string, TokenType> grammarTokenTypeMappingInv = grammarTokenTypeMapping.ToDictionary(x => x.Value, x => x.Key);
   }
}
