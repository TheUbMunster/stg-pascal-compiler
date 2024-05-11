using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler
{
   static class LegalCharacterSet
   {
      #region Consts
      //this is the list of every character that can appear within a pascal program.
      //if a text file contains a character not in this list, then the file is considered
      //to be an illegal program, and cannot be lexed.
      public static readonly char[] legalChars = new char[]
      {
         '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
         'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
         'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
         '!', '"', '\'', '(', ')', '[', ']', '{', '}', '.', '*', '=', '+', '-', '/', '_', '<', '>', ',', ':', ';', '@', '^', ' ', '\r', '\n', '\t'
      };
      private static readonly (char lowerInclusive, char upperInclusive)[] collapsedLegalChars;
      #endregion

      #region Static Ctor
      static LegalCharacterSet()
      {
         //collapses contiguous sequences of the legal chars into "ranges" covering all the chars in that sequence.
         List<(char lowerInclusive, char upperInclusive)> cl = new();
         bool firstIter = false;
         (char lower, char upper) temp = ('\0', '\0');
         char prev = '\0';
         foreach (char c in legalChars.OrderBy(x => (int)x))
         {
            if (!firstIter)
            {
               firstIter = true;
               temp.lower = prev = c;
            }
            if (c - prev > 1)
            {
               temp.upper = prev;
               cl.Add(temp);
               temp.lower = c;
            }
            prev = c;
         }
         temp.upper = legalChars.Max();
         cl.Add(temp);
         collapsedLegalChars = cl.ToArray();
      }
      #endregion

      #region Public Utility
      public static bool IsLegalProgramCharacter(char c)
      {
         foreach (var range in collapsedLegalChars)
            if (range.lowerInclusive <= c && c <= range.upperInclusive)
               return true;
         return false;
      }
      #endregion
   }
}
