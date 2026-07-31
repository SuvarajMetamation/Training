// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A01: Number Guessing Game(Reverse).
// --------------------------------------------------------------------------------------------

namespace A01._1;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      int low = MINVALUE, high = MAXVALUE;
      Console.WriteLine ($"Think of a number between {MINVALUE} and {MAXVALUE}.");
      for (int attempts = 1; attempts <= MAXATTEMPTS; attempts++) {
         int mid = (low + high) / 2;
         Console.WriteLine ($"Is your number {mid}? (high/low/correct): ");
         string response = Console.ReadLine ()?.Trim ().ToLower () ?? string.Empty;
         switch (response) {
            case "correct": WriteColoredMessage ("Guessed successfully!", GREEN); return;
            case "high": high = mid - 1; break;
            case "low": low = mid + 1; break;
            default:
               WriteColoredMessage
                  ("Invalid response. Please enter 'high', 'low', or 'correct'.", RED);
               attempts--;
               continue;
         }
         if (low > high) {
            WriteColoredMessage ("Inconsistent responses. Please start over.", RED);
            return;
         }
         WriteColoredMessage ($"Current range: {low} - {high}", CYAN);
      }
      WriteColoredMessage ("Maximum attempts reached. Unable to determine the number.", RED);
   }

   // Output decorators for more user understandable.
   static void WriteColoredMessage (string message, ConsoleColor color) {
      Console.ForegroundColor = color;
      Console.WriteLine (message);
      Console.ResetColor ();
   }
   #endregion

   #region const ----------------------------------------------------
   const int MINVALUE = 0; // Lowest possible number
   const int MAXVALUE = 100; // Highest possible number 
   const int MAXATTEMPTS = 7; // Maximum number of attempts
   const ConsoleColor GREEN = ConsoleColor.Green; // Successful guess
   const ConsoleColor RED = ConsoleColor.Red; // Invalid response
   const ConsoleColor CYAN = ConsoleColor.Cyan; // Hint
   #endregion
}
#endregion