using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalCompiler.Lexer;
using PascalCompiler.Scanner;

namespace PascalCompiler
{
    static class CommandProcessor
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
         new Flag("n", "The output name of the log file or executable", new string[] { }), //todo: implement logging
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
      public static int Process(string[] args)
      {
         IReadOnlyList<string> flags = args.Where(x => x.StartsWith("-")).Select(x => x.Substring(1)).ToList();
         IReadOnlyList<string> nonflags = args.Where(x => !x.StartsWith("-")).ToList();
         //check for help
         if (flags.Contains("-help") || flags.Contains("help") || flags.Contains("h"))
         {
            Console.WriteLine("PascalCompiler - an ISO/IEC 7185:1990(E) pascal compiler written by TheUbMunster" + Environment.NewLine +
               "Usage: [flags...] [filepath...]" + Environment.NewLine + // | source
               "Flags:" + Environment.NewLine +
               "\t--help | -help | -h - Prints this help message." + Environment.NewLine + Environment.NewLine +
               string.Join(Environment.NewLine, flagDefs.Select(x => $"\t-{x.flagName} - {x.helpMessage}{Environment.NewLine}\tThis flag cannot appear with the following flags: {string.Join(", ", x.mutualExclusionFlags)}{Environment.NewLine}")) + 
               "Filepath:" + Environment.NewLine +
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
            Console.Error.WriteLine(errorAgg.ToString());
            return -1;
         }
         //check if we're being asked to operate on any files.
         if (nonflags.Count == 0)
         {
            Console.Error.WriteLine("No non-flag arguments, nothing to do.");
            return -1;
         }
         //==============
         // SCAN SOURCES
         //==============
         List<Source> sources;
         {
            ConcurrentBag<Source> cSources = new();
            Parallel.For(0, nonflags.Count, (i) =>
            {
               string path = Path.GetFullPath(nonflags[i]);
               cSources.Add(new Source(path));
            });
            sources = cSources.ToList();
         }
         foreach (Source s in sources)
         {
            foreach (Source.Message m in s.Messages)
            {
               PrintMessage(s, m);
            }
            s.ClearMessages();
         }
         if (sources.Any(s => s.ErrorHasOccurred))
         {
            Console.Error.WriteLine("Compilation failed at the SourceScan stage.");
            return -1;
         }
         if (flags.Contains("r"))
         {
            //foreach (Source s in sources)
            //{
            //   Console.WriteLine("idk");
            //}
            Console.WriteLine("Compilation succeeded: scan check complete");
            return 0;
         }
         //=====
         // LEX
         //=====
         Parallel.For(0, sources.Count, (i) =>
         {
            Lexer.Lexer.Lex(sources[i]);
         });
         foreach (Source s in sources)
         {
            foreach (Source.Message m in s.Messages)
            {
               PrintMessage(s, m);
            }
            s.ClearMessages();
         }
         if (sources.Any(s => s.ErrorHasOccurred))
         {
            Console.Error.WriteLine("Compilation failed at the Lexer stage.");
            return -1;
         }
         if (flags.Contains("l"))
         {
            foreach (Source s in sources)
            {
               Console.WriteLine(string.Join("\n", s.LexerTokens));
            }
            Console.WriteLine("Compilation succeeded: lexical analysis complete");
            return 0;
         }



         throw new NotImplementedException(); //at the very end
      }
      public static void PrintMessage(Source source, Source.Message message)
      {
         IReadOnlyDictionary<TokenType, ConsoleColor> lexemColors = new Dictionary<TokenType, ConsoleColor>()
         {
            { TokenType.CharacterString, ConsoleColor.DarkRed },
            { TokenType.Digit, ConsoleColor.Cyan },
            { TokenType.Letter, ConsoleColor.Cyan },
            //keywords
            { TokenType.And, ConsoleColor.Blue },
            { TokenType.Array, ConsoleColor.Blue },
            { TokenType.Begin, ConsoleColor.Blue },
            { TokenType.Case, ConsoleColor.Blue },
            { TokenType.Const, ConsoleColor.Blue },
            { TokenType.Div, ConsoleColor.Blue },
            { TokenType.Do, ConsoleColor.Blue },
            { TokenType.Downto, ConsoleColor.Blue },
            { TokenType.Else, ConsoleColor.Blue },
            { TokenType.End, ConsoleColor.Blue },
            { TokenType.File, ConsoleColor.Blue },
            { TokenType.For, ConsoleColor.Blue },
            { TokenType.Function, ConsoleColor.Blue },
            { TokenType.Goto, ConsoleColor.Blue },
            { TokenType.If, ConsoleColor.Blue },
            { TokenType.In, ConsoleColor.Blue },
            { TokenType.Label, ConsoleColor.Blue },
            { TokenType.Mod, ConsoleColor.Blue },
            { TokenType.Nil, ConsoleColor.Blue },
            { TokenType.Not, ConsoleColor.Blue },
            { TokenType.Of, ConsoleColor.Blue },
            { TokenType.Or, ConsoleColor.Blue },
            { TokenType.Packed, ConsoleColor.Blue },
            { TokenType.Procedure, ConsoleColor.Blue },
            { TokenType.Program, ConsoleColor.Blue },
            { TokenType.Record, ConsoleColor.Blue },
            { TokenType.Repeat, ConsoleColor.Blue },
            { TokenType.Set, ConsoleColor.Blue },
            { TokenType.Then, ConsoleColor.Blue },
            { TokenType.To, ConsoleColor.Blue },
            { TokenType.Type, ConsoleColor.Blue },
            { TokenType.Until, ConsoleColor.Blue },
            { TokenType.Var, ConsoleColor.Blue },
            { TokenType.While, ConsoleColor.Blue },
            { TokenType.With, ConsoleColor.Blue },
            //operators
            { TokenType.Plus, ConsoleColor.Yellow },
            { TokenType.Minus, ConsoleColor.Yellow },
            { TokenType.Asterisk, ConsoleColor.Yellow },
            { TokenType.ForwardSlash, ConsoleColor.Yellow },
            { TokenType.Equals, ConsoleColor.Yellow },
            { TokenType.LessThan, ConsoleColor.Yellow },
            { TokenType.GreaterThan, ConsoleColor.Yellow },
            { TokenType.LeftSquareBracket, ConsoleColor.Yellow },
            { TokenType.RightSquareBracket, ConsoleColor.Yellow },
            { TokenType.Dot, ConsoleColor.Yellow },
            { TokenType.Comma, ConsoleColor.Yellow },
            { TokenType.Colon, ConsoleColor.Yellow },
            { TokenType.Semicolon, ConsoleColor.Yellow },
            { TokenType.UpArrow, ConsoleColor.Yellow },
            { TokenType.OpenParen, ConsoleColor.Yellow },
            { TokenType.CloseParen, ConsoleColor.Yellow },
            { TokenType.KetPair, ConsoleColor.Yellow },
            { TokenType.LessThanOrEqual, ConsoleColor.Yellow },
            { TokenType.GreaterThanOrEqual, ConsoleColor.Yellow },
            { TokenType.Walrus, ConsoleColor.Yellow },
            { TokenType.DoubleDot, ConsoleColor.Yellow },
            //misc
            { TokenType.Comment, ConsoleColor.Green },
            { TokenType.WHITESPACE, ConsoleColor.White },
            { TokenType.LINEBREAK, ConsoleColor.White },
         };

         int lgb10 = (int)Math.Ceiling(Math.Log10(source.LineCount));
         ConsoleColor ofg = Console.ForegroundColor, obg = Console.BackgroundColor;
         ConsoleColor primaryMessageColor;
         Console.BackgroundColor = ConsoleColor.Black;
         switch (message.severity)
         {
            case Source.Severity.Pedantic:
               primaryMessageColor = ConsoleColor.White;
               break;
            case Source.Severity.Information:
               primaryMessageColor = ConsoleColor.Cyan;
               break;
            case Source.Severity.Suggestion:
               primaryMessageColor = ConsoleColor.Blue;
               break;
            case Source.Severity.Warning:
               primaryMessageColor = ConsoleColor.Yellow;
               break;
            case Source.Severity.Error:
               primaryMessageColor = ConsoleColor.Red;
               break;
            default:
               primaryMessageColor = ConsoleColor.DarkMagenta;
               break;
         }
         Console.ForegroundColor = ConsoleColor.Red;
         if (message.isError)
            Console.WriteLine("The following is an error severe enough to cause the compiler to halt:");

         Console.ForegroundColor = primaryMessageColor;
         Console.WriteLine($"{source.Filename} - [{message.phase}] [{message.severity}]: {message.message}");
         if (message.fileLocation.HasValue)
         {
            Console.WriteLine($"The above error was reported to correlate to the following part of [{source.Filename}]:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            //todo: add colorful source reporting if that information is avaliable.
            var loc = source.GetLineColFromFileLocation(message.fileLocation.Value);
            const int windowSize = 3;
            for (int i = -windowSize; i < windowSize + 1; i++)
            {
               int desiredLine = loc.line + i;
               if (desiredLine >= 0 && desiredLine < source.LineCount)
               {
                  if (desiredLine <= source.LexedUpToLineNumber)
                  {
                     List<Token> toks = source.GetLexTokenSourceLine(desiredLine);
                     Console.ForegroundColor = ConsoleColor.DarkMagenta;
                     Console.Write($"{(desiredLine + 1).ToString().PadRight(lgb10)} ");
                     int printedLength = 0;
                     for (int tk = 0; tk < toks.Count; tk++)
                     {
                        if (toks[tk].Type == TokenType.LINEBREAK)
                        {
                           if (tk != toks.Count - 1)
                           { //there's a linebreak and it isn't the last token in the line? WTH?

                           }
                           else continue;
                        }
                        Console.ForegroundColor = lexemColors[toks[tk].Type];
                        Console.Write(toks[tk].Content);
                        printedLength += toks[tk].TokenLength; //should we include the lengths of linebreaks?
                     }
                     Console.ForegroundColor = ConsoleColor.White;
                     string remainder = source.GetSourceLine(desiredLine).Substring(printedLength);
                     Console.WriteLine(remainder);
                  }
                  else
                  {
                     Console.BackgroundColor = ConsoleColor.Black;
                     Console.ForegroundColor = ConsoleColor.DarkMagenta;
                     Console.Write($"{(desiredLine + 1).ToString().PadRight(lgb10)} ");
                     Console.ForegroundColor = ConsoleColor.White;
                     Console.WriteLine($"{source.GetSourceLine(desiredLine)}");
                  }
                  if (i == 0)
                  {
                     //print a pointer to the exact character where the error is, but print it in a way where there's room.
                     Console.ForegroundColor = ConsoleColor.Black;
                     Console.BackgroundColor = ConsoleColor.White;
                     string preamble = "error here ", pointer = "----^";
                     int horizLen = loc.col + 1 + lgb10 + 1;
                     if (horizLen >= preamble.Length + pointer.Length)
                        Console.WriteLine($"{preamble}{pointer}".PadLeft(horizLen));
                     else if (horizLen >= pointer.Length)
                        Console.WriteLine($"{pointer}".PadLeft(horizLen));
                     else
                        Console.WriteLine(pointer.Substring(pointer.Length - horizLen));
                     Console.ForegroundColor = ConsoleColor.White;
                     Console.BackgroundColor = ConsoleColor.Black;
                  }
               }
            }
         }
         else
         {
            Console.WriteLine("The above error did was not reported to correlate to any particular part of the source code.");
         }
         Console.ForegroundColor = ofg;
         Console.BackgroundColor = obg;
      }
   }
}