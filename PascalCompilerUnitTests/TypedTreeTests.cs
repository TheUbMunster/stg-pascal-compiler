//Copyright TheUbMunster, 2024, all rights reserved.

using System.Text;
using PascalCompiler;

namespace PascalCompilerUnitTests
{
   /// <summary>
   /// Runs the compiler's "-t" flag (type check, then print the tree as an s-expression annotated with the types and symbols the
   /// checker assigned) on each program in TypeCheckScripts and compares the output with the expected tree stored in
   /// ExpectedTypedTrees. The programs are written to ISO 7185 and between them use every kind of type, parameter and statement.
   ///
   /// The expected files are snapshots of reviewed output. When the checker changes on purpose, regenerate them by running the
   /// tests with the environment variable UPDATE_TYPED_TREES=1 (every test then rewrites its file and reports Inconclusive),
   /// and review the resulting diff before committing. A failing test writes the actual output to a temp file and names the
   /// first differing line.
   /// </summary>
   [TestClass]
   public class TypedTreeTests
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
      private static string ScriptsDirectory => Path.Combine(RepositoryRoot, "PascalCompilerUnitTests", "TypeCheckScripts");
      private static string ExpectedDirectory => Path.Combine(RepositoryRoot, "PascalCompilerUnitTests", "ExpectedTypedTrees");

      [TestMethod]
      [DataRow("Scalars.pas")]
      [DataRow("Structures.pas")]
      [DataRow("Routines.pas")]
      [DataRow("Labels.pas")]
      public void Program_EmitsTheExpectedTypedSExpression(string script)
      {
         string actual = EmittedSExpression(Path.Combine(ScriptsDirectory, script));
         string expectedPath = Path.Combine(ExpectedDirectory, Path.ChangeExtension(script, ".sexp"));

         if (Environment.GetEnvironmentVariable("UPDATE_TYPED_TREES") == "1")
         {
            Directory.CreateDirectory(ExpectedDirectory);
            File.WriteAllText(expectedPath, actual);
            Assert.Inconclusive($"rewrote {expectedPath}; review it, then run the tests again without UPDATE_TYPED_TREES");
         }

         Assert.IsTrue(File.Exists(expectedPath), $"no expected tree for {script}; run the tests once with UPDATE_TYPED_TREES=1 to create {expectedPath}");
         string expected = Normalize(File.ReadAllText(expectedPath));
         if (expected == actual)
            return;

         string[] expectedLines = expected.Split('\n');
         string[] actualLines = actual.Split('\n');
         int line = 0;
         while (line < expectedLines.Length && line < actualLines.Length && expectedLines[line] == actualLines[line])
            line++;
         string actualPath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(script, ".actual.typed.sexp"));
         File.WriteAllText(actualPath, actual);
         Assert.Fail(
            $"the typed s-expression for {script} differs from {expectedPath} at line {line + 1}:\n" +
            $"   expected: {(line < expectedLines.Length ? expectedLines[line] : "<end of file>")}\n" +
            $"   actual:   {(line < actualLines.Length ? actualLines[line] : "<end of file>")}\n" +
            $"({expectedLines.Length} expected lines, {actualLines.Length} actual lines; actual output written to {actualPath})");
      }

      /// <summary>After a successful check Ambiguity.Collapse has replaced every placeholder, so the printed tree names no Ambiguous node.</summary>
      [TestMethod]
      [DataRow("Scalars.pas")]
      [DataRow("Structures.pas")]
      [DataRow("Routines.pas")]
      [DataRow("Labels.pas")]
      public void Program_HasNoPlaceholderLeftAfterChecking(string script)
      {
         string tree = EmittedSExpression(Path.Combine(ScriptsDirectory, script));
         Assert.IsFalse(tree.Contains("(Ambiguous "), $"{script} still contains an ambiguity placeholder after type checking");
      }

      /// <summary>
      /// Every identifier occurrence in a checked program points at a symbol: the checker resolved all of them. The one exception is the
      /// program name, which 6.10 says has no significance within the program and so denotes nothing.
      /// </summary>
      [TestMethod]
      [DataRow("Scalars.pas")]
      [DataRow("Structures.pas")]
      [DataRow("Routines.pas")]
      [DataRow("Labels.pas")]
      public void Program_HasEverySymbolRecorded(string script)
      {
         string tree = EmittedSExpression(Path.Combine(ScriptsDirectory, script));
         bool nextIsProgramName = false;
         foreach (string line in tree.Split('\n'))
         {
            string trimmed = line.Trim();
            if (trimmed.StartsWith("(ProgramHeading"))
               nextIsProgramName = true;
            bool isIdentifier = trimmed == "(Identifier" || trimmed.StartsWith("(Identifier ");
            if (!isIdentifier)
               continue;
            if (nextIsProgramName)
               nextIsProgramName = false; //the program name: the first identifier of the heading
            else if (!trimmed.StartsWith("(Identifier ->"))
               Assert.Fail($"{script}: an identifier was left without a symbol: {trimmed}");
         }
      }

      /// <summary>
      /// The s-expression "-t" prints for one file: everything from the first line that starts with '(' up to, but not including,
      /// the trailing success line, with normalized line endings.
      /// </summary>
      private static string EmittedSExpression(string sourcePath)
      {
         string output;
         using (MemoryStream ms = new MemoryStream())
         using (TextWriter tw = new StreamWriter(ms))
         {
            CommandProcessor.Process(new string[] { "-no-ansi", "-t", sourcePath }, tw);
            tw.Flush();
            output = Encoding.UTF8.GetString(ms.ToArray());
         }
         output = Normalize(output);
         Assert.IsTrue(output.EndsWith(CommandProcessor.typecheckSucceedMessage), $"-t did not succeed on {sourcePath}:\n{output}");
         output = output.Substring(0, output.Length - CommandProcessor.typecheckSucceedMessage.Length).TrimEnd('\n');
         int treeStart = output.IndexOf("\n(", StringComparison.Ordinal);
         if (output.StartsWith("("))
            treeStart = 0;
         Assert.IsTrue(treeStart >= 0, $"-t printed no s-expression for {sourcePath}:\n{output}");
         return output.Substring(treeStart).TrimStart('\n');
      }

      private static string Normalize(string text)
      {
         return text.Replace("\r\n", "\n").Replace('\r', '\n').TrimEnd('\n');
      }
   }
}
