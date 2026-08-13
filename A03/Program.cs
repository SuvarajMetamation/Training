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
   static void Main (string[] args) {
      string[] wordslist = File.ReadAllLines (@"TData\words.txt");
      char[] letters = { 'U', 'X', 'A', 'L', 'T', 'N', 'E' };
      Dictionary<string, int> wordPoints = new Dictionary<string, int> ();
      foreach (string word in wordslist) {
         if (word.Length >= 4 && word.Contains (letters[0]) && word.All (letters.Contains))
            wordPoints.Add (word, letters.All
               (l => word.Contains (l)) ? word.Length + 7 : word.Length == 4 ? 1 : word.Length);
      }
      var sortedWordPoints = wordPoints.OrderByDescending (kv => kv.Value).ToList ();
      foreach (var kvp in sortedWordPoints) {
         if (kvp.Value == 15)
            ForegroundColor = ConsoleColor.Green;
         WriteLine ($"{kvp.Value,3}. {kvp.Key}");
         ResetColor ();
      }
      WriteLine ("----");
      WriteLine ($"{wordPoints.Values.Sum ()} total");
   }
   #endregion
}
#endregion
