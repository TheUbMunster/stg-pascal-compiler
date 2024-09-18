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
      public void ScanGoodCharacters()
      {
         //concatenation of all legal characters.
         string source = new string(PascalCompiler.Scanner.LegalCharacterSet.legalChars);
         string path = Path.GetTempFileName();
         File.WriteAllText(path, source);
         string output;
         using (MemoryStream ms = new MemoryStream())
         using (TextWriter tw = new StreamWriter(ms))
         {
            CommandProcessor.Process(new string[] { "-no-ansi", "-r", path }, tw);
            tw.Flush();
            output = Encoding.UTF8.GetString(ms.ToArray());
         }
         Assert.IsTrue(output.Contains(CommandProcessor.scanSucceedMessage));
      }

      [TestMethod]
      public void LexGoodHelloWorld()
      {
         string source = 
@"Program Hello:
Begin
   WriteLn('Hello, World!');
   ReadLn;
End.";
         string path = Path.GetTempFileName();
         File.WriteAllText(path, source);
         string output;
         using (MemoryStream ms = new MemoryStream())
         using (TextWriter tw = new StreamWriter(ms))
         {
            CommandProcessor.Process(new string[] { "-no-ansi", "-l", path }, tw);
            tw.Flush();
            output = Encoding.UTF8.GetString(ms.ToArray());
         }
         Assert.IsTrue(output.Contains(CommandProcessor.lexSucceedMessage));
      }

      [TestMethod]
      public void LexBadHelloWorld()
      {
         string source =
@"Program Hello:
Begin
   WriteLn('Hello, World!');
   ReadLn; _______________________
End.";
         string path = Path.GetTempFileName();
         File.WriteAllText(path, source);
         string output;
         using (MemoryStream ms = new MemoryStream())
         using (TextWriter tw = new StreamWriter(ms))
         {
            CommandProcessor.Process(new string[] { "-no-ansi", "-l", path }, tw);
            tw.Flush();
            output = Encoding.UTF8.GetString(ms.ToArray());
         }
         Assert.IsTrue(output.Contains(CommandProcessor.lexFailMessage));
      }
   }
}