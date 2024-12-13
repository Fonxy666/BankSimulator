using dotenv.net;
using MongoDB.Bson;
using MongoDB.Driver;
using BankSimulator.Models;
using BankSimulator.Database;

namespace BankSimulator.Services
{
    internal class UserService(MongoDbConnection mongoDbConnection)
    {
        public async Task<bool> SaveNewUser(User user)
        {
            try
            {
                IMongoCollection<BsonDocument> userTable = await mongoDbConnection.GetUserTable();

                BsonDocument newUser = new BsonDocument
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

                await userTable.InsertOneAsync(newUser);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting new user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Login(string firstName, string lastName, int pinCode)
        {
            try
            {
                IMongoCollection<BsonDocument> userTable = await mongoDbConnection.GetUserTable();

                FilterDefinition<BsonDocument> filteredByFirstName = Builders<BsonDocument>.Filter.Eq("First name", firstName);
                FilterDefinition<BsonDocument> filteredByLastName = Builders<BsonDocument>.Filter.Eq("Last name", lastName);

                FilterDefinition<BsonDocument> combinedFilter = Builders<BsonDocument>.Filter.And(filteredByFirstName, filteredByLastName);

                var result = await userTable.Find(combinedFilter).FirstOrDefaultAsync();

                if (result == null)
                {
                    Console.WriteLine("User not found.");
                    return false;
                }

                string storedHashedPin = result["Pin"].AsString;

                bool isPinValid = BCrypt.Net.BCrypt.Verify(pinCode.ToString(), storedHashedPin);

                if (!isPinValid)
                {
                    Console.WriteLine("User not found.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding user: {ex.Message}");
                return false;
            }
        }
    }
}
