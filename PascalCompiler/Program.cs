//Copyright TheUbMunster, 2024, all rights reserved.

using PascalCompiler.Lexer;
using System.Text.RegularExpressions;
using System.Text;

namespace PascalCompiler
{
   class Program
   {
      static Program()
      {
         Regex.CacheSize = 256;
      }

      static void Main(string[] args)
      {
         StringBuilder errorAggregate = new StringBuilder();
#if DEBUG
         Console.WriteLine("THIS IS A DEBUG BUILD");
#endif
         CommandProcessor.Process(args, null!/*Lexer.Lexer.Lex*/);


//         IEnumerable<string> flags = args.Where(x => x.StartsWith("-"));
//         IEnumerable<string> nonflags = args.Where(x => !x.StartsWith("-"));
//         if (!nonflags.Any())
//         {
//            Console.WriteLine("No non-flag arguments, nothing to do.");
//            return;
//         }
         
//         string fp = nonflags.First();
//         //overhaul this garbage
//         bool lexOnly = flags.Contains("-l");
//         bool parseOnly = flags.Contains("-p");
//         bool typecheckOnly = flags.Contains("-t");
//         bool assembleOnly = flags.Contains("-s");
//         bool buildOnly = flags.Contains("-be");
//         int optimizationLevel = int.Parse(flags.FirstOrDefault(x => x.StartsWith("-O"), "-O0").Substring(2));

//         List<Token> tokens = null!;
//         try
//         {
//            tokens = Lexer.Lex(fp);
//         }
//#pragma warning disable CS0168
//         catch (Exception e)
//#pragma warning restore CS0168
//         {
//#if DEBUG
//            Console.WriteLine(e.ToString());
//#endif
//            errorAggregate.Append(e.Message);
//            tokens = null!;
//         }
      }
   }
}
