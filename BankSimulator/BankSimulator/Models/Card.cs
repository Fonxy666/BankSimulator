namespace BankSimulator.Models
{
    internal class Card(Guid userId, string hashedPin)
    {
        public Guid CardId { get; init; } = Guid.NewGuid();
        public Guid UserId { get; init; } = userId;
        public string CardNumber { get; init; } = CreateCardNumber();
        public decimal Balance { get; private set; } = 0;
        public string HashedPin { get; private set; } = hashedPin;
        private static string CreateCardNumber()
        {
            Random random = new Random();
            string cardNumber = "";

            for (int i = 0; i < 16; i++)
            {
                cardNumber += random.Next(0, 10).ToString();

                if ((i + 1) % 4 == 0 && i != 15)
                {
                    cardNumber += "-";
                }
            }

            return cardNumber;
        }

        public void Deposit(decimal money)
        {
            Balance += money;
        }

        public void Debit(decimal money)
        {
            Balance -= money;
        }
    }
}
