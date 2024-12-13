using BankSimulator.Services;

namespace BankSimulator.View.UiComponents
{
    internal class LoginUi(Func<string[]> getNamesMethod, UserService userService)
    {
        public async Task<bool> Login()
        {
            string[] names = getNamesMethod();
            int pinCode = GetPinCode();

            bool result = await userService.Login(names[0], names[1], pinCode);
            Console.WriteLine(result);
            return true;
        }

        private int GetPinCode()
        {
            int pinCode = 0;
            bool successfulParse = false;

            Console.Write("Pin code:");
            while (!successfulParse)
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out pinCode))
                {
                    if (input.Length == 4)
                    {
                        successfulParse = true;
                    }
                    else
                    {
                        Console.WriteLine("Wrong input, please use only 4 digits.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value.");
                }
            }
            return pinCode;
        }
    }
}
