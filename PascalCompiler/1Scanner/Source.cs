//Copyright TheUbMunster, 2024, all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PascalCompiler.Lexer;
using PascalCompiler.Parser;
using System.IO;

namespace PascalCompiler.Scanner
{
   public class Source
   {
      #region Typedefs
      public enum CompilerPhase
      {
         SourceScan,
         Lexer,
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

      #region Consts
      private static readonly Regex linebreak = new Regex("(\r\n?|\n)", RegexOptions.Compiled);
      //only matches if there's a linebreak @ startIndex specified by linebreakNow.Match(str, startIndex)
      private static readonly Regex linebreakNow = new Regex("\\G(\r\n?|\n)", RegexOptions.Compiled);
      #endregion

      #region Fields
      public IReadOnlyList<Message> Messages { get => messages; }
      private List<Message> messages = new List<Message>();
      public bool ErrorHasOccurred { get; private set; } = false;
      public string FileContents { get; init; }
      public int LineCount { get => lineBeginnings.Count; }
      private List<int> lineBeginnings;
      public string Filename { get; init; }
      /*LEXER*/
      public IReadOnlyList<Token> LexerTokens { get => lexerTokens; }
      private List<Token> lexerTokens = new List<Token>();
      public int LexedUpToFileLocation { get; private set; }
      public bool HasBeenFullyLexed { get; private set; } = false;
      public IEnumerable<Token> LexerTokensFlattenedView 
      {
         get 
         {
            foreach (Token tok in lexerTokens)
            {
               foreach (Token absorbableToken in tok.AbsorbedTokens)
                  yield return absorbableToken;
               yield return tok;
            }
         }
      }
      /*PARSER*/
      public IReadOnlyList<ASTNode> ParserNodes { get => parserNodes; }
      private List<ASTNode> parserNodes = new List<ASTNode>();
      public bool HasBeenFullyParsed { get; private set; } = false;

      #endregion

      #region Ctor
      //public Source(Uri uri) : this(File.ReadAllText(uri.AbsolutePath)) { }
      public Source(string filePath)
      { //the caller *should* pass a fully qualified path
         Filename = Path.GetFileName(filePath);
         //check file encoding
         //just print a warning if not UTF8
         Encoding? e = FileEncoding.GetFileEncoding(filePath);
         if (e == Encoding.UTF8)
         {
            //ideal
            AppendMessage(new(CompilerPhase.SourceScan, Severity.Pedantic, $"File {Filename} is encoded as [{Encoding.UTF8.EncodingName}], good!", null, false));
         }
         else if (e == null)
         {
            AppendMessage(new(CompilerPhase.SourceScan, Severity.Warning, $"File {Filename} is encoded as [{Encoding.ASCII.EncodingName}] or some unknown encoding. Compilation will be attempted, but may not succeed. UTF8 is the only officially supported encoding scheme.", null, false));
         }
         else
         {
            AppendMessage(new(CompilerPhase.SourceScan, Severity.Warning, $"File {Filename} is encoded as [{e.EncodingName}]. Compilation will be attempted, but may not succeed. UTF8 is the only officially supported encoding scheme.", null, false));
         }
         //scan for any/all illegal characters.
         string fileContent = File.ReadAllText(filePath);
         Parallel.For(0, fileContent.Length, (i) =>
         {
            char c = fileContent[i];
            if (!LegalCharacterSet.IsLegalProgramCharacter(c))
            {
               AppendMessage(new(CompilerPhase.SourceScan, Severity.Error, $"Could not lex due to illegal character at position {i}: [UTF8]:\"{Regex.Escape(c.ToString())}\"", i, true));
            }
         });
         //TODO: apparently cr, lf, and crlf are all a thing. fix this, and anything else that does this sort of thing.
         MatchCollection linebreaks = linebreak.Matches(fileContent);
         IEnumerable<string> distinctLineBreaks = linebreaks.Select(x => x.Value).Distinct();
         if (distinctLineBreaks.Count() != 1)
         {
            AppendMessage(new Message(CompilerPhase.SourceScan, Severity.Suggestion, $"You have mixed line endings: {string.Join(", ", distinctLineBreaks.Select(x => x.Replace("\r", "CR").Replace("\n", "LF")))}", null, false));
         }
         if (distinctLineBreaks.Any(x => x == "\r"))
         {
            AppendMessage(new Message(CompilerPhase.SourceScan, Severity.Warning, "You use old-mac style line endings (CR), using this source code file with other computers could break compatibility.", null, false));
         }
         lineBeginnings = linebreaks.Select(x => x.Index + x.Length).Prepend(0).ToList();
         FileContents = fileContent;
      }
      #endregion

