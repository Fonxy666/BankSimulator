using MongoDB.Bson;
using MongoDB.Driver;
using dotenv.net;

namespace BankSimulator.Database
{
    internal class MongoDbConnection
    {
        public async Task<IMongoCollection<BsonDocument>> GetUserTable()
        {
            DotEnv.Load();
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("BankSimulator");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Users");

            return collection;
        }

        public async Task<IMongoCollection<BsonDocument>> GetCardTable()
        {
            DotEnv.Load();
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("BankSimulator");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Cards");

            return collection;
        }

        public async Task<IMongoCollection<BsonDocument>> GetAdressesTable()
        {
            DotEnv.Load();
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("BankSimulator");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Addresses");

            return collection;
        }
    }
}
