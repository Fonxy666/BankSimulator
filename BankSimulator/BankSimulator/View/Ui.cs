using System.Linq;
using BankSimulator.Services;
using BankSimulator.View.UiComponents;

namespace BankSimulator.View
{
    internal class Ui
    {
        private LoginUi loginUi;
        private RegisterUi registerUi;
        private UserService userService;
        private CardService cardService;
        private AddressService addressService;
        private bool appRunning = true;
        private Guid? userId = GetUserIdFromFile();

        public Ui(UserService userService, CardService cardService, AddressService addressService)
        {
            this.userService = userService;
            this.cardService = cardService;
            this.addressService = addressService;
            registerUi = new RegisterUi(new Func<string[]>(GetNames), userService, cardService, addressService);

            loginUi = new LoginUi(new Func<string[]>(GetNames), userService);
        }

        public static string[] GetNames()
        {
            Console.WriteLine("Please give us your full name, first your First Name, and use space(s) between Names.\nIf you have a middle name, simply use space between names.");
            string input = Console.ReadLine();
            string[] nameParts = input.Split(' ');

            string firstName = nameParts[0];
            string? middleName = nameParts.Length > 2 ? string.Join(" ", nameParts.Skip(1).Take(nameParts.Length - 2)) : null;
            string secondName = nameParts.Length > 1 ? nameParts[nameParts.Length - 1] : nameParts[0];
            return middleName == null ? [firstName, secondName] : [firstName, middleName, secondName];
        }

        public async Task Run()
        {
            while (appRunning)
            {
                if (!await EnsureLoggedInAsync())
                {
                    continue;
                }
                await ShowMainMenuAsync();
            }
        }

        private async Task<bool> EnsureLoggedInAsync()
        {
            if (userId == null)
            {
                bool loggedIn = await FirstMenuStep();
                if (!loggedIn)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task ShowMainMenuAsync()
        {
            while (true)
            {
                ShowMenu();
                int inputCode = HandleInput();

                switch (inputCode)
                {
                    case 1:
                        decimal? balance = await cardService.GetBalance(userId.ToString());
                        if (balance == null)
                        {
                            Console.WriteLine("There was an error getting the balance.");
                        }
                        Console.WriteLine($"Your balance is: {balance}");
                        break;
                    case 2:
                        cardService.AddToBalance(userId.ToString(), 20);
                        break;
                    case 6:
                        ClearUserSession();
                        return;
                    case 7:
                        ExitApp();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private int LoginOrRegister()
        {
            int parsedInput = 0;
            bool isValidInput = false;

            while (!isValidInput)
            {
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                string input = Console.ReadLine();

                if (int.TryParse(input, out parsedInput))
                {
                    if (parsedInput == 1 || parsedInput == 2 || parsedInput == 3)
                    {
                        isValidInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input, please use numbers from 1, 2 or 3.");
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
            Console.WriteLine("6. Logout");
            Console.WriteLine("7. Exit");
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
                    if (parsedInput >= 1 && parsedInput <= 7)
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

        private static Guid? GetUserIdFromFile()
        {
            string filePath = "UserId.txt";
            try
            {
                if (File.Exists(filePath))
                {
                    string fileContent = File.ReadAllText(filePath);
                    if (Guid.TryParse(fileContent, out Guid userId))
                    {
                        Console.WriteLine($"User ID {userId} loaded from {filePath}.");
                        return userId;
                    }
                    else
                    {
                        Console.WriteLine("Invalid user ID format in the file.");
                    }
                }
                else
                {
                    Console.WriteLine($"File {filePath} does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the user ID: {ex.Message}");
            }

            return null;
        }

        private void ClearUserSession()
        {
            File.Delete("UserId.txt");
            Console.WriteLine("You logged out.");
        }

        private void ExitApp()
        {
            ClearUserSession();
            Console.WriteLine("Exiting the application...");
            Environment.Exit(0);
        }

        private async Task<bool> FirstMenuStep()
        {
            bool loggedIn = false;
            int loginOrRegister = this.LoginOrRegister();

            if (loginOrRegister == 1)
            {
                await registerUi.Register();
                return loggedIn;
            }
            else if (loginOrRegister == 2)
            {
                bool loginSuccess = await loginUi.Login();
                if (loginSuccess)
                {
                    Console.WriteLine("Success");
                    loggedIn = true;
                    return loggedIn;
                }
                else
                {
                    Console.WriteLine("Login failed. Try again.");
                    return loggedIn;
                }
            }
            else if (loginOrRegister == 3)
            {
                ExitApp();
                appRunning = false;
            }
            else
            {
                Console.WriteLine("Invalid option. Please try again.");
            }

            return loggedIn;
        }
    }
}
