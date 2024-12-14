using BankSimulator.Database;
using BankSimulator.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BankSimulator.Services
{
    internal class AddressService(MongoDbConnection mongoDbConnection)
    {
        public async Task<bool> SaveUserAddress(Address address)
        {
            try
            {
                IMongoCollection<BsonDocument> cardTable = await mongoDbConnection.GetAdressesTable();

                BsonDocument newCard = new BsonDocument
                {
                    { "_id", address.UserId.ToString() },
                    { "zipCode", address.ZipCode },
                    { "country", address.Country },
                    { "city", address.City },
                    { "street", address.Street },
                    { "houseNumber", address.HouseNumber }
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
