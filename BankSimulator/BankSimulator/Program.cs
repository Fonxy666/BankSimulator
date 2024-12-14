using BankSimulator.Database;
using BankSimulator.Services;
using BankSimulator.View;

namespace BankSimulator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            MongoDbConnection connection = GetMongoDbConnection();
            UserService userService = GetUserService(connection);
            AddressService addressService = GetAddressService(connection);
            CardService cardService = GetCardService(connection);
            Ui ui = GetUi(userService, addressService, cardService);

            await ui.Run();
        }

        private static Ui GetUi(UserService userService, AddressService addressService, CardService cardService)
        {
            return new Ui(userService, cardService, addressService);
        }

        private static UserService GetUserService(MongoDbConnection mongoDbConnection)
        {
            return new UserService(mongoDbConnection);
        }

        private static AddressService GetAddressService(MongoDbConnection mongoDbConnection)
        {
            return new AddressService(mongoDbConnection);
        }

        private static CardService GetCardService(MongoDbConnection mongoDbConnection)
        {
            return new CardService(mongoDbConnection);
        }

        private static MongoDbConnection GetMongoDbConnection()
        {
            return new MongoDbConnection();
        }
    }
}
