
// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinee at Trumpf Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// --------------------------------------------------------------------------------------------
// Program.cs
// A12: Wordle
// --------------------------------------------------------------------------------------------
using System.Reflection;
using System.Text;
using static A12.EState;
using static System.Console;
using static System.ConsoleColor;
using static System.ConsoleKey;
using static System.Math;

namespace A12;

#region class Program -----------------------------------------------------------------------------
internal class Program {
   #region Method ---------------------------------------------------
   static void Main () => Wordle.Run ();
   #endregion
}
#endregion

#region class Wordle ------------------------------------------------------------------------------
/// <summary>Class to implement the wordle game</summary>
static class Wordle {
   #region Method ---------------------------------------------------
   /// <summary>Runs the wordle game</summary>
   public static void Run () {
      OutputEncoding = Encoding.UTF8;
      CursorVisible = false;
      Clear ();
      Initialize ();
      DisplayBoard ();
      DisplayAlphabets ();
      while (!sIsGameOver) {
         bool refreshAlphabet = UpdateGame (ReadKey (true));
         DisplayBoard ();
         if (refreshAlphabet) DisplayAlphabets ();
      }
      PrintResult ();
      CursorVisible = true;
   }
   #endregion

   #region Implementation -------------------------------------------
   // Initializes the words for seed and valid words
   static void Initialize () {
      try {
         var (seedWords, validWords) = (LoadStrings ("puzzle.txt"), LoadStrings ("dict.txt"));
         if (seedWords.Length == 0 || validWords.Length == 0)
            throw new InvalidDataException ("Word files cannot be empty.");
         sSeed = seedWords[Random.Shared.Next (seedWords.Length)].ToUpperInvariant ();
         sValidWords = [.. validWords.Select (w => w.ToUpperInvariant ())];
      } catch (Exception ex) {
         WriteLine ($"Error reading files!! {ex.Message}"); Environment.Exit (1);
      }
   }

