using MongoDB.Bson;
using MongoDB.Driver;
using dotenv.net;
using BankSimulator.Models;

namespace BankSimulator.Database
{
    internal class MongoDbConnection
    {
        public async void SaveNewUser(User user)
        {
            DotEnv.Load();
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            MongoClient client = new MongoClient(connectionString);
            var database = client.GetDatabase("Users");
            var collection = database.GetCollection<BsonDocument>("Users");

            var newUser = new BsonDocument
            {
                { "User id", user.UserId.ToString() },
                { "First name", user.FirstName },
                { "Middle name", user.MiddleName != null ? user.MiddleName : BsonNull.Value },
                { "Last name", user.LastName },
                { "Pin", user.PIN },
                { "Balance", user.Balance },
                {
                    "Address", new BsonDocument
                    {
                        { "Zipcode", user.Address.ZipCode },
                        {"Country", user.Address.Country },
                        { "City", user.Address.City },
                        { "Street", user.Address.Street },
                        { "House number", user.Address.HouseNumber }
                    }
                }
            };

            await collection.InsertOneAsync(newUser);
            Console.WriteLine("Document inserted.");
        }

        public async void Login(string firstName, string lastName)
        {
            DotEnv.Load();
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            MongoClient client = new MongoClient(connectionString);
            var database = client!.GetDatabase("Users");
            var collection = database.GetCollection<BsonDocument>("Users");

            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("First name", firstName),
                Builders<BsonDocument>.Filter.Eq("Last name", lastName)
            );
            var user = await collection.Find(filter).FirstOrDefaultAsync();

            if (user != null)
            {
                Console.WriteLine($"User found");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
    }
}
