// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// Number Guessing Game.
// --------------------------------------------------------------------------------------------

namespace A01;

class Program {
#pragma warning disable IDE0060 // Remove unused parameter
   static void Main (string[] args) {
      const int MINVALUE = 1; // Lowest possible number
      const int MAXVALUE = 100; // Highest possible number
      const int MAX = 7; // Maximum guesses allowed
      Console.WriteLine ($"Guess the number between {MINVALUE} and {MAXVALUE}...!");
      int secretNumber = new Random ().Next (MINVALUE, MAXVALUE + 1);
      for (int attempts = 1; attempts <= MAX; attempts++) {
         int guess = ReadGuess ();
         if (guess == secretNumber) {
            Console.WriteLine ("You guessed correctly!");
            return;
         }
         Console.WriteLine (guess > secretNumber ? "Your guess is too high" : "Your guess is too low");
      }
      Console.WriteLine ($"You used all {MAX} guesses. The correct number was {secretNumber}.");
   }
   private static int ReadGuess () {
      while (true) {
         Console.Write ("Enter your guess: ");
         if (int.TryParse (Console.ReadLine (), out int guess)) return guess;
         Console.WriteLine ("Please enter a valid number.");
      }
   }
#pragma warning restore IDE0060 // Remove unused parameter

}
