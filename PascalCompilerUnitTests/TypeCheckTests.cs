//Copyright TheUbMunster, 2024, all rights reserved.

using System.Text;
using PascalCompiler;

namespace PascalCompilerUnitTests
{
   /// <summary>
   /// Runs the compiler's "-t" flag on one-program sources written inline. The failing cases each break exactly one rule of clause 6 and
   /// expect the message that cites it; the passing cases exercise the rules' permitted side, including the places where the parser's
   /// ambiguity placeholders must be resolved by what an identifier denotes.
   /// </summary>
   [TestClass]
   public class TypeCheckTests
   {
      [TestMethod]
      //6.2 scopes
      [DataRow("program t(output); begin x := 1 end.", "6.2.2.1")]
      [DataRow("program t(output); var x: integer; x: real; begin end.", "6.2.2.7")]
      [DataRow("program t(output); procedure p(x: integer); var x: real; begin end; begin end.", "6.2.2.7")]
      [DataRow("program t(output); const c = c; begin end.", "6.2.2.1")]
      [DataRow("program t(output); type p = ^q; begin end.", "6.2.2.9")]
      [DataRow("program t(output); begin integer := 1 end.", "denotes a type")]
      //6.1 lexical values
      [DataRow("program t(output); var i: integer; begin i := 99999999999 end.", "exceeds maxint")]
      [DataRow("program t(output); label 10000; begin 10000: end.", "6.1.6")]
      [DataRow("program t(output); begin writeln('') end.", "6.1.7")]
      //6.4 types
      [DataRow("program t(output); type s = 5..1; begin end.", "6.4.2.4")]
      [DataRow("program t(output); type s = 'a'..5; begin end.", "6.4.2.4")]
      [DataRow("program t(output); type f = file of text; begin end.", "6.4.3.5")]
      [DataRow("program t(output); type r = record case b: Boolean of true: (x: integer) end; begin end.", "cover")]
      [DataRow("program t(output); type r = record case b: Boolean of true: (x: integer); true: (y: integer) end; begin end.", "occurs more than once in this variant-part")]
      [DataRow("program t(output); type r = record x: integer; x: real end; begin end.", "6.4.3.3")]
      //6.4.6 assignment
      [DataRow("program t(output); var i: integer; begin i := 1.5 end.", "6.4.6")]
      [DataRow("program t(output); var s: packed array[1..3] of char; begin s := 'abcd' end.", "6.4.6")]
      [DataRow("program t(output); var f, g: text; begin f := g end.", "6.4.6")]
      //6.5 variables
      [DataRow("program t(output); var i: integer; begin i^ := 1 end.", "6.5.4")]
      [DataRow("program t(output); var i: integer; begin i[1] := 1 end.", "6.5.3.2")]
      [DataRow("program t(output); type r = record x: integer end; var v: r; begin v.y := 1 end.", "has no field")]
      //6.6 routines and parameters
      [DataRow("program t(output); procedure p; forward; begin end.", "declared forward but no")]
      [DataRow("program t(output); procedure p; external; begin end.", "6.1.4")]
      [DataRow("program t(output); procedure p; begin end; procedure p; begin end; begin end.", "6.2.2.7")]
      [DataRow("program t(output); function f: integer; begin end; begin end.", "never assigns its result")]
      [DataRow("program t(output); function f: integer; begin f := 1 end; begin f := 2 end.", "denotes a function")]
      [DataRow("program t(output); type r = record x: integer end; function f: r; begin end; begin end.", "6.6.2")]
      [DataRow("program t(output); procedure p(f: text); begin end; begin end.", "6.6.3.2")]
      [DataRow("program t(output); var r: real; procedure p(var i: integer); begin end; begin p(r) end.", "6.6.3.3")]
      [DataRow("program t(output); procedure p(var i: integer); begin end; begin p(1) end.", "requires a variable-access")]
      [DataRow("program t(output); var a: packed array[1..2] of integer; procedure p(var i: integer); begin end; begin p(a[1]) end.", "designated packed (6.6.3.3)")]
      [DataRow("program t(output); procedure p(i: integer); begin end; begin p end.", "formal parameter(s)")]
      [DataRow("program t(output); procedure q(x: real); begin end; procedure apply(procedure r(i: integer)); begin end; begin apply(q) end.", "congruous")]
      [DataRow("program t(output); var a: array[1..3] of real; function sum(var v: array[l..h: integer] of integer): integer; begin sum := 0 end; begin writeln(sum(a)) end.", "6.6.3.8")]
      [DataRow("program t(output); var i: integer; begin if eof(i) then end.", "6.6.6.5")]
      [DataRow("program t(output); var b: Boolean; begin b := abs(b) end.", "6.6.6.2")]
      [DataRow("program t(output); var c: char; begin c := chr(1.5) end.", "6.6.6.4")]
      [DataRow("program t(output); type p = ^integer; var x: p; begin new(x, 1) end.", "6.6.5.3")]
      //6.7 expressions
      [DataRow("program t(output); var s: set of char; u: set of integer; begin s := s + u end.", "6.7.2.4")]
      [DataRow("program t(output); type r = record x: integer end; var a, b: r; begin if a = b then end.", "table 6")]
      [DataRow("program t(output); var i: integer; s: set of char; begin if i in s then end.", "'in' takes")]
      [DataRow("program t(output); var s: set of char; begin s := ['a', 1] end.", "6.7.1")]
      [DataRow("program t(output); var r: real; begin r := r div 2 end.", "shall be integer")]
      [DataRow("program t(output); var i: integer; b: Boolean; begin b := not i end.", "operand of 'not'")]
      [DataRow("program t(output); var b: Boolean; begin b := -b end.", "A sign applies")]
      [DataRow("program t(output); var i: integer; b: Boolean; begin b := i and i end.", "'and' shall be Boolean")]
      //6.8 statements
      [DataRow("program t(output); var i: integer; begin if i then i := 1 end.", "shall be of Boolean-type")]
      [DataRow("program t(output); var i: integer; begin case i of 1: i := 1; 1: i := 2 end end.", "occurs more than once in this case-statement")]
      [DataRow("program t(output); var i: integer; begin case i of 'a': i := 1 end end.", "compatible with the case-index type")]
      [DataRow("program t(output); var i: integer; procedure p; begin for i := 1 to 2 do end; begin end.", "6.8.3.9")]
      [DataRow("program t(output); var r: real; begin for r := 1 to 2 do end.", "shall be of an ordinal-type")]
      [DataRow("program t(output); var i: integer; begin for i := 1 to 2 do i := 0 end.", "threatens")]
      [DataRow("program t(output); var i: integer; begin for i := 1 to 2 do for i := 1 to 2 do end.", "threatens")]
      [DataRow("program t(output); var i: integer; procedure p; begin i := 0 end; begin for i := 1 to 2 do p end.", "threatens")]
      [DataRow("program t(output); var i: integer; begin with i do end.", "6.8.3.10")]
      [DataRow("program t(output); label 1; begin end.", "no statement of this block is prefixed by it")]
      [DataRow("program t(output); label 1; var i: integer; begin 1: i := 0; 1: i := 1 end.", "prefixes more than one statement")]
      [DataRow("program t(output); label 1; var i: integer; begin goto 1; if i = 0 then 1: i := 1 end.", "6.8.1")]
      [DataRow("program t(output); var i: integer; begin goto 1 end.", "6.8.2.4")]
      //6.9, 6.10 input and output
      [DataRow("program t(output); type r = record x: integer end; var v: r; begin writeln(v) end.", "6.9.3.1")]
      [DataRow("program t(output); var f: file of integer; i: integer; begin readln(f, i) end.", "readln applies only to textfiles")]
      [DataRow("program t(input, output); var b: Boolean; begin read(b) end.", "6.9.1")]
      [DataRow("program t; begin writeln('x') end.", "spelled 'output'")]
      [DataRow("program t(output); var i: integer; begin read(i) end.", "spelled 'input'")]
      [DataRow("program t(output, data); begin end.", "is not declared as a variable")]
      [DataRow("program t(output, output); begin end.", "6.10")]
      public void Program_FailsTypeCheckingWithTheExpectedMessage(string source, string expectedFragment)
      {
         string output = RunTypeCheck(source);
         Assert.IsTrue(output.Contains(CommandProcessor.typecheckFailMessage), $"expected the type check to fail, but the output was:\n{output}");
         Assert.IsTrue(output.Contains("[Typecheck]"), $"expected a Typecheck message, but the output was:\n{output}");
         Assert.IsTrue(output.Contains(expectedFragment), $"expected a message containing \"{expectedFragment}\", but the output was:\n{output}");
      }

