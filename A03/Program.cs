// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A03: Program to build a frequency table for alphabets in a word list.
// --------------------------------------------------------------------------------------------

namespace A03;

using static Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Method ---------------------------------------------------
   static void Main () {
      string[] wordslist = File.ReadAllLines (@"..\..\..\..\TData\words.txt");
      char[] letters = { 'U', 'X', 'A', 'L', 'T', 'N', 'E' };
      char requiredLetter = letters[0];
      Dictionary<string, int> wordPoints = [];
      foreach (string word in wordslist)
         if (word.Length >= 4 && word.Contains (requiredLetter) && word.All (letters.Contains))
            wordPoints[word] = GetWordScore (word, letters);
      var sortedWordPoints = wordPoints.OrderByDescending (kv => kv.Value).ThenBy (kv => kv.Key);
      foreach (var kvp in sortedWordPoints) {
         bool isPangram = IsPangram (kvp.Key, letters);
         if (isPangram) ForegroundColor = ConsoleColor.Green;
         WriteLine ($"{kvp.Value,3}. {kvp.Key}");
         if (isPangram) ResetColor ();
      }
      WriteLine ("----");
      WriteLine ($"{wordPoints.Values.Sum ()} total");
   }

   // Calculates the score based on the word length and whether it contains all letters.
   static int GetWordScore (string word, char[] letters) =>
      (word.Length == 4 ? 1 : word.Length) + (IsPangram (word, letters) ? 7 : 0);

   // Determines whether the word contains all seven letters.
   static bool IsPangram (string word, char[] letters) => letters.All (word.Contains);
   #endregion
}
#endregion
