using Antlr4;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace AntlrTest
{
   public class Program
   {
      static void Main(string[] args)
      {
         Try("1+2+3");
         Try("12+3");
         Try("1++");
      }

      static void Try(string input)
      {
         var str = new AntlrInputStream(input);
         System.Console.WriteLine(input);
         var lexer = new TestGrammarLexer(str);
         var tokens = new CommonTokenStream(lexer);
         var parser = new TestGrammarParser(tokens);
         parser.RemoveErrorListeners();
         //var listener_parser = new ErrorListener();
         //parser.AddErrorListener(listener_parser);
         var tree = parser.prog();
         string.Join(", ", tree.children);
         //if (listener_parser.had_error)
         //   System.Console.WriteLine("error in parse.");
         //else
         //   System.Console.WriteLine("parse completed.");
      }
   }
}