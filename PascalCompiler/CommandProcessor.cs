//Copyright TheUbMunster, 2024, all rights reserved.

using static System.Environment;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using PascalCompiler.Lexer;
using PascalCompiler.Scanner;

namespace PascalCompiler
{
   public static class CommandProcessor
   {
      #region Consts
      private static Flag[] flagDefs = new Flag[]
      { //todo: add "scan only" flag
         new Flag("r", "(DEBUG) This flag is if you want to scan the source and emit the scan info output only. " +
            "If an output file name is specified, the output will be written to the output file. " +
            "If not, it will be printed to stdout.", new[] { "l", "p", "t", "s", "O0", "O1", "O2", "O3" }),
         new Flag("l", "(DEBUG) This flag is if you want to lex the source and emit the lexed output only. " +
            "If an output file name is specified, the output will be written to the output file. " +
            "If not, it will be printed to stdout.", new[] { "r", "p", "t", "s", "O0", "O1", "O2", "O3" }),
         new Flag("p", "(DEBUG) This flag is if you want to lex and parse the source and emit the AST output only. " +
            "If an output file name is specified, the output will be written to the output file. " +
            "If not, it will be printed to stdout.", new[] { "r", "l", "t", "s", "O0", "O1", "O2", "O3" }),
         new Flag("t", "(DEBUG) This flag is if you want to lex, parse, and typecheck the source and emit the typechecking info output only. " +
            "If an output file name is specified, the output will be written to the output file. " +
            "If not, it will be printed to stdout.", new[] { "r", "l", "p", "s", "O0", "O1", "O2", "O3" }),
         new Flag("s", "(DEBUG) This flag is if you want to lex, parse, typecheck, and emit assembly from the source only. " +
            "If an output file name is specified, the output will be written to the output file. " +
            "If not, it will be printed to stdout.", new[] { "r", "l", "p", "t", "O0", "O1", "O2", "O3" }),
         new Flag("O0", "This flag indicates to the compiler to perform no optimizations",
            new[] { "r", "l", "p", "t", "s", "O1", "O2", "O3" }),
         new Flag("O1", "This flag indicates to the compiler to perform minimal optimizations",
            new[] { "r", "l", "p", "t", "s", "O0", "O2", "O3" }),
         new Flag("O2", "This flag indicates to the compiler to perform moderate optimizations",
            new[] { "r", "l", "p", "t", "s", "O0", "O1", "O3" }),
         new Flag("O3", "This flag indicates to the compiler to perform all optimizations",
            new[] { "r", "l", "p", "t", "s", "O0", "O1", "O2" }),
         new Flag("no-ansi", "This flag indicates to use no ansi codes in stdout", new string[] {})
         //new Flag("n", "The output name of the log file or executable", new string[] { }), //todo: implement logging
         //todo: add "interpret" flag
      };
      #endregion

