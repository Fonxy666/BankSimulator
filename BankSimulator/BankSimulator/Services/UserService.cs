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
                    { "_id", user.UserId.ToString() },
                    { "firstName", user.FirstName },
                    { "middleName", user.MiddleName != null ? user.MiddleName : BsonNull.Value },
                    { "lastName", user.LastName }
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

        public async Task<bool> LoginWithNames(string firstName, string lastName, int pinCode, string? middleName)
        {
            try
            {
                IMongoCollection<BsonDocument> userTable = await mongoDbConnection.GetUserTable();

                FilterDefinition<BsonDocument> filteredByFirstName = Builders<BsonDocument>.Filter.Eq("firstName", firstName);
                FilterDefinition<BsonDocument> filteredByLastName = Builders<BsonDocument>.Filter.Eq("lastName", lastName);

                var filters = new List<FilterDefinition<BsonDocument>> { filteredByFirstName, filteredByLastName };

                if (!string.IsNullOrEmpty(middleName))
                {
                    FilterDefinition<BsonDocument> filteredByMiddleName = Builders<BsonDocument>.Filter.Eq("middleName", middleName);
                    filters.Add(filteredByMiddleName);
                }

                FilterDefinition<BsonDocument> combinedFilter = Builders<BsonDocument>.Filter.And(filters);

                var user = await userTable.Find(combinedFilter).FirstOrDefaultAsync();

                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return false;
                }

                IMongoCollection<BsonDocument> cardTable = await mongoDbConnection.GetCardTable();

                FilterDefinition<BsonDocument> filterByUserId = Builders<BsonDocument>.Filter.Eq("userId", user["_id"]);
                var cardResult = await cardTable.Find(filterByUserId).FirstOrDefaultAsync();

                string storedHashedPin = cardResult["pin"].AsString;

                bool isPinValid = BCrypt.Net.BCrypt.Verify(pinCode.ToString(), storedHashedPin);

                if (!isPinValid)
                {
                    Console.WriteLine("User not found.");
                    return false;
                }

                SaveUserIdToFile(user["_id"].AsString);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LoginWithCard(string cardNumber, int pinCode)
        {
            try
            {
                IMongoCollection<BsonDocument> userTable = await mongoDbConnection.GetCardTable();

                FilterDefinition<BsonDocument> filterByCardNumber = Builders<BsonDocument>.Filter.Eq("cardNumber", cardNumber);

                var user = await userTable.Find(filterByCardNumber).FirstOrDefaultAsync();

                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return false;
                }

                string storedHashedPin = user["pin"].AsString;

                bool isPinValid = BCrypt.Net.BCrypt.Verify(pinCode.ToString(), storedHashedPin);

                if (!isPinValid)
                {
                    Console.WriteLine("User not found.");
                    return false;
                }

                SaveUserIdToFile(user["_id"].AsString);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding user: {ex.Message}");
                return false;
            }
        }

        private void SaveUserIdToFile(string userId)
        {
            string filePath = "UserId.txt";
            try
            {
                File.WriteAllText(filePath, userId);
                Console.WriteLine($"User ID saved to {filePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the user ID: {ex.Message}");
            }
        }
    }
}
