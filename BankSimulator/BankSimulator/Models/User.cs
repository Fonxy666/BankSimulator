namespace BankSimulator.Models
{
    internal class User(string firstName, string? middleName, string secondName, Address address, int pin)
    {
        public string FirstName { get; init; } = firstName;
        public string? MiddleName { get; init; } = middleName;
        public string SecondName { get; init; } = secondName;
        public Address Address { get; set; } = address;
        public Guid UserId { get; init; } = Guid.NewGuid();
        public int PIN { get; private set; } = pin;
        public decimal Balance { get; private set; } = 0;

        public bool ChangePin(int pin)
        {
            bool correctPin = this.ExaminePin(pin);

            if (!correctPin)
            {
                return false;
            }

            PIN = pin;
            return true;
        }

        public bool ExaminePin(int pin)
        {
            return PIN == pin;
        }

        public void AddToBalance(decimal money)
        {
            Balance += money;
        }

        public void Debit(int money)
        {
            Balance -= money;
        }

        public override string ToString()
        {
            return $"User ID: {UserId}\n" +
                   $"First Name: {FirstName}\n" +
                   $"Middle Name: {MiddleName ?? "N/A"}\n" +
                   $"Second Name: {SecondName}\n" +
                   $"Address: {Address.ToString()}\n" +
                   $"PIN: {PIN}\n" +
                   $"Balance: {Balance}\n";
        }
    }
}
