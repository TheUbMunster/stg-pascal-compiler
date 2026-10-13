//Copyright TheUbMunster, 2024, all rights reserved.

using System.Text;
using PascalCompiler;

namespace PascalCompilerUnitTests
{
   /// <summary>
   /// Runs the compiler's "-p" flag (parse only, print the parse tree as an s-expression) on each demo program in
   /// Test-Pascal-Scripts and compares the output with the expected tree stored next to these tests in ExpectedParseTrees.
   ///
   /// The expected files are snapshots of reviewed output. When the parser changes on purpose, regenerate them by running the
   /// tests with the environment variable UPDATE_PARSE_TREES=1 (every test then rewrites its file and reports Inconclusive),
   /// and review the resulting diff before committing. A failing test writes the actual output to a temp file and names the
   /// first differing line.
   /// </summary>
   [TestClass]
   public class ParseTreeTests
   {
      private static string RepositoryRoot
      {
         get
         {
            DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "PascalCompiler.sln")))
               dir = dir.Parent;
            Assert.IsNotNull(dir, "could not locate the repository root (the directory containing PascalCompiler.sln)");
            return dir!.FullName;
         }
      }
      private static string ScriptsDirectory => Path.Combine(RepositoryRoot, "PascalCompiler", "Test-Pascal-Scripts");
      private static string ExpectedDirectory => Path.Combine(RepositoryRoot, "PascalCompilerUnitTests", "ExpectedParseTrees");

      [TestMethod]
      [DataRow("Calculator.pas")]
      [DataRow("Factorial.pas")]
      [DataRow("Fibonacci.pas")]
      [DataRow("Palindrome.pas")]
      [DataRow("Reverse.pas")]
      public void DemoProgram_EmitsTheExpectedSExpression(string script)
      {
         string actual = EmittedSExpression(Path.Combine(ScriptsDirectory, script));
         string expectedPath = Path.Combine(ExpectedDirectory, Path.ChangeExtension(script, ".sexp"));

         if (Environment.GetEnvironmentVariable("UPDATE_PARSE_TREES") == "1")
         {
            Directory.CreateDirectory(ExpectedDirectory);
            File.WriteAllText(expectedPath, actual);
            Assert.Inconclusive($"rewrote {expectedPath}; review it, then run the tests again without UPDATE_PARSE_TREES");
         }

         Assert.IsTrue(File.Exists(expectedPath), $"no expected tree for {script}; run the tests once with UPDATE_PARSE_TREES=1 to create {expectedPath}");
         string expected = Normalize(File.ReadAllText(expectedPath));
         if (expected == actual)
            return;

         string[] expectedLines = expected.Split('\n');
         string[] actualLines = actual.Split('\n');
         int line = 0;
         while (line < expectedLines.Length && line < actualLines.Length && expectedLines[line] == actualLines[line])
            line++;
         string actualPath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(script, ".actual.sexp"));
         File.WriteAllText(actualPath, actual);
         Assert.Fail(
            $"the s-expression for {script} differs from {expectedPath} at line {line + 1}:\n" +
            $"   expected: {(line < expectedLines.Length ? expectedLines[line] : "<end of file>")}\n" +
            $"   actual:   {(line < actualLines.Length ? actualLines[line] : "<end of file>")}\n" +
            $"({expectedLines.Length} expected lines, {actualLines.Length} actual lines; actual output written to {actualPath})");
      }

      /// <summary>
      /// The s-expression "-p" prints for one file: everything from the first line that starts with '(' up to, but not including,
      /// the trailing success line, with normalized line endings. Compiler messages from the earlier phases (e.g. the file
      /// encoding note) precede the tree and are not part of the comparison.
      /// </summary>
      private static string EmittedSExpression(string sourcePath)
      {
         string output;
         using (MemoryStream ms = new MemoryStream())
         using (TextWriter tw = new StreamWriter(ms))
         {
            CommandProcessor.Process(new string[] { "-no-ansi", "-p", sourcePath }, tw);
            tw.Flush();
            output = Encoding.UTF8.GetString(ms.ToArray());
         }
         output = Normalize(output);
         Assert.IsTrue(output.EndsWith(CommandProcessor.parseSucceedMessage), $"-p did not succeed on {sourcePath}:\n{output}");
         output = output.Substring(0, output.Length - CommandProcessor.parseSucceedMessage.Length).TrimEnd('\n');
         int treeStart = output.IndexOf("\n(", StringComparison.Ordinal);
         if (output.StartsWith("("))
            treeStart = 0;
         Assert.IsTrue(treeStart >= 0, $"-p printed no s-expression for {sourcePath}:\n{output}");
         return output.Substring(treeStart).TrimStart('\n');
      }

      private static string Normalize(string text)
      {
         return text.Replace("\r\n", "\n").Replace('\r', '\n').TrimEnd('\n');
      }
   }
}
