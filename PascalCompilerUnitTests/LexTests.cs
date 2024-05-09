namespace PascalCompilerUnitTests
{
   [TestClass]
   public class LexTests
   {
      [TestMethod]
      public void LexHelloWorld()
      {
         string hello = @"Program Hello:
Begin
   WriteIn('Hello, World!');
   ReadIn;
End.";
      }
   }
}