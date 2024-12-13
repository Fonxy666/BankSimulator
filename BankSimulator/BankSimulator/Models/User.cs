namespace BankSimulator.Models
{
    internal class User(string firstName, string? middleName, string lastName, Address address, string pin)
    {
        public string FirstName { get; init; } = firstName;
        public string? MiddleName { get; init; } = middleName;
        public string LastName { get; init; } = lastName;
        public Address Address { get; set; } = address;
        public Guid UserId { get; init; } = Guid.NewGuid();
        public string HashedPin { get; private set; } = pin;
        public decimal Balance { get; private set; } = 0;

        public void AddToBalance(decimal money)
        {
            Balance += money;
        }

        public void Debit(int money)
        {
            Balance -= money;
        }
    }
}
