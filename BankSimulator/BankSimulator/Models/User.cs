namespace BankSimulator.Models
{
    internal class User(string firstName, string? middleName, string lastName)
    {
        public string FirstName { get; init; } = firstName;
        public string? MiddleName { get; init; } = middleName;
        public string LastName { get; init; } = lastName;
        public Guid UserId { get; init; } = Guid.NewGuid();
        public Guid CardId { get; private set; } = Guid.Empty;

        public void AddCardId(Guid cardId)
        {
            CardId = cardId;
        }
    }
}
