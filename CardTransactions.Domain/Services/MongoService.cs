using CardTransactions.Domain.Abstractions;
using MongoDB.Driver;
using CardTransactions.Domain.Documents;
using Microsoft.Extensions.Configuration;
using CardTransactions.Domain.Models;

namespace CardTransactions.Domain.Services
{
    public class MongoService : IMongoService
    {
        private readonly IConfiguration _configuration;

        public MongoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void CreateIndex()
        {
            var collection = GetCollection<SalesDocument>();

            var createdDateIndex = Builders<SalesDocument>.IndexKeys.Ascending(w => w.CreatedUtc);
            var createdIndexModel = new CreateIndexModel<SalesDocument>(createdDateIndex);
            collection.Indexes.CreateOne(createdIndexModel);
        }

        public async Task Add(SalesDocument document)
        {
            var collection = GetCollection<SalesDocument>();
            collection.InsertOne(document);

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<SalesDocument>> ListAsync(SalesListModel request)
        {
            var collection = GetCollection<SalesDocument>();
            var filter = BuildFiltersWithRequest(request);
            var docs = await collection.Find(filter).ToListAsync();

            return docs;
        }

        private static FilterDefinition<SalesDocument> BuildFiltersWithRequest(SalesListModel request)
        {
            var filters = new List<FilterDefinition<SalesDocument>>() { Builders<SalesDocument>.Filter.Empty };
            if (request.StartDate.HasValue)
            {
                filters.Add(Builders<SalesDocument>.Filter.Gte(w => w.CreatedUtc, request.StartDate.Value));
            }

            if (request.EndDate.HasValue)
            {
                filters.Add(Builders<SalesDocument>.Filter.Lte(w => w.CreatedUtc, request.EndDate.Value));
            }

            if (request.AmountStart.HasValue)
            {
                filters.Add(Builders<SalesDocument>.Filter.Gte(w => w.Amount, request.AmountStart.Value));
            }

            if (request.AmountEnd.HasValue)
            {
                filters.Add(Builders<SalesDocument>.Filter.Lte(w => w.Amount, request.AmountEnd.Value));
            }

            if (!string.IsNullOrEmpty(request.Id))
            {
                filters.Add(Builders<SalesDocument>.Filter.Eq("Id", request.Id));
            }

            var filter = Builders<SalesDocument>.Filter.And(filters);
            return filter;
        }

        private IMongoCollection<SalesDocument> GetCollection<SalesDocument>()
            where SalesDocument : class, new()
        {
            var db = GetDb();
            var collection = db.GetCollection<SalesDocument>(typeof(SalesDocument).Name);
            return collection;
        }

        private IMongoDatabase GetDb()
        {
            var connectionString = _configuration.GetSection("MongoOptions").Get<MongoOptions>();
            if (connectionString == null)
            {
                throw new Exception(
                    "You must set your 'MONGODB_URI' environmental variable. See\n\t https://www.mongodb.com/docs/drivers/csharp/current/quick-start/#set-your-connection-string");
            }

            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString.ConnectionString));
            var timeOut = TimeSpan.FromSeconds(2);
            settings.ConnectTimeout = timeOut;
            settings.SocketTimeout = timeOut;
            settings.ServerSelectionTimeout = timeOut;

            var client = new MongoClient(settings);
            var db = client.GetDatabase(connectionString.DbName);

            return db;
        }
    }
}
