//Copyright TheUbMunster, 2024, all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLRParser
{
   /// <summary>
   /// This is probably going to have O(69^n) :(
   /// </summary>
   /// <remarks>
   /// My plan for this implementation (to reduce impl headache), is to really lean into the
   /// ambiguity/branching behavior inherent to the design/considerations for most GLR parser
   /// implementations. Namely, I'm going to "see what happens" if I don't include a goto table,
   /// and instead every iteration will try all valid reductions or a single shift. This will
   /// result in insanely huge branching (probably), but I hope to optimize this better later.
   /// I expect a lot of these branches to die off at the end upon failing to fully parse everything,
   /// but there will probably be several different parses so long as the source isn't trivial
   /// (e.g., probably would happen with expressions). I hope to address this issue later.
   /// 
   /// It's possible that some of these "optimizations" to reduce the search space might
   /// observe scenarios where the most branching happens, then I might implement rules to prevent
   /// certain branches from being generating (e.g., if we're "inside a function" don't attempt
   /// to parse things that couldn't validly be "inside a function").
   /// 
   /// I don't know if GLALR parsers "are a thing", but I think this will pull similarities from
   /// GLR (nondeterminism) and LALR (lookahead to prune search space).
   /// 
   /// It seems that "a proper" LR parser (which is all I really want) is so incredibly hard to write
   /// an FSM/transition table for, which is why I appreciate GLR's inherent ability to handle 
   /// nondeterminism (or in my case, laziness to specify all the transition rules).
   /// </remarks>
   public class GLRParser
   {
      public record class State(Stack<ASTNode> parseStack, Queue<ASTNode> inputStream);
      public record class ASTNode(string name, string content, List<ASTNode> children);
      //note: whitespace-esq tokens should be before the astnode (i.e., the astnode consists of the
      //whitespace preceeding it, and then the children astnodes/terminals)

      CNFifier CNFGrammar { get; }

      /// <summary>
      /// Constructs a """GLALR""" parser obeying the grammar structure specified. To view the syntax
      /// for this type of grammar, and some example definitions, see Grammar.cs in this project.
      /// </summary>
      public GLRParser(string[] grammar, string startingNonterminal) 
      {
         CNFGrammar = new CNFifier(grammar, startingNonterminal);

         //general algorithm:
         //while (true)
         /*
          * nondeterministically choose either
          * 1. pop a symbol out of the inputStream and put it into the parseStack
          * 2. nondeterministically reduce the parseStack according to any valid parse rule
          */
         //after explicitizing this, I realize how with any non-trivially sized file, this algorithm
         //is definitely O(c^n) :(. Not feasable.
      }

      public List<ASTNode> Parse(List<ASTNode> input)
      {
         throw new NotImplementedException();
      }
   }
}
