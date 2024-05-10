using System.Diagnostics;
using System.IO.MemoryMappedFiles;

namespace PascalCompilerUnitTests
{
   [TestClass]
   public class LexTests
   {
      [TestMethod]
      public void LexHelloWorld()
      {
         //MemoryMappedFile.CreateNew()
         string hello = @"Program Hello:
Begin
   WriteIn('Hello, World!');
   ReadIn;
End.";
         Process.Start(new ProcessStartInfo()
         {
            FileName = "PascalCompiler.exe",
            Arguments = "-l"
         });
      }
   }
}