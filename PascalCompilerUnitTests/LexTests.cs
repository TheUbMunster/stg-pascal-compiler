//Copyright TheUbMunster, 2024, all rights reserved.

using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Text;
using PascalCompiler;

namespace PascalCompilerUnitTests
{
   [TestClass]
   public class LexTests
   {
      //TODO: somehow allow for a memory mapped file or something so that SSD doesn't undergo unecessary wear.

      [TestMethod]
      [DataRow(true)]
      [DataRow(false)]
      public void ScanGoodCharacters(bool doAnsi)
      {
         //concatenation of all legal characters.
         string sourceCode = new string(PascalCompiler.Scanner.LegalCharacterSet.legalChars);
         string output;
         using (DisposableFile df = new DisposableFile())
         {
            File.WriteAllText(df.FilePath, sourceCode);
            using (MemoryStream ms = new MemoryStream())
            using (TextWriter tw = new StreamWriter(ms))
            {
               string[] args;
               if (doAnsi)
                  args = new string[] { "-r", df.FilePath };
               else
                  args = new string[] { "-no-ansi", "-r", df.FilePath };
               CommandProcessor.Process(args, tw);
               tw.Flush();
               output = Encoding.UTF8.GetString(ms.ToArray());
            }
         }
         Assert.IsTrue(output.Contains(CommandProcessor.scanSucceedMessage));
      }

      [TestMethod]
      [DataRow(true)]
      [DataRow(false)]
      public void LexGoodHelloWorld(bool doAnsi)
      {
         string sourceCode = 
@"Program Hello:
Begin
   WriteLn('Hello, World!');
   ReadLn;
End.";
         string output;
         using (DisposableFile df = new DisposableFile())
         {
            File.WriteAllText(df.FilePath, sourceCode);
            using (MemoryStream ms = new MemoryStream())
            using (TextWriter tw = new StreamWriter(ms))
            {
               string[] args;
               if (doAnsi)
                  args = new string[] { "-l", df.FilePath };
               else
                  args = new string[] { "-no-ansi", "-l", df.FilePath };
               CommandProcessor.Process(args, tw);
               tw.Flush();
               output = Encoding.UTF8.GetString(ms.ToArray());
            }
         }
         Assert.IsTrue(output.Contains(CommandProcessor.lexSucceedMessage));
      }

      [TestMethod]
      [DataRow(true)]
      [DataRow(false)]
      public void LexBadHelloWorld(bool doAnsi)
      {
         string sourceCode =
@"Program Hello:
Begin
   WriteLn('Hello, World!');
   ReadLn; _______________________
End.";
         string output;
         using (DisposableFile df = new DisposableFile())
         {
            File.WriteAllText(df.FilePath, sourceCode);
            using (MemoryStream ms = new MemoryStream())
            using (TextWriter tw = new StreamWriter(ms))
            {
               string[] args;
               if (doAnsi)
                  args = new string[] { "-l", df.FilePath };
               else
                  args = new string[] { "-no-ansi", "-l", df.FilePath };
               CommandProcessor.Process(args, tw);
               tw.Flush();
               output = Encoding.UTF8.GetString(ms.ToArray());
            }
         }
         Assert.IsTrue(output.Contains(CommandProcessor.lexFailMessage));
      }

      [TestMethod]
      [DataRow(true)]
      [DataRow(false)]
      public void LexBadMultilineCommentHelloWorld(bool doAnsi)
      {
         string sourceCode =
@"Program Hello:
Begin
   WriteLn('Hello, World!');
   ReadLn; (*
this
comment
goes
across
many
lines
*) _______________________
End.";
         string output;
         using (DisposableFile df = new DisposableFile())
         {
            File.WriteAllText(df.FilePath, sourceCode);
            using (MemoryStream ms = new MemoryStream())
            using (TextWriter tw = new StreamWriter(ms))
            {
               string[] args;
               if (doAnsi)
                  args = new string[] { "-l", df.FilePath };
               else
                  args = new string[] { "-no-ansi", "-l", df.FilePath };
               CommandProcessor.Process(args, tw);
               tw.Flush();
               output = Encoding.UTF8.GetString(ms.ToArray());
            }
         }
         Assert.IsTrue(output.Contains(CommandProcessor.lexFailMessage));
      }

      [TestMethod]
      [DataRow(true)]
      [DataRow(false)]
      public void LexBadMultilineCommentFactorial(bool doAnsi)
      {
         string sourceCode =
@"program Factorial;
var
    num, fact, i: integer; {declare vars}
begin
    writeln('Enter a number: '); {get input}
    readln(num);
    fact := 1;
    for i := 1 to num do
        fact := fact * i; (*factorialize
        yes yes*) __
    writeln('Factorial of ', num, ' is: ', fact);
end.
";
         string output;
         using (DisposableFile df = new DisposableFile())
         {
            File.WriteAllText(df.FilePath, sourceCode);
            using (MemoryStream ms = new MemoryStream())
            using (TextWriter tw = new StreamWriter(ms))
            {
               string[] args;
               if (doAnsi)
                  args = new string[] { "-l", df.FilePath };
               else
                  args = new string[] { "-no-ansi", "-l", df.FilePath };
               CommandProcessor.Process(args, tw);
               tw.Flush();
               output = Encoding.UTF8.GetString(ms.ToArray());
            }
         }
         Assert.IsTrue(output.Contains(CommandProcessor.lexFailMessage));
      }
   }
}