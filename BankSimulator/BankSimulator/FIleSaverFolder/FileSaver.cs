using BankSimulator.Models;

namespace BankSimulator.FileSaverFolder
{
    internal class FileSaver
    {
        public void SaveRegistration(User user)
        {
            string userData = user.ToString();

            string filePath = $"{user.UserId}-{user.FirstName}-{user.LastName}.txt";

            File.WriteAllText(filePath, userData);

            Console.WriteLine("Successful registration.");
        }
    }
}
