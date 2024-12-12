using BankSimulator.Database;
using BankSimulator.FileSaverFolder;
using BankSimulator.View.UiComponents;

namespace BankSimulator.View
{
    internal class Ui
    {
        private static LoginUi loginUi;
        private RegisterUi registerUi;

        private readonly FileSaver fileSaver;

        public Ui(FileSaver fileSaver)
        {
            this.fileSaver = fileSaver;

            registerUi = new RegisterUi(new Func<string[]>(GetNames), this.fileSaver);

            loginUi = new LoginUi(new Func<string[]>(GetNames));
        }

        public static string[] GetNames()
        {
            Console.WriteLine("Please give us your full name, first your First Name, and use space(s) between Names. If you have a middle name, simply use space between names.");
            string input = Console.ReadLine();
            string[] nameParts = input.Split(' ');

            string firstName = nameParts[0];
            string? middleName = nameParts.Length > 2 ? nameParts[1] : null;
            string secondName = nameParts.Length > 1 ? nameParts[nameParts.Length - 1] : nameParts[0];
            return middleName == null ? [firstName, secondName] : [firstName, middleName, secondName];
        }

        public void Run()
        {
            int loginOrRegister = this.LoginOrRegister();
            if (loginOrRegister == 1)
            {
                registerUi.Register();
            }
            else if (loginOrRegister == 2)
            {
                MongoDbConnection connection = new MongoDbConnection();
                connection.Login("Viktor", "Poszt");
            }
            this.ShowMenu();
            int inputCode = this.HandleInput();
        }

        private int LoginOrRegister()
        {
            int parsedInput = 0;
            bool isValidInput = false;

            while (!isValidInput)
            {
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                string input = Console.ReadLine();

                if (int.TryParse(input, out parsedInput))
                {
                    if (parsedInput == 1 || parsedInput == 2)
                    {
                        isValidInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input, please use numbers from 1 or 2.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value.");
                }
            }
            return parsedInput;
        }

        private void ShowMenu()
        {
            Console.WriteLine("1. Check Balance");
            Console.WriteLine("2. Deposit Money");
            Console.WriteLine("3. Withdraw Money");
            Console.WriteLine("4. Change Pin");
            Console.WriteLine("5. Check transaction history");
            Console.WriteLine("6. Exit");
        }

        private int HandleInput()
        {
            int parsedInput = 0;
            bool isValidInput = false;

            while (!isValidInput)
            {
                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out parsedInput))
                {
                    if (parsedInput >= 1 && parsedInput <= 6)
                    {
                        isValidInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input, please use numbers from 1 to 6.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value.");
                }
            }

            Console.WriteLine($"You entered a valid number: {parsedInput}");
            return parsedInput;
        }
    }
}
