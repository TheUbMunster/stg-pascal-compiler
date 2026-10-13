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
using System;

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
      public static readonly string scanSucceedMessage = "Success: scan check complete";
      public static readonly string lexSucceedMessage = "Success: lexical analysis complete";
      public static readonly string parseSucceedMessage = "Success: parsing complete";

      public static readonly string scanFailMessage = "Compilation failed at the SourceScan stage";
      public static readonly string lexFailMessage = "Compilation failed at the Lexer stage";
      public static readonly string parseFailMessage = "Compilation failed at the Parser stage";
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
         //also strip empty entries?
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
            //TODO: change this so that messages are only read out
            //    1. at the very end
            //    2. when an error occurs, just before exiting
            foreach (Source.Message m in s.Messages)
            {
               PrintMessage(s, m, outputWriter, !flags.Contains("no-ansi"));
            }
            s.ClearMessages();
         }
         if (sources.Any(s => s.ErrorHasOccurred))
         {
            outputWriter.WriteLine(scanFailMessage);
            return -1;
         }
         if (flags.Contains("r"))
         {
            //foreach (Source s in sources)
            //{
            //   Console.WriteLine("idk");
            //}
            outputWriter.WriteLine(scanSucceedMessage);
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
            outputWriter.WriteLine(lexFailMessage);
            return -1;
         }
         if (flags.Contains("l"))
         {
            foreach (Source s in sources)
            {
               outputWriter.WriteLine(string.Join("\n", s.LexerTokensFlattenedView));
            }
            outputWriter.WriteLine(lexSucceedMessage);
            return 0;
         }
         //=======
         // PARSE
         //=======
#if DEBUG
         for (int i = 0; i < sources.Count; i++)
         {
            Parser.Parser.Parse(sources[i]);
         }
#else
         Parallel.For(0, sources.Count, (i) =>
         {
            Parser.Parser.Parse(sources[i]);
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
            outputWriter.WriteLine(parseFailMessage);
            return -1;
         }
         if (flags.Contains("p"))
         {
            foreach (Source s in sources)
            {
               outputWriter.WriteLine(string.Join(Environment.NewLine, s.ParserNodes.Select(x => x.ToString(0, true)))); //the parse tree as an s-expression (-l already prints the tokens)
            }
            outputWriter.WriteLine(parseSucceedMessage);
            return 0;
         }
         //===========
         // TYPECHECK
         //===========

         throw new System.NotImplementedException(); //at the very end
      }
      public static void PrintMessage(Source source, Source.Message message, TextWriter outputWriter, bool doAnsiColoring)
      {
         (TokenType tt, int len) GetTokenTypeAtFileLocation(int fileLocation)
         {
            foreach (Token tok in source.LexerTokensFlattenedView)
            {
               if (tok.FileLocation <= fileLocation && fileLocation < tok.FileLocation + tok.TokenLength)
                  return (tok.Type, (tok.FileLocation + tok.TokenLength) - fileLocation);
            }
            source.AppendMessage(new(Source.CompilerPhase.Runtime, Source.Severity.Error, "During the compiler's attempt to report a line(s) of code, the compiler attempted to refer to the lexem type of an unlexed portion of code.", null, true));
            throw new Exception("An error occurred in the compiler, see the message log.");
         }
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
         if (doAnsiColoring)
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
               if (desiredLine >= 0 && desiredLine < source.LineCount) //so long as the line is within the file bounds
               {
                  if (doAnsiColoring)
                  {
                     outputWriter.Write("\x1b[40m"); //black background
                     outputWriter.Write(lineNumbersColor);
                  }
                  outputWriter.Write($"{(desiredLine + 1).ToString().PadRight(lgb10)} "); //print line number 1 based with proper padding

                  //this stuff gets the line we're meant to print
                  int desiredLineFileLoc = source.GetFileLocFromLineIndex(desiredLine);
                  int desiredLinePlusOneFileLoc = source.GetFileLocFromLineIndex(desiredLine + 1);
                  string normalLine;
                  if (desiredLinePlusOneFileLoc != -1)
                     normalLine = source.FileContents.Substring(desiredLineFileLoc, desiredLinePlusOneFileLoc - desiredLineFileLoc);
                  else
                     normalLine = source.FileContents.Substring(desiredLineFileLoc);
                  int desiredLineLen = normalLine.Length; //calculated afterwards via string incase of -1

                  if (doAnsiColoring)
                  {
                     for (int ind = 0; ind < desiredLineLen;)
                     {
                        //this for loop iterates over segments of this line, determines the color for that segment and prints it.
                        int writtenLength;
                        if (ind + desiredLineFileLoc < source.LexedUpToFileLocation)
                        { //we're still in the lexed code, find the color
                           var res = GetTokenTypeAtFileLocation(ind + desiredLineFileLoc);
                           outputWriter.Write(lexemColors[res.tt]);
                           writtenLength = res.len;
                           //the colored length reported might be much longer than we even have left to print
                           //e.g., multiline comment (but we only want to substring `normalLine` & print for this line)
                           writtenLength = writtenLength + ind > desiredLineLen ? desiredLineLen - ind : writtenLength;
                        }
                        else
                        { //we ran out of lexed code, print in black and white
                           outputWriter.Write("\x1b[37m"); //white foreground
                           writtenLength = desiredLineLen - ind;
                        }
                        if (writtenLength != -1)
                           outputWriter.Write(normalLine.Substring(ind, writtenLength));
                        else
                           outputWriter.Write(normalLine.Substring(ind));
                        ind += writtenLength;
                     }
                  }
                  else
                  {
                     outputWriter.Write(normalLine); //print the line with no coloring
                  }
                  if (i == 0) //this line is the line w/ the error, print the arrow pointing at the error
                  {
                     //print a pointer to the exact character where the error is, but print it in a way where there's room.
                     if (source.LineCount == desiredLine + 1) //0 based index
                        outputWriter.Write('\n'); //need to insert newline bc the line we're reporting on is the very last line of the file
                     if (doAnsiColoring)
                        outputWriter.Write("\x1b[40m"); //black background
                     string left = "error here ----^", right = "^---- error here";
                     int horizLen = linCol.col + 1 + lgb10 + 1; //account for width introduced by the printed line numbers
                     bool chooseLeft = horizLen >= left.Length; //do we choose "error here ----^" vs the alternative?
                     outputWriter.Write(new string(' ', horizLen - (chooseLeft ? left.Length : 1)));
                     if (doAnsiColoring)
                     {
                        outputWriter.Write("\x1b[30m"); //black foreground
                        outputWriter.Write("\x1b[47m"); //white background
                     }
                     if (chooseLeft) //print the arrow
                        outputWriter.WriteLine(left);
                     else
                        outputWriter.WriteLine(right);
                  }
               }
            }
            if (doAnsiColoring)
            {
               outputWriter.Write("\x1b[37m"); //white foreground
               outputWriter.Write("\x1b[40m"); //black background
            }
            outputWriter.Write('\n');
         }
         else
         {
            outputWriter.WriteLine("The above error was not reported to correlate to any particular part of the source code.");
         }
      }
   }
}