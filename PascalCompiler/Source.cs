using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PascalCompiler.Lexer;

namespace PascalCompiler
{
   internal class Source
   {
      #region Typedefs
      public enum CompilerPhase
      {
         SourceScan,
         Lex,
         Parse,
         Typecheck,
         EmitAsm,
         Assemble,
         Link,
         Interpret,
         Runtime //if the compiler has a compiler runtime exception (i.e., the compiler has a bug)
      }
      public enum Severity
      {
         Pedantic,
         Information,
         Suggestion,
         Warning,
         Error
      }
      public record struct Message(CompilerPhase phase, Severity severity, string message, int? fileLocation, bool isError = false);
      #endregion

      #region Fields
      public IReadOnlyList<Message> Messages { get => messages; }
      private List<Message> messages = new List<Message>();
      private bool errorHasOccurred = false;
      public string FileContents { get; init; }
      public int LineCount { get => lineBeginnings.Count; }
      private List<int> lineBeginnings;
      public string Filename { get; init; }
      public IReadOnlyList<Token> LexerTokens { get => lexerTokens; }
      private List<Token> lexerTokens = new List<Token>();

      #endregion

      #region Ctor
      //public Source(Uri uri) : this(File.ReadAllText(uri.AbsolutePath)) { }
      public Source(string filePath)
      {
         Filename = Path.GetFileName(filePath);
         string fileContent = File.ReadAllText(filePath);
         Parallel.For(0, fileContent.Length, (i) =>
         {
            char c = fileContent[i];
            if (!LegalCharacterSet.IsLegalProgramCharacter(c))
            {
               AppendMessage(new(CompilerPhase.SourceScan, Severity.Error, $"Could not lex due to illegal character at position {i}: [UTF8]:\"{Regex.Escape(c.ToString())}\"", i, true));
            }
         });
         lineBeginnings = Regex.Matches(fileContent, "(\\r\\n)|(\\n)").Select(x => x.Index + x.Value.Length).Prepend(0).ToList();
         FileContents = fileContent;
      }
      #endregion

      #region Lexer
      public void AppendLexerToken(Token token)
      {
         lexerTokens.Add(token);
      }
      #endregion

      #region Error Handling
      public void AppendMessage(Message mesg)
      {
         errorHasOccurred |= mesg.isError;
         messages.Add(mesg);
      }

      public (int line, int col) GetLineColFromFileLocation(int fileLocation)
      {
         //regardless of whether it's crlf or just lf, if we see the prev index is a \n, then the current index must be a start of a line.
         int temp = fileLocation;
         while (temp >= 0 || FileContents[temp] != '\n')
         {
            temp--;
         }
         temp += 1; //we don't care about \n, we need to move forward one char.
         int line = lineBeginnings.IndexOf(temp);
         int col = fileLocation - temp;
         return (line, col);
      }

      public string GetSourceLine(int lineIndex)
      {
         if (lineIndex < lineBeginnings.Count)
         {
            int lineStart = lineBeginnings[lineIndex];
            string line;
            if (lineIndex < lineBeginnings.Count - 1)
            {
               int nextLineStart = lineBeginnings[lineIndex + 1];
               line = FileContents.Substring(lineStart, nextLineStart);
            }
            else
               line = FileContents.Substring(lineStart); //this is the last line
            return line;
         }
         else
         {
            AppendMessage(new(CompilerPhase.Runtime, Severity.Error, "During the compiler's attempt to report a line(s) of code, the compiler attempted to refer to a non-existent line of code", null, true));
            throw new Exception("An error occurred in the compiler, see the message log.");
         }
      }

      public bool ErrorHasOccurred() => errorHasOccurred;
      #endregion
   }
}
