namespace A01;

class Program {
   static void Main (string[] args) {
      const int MINVALUE = 1;
      const int MAXVALUE = 100;
      const int MAX = 7;
      Console.WriteLine ($"Guess the number between {MINVALUE} and {MAXVALUE}...!");
      var (secretNumber, attempts) = ((new Random ().Next (MINVALUE, MAXVALUE + 1), (0)));
      while (attempts < MAX) {
         Console.Write ("Enter your guess: ");
         if (!int.TryParse (Console.ReadLine (), out int guess)) {
            Console.WriteLine ("Please enter a valid number.");
            continue;
         }
         attempts++;
         string message = guess switch {
            var x when x == secretNumber => "You guessed correctly!",
            var x when x > secretNumber => "Your guess is too high",
            _ => "Your guess is too low"
         };
         Console.WriteLine (message);
         if (guess == secretNumber)
            return;
      }
      Console.WriteLine ($"You used all {MAX} guesses. The correct number was {secretNumber}.");
   }
}
