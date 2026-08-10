// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A02: Number Guessing using LSB-to-MSB Binary Reconstruction.
// The computer guesses the user's number by determining its binary digits
// from Right-to-Left (LSB -> MSB) using remainder-based questions.
// --------------------------------------------------------------------------------------------

namespace A02;

using static Console;
using static System.ConsoleColor;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      Display ($"Think of a number between {MINVALUE} and {MAXVALUE}.");
      Display ("Answer each question with (Y)es or (N)o.\n");
      int guess = GuessNumber ();
      Display ($"\nYour number is {guess}.", EOutputType.Success);
   }

   // Guesses the user's number using remainder-based binary questions.
   static int GuessNumber () {
      int guess = MINVALUE;
      for (int bit = 0; bit < MAXBITS; bit++) {
         int value = BITVALUE << bit, divisor = value << 1;
         if (ReadResponse (divisor, guess + value))
            guess |= value;
      }
      return guess;
   }

   // Reads and validates the user's Yes/No response.
   static bool ReadResponse (int divisor, int remainder) {
      while (true) {
         Display (
            $"\nWhen your number is divided by {divisor}, is the remainder {remainder}? (Y/N): ",
            EOutputType.Prompt, false);
         switch (ReadKey ().Key) {
            case ConsoleKey.Y: return true;
            case ConsoleKey.N: return false;
            default:
               Display ("\nInvalid input. Press Y or N.", EOutputType.Error);
               break;
         }
      }
   }

   // Displays messages with colours.
   static void Display (string message, EOutputType outType = EOutputType.Info,
      bool newLine = true) {
      if (outType != EOutputType.Info)
         ForegroundColor = outType switch {
            EOutputType.Success => Green,
            EOutputType.Error => Red,
            EOutputType.Hint => Cyan,
            _ => Yellow,
         };
      if (newLine) WriteLine (message);
      else Write (message);
      ResetColor ();
   }
   #endregion

   #region Enums ----------------------------------------------------
   // Represents the type of output displayed.
   enum EOutputType {
      Success, // Successful operation.
      Error,   // Error message.
      Hint,    // Intermediate information.
      Prompt,  // User input prompt.
      Info     // General information.
   }
   #endregion

   #region const ----------------------------------------------------
   const int MINVALUE = 0;     // Lowest possible number
   const int MAXVALUE = 100;   // Highest possible number
   const int MAXBITS = 7;      // 2^7 = 128 > 100.
   const int BITVALUE = 1;     // Base value used for bit operations.

   #endregion
}
# endregion
