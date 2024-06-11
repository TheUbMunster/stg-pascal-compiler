using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalCompiler.Lexer;

namespace PascalCompiler.Parser
{
   public class TokenNode
   {
      public IReadOnlyList<Token> OnlySkippableTokens { get; }
      /// <summary>
      /// All of the tokens that this TokenNode represents.
      /// The last token in this list represents the "actual" token (for the parser's sake),
      /// whereas all the ones that come before it are skippable (e.g., comments, whitespace & linebreaks)
      /// 
      /// The skippable tokens are retained so that the source can be accurately reconstructed for the sake
      /// of compiler reports and debugging (and so line numbers etc are accurate).
      /// </summary>
      public IEnumerable<Token> AllTokens { get => OnlySkippableTokens.Append(Token); }
      public Token Token { get; }
      public int Count { get => OnlySkippableTokens.Count + 1; }
      public TokenType Type { get => Token.Type; }
      public int NodeLength { get => AllTokens.Select(x => x.TokenLength).Sum(); }
      public int FileLocation { get => AllTokens.First().FileLocation; } //the first one bc users need the very beginning, including skippables.
      public bool ContainsSkippableToken { get => Count > 1; }

      public TokenNode(IReadOnlyList<Token> skippableTokens, Token actualToken)
      {
         OnlySkippableTokens = skippableTokens;
         Token = actualToken;
      }
      public override string ToString()
      {
         return Token.ToString();
      }
   }
}
