
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
using static System.StringSplitOptions;

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
      DisplayHeader ();
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
      return reader.ReadToEnd ().Split (['\r', '\n'], RemoveEmptyEntries | TrimEntries);
   }

   // Displays the game board
   static void DisplayBoard () {
      GetPosition (out _, out int top);
      int boardLeft = (WindowWidth - 19) / 2;
      int evaluatedCount = sInputs.Count - sWord.Length;
      for (int row = 0; row < MAXTRIES; row++) {
         for (int col = 0; col < LEN; col++) {
            int index = row * LEN + col;
            char ch = index < sInputs.Count ? sInputs[index].Item1 : ' ';
            SetCursorPosition (boardLeft + col * 4, top + row);
            BackgroundColor = index < evaluatedCount
               ? sInputs[index].Item2 switch {
                  Exact => Green,
                  Misplaced => DarkYellow,
                  Absent => DarkGray,
                  _ => Black
               } : index < sInputs.Count ? DarkGray : Black;
            ForegroundColor = White;
            Write ($" {ch} ");
            ResetColor ();
         }
      }
   }

   // Displays the alphabet with the evaluated letter colors
   static void DisplayAlphabets () {
      GetPosition (out int left, out int top);
      int evaluatedCount = sInputs.Count - sWord.Length;
      var evaluated = sInputs.Take (evaluatedCount);
      SetCursorPosition ((WindowWidth - BOARDWIDTH) / 2, top + MAXTRIES + 1);
      WriteLine (new string ('\u2500', BOARDWIDTH));
      SetCursorPosition (left, top + MAXTRIES + 2);
      int index = 0;
      foreach (char ch in "ABCDEFGHIJKLMNOPQRSTUVWXYZ") {
         BackgroundColor = evaluated.Any (x => x.Item1 == ch && x.Item2 == Exact) ? Green :
                           evaluated.Any (x => x.Item1 == ch && x.Item2 == Misplaced) ? DarkYellow :
                           evaluated.Any (x => x.Item1 == ch && x.Item2 == Absent) ? DarkGray :
                           DarkGray;
         ForegroundColor = White;
         Write ($" {ch} ");
         ResetColor ();
         Write (" ");
         if (++index % ROWSIZE == 0) SetCursorPosition (left, top + MAXTRIES + 2 + index / ROWSIZE);
      }
   }

   // Displays the game title and instructions
   static void DisplayHeader () {
      GetPosition (out int left, out int top);
      SetCursorPosition (left, top - 4);
      ForegroundColor = Green;
      WriteCentered ("W O R D L E");
      ResetColor ();
      SetCursorPosition (left, top - 3);
      WriteCentered ("Guess the five-letter word");
      SetCursorPosition (left, top - 2);
      WriteCentered ("You have six attempts to find it");
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
      SetCursorPosition (left, top + BOARDHEIGHT + 1);
      if (sIsFound) {
         ForegroundColor = Green;
         WriteCentered ("Congratulations! You found the word!");
         SetCursorPosition (left, CursorTop + 1);
         WriteCentered ($"Attempts used: {sInputs.Count / LEN}/{MAXTRIES}");
      } else {
         ForegroundColor = Yellow;
         WriteCentered ("Game Over!");
         SetCursorPosition (left, CursorTop + 1);
         WriteCentered ($"The correct word was: {sSeed}");
      }
      ResetColor ();
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

   #region Helper functions -----------------------------------------
   // Writes text at the horizontal centre of the console
   static void WriteCentered (string text) {
      SetCursorPosition (Max (0, (WindowWidth - text.Length) / 2), CursorTop);
      Write (text);
   }

   // Gets the starting position to centre the game in the console
   static void GetPosition (out int left, out int top) {
      left = Max (0, (WindowWidth - BOARDWIDTH) / 2);
      top = Max (0, (WindowHeight - BOARDHEIGHT) / 2);
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
