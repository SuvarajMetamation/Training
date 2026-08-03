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
      WriteLine ($"Think of a number between {MINVALUE} and {MAXVALUE}, " +
         $"and I'll guess it!");
      while (low <= high) {
         int mid = low + (high - low) / 2;
         EResponse response = ReadResponse (mid);
         if (response == EResponse.Correct) {
            WriteColoredMessage ($"\nI found it! Your number is {mid}.", EMessageType.Success);
            return;
         }
         HandleResponse (response, mid, ref low, ref high);
         WriteColoredMessage ($"\nPossible range: {low} to {high}", EMessageType.Hint);
      }
      WriteColoredMessage ("\nYour responses are inconsistent. Please restart the game.",
         EMessageType.Error);
   }

   // Reads and validates the user's response.
   static EResponse ReadResponse (int guess) {
      while (true) {
         WriteColoredMessage ($"Is your number {guess}? (H)igh, (L)ow, or (C)orrect: ",
            EMessageType.Prompt, false);
         switch (ReadKey ().Key) {
            case ConsoleKey.H: return EResponse.High;
            case ConsoleKey.L: return EResponse.Low;
            case ConsoleKey.C: return EResponse.Correct;
         }
         WriteColoredMessage ("\nInvalid input. Press H, L or C.", EMessageType.Error);
      }
   }

   // Processes the user's response and updates the search range.
   static void HandleResponse (EResponse response, int mid, ref int low, ref int high) {
      if (response == EResponse.High) high = mid - 1;
      else low = mid + 1;
   }

   // Output decorators for more user understandable.
   static void WriteColoredMessage (string message, EMessageType messageType,
      bool newLine = true) {
      ForegroundColor = messageType switch {
         EMessageType.Success => Green,
         EMessageType.Error => Red,
         EMessageType.Hint => Cyan,
         EMessageType.Prompt => Yellow,
         // This is an invalid enum value and should never happen.
         _ => throw new ArgumentOutOfRangeException (nameof (messageType)),
      };
      if (newLine) WriteLine (message);
      else Write (message);
      ResetColor ();
   }
   #endregion

   // Represents the user's response to the computer's guess.
   #region enum -----------------------------------------------------
   enum EResponse {
      High, // The guessed number is too high.
      Low, // The guessed number is too low.
      Correct // The guessed number is correct.
   }

   // Represents the type of message displayed to the user.
   enum EMessageType {
      Success,
      Error,
      Hint,
      Prompt
   }
   #endregion

   #region const ----------------------------------------------------
   const int MINVALUE = 0; // Lowest possible number
   const int MAXVALUE = 100; // Highest possible number
   #endregion
}
#endregion