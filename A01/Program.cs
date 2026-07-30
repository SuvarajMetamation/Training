// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// Number Guessing Game.
// --------------------------------------------------------------------------------------------

namespace A01;

class Program {
   const int MINVALUE = 1; // Lowest possible number
   const int MAXVALUE = 100; // Highest possible number
   const int MAXATTEMPTS = 7; // Maximum guesses allowed
   static void Main () {
      Console.WriteLine ($"Guess the number between {MINVALUE} and {MAXVALUE}...!");
      int secretNumber = new Random ().Next (MINVALUE, MAXVALUE + 1);
      for (int attempts = 1; attempts <= MAXATTEMPTS; attempts++) {
         int guess = ReadGuess ();
         if (guess == secretNumber) {
            Console.WriteLine ("You guessed correctly!");
            return;
         }
         Console.WriteLine (guess > secretNumber ? "Your guess is too high" : "Your guess is too low");
      }
      Console.WriteLine ($"You used all {MAXATTEMPTS} guesses. The correct number was {secretNumber}.");
   }

   // Reads and validates the user's guess.
   static int ReadGuess () {
      while (true) {
         Console.Write ("Enter your guess: ");
         if (int.TryParse (Console.ReadLine (), out int guess) && guess >= MINVALUE && guess <= MAXVALUE) return guess;
         Console.WriteLine ($"Please enter a number between {MINVALUE} and {MAXVALUE}.");
      }
   }
}
