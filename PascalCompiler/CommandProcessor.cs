using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler
{
   static class CommandProcessor
   {
      #region Consts
      private static Flag[] flagDefs = new Flag[]
      {
         new Flag("l", "(DEBUG) This flag is if you want to lex the source and emit the lexed output only. " +
            "If an output file name is specified, the output will be written to the output file. " +
            "If not, it will be printed to stdout.", new[] { "p", "t", "s", "O0", "O1", "O2", "O3" }),
         new Flag("p", "(DEBUG) This flag is if you want to lex and parse the source and emit the AST output only. " +
            "If an output file name is specified, the output will be written to the output file. " +
            "If not, it will be printed to stdout.", new[] { "l", "t", "s", "O0", "O1", "O2", "O3" }),
         new Flag("t", "(DEBUG) This flag is if you want to lex, parse, and typecheck the source and emit the typechecking info output only. " +
            "If an output file name is specified, the output will be written to the output file. " +
            "If not, it will be printed to stdout.", new[] { "l", "p", "s", "O0", "O1", "O2", "O3" }),
         new Flag("s", "(DEBUG) This flag is if you want to lex, parse, typecheck, and emit assembly from the source only. " +
            "If an output file name is specified, the output will be written to the output file. " +
            "If not, it will be printed to stdout.", new[] { "l", "p", "t", "O0", "O1", "O2", "O3" }),
         new Flag("O0", "This flag indicates to the compiler to perform no optimizations", 
            new[] { "l", "p", "t", "s", "O1", "O2", "O3" }),
         new Flag("O1", "This flag indicates to the compiler to perform minimal optimizations",
            new[] { "l", "p", "t", "s", "O0", "O2", "O3" }),
         new Flag("O2", "This flag indicates to the compiler to perform moderate optimizations",
            new[] { "l", "p", "t", "s", "O0", "O1", "O3" }),
         new Flag("O3", "This flag indicates to the compiler to perform all optimizations",
            new[] { "l", "p", "t", "s", "O0", "O1", "O2" }),
         new Flag("n", "The output name of the executable",
            new[] { "l", "p", "t", "s" }),
         //new Flag("z", "This flag indicates to the compiler that the source code will be passed via",
         //   new string[] { }),
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
      public static int Process(string[] args, Action<Source> lexerDelegate)
      {
         IReadOnlyList<string> flags = args.Where(x => x.StartsWith("-")).Select(x => x.Substring(1)).ToList();
         IReadOnlyList<string> nonflags = args.Where(x => !x.StartsWith("-")).ToList();
         //check for help
         if (flags.Contains("-help") || flags.Contains("help") || flags.Contains("h"))
         {
            Console.WriteLine("PascalCompiler - a ISO/IEC 7185:1990(E) pascal compiler written by TheUbMunster" + Environment.NewLine +
               "Usage: [flags...] [filepath...]" + Environment.NewLine + // | source
               "Flags:" + Environment.NewLine +
               "\t--help | -help | -h - Prints this help message." + Environment.NewLine + Environment.NewLine +
               string.Join(Environment.NewLine, flagDefs.Select(x => $"\t-{x.flagName} - {x.helpMessage}{Environment.NewLine}\tThis flag cannot appear with the following flags: {string.Join(", ", x.mutualExclusionFlags)}{Environment.NewLine}")) + 
               "Filepath:" + Environment.NewLine +
               "\tfilepath(s) to pascal source file(s) to compile.");
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
         //create sources.
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

         }
      }
      public static void PrintMessage(Source source, Source.Message message)
      {
         int lgb10 = (int)Math.Ceiling(Math.Log10(source.LineCount));
         ConsoleColor ofg = Console.ForegroundColor, obg = Console.BackgroundColor;
         ConsoleColor primaryMessageColor;
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
         Console.WriteLine($"{source.Filename} - [{message.phase}] [{message.severity}]:");
         Console.WriteLine(message.message);
         if (message.fileLocation.HasValue)
         {
            Console.WriteLine($"The above error was reported to correlate to the following part of [{source.Filename}]:");
            //todo: add colorful source reporting if that information is avaliable.
            var loc = source.GetLineColFromFileLocation(message.fileLocation.Value);
            const int windowSize = 2;
            for (int i = -windowSize; i < windowSize + 1; i++)
            {
               int desiredLine = loc.line + i;
               if (desiredLine > 0 && desiredLine < source.LineCount)
                  ;
               if (i == 0)
                  ;
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