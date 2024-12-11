using BankSimulator.FileSaverFolder;
using BankSimulator.Models;

namespace BankSimulator.View.UiComponents
{
    internal class RegisterUi(Func<string[]> getNamesMethod, FileSaver fileSaver)
    {
        public void Register()
        {
            string[] names = getNamesMethod();
            Address address = GetAddress();
            User newUser = new User(
                names[0],
                names.Length > 2 ? names[1] : null,
                names.Length > 2 ? names[2] : names[1],
                address,
                6666
                );
            fileSaver.SaveRegistration(newUser);
        }

        private Address GetAddress()
        {
            Console.WriteLine("Please give us your Address:");
            int zipCode = GetZipCode();
            string country = GetCountry();
            string city = GetCity();
            string street = GetStreet();
            int houseNumber = GetHouseNumber();
            Address address = new Address(zipCode, country, city, street, houseNumber);
            return address;
        }

        private int GetZipCode()
        {
            int zipCode = 0;
            bool parsedZipCode = false;

            Console.Write("ZipCode:");
            while (!parsedZipCode)
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out zipCode))
                {
                    if (input.Length == 4)
                    {
                        parsedZipCode = true;
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
            return zipCode;
        }

        private string GetCountry()
        {
            Console.Write("Country:");
            return Console.ReadLine();
        }

        private string GetCity()
        {
            Console.Write("City:");
            return Console.ReadLine();
        }

        private string GetStreet()
        {
            Console.Write("Street:");
            return Console.ReadLine();
        }

        private int GetHouseNumber()
        {
            int houseNumber = 0;
            bool parsedZipCode = false;

            Console.Write("House number:");

            while (!parsedZipCode)
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out houseNumber))
                {
                    parsedZipCode = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value.");
                }
            }
            return houseNumber;
        }
    }
}
