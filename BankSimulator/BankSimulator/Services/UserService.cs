﻿using MongoDB.Bson;
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

        public async Task<bool> LoginWithNames(string firstName, string lastName, int pinCode)
        {
            try
            {
                IMongoCollection<BsonDocument> userTable = await mongoDbConnection.GetUserTable();

                FilterDefinition<BsonDocument> filteredByFirstName = Builders<BsonDocument>.Filter.Eq("firstName", firstName);
                FilterDefinition<BsonDocument> filteredByLastName = Builders<BsonDocument>.Filter.Eq("lastName", lastName);

                FilterDefinition<BsonDocument> combinedFilter = Builders<BsonDocument>.Filter.And(filteredByFirstName, filteredByLastName);

                var result = await userTable.Find(combinedFilter).FirstOrDefaultAsync();

                if (result == null)
                {
                    Console.WriteLine("User not found.");
                    return false;
                }

                IMongoCollection<BsonDocument> cardTable = await mongoDbConnection.GetCardTable();

                FilterDefinition<BsonDocument> filterByUserId = Builders<BsonDocument>.Filter.Eq("userId", result["_id"]);
                var cardResult = await cardTable.Find(filterByUserId).FirstOrDefaultAsync();

                string storedHashedPin = cardResult["pin"].AsString;

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

        public async Task<bool> LoginWithCard(string cardNumber, int pinCode)
        {
            try
            {
                IMongoCollection<BsonDocument> userTable = await mongoDbConnection.GetCardTable();

                FilterDefinition<BsonDocument> filterByCardNumber = Builders<BsonDocument>.Filter.Eq("cardNumber", cardNumber);

                var result = await userTable.Find(filterByCardNumber).FirstOrDefaultAsync();

                if (result == null)
                {
                    Console.WriteLine("User not found.");
                    return false;
                }

                string storedHashedPin = result["pin"].AsString;

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
