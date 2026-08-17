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
         var chars = File.ReadAllText (@"..\..\..\..\TData\words.txt");
         if (chars.Length == 0) {
            WriteLine ("No words found in the file.");
            return;
         }
         Dictionary<char, int> freqTable = [];
         foreach (var ch in chars)
            if (char.IsLetter (ch)) freqTable[ch] = freqTable.GetValueOrDefault (ch) + 1;
         WriteLine ("Letter | Occurrences\n--------------------");
         var sortedTable = freqTable.OrderByDescending (a => a.Value).Take (7);
         foreach (var (ch, occur) in sortedTable) WriteLine ($"{ch,-6} | {occur}");
      } catch (IOException ex) { WriteLine ($"Error reading file: {ex.Message}"); }
   }
   #endregion
}
#endregion
