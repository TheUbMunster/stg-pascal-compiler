using Antlr4;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.Diagnostics;

namespace AntlrTest
{
   public class Program
   {
      const string AntlrPath = "\"D:\\source\\java\\antlr-4.13.2-complete.jar\"";
      const string GrammarName = "TestGrammar";
      const string StartRule = "prog";
      const string GrammarFilename = "TestGrammar.g4";
      const string TestFilename = "test.txt";
      static void Main(string[] args)
      {
         Try("1+2+3");
         Try("12+3");
         Try("1++");
         Try("Asdf");
         string testString = "1 + 2 + 3";
         File.WriteAllText(TestFilename, testString);
         Process.Start("java", $"-jar {AntlrPath} {GrammarFilename}");
         Process.Start("javac", $"-cp {AntlrPath} {GrammarName}*.java");
         Process guiProcess = Process.Start("java", $"-cp .;{AntlrPath} org.antlr.v4.gui.TestRig {GrammarName} {StartRule} -gui {TestFilename}");
         guiProcess.WaitForExit();
      }

      static void Try(string input)
      {
         var str = new AntlrInputStream(input);
         Console.WriteLine(input);
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