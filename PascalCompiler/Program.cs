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
         Regex.CacheSize = 1000; //we use a lot of regex.
      }

      static void Main(string[] args)
      {
         Console.ForegroundColor = ConsoleColor.White;
         Console.BackgroundColor = ConsoleColor.Black;
         Console.Clear();
#if DEBUG
         Console.WriteLine("THIS IS A DEBUG BUILD");
#endif
         CommandProcessor.Process(args);
      }
   }
}