      [TestMethod]
      [DataRow("program t(output); var r: real; i: integer; begin i := 2; r := i end.")]
      [DataRow("program t(output); var p: ^integer; begin p := nil; if p = nil then new(p) end.")]
      [DataRow("program t(output); var s: packed set of 1..3; begin s := [1, 2]; s := s + [3]; s := [] end.")]
      [DataRow("program t(output); type sub = 1..5; var a: sub; i: integer; begin a := 3; i := a + 1; a := i end.")]
      [DataRow("program t(output); var s: packed array[1..5] of char; begin s := 'hello'; if s < 'world' then s := 'abcde' end.")]
      [DataRow("program t(output); function f: integer; procedure g; begin f := 1 end; begin g end; begin writeln(f) end.")]
      [DataRow("program t(output); procedure p; forward; procedure q; begin p end; procedure p; begin end; begin q end.")]
      [DataRow("program t(output); type r = record x: integer end; var v: r; x: real; begin with v do x := 1; x := 2.5 end.")]
      [DataRow("program t(output); var i: integer; begin i := 0; while i < 3 do i := i + 1; writeln(i:3) end.")]
      [DataRow("program t(output); var c: char; begin c := 'a'; case c of 'a', 'b': c := 'c'; 'd': c := 'e' end end.")]
      [DataRow("program t(input, output); var i: integer; begin read(i); readln; write(i); writeln end.")]
      [DataRow("program t(output); type p = ^n; n = record v: integer; next: p end; var h: p; begin new(h); h^.next := nil end.")]
      [DataRow("program t(output); var a: array[1..3] of array[1..2] of integer; begin a[1][2] := a[1, 2] + 1 end.")]
      [DataRow("program t(output); label 1; var i: integer; begin i := 0; 1: i := i + 1; if i < 3 then goto 1 end.")]
      [DataRow("program t(output); var i: integer; begin i := 0; for i := 1 to 2 do writeln(i); i := 3 end.")]
      [DataRow("program t(output); begin writeln('it''s', '''') end.")]
      [DataRow("program t(output); type c = (red, green); var s: set of c; x: c; begin s := [red]; x := green; if x in s then s := s - [x] end.")]
      [DataRow("program t(output); var r: real; begin r := 1; r := r * 2 / 3 - 4 + abs(-1) end.")]
      public void Program_PassesTypeChecking(string source)
      {
         string output = RunTypeCheck(source);
         Assert.IsTrue(output.Contains(CommandProcessor.typecheckSucceedMessage), $"expected the type check to succeed, but the output was:\n{output}");
         Assert.IsFalse(output.Contains("[Typecheck]"), $"expected no Typecheck message, but the output was:\n{output}");
      }

      /// <summary>The output of "-no-ansi -t" on the given source, written to a temporary file for the run.</summary>
      private static string RunTypeCheck(string source)
      {
         using DisposableFile df = new DisposableFile();
         File.WriteAllText(df.FilePath, source);
         using MemoryStream ms = new MemoryStream();
         using (TextWriter tw = new StreamWriter(ms, leaveOpen: true))
         {
            CommandProcessor.Process(new string[] { "-no-ansi", "-t", df.FilePath }, tw);
            tw.Flush();
         }
         return Encoding.UTF8.GetString(ms.ToArray());
      }
   }
}
