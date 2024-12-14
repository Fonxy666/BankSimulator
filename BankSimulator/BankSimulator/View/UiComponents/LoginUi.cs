﻿using BankSimulator.Services;
using MongoDB.Bson.IO;
using Newtonsoft.Json;

namespace BankSimulator.View.UiComponents
{
    internal class LoginUi(Func<string[]> getNamesMethod, UserService userService)
    {
        public async Task<bool> Login()
        {
            int loginType = DecideHowToLogin();

            if (loginType == 1)
            {
                string bankCardNumber = GetCardNumber();
                int pinCode = GetPinCode();
                return await userService.LoginWithCard(bankCardNumber, pinCode);
            }
            else if (loginType == 2)
            {
                string[] names = getNamesMethod();
                int pinCode = GetPinCode();
                return await userService.LoginWithNames(names[0], names[1], pinCode);
            }

            return true;
        }

        public int DecideHowToLogin()
        {
            int loginType = 0;
            bool successfulParse = false;

            Console.Write("Choose how to log in.\nPress '1' to log in via card number\nPress '2' to log in via name.");
            while (!successfulParse)
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out loginType))
                {
                    if (loginType == 1 || loginType == 2)
                    {
                        successfulParse = true;
                    }
                    else
                    {
                        Console.WriteLine("Wrong input, please use digits.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value.");
                }
            }

            return loginType;
        }

        private string GetCardNumber()
        {
            string input = "";
            bool successfulParse = false;

            Console.Write("Card number:");
            while (!successfulParse)
            {
                input = Console.ReadLine();

                if (input.Length == 19)
                {
                    successfulParse = true;
                }
                else
                {
                    Console.WriteLine("Wrong input, please use 4x4 digits with (-)s.");
                }
            }
            return input;
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

        public void SaveUserSession(string userId)
        {
            string SessionFile = "userSession.json";

            var userSession = new UserSession { UserId = userId };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(userSession);
            File.WriteAllText(SessionFile, json);
            Console.WriteLine("User logged in and session saved.");
        }
    }

    public class UserSession
    {
        public string UserId { get; set; }
    }
}
