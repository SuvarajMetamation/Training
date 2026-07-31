// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A01: Number Guessing Game.
// --------------------------------------------------------------------------------------------

namespace A01;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      Console.WriteLine ($"Guess the number between {MINVALUE} and {MAXVALUE}...!");
      int secretNumber = new Random ().Next (MINVALUE, MAXVALUE + 1);
      for (int attempts = 1; attempts <= MAXATTEMPTS; attempts++) {
         int guess = ReadGuess ();
         if (guess == secretNumber) {
            WriteColoredMessage ("You guessed correctly!", ConsoleColor.Green);
            return;
         }
         WriteColoredMessage (guess > secretNumber ? "Your guess is too high" :
            "Your guess is too low", YELLOW);
         WriteColoredMessage ($"Remaining guesses: {MAXATTEMPTS - attempts}", CYAN);
      }
      WriteColoredMessage (
         $"You used all {MAXATTEMPTS} guesses. The correct number was {secretNumber}.",
         ConsoleColor.DarkGreen);
   }

   // Reads and validates the user's guess.
   static int ReadGuess () {
      while (true) {
         Console.Write ("Enter your guess: ");
         if (int.TryParse (Console.ReadLine (), out int guess) && guess >= MINVALUE &&
            guess <= MAXVALUE) return guess;
         WriteColoredMessage ($"Please enter a number between {MINVALUE} and {MAXVALUE}.", RED);
      }
   }

   // Output decorators for more user understandable.
   static void WriteColoredMessage (string message, ConsoleColor color) {
      Console.ForegroundColor = color;
      Console.WriteLine (message);
      Console.ResetColor ();
   }
   #endregion

   #region const ----------------------------------------------------
   const int MINVALUE = 1; // Lowest possible number
   const int MAXVALUE = 100; // Highest possible number
   const int MAXATTEMPTS = 7; // Maximum guesses allowed
   const ConsoleColor YELLOW = ConsoleColor.Yellow; // Too high/too low hints
   const ConsoleColor CYAN = ConsoleColor.Cyan; // Remaining guess
   const ConsoleColor RED = ConsoleColor.Red; // Invalid Input
   #endregion
}
#endregion