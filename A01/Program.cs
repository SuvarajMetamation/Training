namespace A01;

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        Console.WriteLine("Think of a number between 1 and 100.");
        int secretNumber = random.Next(1, 101), guess;
        Console.WriteLine("Guess the number between 1 and 100...!");
        do
        {
            Console.Write("Enter your guess: ");
            if (!int.TryParse(Console.ReadLine(), out guess))
            {
                Console.WriteLine("Please enter a valid number.");
                continue;
            }
            if (guess > secretNumber)
            {
                Console.WriteLine("Your guess is too high");
            }
            else if (guess < secretNumber)
            {
                Console.WriteLine("Your guess is too low");
            }
            else
            {
                Console.WriteLine("You guessed correctly");
            }
        } while (guess != secretNumber);

    }
}
