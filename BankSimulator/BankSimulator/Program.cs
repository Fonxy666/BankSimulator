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
            Ui ui = GetUi(userService);

            await ui.Run();
        }

        private static Ui GetUi(UserService userService)
        {
            return new Ui(userService);
        }

        private static UserService GetUserService(MongoDbConnection mongoDbConnection)
        {
            return new UserService(mongoDbConnection);
        }

        private static MongoDbConnection GetMongoDbConnection()
        {
            return new MongoDbConnection();
        }
    }
}