      #region Lexer
      public void AddLexerToken(Token token)
      {
         if (HasBeenFullyLexed)
         {
            AppendMessage(new(CompilerPhase.Runtime, Severity.Error, $"The compiler attempted to lex the same file ({Filename}) more than once", null, true));
            throw new Exception("An error occurred in the compiler, see the message log.");
         }
         else
         {
            lexerTokens.Add(token);
            int tokenLocPlusLen = token.FileLocation + token.TokenLength;
            if (tokenLocPlusLen >= LexedUpToFileLocation) //gte bc some tokens (e.g., EOF) are zero-width.
               LexedUpToFileLocation = tokenLocPlusLen;
            else //this means a token was added, but the token belonged *earlier* in the source?
            {
               //technechally we can sort by location to fix this issue, but *WTH*?
               AppendMessage(new(CompilerPhase.Runtime, Severity.Error, $"Tokens lexed from ({Filename}) were added to the token collection out of file-order", null, true));
               throw new Exception("An error occurred in the compiler, see the message log.");
            }
         }
      }

      public void LexingComplete()
      {
         if (HasBeenFullyLexed)
         {
            AppendMessage(new(CompilerPhase.Runtime, Severity.Error, $"The compiler attempted to lex the same file ({Filename}) more than once", null, true));
            throw new Exception("An error occurred in the compiler, see the message log.");
         }
         else
            HasBeenFullyLexed = true;
      }
      #endregion

      #region Parser
      public void AddParserNode(ASTNode node)
      {
         if (HasBeenFullyParsed)
         {
            AppendMessage(new(CompilerPhase.Runtime, Severity.Error, $"The compiler attempted to parse the same file ({Filename}) more than once", null, true));
            throw new Exception("An error occurred in the compiler, see the message log.");
         }
         else
         {
            parserNodes.Add(node);
         }
      }

      public void ParsingComplete()
      {
         if (HasBeenFullyParsed)
         {
            AppendMessage(new(CompilerPhase.Runtime, Severity.Error, $"The compiler attempted to parse the same file ({Filename}) more than once", null, true));
            throw new Exception("An error occurred in the compiler, see the message log.");
         }
         else
            HasBeenFullyParsed = true;
      }
      #endregion

      #region Error Handling
      public void ClearMessages() => messages.Clear(); // do not clear "ErrorHasOccurred"

      public void AppendMessage(Message mesg)
      {
         ErrorHasOccurred |= mesg.isError;
         messages.Add(mesg);
      }

      public (int line, int col) GetLineColFromFileLocation(int fileLocation)
      {
         int temp = fileLocation;
         Match m = null!;
         while (temp >= 0 && !(m = linebreakNow.Match(FileContents, temp)).Success)
         {
            temp--; //walk back to the nearest linebreak to `fileLocation`
         }
         if (temp >= 0)
            temp += m.Length; //move forward the length of the match, so the index
         else
            temp = 0; //there is no prior linebreak because we're at the beginning of the file.
         //zero based line index
         int line = lineBeginnings.IndexOf(temp);
         int col = fileLocation - temp; //carat - filelocation of this line
         return (line, col);
      }

      /// <summary>
      /// Returns the fileLocation corresponding to a 0-based line index.
      /// </summary>
      /// <param name="lineIndex">The 0-based line index to get the fileLocation of</param>
      /// <returns>The fileLocation corresponding to <paramref name="lineIndex"/>, or -1 if beyond <c>EOF</c></returns>
      public int GetFileLocFromLineIndex(int lineIndex)
      {
         if (lineIndex < lineBeginnings.Count)
         {
            return lineBeginnings[lineIndex];
         }
         else
         {
            //AppendMessage(new(CompilerPhase.Runtime, Severity.Error, "During the compiler's attempt to report a line(s) of code, the compiler attempted to refer to a non-existent line of code", null, true));
            //throw new Exception("An error occurred in the compiler, see the message log.");
            return -1;
         }
      }
      #endregion
   }
}
