// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A04: Spelling bee extension.
// --------------------------------------------------------------------------------------------

namespace A04;

using static Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Method ---------------------------------------------------
   static void Main () {
      try {
         var chars = File.ReadAllText (@"TData\words.txt");
         if (chars.Length == 0) {
            WriteLine ("No words found in the file.");
            return;
         }
         WriteLine ("Letter | Occurrences\n--------------------");
         var sortedTable = BuildTable (chars).OrderByDescending (a => a.Value);
         foreach (var (ch, occur) in sortedTable.Take (7)) {
            ForegroundColor = ConsoleColor.Green;
            WriteLine ($"{ch,-6} | {occur}");
            ResetColor ();
         }
      } catch (IOException ex) { WriteLine ($"Error reading file: {ex.Message}"); }
   }
   #endregion

   #region Implementation -------------------------------------------
   static Dictionary<char, int> BuildTable (string chars) {
      Dictionary<char, int> freqTable = [];
      foreach (var ch in chars.ToUpper ())
         if (ch is >= 'A' and <= 'Z')
            freqTable[ch] = freqTable.GetValueOrDefault (ch) + 1;
      return freqTable;
   }
   #endregion
}
#endregion
