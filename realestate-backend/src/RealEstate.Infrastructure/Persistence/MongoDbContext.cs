using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using RealEstate.Domain.Entities;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Persistence
{
    public class MongoDbContext(IConfiguration configuration)
    {
        private readonly IMongoDatabase _database = InitializeDatabase(configuration);

        private static IMongoDatabase InitializeDatabase(IConfiguration configuration)
        {
            var connectionString = configuration["MongoSettings:ConnectionString"];
            var databaseName = configuration["MongoSettings:DatabaseName"];

            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }

        public IMongoCollection<Owner> Owners => _database.GetCollection<Owner>("Owners");
        public IMongoCollection<Property> Properties => _database.GetCollection<Property>("Properties");
        public IMongoCollection<PropertyImage> PropertyImages => _database.GetCollection<PropertyImage>("PropertyImages");
        public IMongoCollection<PropertyTrace> PropertyTraces => _database.GetCollection<PropertyTrace>("PropertyTraces");

        public async Task DropDatabaseAsync()
        {
            await _database.Client.DropDatabaseAsync(_database.DatabaseNamespace.DatabaseName);
        }
    }
}