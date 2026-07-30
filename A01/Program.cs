namespace A01;

class Program {
    static void Main (string[] args) {
        Random random = new Random ();
        Console.WriteLine ("Think of a number between 1 and 100.");
        var (secretNumber, attempts) = ((random.Next (1, 101), (0)));
        const int maxAttempts = 7;
        Console.WriteLine ("Guess the number between 1 and 100...!");
        while (attempts < maxAttempts) {
            Console.Write ("Enter your guess: ");
            int guess = int.Parse (Console.ReadLine ());
            attempts++;
            if (guess == secretNumber) {
                Console.WriteLine ("You guessed correctly!");
                return;
            } else if (guess > secretNumber) {
                Console.WriteLine ("Your guess is too high");
            } else {
                Console.WriteLine ("Your guess is too low");
            }
        }
        Console.WriteLine ($"You used all {maxAttempts} guesses. The correct number was {secretNumber}.");
    }
}
