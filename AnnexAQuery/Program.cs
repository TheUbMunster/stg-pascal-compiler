using System.Text.RegularExpressions;
using GLRParser;

namespace AnnexAQuery
{
   internal class Program
   {      
      const int minQueryLength = 2;
      private static readonly Regex matchNonTerminalNonDef = new Regex("(?<= )[A-Za-z\\-]+(?= )", RegexOptions.Compiled);
      private static readonly Regex matchDef = new Regex("^[A-Za-z\\-]+(?= =)", RegexOptions.Compiled);
      private static readonly Dictionary<string, (int defIndex, List<int> mentionsIndices)> data = new();
      private static void GatherInfo()
      {
         {
            int ind = 0;
            foreach (string s in Grammar.fullGrammarSource)
            {
               var mm = matchDef.Matches(s);
               if (mm.Count > 1)
                  throw new Exception($"Somehow multiple definitions exist for definition type: {string.Join(", ", mm.Where(x => x.Success).Select(x => x.Value))}");
               if (mm.First().Value.Length < minQueryLength)
                  throw new Exception($"minQueryLength of {minQueryLength} is too restrictive! There exist grammar nonterminals that are shorter than that! e.g., \"{mm.First().Value}\"");
               data.Add(mm.First().Value, (ind, new List<int>()));
               Console.WriteLine(mm.First().Value);
               ind++; //last line in foreach
            }
         }
         {
            int ind = 0;
            foreach (string s in Grammar.fullGrammarSource)
            {
               var mm = matchNonTerminalNonDef.Matches(s);
               foreach (string assoc in mm.Select(x => x.Value).Distinct())
               {
                  if (!data.ContainsKey(assoc))
                     Console.WriteLine($"Data entry for grammar symbol \"{assoc}\" does not exist! Is this a typo in the grammar source?");
                  else
                     data[assoc].mentionsIndices.Add(ind);
               }
               ind++; //last line in foreach
            }
         }
      }
      static void Main(string[] args)
      {
         GatherInfo();
         while (true)
         {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Query a non-terminal grammar symbol (i.e., not a lexer token):");
            Console.ForegroundColor = ConsoleColor.White;
            string input = Console.ReadLine()!.Trim();
            if (input.Length < minQueryLength)
            {
               Console.WriteLine($"Query length must be at least {minQueryLength} characters");
               continue; //we want the list of matching options to be somewhat short, so enforce min length on query.
            }
            var queryMatches = data.Keys.Where(x => x.Contains(input));
            int matchCount = queryMatches.Count();
            string choice = string.Empty;
            if (matchCount == 0)
            {
               Console.WriteLine($"No matches for query: \"{input}\"");
               continue;
            }
            if (matchCount == 1)
            {
               choice = queryMatches.First();
            }
            else if (data.Keys.Where(x => x == input).Count() == 1)
            {
               choice = data.Keys.Where(x => x == input).First();
            }
            else
            {
               while (true)
               {
                  Console.WriteLine($"Multiple matches for query: \"{input}\", choose:");
                  int ind = 0;
                  foreach (var me in queryMatches)
                  {
                     Console.WriteLine($"{ind}: {me}");
                     ind++;
                  }
                  string indChoice = Console.ReadLine()!;
                  if (!int.TryParse(indChoice, out ind))
                  {
                     Console.WriteLine("You didn't enter a valid index: try again.");
                     continue;
                  }
                  if (ind < 0 || ind >= matchCount)
                  {
                     Console.WriteLine("You didn't enter a valid index: try again.");
                  }
                  choice = queryMatches.ElementAt(ind);
                  break;
               }
            }
            Console.WriteLine("DEFINITION:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Grammar.fullGrammarSource[data[choice].defIndex]);
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < data[choice].mentionsIndices.Count; i++)
            {
               if (i == 0)
                  Console.WriteLine("Press enter for definitions that this grammar symbol is mentioned in, or q to quit.");
               input = Console.ReadLine()!;
               if (input.Trim().ToLower() == "q")
                  break;
               else
               {
                  Console.ForegroundColor = ConsoleColor.Yellow;
                  Console.WriteLine(Grammar.fullGrammarSource[data[choice].mentionsIndices[i]]);
                  Console.ForegroundColor = ConsoleColor.White;
               }
            }
         }
      }
   }
}
