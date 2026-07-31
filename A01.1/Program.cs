// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A01: Number Guessing Game(Reverse).
// The computer uses the Binary Search algorithm to guess the number.
// chosen by the user within the specified range.
// --------------------------------------------------------------------------------------------

namespace A01._1;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      int low = MINVALUE, high = MAXVALUE;
      Console.WriteLine ($"Think of a number between {MINVALUE} and {MAXVALUE}, " +
         $"and I'll guess it!");
      while (low <= high) {
         int mid = CalculateMid (low, high);
         if (HandleResponse (ReadResponse (mid), mid, ref low, ref high)) return;
         WriteColoredMessage ($"Possible range: {low} to {high}", CYAN);
      }
      WriteColoredMessage ("Your responses are inconsistent. Please restart the game.", RED);
   }

   // Calculates the midpoint of the current search range.
   static int CalculateMid (int minimum, int maximum) {
      return minimum + (maximum - minimum) / 2;
   }

   // Reads and validates the user's response.
   static Response ReadResponse (int guess) {
      while (true) {
         WriteColoredMessage ($"Is your number {guess}? Press H (High), L (Low), or C (Correct): ",
            YELLOW, false);
         switch (Console.ReadKey (true).Key) {
            case ConsoleKey.H:
               Console.WriteLine ("H");
               return Response.High;
            case ConsoleKey.L:
               Console.WriteLine ("L");
               return Response.Low;
            case ConsoleKey.C:
               Console.WriteLine ("C");
               return Response.Correct;
            default:
               WriteColoredMessage (
                   "Invalid input. Press H, L or C.",
                   RED);
               break;
         }
      }
   }

   // Processes the user's response and updates the search range.
   static bool HandleResponse (Response response, int mid, ref int low, ref int high) {
      switch (response) {
         case Response.Correct:
            WriteColoredMessage ($"I found it! Your number is {mid}.", GREEN);
            return true;
         case Response.High:
            high = mid - 1;
            return false;
         case Response.Low:
            low = mid + 1;
            return false;
         default:
            throw new InvalidOperationException ("Unexpected response.");
      }
   }

   // Output decorators for more user understandable.
   static void WriteColoredMessage (string message, ConsoleColor color, bool newLine = true) {
      Console.ForegroundColor = color;
      if (newLine) Console.WriteLine (message);
      else Console.Write (message);
      Console.ResetColor ();
   }
   #endregion

   #region enum -----------------------------------------------------
   enum Response {
      High,
      Low,
      Correct
   }
   #endregion

   #region const ----------------------------------------------------
   const int MINVALUE = 0; // Lowest possible number
   const int MAXVALUE = 100; // Highest possible number
   const ConsoleColor GREEN = ConsoleColor.Green; // Successful guess
   const ConsoleColor RED = ConsoleColor.Red; // Invalid response
   const ConsoleColor CYAN = ConsoleColor.Cyan; // Hint
   const ConsoleColor YELLOW = ConsoleColor.Yellow;
   #endregion
}
#endregion