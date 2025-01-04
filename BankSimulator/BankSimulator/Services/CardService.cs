using BankSimulator.Database;
using BankSimulator.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BankSimulator.Services
{
    internal class CardService(MongoDbConnection mongoDbConnection)
    {
        public async Task<bool> SaveUserCard(Card card)
        {
            try
            {
                IMongoCollection<BsonDocument> cardTable = await mongoDbConnection.GetCardTable();

                BsonDocument newCard = new BsonDocument
                {
                    { "_id", card.CardId.ToString() },
                    { "userId", card.UserId.ToString() },
                    { "cardNumber", card.CardNumber },
                    { "balance", card.Balance },
                    { "pin", card.HashedPin }
                };

                await cardTable.InsertOneAsync(newCard);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting new card: {ex.Message}");
                return false;
            }
        }

        public async Task<decimal?> GetBalance(string userId)
        {
            try
            {
                IMongoCollection<BsonDocument> cardTable = await mongoDbConnection.GetCardTable();

                FilterDefinition<BsonDocument> filterByUserId = Builders<BsonDocument>.Filter.Eq("userId", userId);

                var card = await cardTable.Find(filterByUserId).FirstOrDefaultAsync();

                if (card == null)
                {
                    Console.WriteLine("Card not found.");
                    return null;
                }

                decimal balance = card["balance"].AsDecimal;
                return balance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking balance: {ex.Message}");
                return null;
            }
        }

        public async Task AddToBalance(string userId, decimal amount)
        {
            try
            {
                IMongoCollection<BsonDocument> cardTable = await mongoDbConnection.GetCardTable();

                FilterDefinition<BsonDocument> filterByUserId = Builders<BsonDocument>.Filter.Eq("userId", userId);

                var card = await cardTable.Find(filterByUserId).FirstOrDefaultAsync();

                if (card == null)
                {
                    Console.WriteLine("User not found.");
                    return;
                }

                decimal balance = card["balance"].AsDecimal;
                balance += amount;

                var update = Builders<BsonDocument>.Update.Set("balance", balance);
                await cardTable.UpdateOneAsync(filterByUserId, update);

                Console.WriteLine("Balance updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating balance: {ex.Message}");
            }
        }
    }
}
