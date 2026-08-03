// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A01.1: Number Guessing Game(Reverse).
// The computer uses the Binary Search algorithm to guess the number.
// chosen by the user within the specified range.
// --------------------------------------------------------------------------------------------

namespace A01._1;

using static Console;
using static System.ConsoleColor;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      int low = MINVALUE, high = MAXVALUE;
      Display ($"Think of a number between {MINVALUE} and {MAXVALUE}, " +
         $"and I'll guess it!", EOutputType.Info);
      while (low <= high) {
         int mid = low + (high - low) / 2;
         EResponse response = ReadResponse (mid);
         if (response == EResponse.Correct) {
            Display ($"\nI found it! Your number is {mid}.", EOutputType.Success);
            return;
         }
         HandleResponse (response, mid, ref low, ref high);
         Display ($"\nPossible range: {low} to {high}", EOutputType.Hint);
      }
      Display ("\nYour responses are inconsistent. Please restart the game.",
         EOutputType.Error);
   }

   // Reads and validates the user's response.
   static EResponse ReadResponse (int guess) {
      while (true) {
         Display ($"Is your number {guess}? (H)igh, (L)ow, or (C)orrect: ",
            EOutputType.Prompt, false);
         switch (ReadKey ().Key) {
            case ConsoleKey.H: return EResponse.High;
            case ConsoleKey.L: return EResponse.Low;
            case ConsoleKey.C: return EResponse.Correct;
         }
         Display ("\nInvalid input. Press H, L or C.", EOutputType.Error);
      }
   }

   // Processes the user's response and updates the search range.
   static void HandleResponse (EResponse response, int mid, ref int low, ref int high) {
      if (response == EResponse.High) high = mid - 1;
      else low = mid + 1;
   }

   // Output decorators for more user understandable.
   static void Display (string str, EOutputType outType,
      bool newLine = true) {
      ForegroundColor = outType switch {
         EOutputType.Success => Green,
         EOutputType.Error => Red,
         EOutputType.Hint => Cyan,
         EOutputType.Prompt => Yellow,
         EOutputType.Info => White,
      };
      if (newLine) WriteLine (str);
      else Write (str);
      ResetColor ();
   }
   #endregion

   // Represents the user's response to the computer's guess.
   #region Enums ----------------------------------------------------
   enum EResponse {
      High, // The guessed number is too high.
      Low, // The guessed number is too low.
      Correct // The guessed number is correct.
   }

   // Represents the type of message displayed to the user.
   enum EOutputType {
      Success,  // Indicates a successful operation.
      Error, // Indicates an error or invalid input.
      Hint,  // Displays informational hints.
      Prompt, // Prompts the user for input.
      Info  // Displays general information.
   }
   #endregion

   #region const ----------------------------------------------------
   const int MINVALUE = 0; // Lowest possible number
   const int MAXVALUE = 100; // Highest possible number
   #endregion
}
#endregion