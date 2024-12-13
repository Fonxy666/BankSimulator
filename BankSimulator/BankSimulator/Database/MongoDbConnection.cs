using MongoDB.Bson;
using MongoDB.Driver;
using dotenv.net;
using BankSimulator.Models;

namespace BankSimulator.Database
{
    internal class MongoDbConnection
    {
        public async Task<bool> SaveNewUser(User user)
        {
            try
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
                    { "Pin", user.HashedPin },
                    { "Balance", user.Balance },
                    {
                        "Address", new BsonDocument
                        {
                            { "Zipcode", user.Address.ZipCode },
                            { "Country", user.Address.Country },
                            { "City", user.Address.City },
                            { "Street", user.Address.Street },
                            { "House number", user.Address.HouseNumber }
                        }
                    }
                };

                await collection.InsertOneAsync(newUser);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting new user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Login(string firstName, string lastName)
        {
            try
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
                    return true;
                }
                else
                {
                    Console.WriteLine("User not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user: {ex.Message}");
                return false;
            }
        }
    }
}
