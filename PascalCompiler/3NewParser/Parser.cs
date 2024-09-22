//Copyright TheUbMunster, 2024, all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLRParser;
using PascalCompiler.Lexer;
using PascalCompiler.Scanner;

namespace PascalCompiler.NewParser
{
   public static class Parser
   {
      public record class ASTNode(string name);
      public record class TerminalASTNode(string name, string termContent) : ASTNode(name);
      public record class NonterminalASTNode(string name, List<ASTNode> children) : ASTNode(name);
      public static void Parse(Source source)
      {
         CNFifier CNFGrammar = new CNFifier(PascalGrammar.grammarSource, PascalGrammar.startingNonterminal);

         //Idea:
         //Since the original grammar is the "useful form" of the grammar, and the grammar that the CNF-ifier generates
         //is less useful for the programmer since it's in an arbitrary form, here's my idea to convert between the two.

         //Say a parse tree is generated via the CNF-ified form of the grammar, and we want to re-arrange/manipulate this
         //tree to be in terms of the "original nodes".

         /*
          * Say capital letters represent the original nonterminals, and lowercase letters represent the nonterminals
          * introduced by the CNF-ifier, and things in 'quotes' are terminals.
          * 
          * from:
          * A -> '=' 'zaza' '()' ':D'
          * 
          * to (CNF-ified):
          * A -> b c
          * b -> d e
          * c -> f g
          * d -> '='
          * e -> 'zaza'
          * f -> '()'
          * g -> ':D'
          * 
          * we simply take the CNF-ified form and inline the new terminals at their call site recursively
          * 
          * first:
          * A -> b c
          * b -> '=' 'zaza'
          * c -> '()' ':D'
          * 
          * then:
          * A -> '=' 'zaza' '()' ':D'
          * 
          * If there are multiple capital-letter-nonterminals, we only inline up to the nearest capital-letter-nonterminal.
          * (i.e., don't inline the capital-letter-nonterminals).
          * 
          * Before trying this on a parse tree, first I would like to try it on the CNF grammar structure to produce an equivalent grammar
          * (it won't be CNF), where all recipies comprise an O.G. nonterminal. This way, hopefully the contents of a parse tree can be
          * "flatly applied" to this form of the grammar, which should be compatible with the original nonterminal recipies.
          * 
          * foreach recipie:
          *    foreach nonterminal in RHS
          *       if aforementioned nonterminal is OG, then inline its nonterminal ingredients here
          *       //(if there are multiple recipies for a non-OG nonterminal, then do so via an "or list" i.e. a | b | c | d)
          *       //e.g., given the recipie
          *       //A -> b c
          *       //b -> w x
          *       //b -> y z
          *       //c -> 'jo mama'
          *       
          *       //the OG "A" would turn into
          *       //A -> ((w x) | (y z)) 'jo mama'
          *       //hopefully this "A" can be flatly applied to the OG nonterminal "A" (pre-CNF-ified).
          */

         //https://en.wikipedia.org/wiki/CYK_algorithm
         //https://www.geeksforgeeks.org/cyk-algorithm-for-context-free-grammar/

         //CYK as I understand it:

         //"n" denotes the length of the input token stream
         //"G" denotes the ruleset (list of recipies)

         //1. if the input is the empty string, and the empty string trivially satisfies the grammar, accept, otherwise reject.

         //2. (for terminal productions in the CNF-ified grammar G)
         //table[n, n]
         //for i = 1 to n:
         //  foreach var A in G:
         //    if (A -> b is a rule that exists in G where "b" = w_i for some i) //what does this last part mean? not sure
         //      table[i, i] = A

         //3. (for nonterminal productions in the CNF-ified grammar G)
         //for l = 2 to n:
         //  for i = 1 to n-l+1:
         //    j=i+l-1
         //    for k = i to j-1:
         //      foreach var A -> BC in G:
         //        if table[i, k] contains B and table[k + 1, j] contains C
         //          table[i, j] = A

         //4.
         //if S is in table[1, n] //S is the start state nonterminal
         //  accept the input
         //else
         //  reject the input

         //to get the parse tree, you need to backtrack???
         //the wikipedia page contains this info

         //wikipedia version (seems to be one-based indexing):
         /*
          let the input be a string I consisting of n characters: a1 ... an.
          let the grammar contain r nonterminal symbols R1 ... Rr, with start symbol R1.
          let P[n,n,r] be an array of booleans. Initialize all elements of P to false.
          let back[n,n,r] be an array of lists of backpointing triples. Initialize all elements of back to the empty list.
          
          for each s = 1 to n
              for each unit production Rv → as
                  set P[1,s,v] = true
          
          for each l = 2 to n -- Length of span
              for each s = 1 to n-l+1 -- Start of span
                  for each p = 1 to l-1 -- Partition of span
                      for each production Ra    → Rb Rc
                          if P[p,s,b] and P[l-p,s+p,c] then
                              set P[l,s,a] = true, 
                              append <p,b,c> to back[l,s,a]
          
          if P[n,1,1] is true then
              I is member of language
              return back -- by retracing the steps through back, one can easily construct all possible parse trees of the string.
          else
              return "not a member of language"
          */

         //it seems for this implementation to be easier, I should expose the internal types of CNFifier
         IReadOnlyList<Token> I = new List<Token>(source.LexerTokens);
         int n = I.Count;
         int r = CNFGrammar.CNFGrammar.Count;
         bool[,,] P = new bool[n, n, r];
         List<(int x, int y, int z)>[,,] back = new List<(int x, int y, int z)>[n, n, r];
         //initialize "back"
         for (int n1 = 0; n1 < n; n1++)
            for (int n2 = 0; n2 < n; n2++)
               for (int r1 = 0; r1 < r; r1++)
                  back[n1, n2, r1] = new List<(int x, int y, int z)>();

         //NOTE: any of these loop bounds may have off-by-one errors.

         for (int s = 0; s < n; s++)
         {
            for (int v = 0; v < r; v++)
            {
               TokenType a_s = I[s].Type;
               CNFifier.Production prod = CNFGrammar.CNFGrammar[v];

               if (prod.rhs.ingredients.Count == 1)
               {
                  //this should be a "nonterminal -> terminal" type recipie
                  CNFifier.Terminal term = ((CNFifier.ItemIngredient)prod.rhs.ingredients[0]).t!;
                  //if typeof(term) == typeof(a_s)
                     //P[0, s, v] = true;
               }
            }
         }

         for (int l = 1; l < n; l++) /*length of span*/
         {
            for (int s = 0; s < n - l; s++) /*start of span*/
            {
               for (int p = 0; p < l; p++) /*partition of span*/
               {
                  for (int a = 0; a < r; a++) //foreach R_a -> R_b R_c
                  {
                     for (int b = 0; b < r; b++) //foreach R_a -> R_b R_c
                     {
                        for (int c = 0; c < r; c++) //foreach R_a -> R_b R_c
                        {
                           if (P[p, s, b] && P[l - p, s + p, c])
                           {
                              P[l, s, a] = true;
                              back[l, s, a].Add((p, b, c));
                           }
                        }
                     }
                  }
               }
            }
         }

         if (P[n, 0, 0])
         {
            //parse successful
            //using "back", one can obtain any valid parse via backtracking.
         }
         else
         {
            //parse failed
         }
      }
   }
}
