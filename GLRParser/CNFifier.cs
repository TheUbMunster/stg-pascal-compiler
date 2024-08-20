//Copyright TheUbMunster, 2024, all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
         public override string ToString() => Regex.Replace($"{lhs} = {rhs?.ToString() ?? ""} .", " +", " ").Trim();
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
         public override string ToString() => (nt?.ToString() ?? t?.ToString())!;
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
         public override string ToString()
         {
            switch (groupType)
            {
               case VariableIngredientType.UNDEFINED:
                  throw new Exception();
               case VariableIngredientType.ZeroOrOne:
                  return $" [ {item} ] ";
               case VariableIngredientType.ZeroOrMore:
                  return $" {{ {item} }} ";
               default:
                  throw new Exception();
            }
         }
      }
      private record class GroupIngredient(List<Ingredient> ingredients) : Ingredient
      {
         public override Ingredient Copy()
         {
            GroupIngredient gi = new(ingredients.Select(i => i.Copy()).ToList());
            return gi;
         }
         public override string ToString() => " ( " + string.Join(' ', ingredients) + " ) ";
      }
      private record class OrIngredient(Ingredient lhs, Ingredient rhs) : Ingredient
      {
         public override Ingredient Copy()
         {
            OrIngredient oi = new(lhs.Copy(), rhs.Copy());
            return oi;
         }
         public override string ToString() => $"( {lhs} | {rhs} )";
      }
      private record class Nonterminal(string Name)
      {
         public Nonterminal Copy() => new(Name);
         public override string ToString() => $" {Name} ";
      }
      private record class Terminal(string Name)
      {
         public Terminal Copy() => new(Name);
         public override string ToString() => $" '{Name}' ";
      }
      #endregion

      public string[] GrammarSource { get; }
      public string[] CNFGrammarSource { get; }
      /// <summary>
      /// Takes an input grammar adhering to the following rules: <br/><br/>
      /// 
      /// <b>
      /// All non-terminals consist of [A-Za-z-] (upon converting the grammar,
      /// some of the non-terminals in the output grammar may contain underscores
      /// (_) as well). All terminals are encapsulated in single quotes "'".
      /// 
      /// This CNFifier is designed to handle grammar adhering to the same rules
      /// as the pascal specification.
      /// </b>
      /// <br/><br/>
      /// and returns an equivalent grammar in Chomsky Normal Form (CNF).
      /// </summary>
      /// <remarks>
      /// For use in a compiler, and designed to work with a lightly modified version of the 
      /// pascal grammar in mind. It's possible this has bugs, but it works for my purposes.
      /// </remarks>
      public CNFifier(string[] grammarSource, string startingNonterminal) 
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
         List<Production> testGrammar = new List<Production>()
         {
            //A = 'qwe' [ ('rty' | 'fgh') ] 'uio' .
            //new Production(
            //   new Nonterminal("A"),
            //   new GroupIngredient(new List<Ingredient>() 
            //   {
            //      new ItemIngredient(null, new Terminal("qwe")),
            //      new VariableIngredient(
            //         new OrIngredient(
            //            new ItemIngredient(null, new Terminal("rty")),
            //            new ItemIngredient(null, new Terminal("fgh"))
            //         ), VariableIngredientType.ZeroOrOne),
            //      new ItemIngredient(null, new Terminal("uio")),
            //   })),

            //B = 'asd' { ( C | 'jkl' ) } 'qwe' .
            //new Production(
            //   new Nonterminal("B"),
            //   new GroupIngredient(new List<Ingredient>()
            //   {
            //      new ItemIngredient(null, new Terminal("asd")),
            //      new VariableIngredient(
            //         new OrIngredient(
            //            new ItemIngredient(new Nonterminal("C"), null), 
            //            new ItemIngredient(null, new Terminal("jkl"))), 
            //         VariableIngredientType.ZeroOrMore),
            //      new ItemIngredient(null, new Terminal("qwe"))
            //   })),
            //C = 'rty' 'aoe' .
            //new Production(
            //   new Nonterminal("C"),
            //   new GroupIngredient( new List<Ingredient>()
            //   {
            //      new ItemIngredient(null, new Terminal("rty")),
            //      new ItemIngredient(null, new Terminal("aoe")),
            //   })),

            //A = B C D E F G H
            new Production(
               new Nonterminal("A"),
               new GroupIngredient(new List<Ingredient>()
               {
                  new ItemIngredient(new Nonterminal("B"), null),
                  new ItemIngredient(new Nonterminal("C"), null),
                  new ItemIngredient(new Nonterminal("D"), null),
                  new ItemIngredient(new Nonterminal("E"), null),
                  new ItemIngredient(new Nonterminal("F"), null),
                  new ItemIngredient(new Nonterminal("G"), null),
                  new ItemIngredient(new Nonterminal("H"), null),
               })),
         };
         List<Production> parsedGrammar = new List<Production>();

         //temp:
         parsedGrammar = testGrammar;
         startingNonterminal = "A";

         //TODO: convert grammar source into list of productions.

         //call in either of these orders:
         //?VARIABLE?,...,...,START,TERM,BIN,DEL,UNIT
         //?VARIABLE?,...,...,START,BIN,DEL,UNIT,TERM
         //a possible optimization would be to try multiple (valid) combinations, and pick the smallest
         //for now, I don't care.
         bool needPreTransform = true;
         while (needPreTransform)
         {
            //keep attempting to apply these transformations until it no longer does anything.
            bool t = false;
            t |= TransformGrammarVARIABLE(parsedGrammar);
            t |= TransformGrammarEITHER(parsedGrammar);
            t |= TransformGrammarOPTIONAL(parsedGrammar);
            needPreTransform = t;
         }
         //at this point there should be no { }, [ ], or |
         //probably need to flatten nested groups into a single top level group.
         startingNonterminal = TransformGrammarSTART(parsedGrammar, startingNonterminal);
         TransformGrammarTERM(parsedGrammar);
         TransformGrammarBIN(parsedGrammar);
         TransformGrammarDEL(parsedGrammar);
         TransformGrammarUNIT(parsedGrammar);
         bool success = true;
         foreach (Production p in parsedGrammar)
         { //check to make sure it meets the requirements of CNF
            if (p.rhs == null)
               success &= false;
            else
            {
               int itemCount = p.rhs!.ingredients.Count;
               if (itemCount == 1)
               { //should be terminal
                  ItemIngredient ii = (ItemIngredient)p.rhs!.ingredients[0];
                  if (ii.t == null || ii.nt != null)
                     success &= false;
               }
               else if (itemCount == 2)
               { //should be two nonterminal
                  ItemIngredient ii1 = (ItemIngredient)p.rhs!.ingredients[0];
                  ItemIngredient ii2 = (ItemIngredient)p.rhs!.ingredients[1];
                  if (ii1.t != null || ii1.nt == null)
                     success &= false;
                  if (ii2.t != null || ii2.nt == null)
                     success &= false;
               }
            }
         }
         if (!success)
            throw new Exception("Applied all CNFifier transformations, and yet output grammar was not CNF");
         CNFGrammarSource = parsedGrammar.Select(x => x.ToString()).ToArray();
      }

      #region Utility
      #region Wikipedia transformations
      //see https://en.wikipedia.org/wiki/Chomsky_normal_form
      private string TransformGrammarSTART(List<Production> intermediateGrammar, string oldStartSymbol)
      {
         //ensure that the starting symbol does not appear on the right hand side of a recipie (by making a new start symbol)
         string newNTName = CreateUniqueIngredientName();
         intermediateGrammar.Add(new Production(new Nonterminal(newNTName),
            new GroupIngredient(new List<Ingredient>()
            {
               new ItemIngredient(new Nonterminal(oldStartSymbol), null)
            })));
         return newNTName;
      }

      private void TransformGrammarTERM(List<Production> intermediateGrammar)
      {
         //assume everything is in a single top level nested group.
         int count = intermediateGrammar.Count;
         for (int i = count - 1; i >= 0; i--)
         {
            Production p = intermediateGrammar[i];
            if (p.rhs != null)
               foreach (Ingredient ing in p.rhs!.ingredients)
               {
                  switch (ing)
                  {
                     case ItemIngredient ii:
                        if (ii.t != null)
                        {
                           string newNTName = CreateUniqueIngredientName();
                           Production np = new Production(new Nonterminal(newNTName), new GroupIngredient(new List<Ingredient>()
                           {
                              new ItemIngredient(null, ii.t)
                           }));
                           ItemIngredient replacement = new ItemIngredient(new Nonterminal(newNTName), null);
                           int ind = p.rhs!.ingredients.IndexOf(ing);
                           p.rhs!.ingredients.Remove(ing);
                           p.rhs!.ingredients.Insert(ind, replacement);
                        }
                        break;
                     default:
                        throw new Exception("Error CNFifier expected a flattened list of terminals/nonterminals");
                  }
               }
         }
      }

      private void TransformGrammarBIN(List<Production> intermediateGrammar)
      {
         //A -> B C D E
         //A -> B X1
         //X1 -> C X2
         //X2 -> D E

         //all recipies at this point should be either 1. a single terminal, or 2. one or more nonterminals
         //we want the recipies that have >= 3 nonterminals in the recipie
         int count = intermediateGrammar.Count;
         for (int i = count - 1; i >= 0; i--)
         {
            Production prod = intermediateGrammar[i];
            if (prod.rhs != null)
            {
               ItemIngredient ee = (ItemIngredient)prod.rhs!.ingredients[0];
               int ingCount = prod.rhs!.ingredients.Count;
               if (ee.nt != null && ingCount >= 3)
               {
                  intermediateGrammar.RemoveAt(i);
                  string[] newNTNames = new string[ingCount - 2];
                  for (int j = 0; j < newNTNames.Length; j++)
                     newNTNames[j] = CreateUniqueIngredientName();
                  List<ItemIngredient> iis = prod.rhs!.ingredients.Select(x => (ItemIngredient)x).ToList();
                  Nonterminal ognt = prod.lhs;
                  for (int c = 0; c < ingCount - 1; c++)
                  {
                     Nonterminal recipNT;
                     if (c == 0)
                        recipNT = ognt;
                     else
                        recipNT = new Nonterminal(newNTNames[c - 1]);
                     ItemIngredient left = iis[c];
                     ItemIngredient right;
                     if (c == ingCount - 2)
                        right = iis[c + 1];
                     else
                        right = new ItemIngredient(new Nonterminal(newNTNames[c]), null);
                     intermediateGrammar.Add(new Production(recipNT, new GroupIngredient(new List<Ingredient>() { left, right })));
                  }
               }
            }
         }
      }

      private void TransformGrammarDEL(List<Production> intermediateGrammar)
      {

      }

      private void TransformGrammarUNIT(List<Production> intermediateGrammar)
      {
         //given
         //A -> B
         //B -> ...
         //B -> ...
         //B -> ...
         //B -> ...

         //remove A -> B and replace others with
         //A -> ...
         //A -> ...
         //A -> ...
         //A -> ...
      }
      #endregion

      #region Special transformations
      //returns false if no transformation was made
      private bool TransformGrammarVARIABLE(List<Production> intermediateGrammar)
      {
         //I'm not sure if this is foolproof, probably best to apply this FIRST

         //Example of "0 or more to CNF-able"
         /* Note: { } indicates 0 or more of what's contained within
          * A -> asd { B } jkl
          * 
          * to:
          * 
          * A -> ZZ | eZ | Ze | e
          * Z -> ZZ | eZ | Ze | e (delete Z -> eZ | Ze because it's idempotent)
          * Z -> e (delete this one since it's been implemented at all call sites)
          *        (it's kept here for posterity)
          * Z -> B
          * 
          * cleaned:
          * A -> asd Z jkl | asd e jkl
          * Z -> ZZ
          * Z -> B
          *
          * Note that since A meets the requirements for "nullable", all call sites for A need
          * to have a recipie with A omitted. e.g.:
          * 
          * if some recipie Q -> AR exists, then we need to create an extra recipie (alongside the original):
          * Q -> R (with A omitted).
          */
         #region Helpers
         bool FixOneVariable(Production prod, out Production? partA, out Production? partB, out Production? partC, out Production? partD)
         {
            //breadth first search through the whole thing, resolve a single highest level thing,
            //then repeat the process on the two children, on and on. If you call this function
            //and there were no optionals to resolve, then return false, otherwise, true.
            partA = partB = partC = partD = null;
            if (prod.rhs != null) //this just checks one level. If the entire level fails, we need to recur on GroupIngredients at this level.
            {
               string newNTName = CreateUniqueIngredientName();
               Production potentialModifiedOriginalWith = prod.Copy(),
                  potentialModifiedOriginalWithout = prod.Copy(),
                  potentialPartC = 
                  new Production(new Nonterminal(newNTName), 
                     new GroupIngredient(new List<Ingredient>() 
                     {
                        new ItemIngredient(new Nonterminal(newNTName), null),
                        new ItemIngredient(new Nonterminal(newNTName), null) 
                     })
                  ),
                  potentialPartD = new Production(new Nonterminal(newNTName), new GroupIngredient(new List<Ingredient>()));
               //extract { B }, stick it in potentialPartC, remove it from potentialModifiedOriginal
               { //WITH Z
                  Queue<(GroupIngredient parent, Ingredient inQuestion)> zeroOrOneVIs = new();
                  RecurAddBFS(ref zeroOrOneVIs, potentialModifiedOriginalWith.rhs!, BFSGoals.VIRepeat);
                  if (zeroOrOneVIs.Count == 0)
                     return false;
                  var chosenVI = zeroOrOneVIs.Peek(); //this is >= level to any other VI (BFS)

                  int ind = chosenVI.parent.ingredients.IndexOf(chosenVI.inQuestion);
                  chosenVI.parent.ingredients.Remove(chosenVI.inQuestion);
                  chosenVI.parent.ingredients.Insert(ind, new ItemIngredient(new Nonterminal(newNTName), null));
                  partA = potentialModifiedOriginalWith;
               }
               { //WITHOUT Z
                  Queue<(GroupIngredient parent, Ingredient inQuestion)> zeroOrOneVIs = new();
                  RecurAddBFS(ref zeroOrOneVIs, potentialModifiedOriginalWithout.rhs!, BFSGoals.VIRepeat);
                  var chosenVI = zeroOrOneVIs.Peek(); //this is >= level to any other VI (BFS)

                  int ind = chosenVI.parent.ingredients.IndexOf(chosenVI.inQuestion);
                  chosenVI.parent.ingredients.Remove(chosenVI.inQuestion);
                  partB = potentialModifiedOriginalWithout;
                  potentialPartD.rhs!.ingredients.Add(((VariableIngredient)chosenVI.inQuestion).item);
               }
               partC = potentialPartC;
               partD = potentialPartD;
               return true;
            }
            else
               return false; //empty production.
         }
         #endregion

         bool anythingChanged = false; //true if >= 1 transformation occurred.
         int count = intermediateGrammar.Count; //the list will re-grow as we transform, but we want to iter based on the original count.
         for (int i = count - 1; i >= 0; i--)
         {
            Production c = intermediateGrammar[i];
            intermediateGrammar.RemoveAt(i);
            List<Production> currents = [c];
            //as we repeatedly call FixOneOptional on this, if it returns false, then add to
            //intermediateGrammar instead of currents.
            while (currents.Count > 0)
            {
               Production cc = currents[0];
               currents.RemoveAt(0);
               if (FixOneVariable(cc, out Production? partA, out Production? partB, out Production? partC, out Production? partD))
               {
                  currents.Add(partA!);
                  currents.Add(partB!);
                  currents.Add(partC!);
                  currents.Add(partD!);
                  anythingChanged |= true;
               }
               else
                  intermediateGrammar.Add(cc);
            }
         }
         return anythingChanged;
      }

      //returns false if no transformation was made
      private bool TransformGrammarEITHER(List<Production> intermediateGrammar)
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
         #region Helpers
         bool FixOneEither(Production prod, out Production? withLeft, out Production? withRight)
         {
            //breadth first search through the whole thing, resolve a single highest level thing,
            //then repeat the process on the two children, on and on. If you call this function
            //and there were no optionals to resolve, then return false, otherwise, true.
            withLeft = withRight = null;
            if (prod.rhs != null) //this just checks one level. If the entire level fails, we need to recur on GroupIngredients at this level.
            {
               Production potentialWithLeft = prod.Copy(), potentialWithRight = prod.Copy();
               //create two variants of this recipie, one with chosenVI.item taken the place of chosenVI,
               //and one with nothing present.
               { //WITH left
                  Queue<(GroupIngredient parent, Ingredient inQuestion)> zeroOrOneVIs = new();
                  RecurAddBFS(ref zeroOrOneVIs, potentialWithLeft.rhs!, BFSGoals.OrIngr);
                  if (zeroOrOneVIs.Count == 0)
                     return false;
                  var chosenVI = zeroOrOneVIs.Peek(); //this is >= level to any other VI (BFS)

                  int ind = chosenVI.parent.ingredients.IndexOf(chosenVI.inQuestion);
                  chosenVI.parent.ingredients.Remove(chosenVI.inQuestion);
                  chosenVI.parent.ingredients.Insert(ind, ((OrIngredient)chosenVI.inQuestion).lhs);
                  withLeft = potentialWithLeft;
               }
               { //WITH right
                  Queue<(GroupIngredient parent, Ingredient inQuestion)> zeroOrOneVIs = new();
                  RecurAddBFS(ref zeroOrOneVIs, potentialWithRight.rhs!, BFSGoals.OrIngr);
                  if (zeroOrOneVIs.Count == 0)
                     return false;
                  var chosenVI = zeroOrOneVIs.Peek(); //this is >= level to any other VI (BFS)

                  int ind = chosenVI.parent.ingredients.IndexOf(chosenVI.inQuestion);
                  chosenVI.parent.ingredients.Remove(chosenVI.inQuestion);
                  chosenVI.parent.ingredients.Insert(ind, ((OrIngredient)chosenVI.inQuestion).rhs);
                  withRight = potentialWithRight;
               }
               return true;
            }
            else
               return false; //empty production.
         }
         #endregion

         bool anythingChanged = false; //true if >= 1 transformation occurred.
         int count = intermediateGrammar.Count; //the list will re-grow as we transform, but we want to iter based on the original count.
         for (int i = count - 1; i >= 0; i--)
         {
            Production c = intermediateGrammar[i];
            intermediateGrammar.RemoveAt(i);
            List<Production> currents = [c];
            //as we repeatedly call FixOneOptional on this, if it returns false, then add to
            //intermediateGrammar instead of currents.
            while (currents.Count > 0)
            {
               Production cc = currents[0];
               currents.RemoveAt(0);
               if (FixOneEither(cc, out Production? withLeft, out Production? withRight))
               {
                  currents.Add(withLeft!);
                  currents.Add(withRight!);
                  anythingChanged |= true;
               }
               else
                  intermediateGrammar.Add(cc);
            }
         }
         return anythingChanged;
      }

      //returns false if no transformation was made
      private bool TransformGrammarOPTIONAL(List<Production> intermediateGrammar)
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

         #region Helpers
         bool FixOneOptional(Production prod, out Production? with, out Production? without)
         {
            //breadth first search through the whole thing, resolve a single highest level thing,
            //then repeat the process on the two children, on and on. If you call this function
            //and there were no optionals to resolve, then return false, otherwise, true.
            with = without = null;
            if (prod.rhs != null) //this just checks one level. If the entire level fails, we need to recur on GroupIngredients at this level.
            {
               Production potentialWith = prod.Copy(), potentialWithout = prod.Copy();
               //create two variants of this recipie, one with chosenVI.item taken the place of chosenVI,
               //and one with nothing present.
               { //WITH
                  Queue<(GroupIngredient parent, Ingredient inQuestion)> zeroOrOneVIs = new();
                  RecurAddBFS(ref zeroOrOneVIs, potentialWith.rhs!, BFSGoals.VIOptional);
                  if (zeroOrOneVIs.Count == 0)
                     return false;
                  var chosenVI = zeroOrOneVIs.Peek(); //this is >= level to any other VI (BFS)
                  int ind = chosenVI.parent.ingredients.IndexOf(chosenVI.inQuestion);
                  chosenVI.parent.ingredients.Remove(chosenVI.inQuestion);
                  chosenVI.parent.ingredients.Insert(ind, ((VariableIngredient)chosenVI.inQuestion).item);
                  with = potentialWith;
               }
               { //WITHOUT
                  Queue<(GroupIngredient parent, Ingredient inQuestion)> zeroOrOneVIs = new();
                  RecurAddBFS(ref zeroOrOneVIs, potentialWithout.rhs!, BFSGoals.VIOptional);
                  //if we didn't return in the "with" due to zerOrOneVIs == 0, then we won't here either (omit check).
                  var chosenVI = zeroOrOneVIs.Peek(); //this is >= level to any other VI (BFS)
                  chosenVI.parent.ingredients.Remove(chosenVI.inQuestion);
                  without = potentialWithout;
               }
               return true;
            }
            else
               return false; //empty production.
         }
         #endregion

         bool anythingChanged = false; //true if >= 1 transformation occurred.
         int count = intermediateGrammar.Count; //the list will re-grow as we transform, but we want to iter based on the original count.
         for (int i = count - 1; i >= 0; i--)
         {
            Production c = intermediateGrammar[i];
            intermediateGrammar.RemoveAt(i);
            List<Production> currents = [c];
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
                  anythingChanged |= true;
               }
               else
                  intermediateGrammar.Add(cc);
            }
         }
         return anythingChanged;
      }
      #endregion

      #region Helper helpers
      enum BFSGoals
      {
         VIOptional,
         VIRepeat,
         OrIngr
      }
      void RecurAddBFS(ref Queue<(GroupIngredient parent, Ingredient inQuestion)> col, GroupIngredient pgi, BFSGoals goal)
      {
         List<GroupIngredient> gisTodo = new();
         foreach (Ingredient i in pgi.ingredients)
         {
            switch (i)
            {
               case VariableIngredient vi:
                  if (vi.groupType == VariableIngredientType.ZeroOrOne && goal == BFSGoals.VIOptional) //bingo
                  {
                     //we need to copy the entirety of "prod", re-navigate to this same spot, delete this variable ingredient,
                     //and replace this spot with "with" and "without"
                     col.Enqueue((pgi, vi));
                  }
                  if (vi.groupType == VariableIngredientType.ZeroOrMore && goal == BFSGoals.VIRepeat)
                  {
                     col.Enqueue((pgi, vi));
                  }
                  break;
               case GroupIngredient gi:
                  //add to a collection, then once this foreach is done, recur on all the gi's at this level.
                  gisTodo.Add(gi);
                  break;
               case ItemIngredient ii: //terminal or nonterminal, probably don't need to do anything.
                  break;
               case OrIngredient oi: //A | B, probably don't need to do anything.
                  if (goal == BFSGoals.OrIngr)
                  {
                     col.Enqueue((pgi, oi));
                  }
                  break;
               default:
                  throw new ArgumentException("Tried to switch on ingredient type, but ingredient type was missing");
            }
         }
         foreach (GroupIngredient gi in gisTodo)
         {
            RecurAddBFS(ref col, gi, goal);
         }
      }
      #endregion

      private ulong uniqueIngredientCounter = 0;
      //numbers:
      //private string CreateUniqueIngredientName() => $"_{uniqueIngredientCounter++}";
      //letters:
      private string CreateUniqueIngredientName()
      {
         StringBuilder res = new();
         //see https://stackoverflow.com/questions/923771/quickest-way-to-convert-a-base-10-number-to-any-base-in-net
         //hexavigesimal

         //note: 'a' is '0', meaning it goes: "w x y z ba bb bc bd"... skipping "aa" because "aa" = "00" = "a" = "0".
         //similarly, it goes "zw zx zy zz baa bab bac bad"...

         char[] baseChars = Enumerable.Range('a', 26).Select(x => (char)x).ToArray();
         ulong targetBase = (ulong)baseChars.Length;
         ulong value = uniqueIngredientCounter;
         do
         {
            res.Insert(0, baseChars[value % targetBase]);
            value = value / targetBase;
         }
         while (value > 0);

         //at the very end
         uniqueIngredientCounter++;
         return '_' + res.ToString();
      }
      #endregion
   }
}
