using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLRParser
{
   internal class CNFifier
   {
      #region Typedefs
      private record class Production(Nonterminal lhs, GroupIngredient? rhs) // if rhs == null, then it's an A -> epsilon (empty) recipie.
      {
         public Production Copy()
         {
            Production res = new(lhs.Copy(), (GroupIngredient?)rhs?.Copy()); //type system makes me sad sometimes
            return res;
         }
      }
      private abstract record class Ingredient() //conceptually similar to a mathematical "term"
      {
         public abstract Ingredient Copy();
      }
      private record class ItemIngredient(Nonterminal? nt, Terminal? t) : Ingredient
      {
         public override Ingredient Copy()
         {
            ItemIngredient ii = new(nt?.Copy(), t?.Copy());
            return ii;
         }
      }
      private enum VariableIngredientType
      {
         UNDEFINED,
         Optional = 1,
         ZeroOrOne = 1,
         ZeroOrMore
      }
      private record class VariableIngredient(Ingredient item, VariableIngredientType groupType) : Ingredient
      {
         public override Ingredient Copy()
         {
            VariableIngredient vi = new(item.Copy(), groupType);
            return vi;
         }
      }
      private record class GroupIngredient(List<Ingredient> ingredients) : Ingredient
      {
         public override Ingredient Copy()
         {
            GroupIngredient gi = new(ingredients.Select(i => i.Copy()).ToList());
            return gi;
         }
      }
      private record class OrIngredient(Ingredient lhs, Ingredient rhs) : Ingredient
      {
         public override Ingredient Copy()
         {
            OrIngredient oi = new(lhs.Copy(), rhs.Copy());
            return oi;
         }
      }
      private record class Nonterminal(string name)
      {
         public Nonterminal Copy() => new(name);
      }
      private record class Terminal(string name)
      {
         public Terminal Copy() => new(name);
      }
      #endregion

      public string[] GrammarSource { get; }
      public string[] CNFGrammarSource { get; }
      /// <summary>
      /// Takes an input grammar adhering to the following rules: <br/><br/>
      /// 
      /// <b>
      /// All terminals and non-terminals consist of [A-Za-z] (upon converting the grammar,
      /// the output grammar may contain underscores (_) as well).
      /// </b>
      /// <br/><br/>
      /// and returns an equivalent grammar in Chomsky Normal Form (CNF).
      /// </summary>
      /// <remarks>
      /// For use in a compiler, and designed to work with a lightly modified version of the 
      /// pascal grammar in mind. It's possible this has bugs, but it works for my purposes.
      /// </remarks>
      public CNFifier(string[] grammarSource) 
      {
         GrammarSource = grammarSource;
         //things of relevance:
         //https://en.wikipedia.org/wiki/Chomsky_normal_form
         //https://cs.stackexchange.com/questions/18052/rules-regarding-chomsky-normal-form-cnf-grammars

         /*   FOR IMPLEMENTATION REFERENCE:
          * 
          *   = Shall be defined to be
          *   > Shall have as an alternative definition
          *   | Alternatively
          *   . End of definition
          *   [ x ] 0 or 1 instance of x
          *   { x } 0 or more instances of x
          *   ( x | y ) Grouping: either of x or y
          *   'xyz' The terminal symbol xyz
          *   meta-identifier A nonterminal symbol
          */

         //call in either of these orders:
         //?VARIABLE?,...,...,START,TERM,BIN,DEL,UNIT
         //?VARIABLE?,...,...,START,BIN,DEL,UNIT,TERM
         //a possible optimization would be to try multiple (valid) combinations, and pick the smallest
         //for now, I don't care.
      }

      #region Utility
      private void TransformGrammarSTART(List<Production> intermediateGrammar)
      {

      }

      private void TransformGrammarTERM(List<Production> intermediateGrammar)
      {

      }

      private void TransformGrammarBIN(List<Production> intermediateGrammar)
      {

      }

      private void TransformGrammarDEL(List<Production> intermediateGrammar)
      {

      }

      private void TransformGrammarUNIT(List<Production> intermediateGrammar)
      {

      }

      private void TransformGrammarVARIABLE(List<Production> intermediateGrammar)
      {
         //I'm not sure if this is foolproof, probably best to apply this FIRST

         //Example of "0 or more to CNF-able"
         /* Note: { } indicates 0 or more of what's contained within
          * A -> { B }
          * 
          * to:
          * 
          * A -> ZZ | eZ | Ze | e
          * Z -> ZZ | eZ | Ze | e
          * Z -> e (delete this one since it's been implemented at all call sites)
          *        (it's kept here for posterity)
          * Z -> B
          *
          * Note that since A meets the requirements for "nullable", all call sites for A need
          * to have a recipie with A omitted. e.g.:
          * 
          * if some recipie Q -> AR exists, then we need to create an extra recipie (alongside the original):
          * Q -> R (with A omitted).
          */
      }

      private void TransformGrammarEITHER(List<Production> intermediateGrammar)
      {
         //given how casually defined A -> B | C <=> A -> B, A -> C seems to be, I think
         //this transformation is guaranteed to not conflict with anything. To be safe,
         //probably only apply this transformation to a single uppermost OR, and then repeat
         //until there are no ORs left in the expression.
         //e.g.:
         /*
          * A -> ( B | C )
          * 
          * to:
          * 
          * A -> B
          * A -> C
          * 
          * ... this process may need to be applied multiple times
          * 
          * A -> asdf ( ( B | C ) | ( D | E ) ) jkl
          * 
          * to:
          * 
          * A -> asdf ( ( B | C ) ) jkl
          * A -> asdf ( ( D | E ) ) jkl
          * 
          * to:
          * 
          * A -> asdf B jkl
          * A -> asdf C jkl
          * A -> asdf D jkl
          * A -> asdf E jkl
          * 
          * Note that ((A | B) | C) = (A | (B | C)) = (A | B | C)
          */
      }

      private void TransformGrammarOPTIONAL(List<Production> intermediateGrammar)
      {
         //PROBLEM!!!
         //in this situation:
         /*
          * A -> B { C [ D ] } E
          * 
          * we *CANNOT* call this and traverse inside like this:
          * A -> B { C D } E
          * A -> B { C } E
          * 
          * because the original rule it was permissible to have some of the {}'s include D 
          * and others not, in the same production.
          * 
          * e.g., the above simplification misses out on:
          * A -> B C D C D C C C D C D E
          * 
          * tldr, this function cannot "pierce a {}", and it's probably also true that the VARIABLE one
          * can't "pierce a []".
          * 
          * TransformGrammarOPTIONAL and TransformGrammarVARIABLE probably need to be mutually recursive.
          * probably also include TransformGrammarEITHER as well (i.e., none of these can pierce each other.)
          */


         //given how casually defined A -> [ B ] <=> A -> B, A -> e seems to be, I think
         //this transformation is guaranteed to not conflict with anything. To be safe,
         //probably only apply this transformation to a single uppermost [], and then repeat
         //until there are no []s left in the expression.
         //e.g.:
         /*
          * A -> [ B ]
          * 
          * to:
          * 
          * A -> B
          * A -> e   (empty string to be removed by TransformGrammarDEL)
          * 
          * ... this process may need to be applied multiple times
          * 
          * A -> [ B [ C ] ]
          * 
          * to:
          * 
          * A -> B [ C ]
          * A -> [ C ]
          * 
          * to:
          * 
          * A -> BC
          * A -> B
          * A -> C
          * A -> e   (empty string to be removed by TransformGrammarDEL) 
          */

         bool FixOneOptional(Production prod, out Production? with, out Production? without)
         {
            //breadth first search through the whole thing, resolve a single highest level thing,
            //then repeat the process on the two children, on and on. If you call this function
            //and there were no optionals to resolve, then return false, otherwise, true.
            with = without = null;
            if (prod.rhs != null) //this just checks one level. If the entire level fails, we need to recur on GroupIngredients at this level.
               foreach (Ingredient i in prod.rhs.ingredients)
               {
                  switch(i)
                  {
                     case VariableIngredient vi:
                        if (vi.groupType == VariableIngredientType.ZeroOrOne) //bingo
                        {
                           //we need to copy the entirety of "prod", re-navigate to this same spot, delete this variable ingredient,
                           //and replace this spot with "with" and "without"
                           return true;
                        }
                        break;
                     case GroupIngredient gi:
                        //add to a collection, then once this foreach is done, recur on all the gi's at this level.
                        break;
                     case ItemIngredient ii:
                        break;
                     case OrIngredient oi:
                        break;
                     default:
                        throw new ArgumentException("Tried to switch on ingredient type, but ingredient type was missing");
                  }
               }
            else
               return false; //empty production.
         }
         int count = intermediateGrammar.Count; //the list will re-grow
         for (int i = count; i >= 0; i--)
         {
            Production c = intermediateGrammar[i];
            intermediateGrammar.RemoveAt(i);
            List<Production> currents = new List<Production>();
            currents.Add(c);
            //as we repeatedly call FixOneOptional on this, if it returns false, then add to
            //intermediateGrammar instead of currents.
            while (currents.Count > 0)
            {
               Production cc = currents[0];
               currents.RemoveAt(0);
               if (FixOneOptional(cc, out Production? with, out Production? without))
               {
                  currents.Add(with!);
                  currents.Add(without!);
               }
               else
                  intermediateGrammar.Add(cc);
            }
         }
      }

      private ulong uniqueIngredientCounter = 0;
      private string CreateUniqueIngredientName() => $"_{uniqueIngredientCounter++}";
      #endregion
   }
}