   // Collects words from text files
   static string[] LoadStrings (string file) {
      using var stream = Assembly.GetExecutingAssembly ().GetManifestResourceStream ($"A12.Data.{file}")
         ?? throw new FileNotFoundException ($"Resource {file} was not found.");
      using var reader = new StreamReader (stream);
      return reader.ReadToEnd ().Split (['\r', '\n'],
         StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
   }

   // Displays the game board
   static void DisplayBoard () {
      GetPosition (out int left, out int top);
      int boardLeft = left + (BOARDWIDTH - LEN * 2 + 1) / 2;
      int evaluatedCount = sInputs.Count - sWord.Length;
      foreach (int row in Enumerable.Range (0, MAXTRIES)) {
         SetCursorPosition (boardLeft, top + row);
         foreach (int col in Enumerable.Range (0, LEN)) {
            int index = row * LEN + col;
            if (index < sInputs.Count) {
               var (ch, state) = sInputs[index];
               if (index < evaluatedCount)
                  ForegroundColor = state switch {
                     Exact => Green,
                     Misplaced => Blue,
                     _ => DarkGray
                  };
               Write ($"{ch} ");
               ResetColor ();
            } else Write (index == sInputs.Count ? "\u25cc " : ". ");
         }
      }
   }

   // Gets the starting position to centre the game in the console
   static void GetPosition (out int left, out int top) {
      left = Max (0, (WindowWidth - BOARDWIDTH) / 2);
      top = Max (0, (WindowHeight - BOARDHEIGHT) / 2);
   }

   // Displays the alphabet with the evaluated letter colors
   static void DisplayAlphabets () {
      GetPosition (out int left, out int top);
      int evaluatedCount = sInputs.Count - sWord.Length;
      var evaluated = sInputs.Take (evaluatedCount);
      SetCursorPosition (left, top + MAXTRIES + 1);
      WriteLine (new string ('\u2500', BOARDWIDTH));
      SetCursorPosition (left, top + MAXTRIES + 2);
      int index = 0;
      foreach (char ch in "ABCDEFGHIJKLMNOPQRSTUVWXYZ") {
         ForegroundColor = evaluated switch {
            _ when evaluated.Contains ((ch, Exact)) => Green,
            _ when evaluated.Contains ((ch, Misplaced)) => Blue,
            _ when evaluated.Contains ((ch, Absent)) => DarkGray,
            _ => White
         };
         Write ($"{ch}   ");
         ResetColor ();
         if (++index % ROWSIZE == 0) SetCursorPosition (left, top + MAXTRIES + 2 + index / ROWSIZE);
      }
   }

   // Updates the game state
   static bool UpdateGame (ConsoleKeyInfo info) {
      char ch = char.ToUpperInvariant (info.KeyChar);
      // Accept alphabetic characters
      if (ch is >= 'A' and <= 'Z' && sWord.Length < LEN) {
         sInputs.Add ((ch, EState.None));
         sWord += ch;
         return false;
      }
      // Evaluate the entered word
      if (info.Key == Enter && sWord.Length == LEN) {
         if (sValidWords.Contains (sWord)) {
            sIsFound = sWord == sSeed;
            Restructure ();
            sWord = string.Empty;
            sIsGameOver = sIsFound || sInputs.Count / LEN == MAXTRIES;
            return true;
         }
         sInputs.RemoveRange (sInputs.Count - LEN, LEN);
         PrintMsg (sWord);
         sWord = string.Empty;
         return false;
      }
      // Remove the last entered character
      if ((info.Key is Backspace or Delete) && sWord.Length > 0) {
         sInputs.RemoveAt (sInputs.Count - 1);
         sWord = sWord[..^1];
      }
      return false;
   }

   // Restructures the list based on the seed word
   static void Restructure () {
      var (rem, result) = (new Dictionary<char, int> (), new EState[LEN]);
      // Identify exact matches and count remaining letters
      for (int i = 0; i < LEN; i++) {
         char ch = sSeed[i];
         if (sWord[i] == ch) result[i] = Exact;
         else rem[ch] = rem.GetValueOrDefault (ch) + 1;
      }
      // Identify misplaced and absent letters
      for (int i = 0; i < LEN; i++) {
         char ch = sWord[i];
         if (result[i] == EState.None) {
            if (rem.GetValueOrDefault (ch) > 0) {
               result[i] = Misplaced;
               rem[ch]--;
            } else result[i] = Absent;
         }
      }
      // Replace the entered letters with evaluated results
      sInputs.RemoveRange (sInputs.Count - LEN, LEN);
      sInputs.AddRange (sWord.Select ((ch, i) => (ch, result[i])));
   }

   // Prints the game result
   static void PrintResult () {
      GetPosition (out int left, out int top);
      SetCursorPosition (left, Min (WindowHeight - 1, top + BOARDHEIGHT + 1));
      if (sIsFound) {
         ForegroundColor = Green;
         WriteLine ($"You found the word in {sInputs.Count / LEN} tries");
         ResetColor ();
      } else WriteLine ($"Sorry - the word was {sSeed}");
   }

   // Prints a message for an invalid word
   static void PrintMsg (string s) {
      GetPosition (out int left, out int top);
      int messageTop = Min (WindowHeight - 1, top + BOARDHEIGHT + 1);
      SetCursorPosition (left, messageTop);
      ForegroundColor = Yellow;
      WriteLine ($"{s} is not a word");
      ResetColor ();
      WriteLine ("Enter any key to continue");
      ReadKey (true);
      SetCursorPosition (left, messageTop);
      Write (new string (' ', 55));
   }
   #endregion

   #region Private data ---------------------------------------------
   static List<(char, EState)> sInputs = [];
   static bool sIsGameOver, sIsFound;
   static string sSeed = string.Empty, sWord = string.Empty;
   static string[] sValidWords = [];
   #endregion

   #region Constants ------------------------------------------------
   const int LEN = 5;
   const int MAXTRIES = 6;
   const int ROWSIZE = 7;
   const int BOARDWIDTH = ROWSIZE * 4;
   const int BOARDHEIGHT = MAXTRIES + 6;
   #endregion
}
#endregion

#region enum EState -------------------------------------------------------------------------------
enum EState { None, Exact, Misplaced, Absent }
#endregion
