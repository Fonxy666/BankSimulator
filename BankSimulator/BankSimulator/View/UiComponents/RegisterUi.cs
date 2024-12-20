﻿using BankSimulator.Models;
using BankSimulator.Services;

namespace BankSimulator.View.UiComponents
{
    internal class RegisterUi(Func<string[]> getNamesMethod, UserService userService, CardService cardService, AddressService addressService)
    {
        public async Task<bool> Register()
        {
            string[] names = getNamesMethod();
            User newUser = new User(
                names[0],
                names.Length > 2 ? names[1] : null,
                names.Length > 2 ? names[2] : names[1]
                );
            bool userIsSaved = await userService.SaveNewUser(newUser);
            if (!userIsSaved)
            {
                Console.WriteLine("There was an error saving the new User to the database.");
                return false;
            }

            int pinCode = GetPinCode();
            string hashedPin = BCrypt.Net.BCrypt.HashPassword(pinCode.ToString());
            Card newCard = new Card(
                newUser.UserId,
                hashedPin
                );
            bool cardIsSaved = await cardService.SaveUserCard(newCard);
            if (!cardIsSaved)
            {
                Console.WriteLine("There was an error saving the new Card to the database.");
                return false;
            }

            Address address = GetAddress(newUser.UserId);
            bool addressIsSaved = await addressService.SaveUserAddress(address);
            if (!addressIsSaved)
            {
                Console.WriteLine("There was an error saving the new Address to the database.");
                return false;
            }

            Console.WriteLine("Succesful registration.");
            return true;
        }

        private Address GetAddress(Guid userId)
        {
            Console.WriteLine("Please give us your Address:");
            int zipCode = GetZipCode();
            string country = GetCountry();
            string city = GetCity();
            string street = GetStreet();
            int houseNumber = GetHouseNumber();
            Address address = new Address(userId, zipCode, country, city, street, houseNumber);
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
