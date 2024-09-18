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
      public void LexHelloWorld()
      {
         string hello = 
@"Program Hello:
Begin
   WriteLn('Hello, World!');
   ReadLn;
End.";
         string path = Path.GetTempFileName();
         File.WriteAllText(path, hello);
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
   }
}