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
      //refine this concept?
      //private enum GrammarGuarantees
      //{
      //   NoDuplicateEntries = 1,
      //   AllRecipiesContainingTerminalsAreJustOneTerminal = 2,
      //   AllRecipiesHaveTwoOrLessIngredients = 4,
      //   AllRecipiesThatArentASingleTerminalHaveTwoNonterminals = 8,
      //   NoCurlySquareOrOrs = 16,
      //}

      //Nomenclature:
      /* Production: a parsed representation of a single grammar entry
       * Recipie: the right hand side of a production (what the LHS nonterminal is "made of")
       * Nonterminal: an abstract grammar item that is "made of" other things
       * Terminal: a non-abstract grammar item that represents a lexem (is not "made of" other things; concrete)
       * Ingredient: anything in the recipie.
       * GroupIngredient: an ingredient made of other ingredients
       * ...
       */
      private record class Production(Nonterminal lhs, GroupIngredient rhs) // if rhs.ingredients.Coutn == 0, then it's an A -> epsilon (empty) recipie.
      { //value equality should be sufficient.
         public Production Copy()
         {
            Production res = new(lhs.Copy(), (GroupIngredient)rhs.Copy()); //type system makes me sad sometimes
            return res;
         }
         //this is heavy but probably not used for non-debug purposes.
         public override string ToString() => Regex.Replace($"{lhs} = {rhs?.ToString() ?? ""} .", " +", " ").Trim();
      }
      private abstract record class Ingredient() //conceptually similar to a mathematical "term"
      {
         public abstract Ingredient Copy();
      }
      private record class ItemIngredient(Nonterminal? nt, Terminal? t) : Ingredient
      { //value equality should be sufficient.
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
      { //value equality should be sufficient.
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
         /// <summary>
         /// Should only be read when otherwise sure that this group is *items only*.
         /// </summary>
         public IEnumerable<ItemIngredient> ingredientsAsItems { get => ingredients.Select(x => (x as ItemIngredient)!); }
         public override string ToString() => " ( " + string.Join(' ', ingredients) + " ) ";
         public virtual bool Equals(GroupIngredient? other)
         {
            if (other is not null)
               return other.ingredients.SequenceEqual(ingredients);
            else return false;
         }
         public override int GetHashCode() => base.GetHashCode();
      }
      private record class OrIngredient(Ingredient lhs, Ingredient rhs) : Ingredient
      { //value equality should be sufficient.
         public override Ingredient Copy()
         {
            OrIngredient oi = new(lhs.Copy(), rhs.Copy());
            return oi;
         }
         public override string ToString() => $"( {lhs} | {rhs} )";
      }
      private record class Nonterminal(string Name) //maybe this should derive from ingredient and ItemIngredient be deleted.
      { //value equality should be sufficient.
         public Nonterminal Copy() => new(Name);
         public override string ToString() => $" {Name} ";
      }
      private record class Terminal(string Name) //maybe this should derive from ingredient and ItemIngredient be deleted.
      { //value equality should be sufficient.
         public Terminal Copy() => new(Name);
         public override string ToString() => $" '{Name}' ";
      }
      #endregion

      public string[] GrammarSource { get; }
      public string StartingNonterminal { get; }
      public string[] CNFGrammarSource { get; }
      public string CNFStartingNonterminal { get; }
      /// <summary>
      /// Takes an input grammar adhering to the following rules: <br/><br/>
      /// 
      /// <b>
      /// * All non-terminals consist of [A-Za-z-] (upon converting the grammar,
      /// some of the non-terminals in the output grammar may contain underscores
      /// (_) as well).<br/>
      /// * All terminals consist of [TODO].<br/>
      /// * All terminals are encapsulated in single quotes "'".<br/>
      /// * All terminals that contain a literal single quote must be escaped with a
      /// backslash. (e.g., "'\''").<br/><br/>
      /// 
      /// This CNFifier is designed to handle grammar adhering to the same rules
      /// as the pascal specification.
      /// </b>
      /// <br/><br/>
      /// and returns an equivalent grammar in Chomsky Normal Form (CNF).
      /// </summary>
      /// <remarks>
      /// Intended for use in a compiler, and designed to work with a lightly modified version of the 
      /// pascal grammar in mind. It's possible this has bugs, but it works for my purposes.
      /// </remarks>
      public CNFifier(string[] grammarSource, string startingNonterminal) 
      {
         GrammarSource = grammarSource;
         StartingNonterminal = startingNonterminal;
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
         //Regex eqToken = new Regex("[A-Za-z-]+\\s*(=)", RegexOptions.Compiled);

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
            //new Production(
            //   new Nonterminal("A"),
            //   new GroupIngredient(new List<Ingredient>()
            //   {
            //      new ItemIngredient(new Nonterminal("B"), null),
            //      new ItemIngredient(new Nonterminal("C"), null),
            //      new ItemIngredient(new Nonterminal("D"), null),
            //      new ItemIngredient(new Nonterminal("E"), null),
            //      new ItemIngredient(new Nonterminal("F"), null),
            //      new ItemIngredient(new Nonterminal("G"), null),
            //      new ItemIngredient(new Nonterminal("H"), null),
            //   })),

            /*S0 → AbB | C
               B → AA | AC
               C → b | c
               A → a | ε
            */
            //new Production(new Nonterminal("S"), new GroupIngredient(new List<Ingredient>()
            //{
            //   new OrIngredient(new GroupIngredient(new List<Ingredient>()
            //   {
            //      new ItemIngredient(new Nonterminal("A"), null),
            //      new ItemIngredient(null, new Terminal("b")),
            //      new ItemIngredient(new Nonterminal("B"), null),
            //   }), new ItemIngredient(new Nonterminal("C"), null))
            //})),
            //new Production(new Nonterminal("B"), new GroupIngredient(new List<Ingredient>()
            //{
            //   new OrIngredient(new GroupIngredient(new List<Ingredient>()
            //   {
            //      new ItemIngredient(new Nonterminal("A"), null),
            //      new ItemIngredient(new Nonterminal("A"), null),
            //   }), new GroupIngredient(new List<Ingredient>()
            //   {
            //      new ItemIngredient(new Nonterminal("A"), null),
            //      new ItemIngredient(new Nonterminal("C"), null),
            //   }))
            //})),
            //new Production(new Nonterminal("C"), new GroupIngredient(new List<Ingredient>()
            //{
            //   new OrIngredient(new GroupIngredient(new List<Ingredient>()
            //   {
            //      new ItemIngredient(null, new Terminal("b")),
            //   }), new GroupIngredient(new List<Ingredient>()
            //   {
            //      new ItemIngredient(null, new Terminal("c")),
            //   }))
            //})),
            //new Production(new Nonterminal("A"), new GroupIngredient(new List<Ingredient>()
            //{
            //   new OrIngredient(new GroupIngredient(new List<Ingredient>()
            //   {
            //      new ItemIngredient(null, new Terminal("a")),
            //   }), new GroupIngredient(new List<Ingredient>()))
            //})),
            /*
              Expr	→ Term	| Expr AddOp Term	| AddOp Term
              Term	→ Factor	| Term MulOp Factor
              Factor	→ Primary	| Factor ^ Primary
              Primary	→ number	| variable	| ( Expr )
              AddOp	→ +	| −
              MulOp	→ *	| /
            */
            new Production(new Nonterminal("Expr"), new GroupIngredient(new List<Ingredient>()
            {
               new OrIngredient
               (
                  new ItemIngredient(new Nonterminal("Term"), null), 
                  new OrIngredient
                  (
                     new GroupIngredient(new List<Ingredient>()
                     {
                        new ItemIngredient(new Nonterminal("Expr"), null),
                        new ItemIngredient(new Nonterminal("AddOp"), null),
                        new ItemIngredient(new Nonterminal("Term"), null),
                     }),
                     new GroupIngredient(new List<Ingredient>()
                     {
                        new ItemIngredient(new Nonterminal("AddOp"), null),
                        new ItemIngredient(new Nonterminal("Term"), null),
                     })
                  )
               )
            })),
            new Production(new Nonterminal("Term"), new GroupIngredient(new List<Ingredient>()
            {
               new OrIngredient
               (
                  new ItemIngredient(new Nonterminal("Factor"), null),
                  new GroupIngredient(new List<Ingredient>()
                  {
                     new ItemIngredient(new Nonterminal("Term"), null),
                     new ItemIngredient(new Nonterminal("MulOp"), null),
                     new ItemIngredient(new Nonterminal("Factor"), null),
                  })
               )
            })),
            new Production(new Nonterminal("Factor"), new GroupIngredient(new List<Ingredient>()
            {
               new OrIngredient
               (
                  new ItemIngredient(new Nonterminal("Primary"), null),
                  new GroupIngredient(new List<Ingredient>()
                  {
                     new ItemIngredient(new Nonterminal("Factor"), null),
                     new ItemIngredient(null, new Terminal("^")),
                     new ItemIngredient(new Nonterminal("Primary"), null),
                  })
               )
            })),
            new Production(new Nonterminal("Primary"), new GroupIngredient(new List<Ingredient>()
            {
               new OrIngredient
               (
                  new ItemIngredient(null, new Terminal("number")),
                  new OrIngredient
                  (
                     new ItemIngredient(null, new Terminal("variable")),
                     new GroupIngredient(new List<Ingredient>()
                     {
                        new ItemIngredient(null, new Terminal("(")),
                        new ItemIngredient(new Nonterminal("Expr"), null),
                        new ItemIngredient(null, new Terminal(")")),
                     })
                  )
               )
            })),
            new Production(new Nonterminal("AddOp"), new GroupIngredient(new List<Ingredient>()
            {
               new OrIngredient
               (
                  new ItemIngredient(null, new Terminal("+")),
                  new ItemIngredient(null, new Terminal("-"))
               )
            })),
            new Production(new Nonterminal("MulOp"), new GroupIngredient(new List<Ingredient>()
            {
               new OrIngredient
               (
                  new ItemIngredient(null, new Terminal("*")),
                  new ItemIngredient(null, new Terminal("/"))
               )
            })),
         };
         List<Production> parsedGrammar = new List<Production>();

         //bool eq = new Terminal("asdf").Equals(new Terminal("asdf")); //true value equality
         //bool eq = ((Ingredient)new GroupIngredient(new List<Ingredient>() { new ItemIngredient(null, null) })).Equals(((Ingredient)new GroupIngredient(new List<Ingredient>() { new ItemIngredient(null, null) }))); //true

         //temp:
         parsedGrammar = testGrammar;
         startingNonterminal = "Expr";

         //TODO: convert grammar source into list of productions.

         /* Grammar Grammar:
          * 
          * terminal = '\'.+\'' .
          * nonterminal = [A-Za-z-_]+ .
          * ingredients = '(' ingredients ')' .
          * ingredients = '[' ingredients ']' .
          * ingredients = '{' ingredients '}' .
          * ingredients = ingredients '|' ingredients .
          * ingredients = ingredients ingredients .
          * production = nonterminal '=' ingredients '.' .
          */
         Regex whitespace = new Regex("\\s+");
         //todo match to nearest closing ', but match as few as possible, unless the matched ' is escaped (i.e., \') to prevent situations like '(' ')' being a single terminal containing "(' ')".
         Regex terminal = new Regex("'.+'");
         Regex nonterminal = new Regex("[A-Za-z-_]+");
         Regex l_paren = new Regex("\\(");
         Regex r_paren = new Regex("\\)");
         Regex l_square = new Regex("\\[");
         Regex r_square = new Regex("\\]");
         Regex l_curly = new Regex("\\{");
         Regex r_curly = new Regex("\\}");
         Regex period = new Regex("\\.");
         Regex equals = new Regex("\\=");
         Regex pipe = new Regex("\\|");

         //TODO: after each transformation step(s), ensure the desired property was achieved (so that subsequent transformation methods don't need to check).

         if (!parsedGrammar.Any(x => x.lhs.Name == startingNonterminal))
            throw new Exception("The starting nonterminal does not appear in the grammar source!");

         //call in either of these orders:
         //?VARIABLE?,...,...,START,TERM,BIN,DEL,UNIT
         //?VARIABLE?,...,...,START,BIN,DEL,UNIT,TERM
         //a possible optimization would be to try multiple (valid) combinations, and pick the smallest
         //for now, I don't care.
         bool needPreTransform = true;
         while (needPreTransform)
         {
            //keep attempting to apply these transformations until it no longer does anything.
            //since these are 'non-piercing', I don't think the order of these can affect the blow-up of the grammar.
            bool t = false;
            t |= TransformGrammarVARIABLE(parsedGrammar);
            t |= TransformGrammarEITHER(parsedGrammar);
            t |= TransformGrammarOPTIONAL(parsedGrammar);
            needPreTransform = t;
         }
         //at this point there should be no { }, [ ], or |
         //probably need to flatten nested groups into a single top level group.
         SimplifySinglyGroups(parsedGrammar);
         CNFStartingNonterminal = startingNonterminal = TransformGrammarSTART(parsedGrammar, startingNonterminal);
         TransformGrammarTERM(parsedGrammar);
         TransformGrammarBIN(parsedGrammar);
         TransformGrammarDEL(parsedGrammar);
         //there is an error somewhere from this point forward
         TransformGrammarUNIT(parsedGrammar); //even if this messes with the starting recipie, the startingNonterminal won't change.
         RemoveDuplicates(parsedGrammar); //just in case (if this ever finds any duplicates, maybe investigate if one of the transformations has a bug causing it).
         SortLexicographically(parsedGrammar);
         bool success = CheckAdheresToCNF(parsedGrammar);
         if (!success)
            throw new Exception("Applied all CNFifier transformations, and yet output grammar was not CNF");
         CNFGrammarSource = parsedGrammar.Select(x => x.ToString()).ToArray();
         List<Production> ord = OrdGrammarRepresentation(parsedGrammar); //needs testing

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











         /*
          * For this CNFifier to be actually useful in making a *parser*, and not just a recognizer,
          * the parser tree would be best to be constructed of the form akin to the *original grammar*,
          * not the transformed one. Since code needs to be written to read the nodes at compile time,
          * it won't work with whatever the CNFifier spits out, so we need to make a system that "keeps
          * track" of the transformations made on each grammar node. If transformations are "atomic",
          * then every time a transformation occurs, that transformation can be reversed later after
          * to create an AST node tree that is comprised of nodes from the original grammar.
          * 
          * To handle this, I think the idea of a "mapping" might be useful. For example, if a grammar
          * like this:
          * 
          * A -> B c D
          * 
          * Gets transformed into something like this (CNF):
          * 
          * A -> B _A_bin_partial
          * _A_bin_partial -> _c_removed_term D
          * _c_removed_term -> c
          * 
          * And *if* we are able to somehow guarantee that an "A" nonterminal in the former is logically
          * equivalent to an "A" nonterminal in the transformed version, then we can "match them together".
          * 
          * This guarantee probably can only be enforced on the LHS of productions, because if we enforce it
          * on both, then the grammar is 100% rigid, and no transformations could even occur, right?
          * 
          * One possibility would be to "linearize" the parse and "forcefully impose" the result on the original
          * grammar structure (but in certain situations, this there might be ambiguity (e.g., left handed vs 
          * right handed expressions).
          * 
          * See this stackxchange post I made: https://stackoverflow.com/questions/78919141/how-to-convert-a-parse-tree-of-one-grammar-in-cnf-form-and-apply-it-to-the-ori
          * 
          * ~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~
          * Originally, I had thought the above approach was flawed and I had begun to stub out another thought,
          * but then I realized it's the same idea, but stated differently.
          * 
          * This seems flawed, and I think theres a better idea:
          * 
          * If we take a look at the starting recipies of both:
          * A -> B c D
          * A -> B _A_bin_partial
          * 
          * Somehow a parse tree of the latter form must somehow contain everything the former does.
          * Suppose that we instantiate an empty ASTNode of the former form (with the intent to 
          * Stepping through one by one, we see that
          * 
          * A -> B c D
          *      ^
          *      |------ This (former)
          *      
          * A -> B _A_bin_partial
          *      ^ ^ 
          *      | |
          *      | ----- And potentially also this (latter)
          *      |
          *      |------ And this (latter)
          *      
          * The latter information must correlate to the former.
          *      
          *      
          * -===================================================================================-
          * 
          * Taking it one by one, here are my thoughts on each of the individual transformations:
          * 
          * VARIABLE:
          * EITHER:
          * OPTIONAL:
          * 
          * -===================-
          * SimplifySinglyGroups:
          * -===================-
          * All this does is removes crazy nested parenthesis like these "A = ( ( ( NONTERM ) ) ) ."
          * and fixes it to something like this: "A = ( NONTERM ) ."
          * 
          * -====-
          * START:
          * -====-
          * This technechally doesn't manipulate the grammar, it *adds* to it. If a grammar has multiple
          * recipies from the start node, this condenses it to a single start node (this also guarantees
          * that the (literal) start node never appears on the RHS of a recipie, which is required for CYK).
          * I'm not sure how to map this.
          * 
          * -===-
          * TERM:
          * -===-
          * Since this creates a new recipie *dedicated* for [this] recipie, anytime we try to "handle"
          * an ASTNode of the new type in the output parse, we know the only place it could possibly belong
          * is to a [this] type recipie, so the mapping seems trivial.
          * 
          * BIN:
          * DEL:
          * UNIT:
          * RemoveDuplicates:
          * SortLexicographically:
          */
      }

      #region Utility
      #region Wikipedia transformations
      //see https://en.wikipedia.org/wiki/Chomsky_normal_form
      private string TransformGrammarSTART(List<Production> intermediateGrammar, string oldStartSymbol)
      {
         //ensure that the starting symbol does not appear on the right hand side of a recipie (by making a new start symbol)
         string newNTName = CreateUniqueIngredientName($"{oldStartSymbol}_renamed_start");
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
            int ingCount = p.rhs.ingredients.Count;
            for (int j = ingCount - 1; j >= 0; j--)
            {
               Ingredient ing = p.rhs.ingredients[j];
               switch (ing)
               {
                  case ItemIngredient ii:
                     if (ii.t != null && ingCount > 1) //we have a terminal, and this recipie has more than one ingredient.
                     {
                        string newNTName = CreateUniqueIngredientName($"{p.lhs.Name}_removed_term");
                        Production np = new Production(new Nonterminal(newNTName), new GroupIngredient(new List<Ingredient>()
                        {
                           new ItemIngredient(null, ii.t)
                        }));
                        ItemIngredient replacement = new ItemIngredient(new Nonterminal(newNTName), null);
                        int ind = p.rhs!.ingredients.IndexOf(ing);
                        p.rhs!.ingredients.Remove(ing);
                        p.rhs!.ingredients.Insert(ind, replacement);
                        intermediateGrammar.Add(np);
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
            int ingCount = prod.rhs.ingredients.Count;
            if (ingCount > 0) //not an empty recipie
            {
               ItemIngredient ee = (ItemIngredient)prod.rhs!.ingredients[0];
               if (ee.nt != null && ingCount >= 3)
               {
                  intermediateGrammar.RemoveAt(i);
                  string[] newNTNames = new string[ingCount - 2];
                  for (int j = 0; j < newNTNames.Length; j++)
                     newNTNames[j] = CreateUniqueIngredientName($"{prod.lhs.Name}_bin_partial");
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
         #region Helpers
         static List<Production> RemoveEmptyProductions(List<Production> prods)
         { //goes through the list of productions and removes from it (& returns) all A -> empty productions.
            List<Production> emptyProds = new List<Production>();
            int count = prods.Count;
            for (int i = count - 1; i >= 0; i--)
            {
               if (prods[i].rhs.ingredients.Count == 0)
               {
                  emptyProds.Add(prods[i]);
                  prods.RemoveAt(i);
               }
            }
            return emptyProds;
         }
         static List<Production> RemoveNullableProductions(List<Production> prods, List<Production> emptyProductions)
         {
            List<Production> nullProds = new List<Production>();
            while (true)
            {
               IEnumerable<Nonterminal> currentNullProds = nullProds.Select(x => x.lhs).Concat(emptyProductions.Select(x => x.lhs));
               List<int> nps = Enumerable.Range(0, prods.Count).Where(i =>
               {
                  Production x = prods[i];
                  return x.rhs!.ingredients.All(y => currentNullProds.Contains(((ItemIngredient)y).nt));
               }).ToList(); //probably should force eager eval.
               if (nps.Count() == 0) //if there are no more nully productions, we're done.
                  break;
               nullProds.AddRange(nps.Select(i => prods[i])); //add the chosen nully prods to the nullProds
               foreach (int p in nps)
                  prods.RemoveAt(p); //remove the nully prods from prods.
            }
            return nullProds;
         }
         static List<Production> PermuteProduction(Production prod, List<Nonterminal> nullProds)
         {
            //the beginnings of my attempt at a generalized version of the below.
            //static void Asdf(int ind, Production prod, ref Queue<Production> results, List<Nonterminal> nullProds)
            //{
            //   {
            //      int desiredIndex = ind;
            //      for (int i = 0; i < prod.rhs!.ingredients.Count; i++)
            //      {
            //         ItemIngredient ii = (ItemIngredient)prod.rhs!.ingredients[i];
            //         if (nullProds.Contains(ii.nt!))
            //      }
            //   }
            //}

            List<Production> resultProds = new List<Production>();
            int iCount = prod.rhs!.ingredients.Count;
            if (iCount == 1)
            {
               ItemIngredient ii = (ItemIngredient)prod.rhs!.ingredients[0];
               if (nullProds.Contains(ii.nt!))
               {
                  resultProds.Add(prod);
                  Production without = prod.Copy();
                  without.rhs!.ingredients.RemoveAt(0);
                  resultProds.Add(without);
               }
               else throw new Exception(); //prod should have had >= 1 nully ingredient
            }
            else if (iCount == 2)
            {
               ItemIngredient ii1 = (ItemIngredient)prod.rhs!.ingredients[0];
               ItemIngredient ii2 = (ItemIngredient)prod.rhs!.ingredients[1];
               if (nullProds.Contains(ii1.nt!))
               {
                  if (nullProds.Contains(ii2.nt!))
                  { //both
                     resultProds.Add(prod);
                     Production withoutFirst = prod.Copy(), withoutSecond = prod.Copy(), withoutEither = prod.Copy();
                     withoutFirst.rhs.ingredients.RemoveAt(0);
                     withoutSecond.rhs.ingredients.RemoveAt(1);
                     withoutEither.rhs.ingredients.RemoveAt(1); //index 1 *FIRST* because shrink
                     withoutEither.rhs.ingredients.RemoveAt(0); //then index 0
                     resultProds.Add(withoutFirst);
                     if (ii1 != ii2) //prevent duplicate in the case that the production is two identical nonterminals
                        resultProds.Add(withoutSecond);
                     //resultProds.Add(withoutEither); //this itself is just an empty production (Don't bother including)
                  }
                  else
                  { //first only
                     resultProds.Add(prod);
                     Production without = prod.Copy();
                     without.rhs!.ingredients.RemoveAt(0);
                     resultProds.Add(without);
                  }
               }
               else
               {
                  if (nullProds.Contains(ii2.nt!))
                  { //second only
                     resultProds.Add(prod);
                     Production without = prod.Copy();
                     without.rhs!.ingredients.RemoveAt(1);
                     resultProds.Add(without);
                  }
                  else throw new Exception(); //prod should have had >= 1 nully ingredient
               }
            }
            else throw new Exception();
            return resultProds;
         }

         static List<Production> RemoveImpactedProductions(List<Production> prods, List<Nonterminal> nts)
         {
            List<Production> result = new List<Production>();
            int count = prods.Count;
            for (int i = count - 1; i >= 0; i--)
            {
               Production p = prods[i];
               if (p.rhs.ingredientsAsItems.Select(x => x.nt).Intersect(nts).Any())
               {
                  prods.RemoveAt(i);
                  result.Add(p);
               }
            }
            return result;
         }
         #endregion
         //Note: all empties (A = ( ) .) are nully, but not all nully are empties
         List<Production> empties = RemoveEmptyProductions(intermediateGrammar); //do not add back
         List<Production> nulls = RemoveNullableProductions(intermediateGrammar, empties); //these need to be added back
         List<Nonterminal> nullsNT = nulls.Select(x => x.lhs).Concat(empties.Select(x => x.lhs)).ToList();
         List<Production> impacted = RemoveImpactedProductions(intermediateGrammar, nullsNT);
         foreach (Production np in nulls.Concat(impacted)) //needs to iterate over every production that has a nully nonterminal in the recipie
         {
            List<Production> newP = PermuteProduction(np, nullsNT);
            intermediateGrammar.AddRange(newP);
         }
         //intermediateGrammar.AddRange(nulls); //do not explicitly add these back because PermuteProduction includes them in the return value.

         //List<Production> nulls = RemoveEmptyProductions(intermediateGrammar);
         //while (nulls.Count > 0)
         //{ //based on the null transformations, implement them at call sites etc.
         //   //for (int n = 0; n < nulls.Count; n++)
         //   foreach (Nonterminal nnt in nulls.Select(x => x.lhs))
         //   {
         //      //all recipies should be either one terminal or 1-2 nonterminals I think
         //      int count = intermediateGrammar.Count;
         //      for (int i = count - 1; i >= 0; i--)
         //      {
         //         Production prod = intermediateGrammar[i];
         //         int nullIngCount = prod.rhs!.ingredients.Count(x => nnt.Equals((ItemIngredient)x));
         //         if (nullIngCount > 0)
         //         { //this production contains at least one of this nully nonterminal
         //            intermediateGrammar.RemoveAt(i);
         //            //if 1, just delete 
         //         }
         //      }

         //      //Nonterminal nnt = nulls[n].lhs;

         //      //??? not sure if this is a great explanation ???
         //      //one by one, iterate through all the recipies that contain (at least one of) this NT in the recipie.
         //      //make all permutations of with/without (present/empty) of that NT.
         //      //e.g.:
         //      //A -> B nnt
         //      //to:
         //      //A -> B
         //      //A -> B e

         //      //for every null recipie, if that is the only recipie for that NT, we can just go to all
         //      //recipies that NT is an ingredient of and just simply remove it (and remove the null recipie).
         //   }
         //   //at the very end
         //   nulls = RemoveEmptyProductions(intermediateGrammar); //old nulls get discarded
         //}
      }

      private void TransformGrammarUNIT(List<Production> intermediateGrammar)
      {
         //given
         //A -> B
         //B -> ...
         //B -> ...
         //B -> ...
         //B -> ...

         //TODO: this description was inaccurate, the algorithm needs to be corrected:
         //remove A -> B and copy/paste the B -> ... rules and replace lhs with A
         //B -> ...
         //B -> ...
         //B -> ...
         //B -> ...
         //A -> ...
         //A -> ...
         //A -> ...
         //A -> ...
         bool anyChanges;
         do
         {
            anyChanges = false;

            int count = intermediateGrammar.Count;
            for (int i = count - 1; i >= 0; i--)
            {
               Production prod = intermediateGrammar[i];
               ItemIngredient rhs = ((ItemIngredient)prod.rhs!.ingredients[0]);
               if (prod.rhs.ingredients.Count == 1 && rhs.nt != null)
               { //this is a nonterminal -> nonterminal production
                  intermediateGrammar.RemoveAt(i); //remove A -> B

                  int ccount = intermediateGrammar.Count;
                  for (int j = ccount - 1; j >= 0; j--) //find all B -> ... and add A -> ... variant
                     if (intermediateGrammar[j].lhs == rhs.nt)
                     {
                        anyChanges |= true;
                        intermediateGrammar.Add(new Production(prod.lhs.Copy(), (GroupIngredient)intermediateGrammar[j].rhs.Copy()));
                     }
               }
            }
         } while (anyChanges);
      }

      //maybe this belongs in a "corrective transformations" category?
      private void RemoveDuplicates(List<Production> intermediateGrammar)
      {
         int count = intermediateGrammar.Count;
         for (int i = count - 1; i >= 0; i--)
         {
            Production potentialDuplicate = intermediateGrammar[i];
            intermediateGrammar.RemoveAt(i);
            //this is doing ~n^2 comparisons, can be made more efficient for (1/2)*n^2 comparisons
            if (intermediateGrammar.All(x => x != potentialDuplicate)) //do any others match it?
               intermediateGrammar.Insert(i, potentialDuplicate); //no matches, go ahead and add it back.
         }
      }

      private void SimplifySinglyGroups(List<Production> intermediateGrammar)
      {
         //e.g.
         //A -> ( ( B ) )
         //A -> ( B ( C ) )
         static void Simplify(Production prod)
         { //return true if any modification was made so we know to keep checking.

            bool modificationWasMade;
            do
            {
               modificationWasMade = false;
               for (int i = 0; i < prod.rhs.ingredients.Count; i++)
               {
                  if (prod.rhs.ingredients[i] is GroupIngredient)
                  {
                     GroupIngredient gi = ((GroupIngredient)prod.rhs.ingredients[i]);
                     prod.rhs.ingredients.RemoveAt(i);
                     prod.rhs.ingredients.InsertRange(i, gi.ingredients);
                     modificationWasMade = true;
                  }
               }
            } while (modificationWasMade);
         }
         //if there are any groups that contain a single item, instead just delete the group and put
         //the single item where the group was.
         int count = intermediateGrammar.Count;
         for (int i = count - 1; i >= 0; i--)
         {
            Production prod = intermediateGrammar[i];
            intermediateGrammar.RemoveAt(i);
            Simplify(prod);
            intermediateGrammar.Insert(i, prod);
         }
      }

      private void SortLexicographically(List<Production> intermediateGrammar)
      {
         intermediateGrammar.Sort(new Comparison<Production>((a, b) => a.lhs.Name.CompareTo(b.lhs.Name)));
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
               Production potentialModifiedOriginalWith = prod.Copy(),
                  potentialModifiedOriginalWithout = prod.Copy(),
                  potentialPartD;
               //extract { B }, stick it in potentialPartC, remove it from potentialModifiedOriginal
               string newNTName;
               { //WITH Z
                  Queue<(GroupIngredient parent, Ingredient inQuestion)> zeroOrOneVIs = new();
                  RecurAddBFS(ref zeroOrOneVIs, potentialModifiedOriginalWith.rhs!, BFSGoals.VIRepeat);
                  if (zeroOrOneVIs.Count == 0)
                     return false;
                  newNTName = CreateUniqueIngredientName($"{prod.lhs.Name}_extract_variable"); //we know we're using it, so allocate it.
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
                  potentialPartD = new Production(new Nonterminal(newNTName), new GroupIngredient(new List<Ingredient>()));
                  potentialPartD.rhs!.ingredients.Add(((VariableIngredient)chosenVI.inQuestion).item);
               }
               partC = new Production(new Nonterminal(newNTName),
                     new GroupIngredient(new List<Ingredient>()
                     {
                        new ItemIngredient(new Nonterminal(newNTName), null),
                        new ItemIngredient(new Nonterminal(newNTName), null)
                     })
                  );
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
         static bool FixOneEither(Production prod, out Production? withLeft, out Production? withRight)
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
         static bool FixOneOptional(Production prod, out Production? with, out Production? without)
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
      private static void RecurAddBFS(ref Queue<(GroupIngredient parent, Ingredient inQuestion)> col, GroupIngredient pgi, BFSGoals goal)
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
      private string CreateUniqueIngredientName(string? regarding = null)
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
         return '_' + regarding ?? string.Empty + '_' + res.ToString();
      }
      #endregion

      #region Check
      private bool CheckAdheresToCNF(List<Production> intermediateGramar)
      {
         bool success = true;
         foreach (Production p in intermediateGramar)
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
         return success;
      }

      private bool CheckNoDuplicateProductions(List<Production> intermediateGrammar)
      {
         int count = intermediateGrammar.Count;
         for (int i = count - 1; i >= 0; i--)
         {
            Production potentialDuplicate = intermediateGrammar[i];
            if (intermediateGrammar.Count(x => x == potentialDuplicate) > 1)
               return false;
         }
         return true;
      }
      #endregion

      private List<Production> OrdGrammarRepresentation(List<Production> intermediateGrammar)
      {
         //todo: see if there's a clean way to prevent right paren groupings
         List<Production> result = new List<Production>();
         var prodGroups = intermediateGrammar.GroupBy(x => x.lhs); //groups with the same LHS
         foreach (var group in prodGroups)
         {
            Production newProd = new Production(group.First().lhs, new GroupIngredient(new List<Ingredient>()));
            List<GroupIngredient> rhss = group.Select(x => (x.rhs.Copy() as GroupIngredient)!).ToList();
            if (rhss.Count == 1)
               newProd.rhs.ingredients.Add(rhss[0]);
            else if (rhss.Count == 2)
               newProd.rhs.ingredients.Add(new OrIngredient(rhss[0], rhss[1]));
            else
            { //hard to do iteravely because the code require mutability, but OrIngredient is immutable.
               static OrIngredient RecurAllOrs(List<GroupIngredient> ings, int ind)
               {
                  if (ind == ings.Count - 2)
                     return new OrIngredient(ings[ings.Count - 2], ings[ings.Count - 1]);
                  else
                     return new OrIngredient(ings[ind], RecurAllOrs(ings, ind + 1));
               }
               newProd.rhs.ingredients.Add(RecurAllOrs(rhss, 0));
            }
            result.Add(newProd);
         }
         return result;
      }
   }
}
