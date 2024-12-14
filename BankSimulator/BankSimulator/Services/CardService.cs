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
    }
}