      #region Typedefs
      /// <summary>
      /// An object representing a flag argument for this pascal compiler
      /// </summary>
      /// <param name="flagName">The name of the flag, not including a single leading hyphen. If the
      /// flag is meant to be used with two hyphens (e.g., "--help"), then the flag name should be "-help"</param>
      /// <param name="helpMessage">The help message included with this particular flag</param>
      /// <param name="mutualExclusionFlags">What other flag names are not a legal combination with this flag</param>
      private record struct Flag(string flagName, string helpMessage, string[] mutualExclusionFlags);
      #endregion
      //todo: make this function the entry point for unit testing methods, add the ability to past streams instead of filepaths via args.
      public static int Process(string[] args, TextWriter outputWriter)
      {
         //overhaul this so flags and nonflags orderings are preserved.
         IReadOnlyList<string> flags = args.Where(x => x.StartsWith("-")).Select(x => x.Substring(1)).ToList();
         IReadOnlyList<string> nonflags = args.Where(x => !x.StartsWith("-")).ToList();
         //check for help
         if (flags.Contains("-help") || flags.Contains("help") || flags.Contains("h"))
         {
            outputWriter.WriteLine("PascalCompiler - an ISO/IEC 7185:1990(E) pascal compiler written by TheUbMunster" + NewLine +
               "Usage: [flags...] [filepath...]" + NewLine + // | source
               "Flags:" + NewLine +
               "\t--help | -help | -h - Prints this help message." + NewLine + NewLine +
               string.Join(NewLine, flagDefs.Select(x => $"\t-{x.flagName} - {x.helpMessage}{NewLine}\tThis flag cannot appear with the following flags: {string.Join(", ", x.mutualExclusionFlags)}{NewLine}")) +
               "Filepath:" + NewLine +
               "\tfilepath(s) to pascal source file(s) to compile."); //need to disambiguate source file(s) from program arguments (when running via interpreter mode, maybe make each file be preceeded with -f, i.e., -ffilename.pas).
            return 0;
         }
         StringBuilder errorAgg = new StringBuilder();
         //check flag exclusion rules.
         foreach (string flag in flags) //skip the leading hyphen
         {
            if (flagDefs.All(x => x.flagName != flag))
               errorAgg.AppendLine($"The flag {flag} is not a recognized flag.");
            else if (flagDefs.First(x => x.flagName == flag).mutualExclusionFlags.Intersect(flags).Any())
               errorAgg.AppendLine($"The flag {flag} is present with one or more conflicting options. See --help for this information");
         }
         if (errorAgg.Length > 0)
         {
            outputWriter.WriteLine(errorAgg.ToString());
            return -1;
         }
         //check if we're being asked to operate on any files.
         if (nonflags.Count == 0)
         {
            outputWriter.WriteLine("No non-flag arguments, nothing to do.");
            return -1;
         }
         //==============
         // SCAN SOURCES
         //==============
         List<Source> sources;
         {
            ConcurrentBag<Source> cSources = new();
#if DEBUG
            for (int i = 0; i < nonflags.Count; i++)
            {
               string path = Path.GetFullPath(nonflags[i]);
               cSources.Add(new Source(path));
            };
#else
            Parallel.For(0, nonflags.Count, (i) =>
            {
               string path = Path.GetFullPath(nonflags[i]);
               cSources.Add(new Source(path));
            });
#endif
            sources = cSources.ToList();
         }
         foreach (Source s in sources)
         {
            foreach (Source.Message m in s.Messages)
            {
               PrintMessage(s, m, outputWriter, !flags.Contains("no-ansi"));
            }
            s.ClearMessages();
         }
         if (sources.Any(s => s.ErrorHasOccurred))
         {
            outputWriter.WriteLine("Compilation failed at the SourceScan stage.");
            return -1;
         }
         if (flags.Contains("r"))
         {
            //foreach (Source s in sources)
            //{
            //   Console.WriteLine("idk");
            //}
            outputWriter.WriteLine("Compilation succeeded: scan check complete");
            return 0;
         }
         //=====
         // LEX
         //=====
#if DEBUG
         for (int i = 0; i < sources.Count; i++)
         {
            Lexer.Lexer.Lex(sources[i]);
         }
#else
         Parallel.For(0, sources.Count, (i) =>
         {
            Lexer.Lexer.Lex(sources[i]);
         });
#endif
         foreach (Source s in sources)
         {
            foreach (Source.Message m in s.Messages)
            {
               PrintMessage(s, m, outputWriter, !flags.Contains("no-ansi"));
            }
            s.ClearMessages();
         }
         if (sources.Any(s => s.ErrorHasOccurred))
         {
            outputWriter.WriteLine("Compilation failed at the Lexer stage.");
            return -1;
         }
         if (flags.Contains("l"))
         {
            foreach (Source s in sources)
            {
               outputWriter.WriteLine(string.Join("\n", s.LexerTokensFlattenedView));
            }
            outputWriter.WriteLine("Compilation succeeded: lexical analysis complete");
            return 0;
         }
         //=======
         // PARSE
         //=======
#if DEBUG
         for (int i = 0; i < sources.Count; i++)
         {
            NewParser.Parser.Parse(sources[i]);
         }
#else
         Parallel.For(0, sources.Count, (i) =>
         {
            NewParser.Parser.Parse(sources[i]);
         });
#endif
         foreach (Source s in sources)
         {
            foreach (Source.Message m in s.Messages)
            {
               PrintMessage(s, m, outputWriter, !flags.Contains("no-ansi"));
            }
            s.ClearMessages();
         }
         if (sources.Any(s => s.ErrorHasOccurred))
         {
            outputWriter.WriteLine("Compilation failed at the Lexer stage.");
            return -1;
         }
         if (flags.Contains("p"))
         {
            foreach (Source s in sources)
            {
               outputWriter.WriteLine(string.Join("\n", s.LexerTokens));
            }
            outputWriter.WriteLine("Compilation succeeded: parsing complete");
            return 0;
         }
         //===========
         // TYPECHECK
         //===========

         throw new System.NotImplementedException(); //at the very end
      }
      public static void PrintMessage(Source source, Source.Message message, TextWriter outputWriter, bool doAnsiColoring)
      {
         //string is ansi color code https://talyian.github.io/ansicolors/
         IReadOnlyDictionary<TokenType, string> lexemColors = new Dictionary<TokenType, string>()
         {
            { TokenType.CharacterString,      "\x1b[38;5;208m" },
            { TokenType.Digit,                "\x1b[36m" },
            { TokenType.Letter,               "\x1b[36m" },
            //keywords
            { TokenType.And,                  "\x1b[34m" },
            { TokenType.Array,                "\x1b[34m" },
            { TokenType.Begin,                "\x1b[34m" },
            { TokenType.Case,                 "\x1b[34m" },
            { TokenType.Const,                "\x1b[34m" },
            { TokenType.Div,                  "\x1b[34m" },
            { TokenType.Do,                   "\x1b[34m" },
            { TokenType.Downto,               "\x1b[34m" },
            { TokenType.Else,                 "\x1b[34m" },
            { TokenType.End,                  "\x1b[34m" },
            { TokenType.File,                 "\x1b[34m" },
            { TokenType.For,                  "\x1b[34m" },
            { TokenType.Function,             "\x1b[34m" },
            { TokenType.Goto,                 "\x1b[34m" },
            { TokenType.If,                   "\x1b[34m" },
            { TokenType.In,                   "\x1b[34m" },
            { TokenType.Label,                "\x1b[34m" },
            { TokenType.Mod,                  "\x1b[34m" },
            { TokenType.Nil,                  "\x1b[34m" },
            { TokenType.Not,                  "\x1b[34m" },
            { TokenType.Of,                   "\x1b[34m" },
            { TokenType.Or,                   "\x1b[34m" },
            { TokenType.Packed,               "\x1b[34m" },
            { TokenType.Procedure,            "\x1b[34m" },
            { TokenType.Program,              "\x1b[34m" },
            { TokenType.Record,               "\x1b[34m" },
            { TokenType.Repeat,               "\x1b[34m" },
            { TokenType.Set,                  "\x1b[34m" },
            { TokenType.Then,                 "\x1b[34m" },
            { TokenType.To,                   "\x1b[34m" },
            { TokenType.Type,                 "\x1b[34m" },
            { TokenType.Until,                "\x1b[34m" },
            { TokenType.Var,                  "\x1b[34m" },
            { TokenType.While,                "\x1b[34m" },
            { TokenType.With,                 "\x1b[34m" },
            //operators
            { TokenType.Plus,                 "\x1b[33m" },
            { TokenType.Minus,                "\x1b[33m" },
            { TokenType.Asterisk,             "\x1b[33m" },
            { TokenType.ForwardSlash,         "\x1b[33m" },
            { TokenType.Equals,               "\x1b[33m" },
            { TokenType.LessThan,             "\x1b[33m" },
            { TokenType.GreaterThan,          "\x1b[33m" },
            { TokenType.LeftSquareBracket,    "\x1b[33m" },
            { TokenType.RightSquareBracket,   "\x1b[33m" },
            { TokenType.Dot,                  "\x1b[33m" },
            { TokenType.Comma,                "\x1b[33m" },
            { TokenType.Colon,                "\x1b[33m" },
            { TokenType.Semicolon,            "\x1b[33m" },
            { TokenType.UpArrow,              "\x1b[33m" },
            { TokenType.OpenParen,            "\x1b[33m" },
            { TokenType.CloseParen,           "\x1b[33m" },
            { TokenType.KetPair,              "\x1b[33m" },
            { TokenType.LessThanOrEqual,      "\x1b[33m" },
            { TokenType.GreaterThanOrEqual,   "\x1b[33m" },
            { TokenType.Walrus,               "\x1b[33m" },
            { TokenType.DoubleDot,            "\x1b[33m" },
            //misc
            { TokenType.Comment,              "\x1b[32m" },
            { TokenType.WHITESPACE,           "\x1b[37m" },
            { TokenType.LINEBREAK,            "\x1b[37m" },
            { TokenType.EOF,                  "\x1b[37m" }
         };
         string lineNumbersColor = "\x1b[35m"; //magenta foreground
         int lgb10 = (int)System.Math.Ceiling(System.Math.Log10(source.LineCount));
         string primaryMessageColor;
         if (doAnsiColoring)
            outputWriter.Write("\x1b[40m"); //black background

         switch (message.severity)
         {
            case Source.Severity.Pedantic:
               primaryMessageColor = "\x1b[37m"; //white foreground
               break;
            case Source.Severity.Information:
               primaryMessageColor = "\x1b[36m"; //cyan foreground
               break;
            case Source.Severity.Suggestion:
               primaryMessageColor = "\x1b[34m"; //blue foreground
               break;
            case Source.Severity.Warning:
               primaryMessageColor = "\x1b[33m"; //yellow foreground
               break;
            case Source.Severity.Error:
               primaryMessageColor = "\x1b[31m"; //red foreground
               break;
            default:
               primaryMessageColor = "\x1b[35m"; //magenta foreground
               break;
         }
         if (message.isError)
         {
            if (doAnsiColoring)
               outputWriter.Write("\x1b[31m"); //red foreground
            outputWriter.WriteLine("The following is an error severe enough to cause the compiler to halt:");
         }
         outputWriter.Write(primaryMessageColor);
         outputWriter.WriteLine($"{source.Filename} - [{message.phase}] [{message.severity}]: {message.message}");
         if (message.fileLocation.HasValue)
         {
            outputWriter.WriteLine($"The above error was reported to correlate to the following part of [{source.Filename}]:");
            if (doAnsiColoring)
            {
               outputWriter.Write("\x1b[37m"); //white foreground
               outputWriter.Write("\x1b[40m"); //black background
            }
            var linCol = source.GetLineColFromFileLocation(message.fileLocation.Value);
            const int windowSize = 3;
            for (int i = -windowSize; i < windowSize + 1; i++)
            {
               int desiredLine = linCol.line + i; //0 based line index
               if (desiredLine >= 0 && desiredLine < source.LineCount)
               {
                  if (doAnsiColoring)
                  {
                     outputWriter.Write("\x1b[40m"); //black background
                     outputWriter.Write(lineNumbersColor);
                  }
                  outputWriter.Write($"{(desiredLine + 1).ToString().PadRight(lgb10)} "); //print line number

                  //this is to get the tokens that represent the n'th line of source code
                  //it could be made more efficient but would require a partial refactor of `Source`.
                  IEnumerable<Token> toksOfThisLine = source.LexerTokensFlattenedView;
                  int skipLines = desiredLine;
                  while (skipLines > 0)
                  {
                     toksOfThisLine = toksOfThisLine.SkipWhile(x => x.Type != TokenType.LINEBREAK);
                     toksOfThisLine = toksOfThisLine.Skip(1);
                     skipLines--;
                  }
                  {
                     List<Token> temp = toksOfThisLine.ToList();
                     int ind = temp.FindIndex(x => x.Type == TokenType.LINEBREAK);
                     if (ind != -1)
                        toksOfThisLine = temp.Take(ind + 1); //include linebreak itself
                     else
                        toksOfThisLine = temp;
                  }
                  //3 cases:
                  //is fully complete lexed line (+++)
                  //is partially complete lexed line (single) (TODO)
                  //is not lexed (+++)
                  if (toksOfThisLine.Any()) //if fully complete lexed line, or partially complete lexed line
                  {
                     int printedLength = 0;
                     foreach (Token tok in toksOfThisLine)
                     //for (int tk = 0; tk < toks.Count; tk++)
                     {
                        //this *should* be the last token in toksOfThisLine. Consider not doing "include linebreak itself" above.
                        if (tok.Type == TokenType.LINEBREAK)
                           continue;
                        if (doAnsiColoring)
                        {
                           //Console.ForegroundColor = lexemColors[toks[tk].Type];
                           outputWriter.Write(lexemColors[tok.Type]);
                        }
                        outputWriter.Write(tok.Content);
                        printedLength += tok.TokenLength; //should we include the lengths of linebreaks?
                     }
                     if (doAnsiColoring)
                        outputWriter.Write("\x1b[37m"); //white foreground
                     string remainder = source.GetSourceLine(desiredLine).Substring(printedLength);
                     outputWriter.WriteLine(remainder);
                  }
                  else
                  {
                     if (doAnsiColoring)
                        outputWriter.Write("\x1b[37m"); //white foreground
                     outputWriter.WriteLine($"{source.GetSourceLine(desiredLine)}");
                  }
                  if (i == 0)
                  {
                     //print a pointer to the exact character where the error is, but print it in a way where there's room.
                     //TODO: if it's really close to the left edge, make the arrow enter from the right instead.
                     if (doAnsiColoring)
                     {
                        outputWriter.Write("\x1b[30m"); //black foreground
                        outputWriter.Write("\x1b[47m"); //white background
                     }
                     string preamble = "error here ", pointer = "----^";
                     int horizLen = linCol.col + 1 + lgb10 + 1;
                     if (horizLen >= preamble.Length + pointer.Length)
                        outputWriter.WriteLine($"{preamble}{pointer}".PadLeft(horizLen));
                     else if (horizLen >= pointer.Length)
                        outputWriter.WriteLine($"{pointer}".PadLeft(horizLen));
                     else
                        outputWriter.WriteLine(pointer.Substring(pointer.Length - horizLen));
                     if (doAnsiColoring)
                     {
                        outputWriter.Write("\x1b[37m"); //white foreground
                        outputWriter.Write("\x1b[40m"); //black background
                     }
                  }
               }
            }
         }
         else
         {
            outputWriter.WriteLine("The above error was not reported to correlate to any particular part of the source code.");
         }
      }
   }
}