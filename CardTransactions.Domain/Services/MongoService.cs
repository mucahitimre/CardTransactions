using CardTransactions.Domain.Abstractions;
using CardTransactions.Domain.Documents;
using CardTransactions.Domain.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace CardTransactions.Domain.Services
{
    /// <summary>
    /// The mongo service
    /// </summary>
    /// <seealso cref="CardTransactions.Domain.Abstractions.IMongoService" />
    public class MongoService : IMongoService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MongoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Creates the index.
        /// </summary>
        public async void CreateIndexAsync()
        {
            var collection = GetCollection<SalesDocument>();

            var createdDateIndex = Builders<SalesDocument>.IndexKeys.Ascending(w => w.Timestamp);
            var createdIndexModel = new CreateIndexModel<SalesDocument>(createdDateIndex);
            await collection.Indexes.CreateOneAsync(createdIndexModel);
        }

        /// <summary>
        /// Adds the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        public async Task Add(SalesDocument document)
        {
            var collection = GetCollection<SalesDocument>();
            await collection.InsertOneAsync(document);
        }

        /// <summary>
        /// Bulks the specified document.
        /// </summary>
        public async Task CreateDumyDateAsync()
        {
            var collection = GetCollection<SalesDocument>();
            var isHaveData = await collection.Find(w => true).AnyAsync();
            if (!isHaveData)
            {
                var file = File.ReadAllText("DumyData.json");
                var documents = JsonConvert.DeserializeObject<List<SalesDocument>>(file);
                await collection.InsertManyAsync(documents);
            }
        }

        /// <summary>
        /// Lists the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<IEnumerable<SalesDocument>> ListAsync(SalesListFilter request)
        {
            var collection = GetCollection<SalesDocument>();
            var filter = BuildFiltersWithRequest(request);
            var docs = await collection.Find(filter).Sort(Builders<SalesDocument>.Sort.Descending(w => w.Timestamp)).ToListAsync();

            return docs;
        }

        private static FilterDefinition<SalesDocument> BuildFiltersWithRequest(SalesListFilter request)
        {
            var filters = new List<FilterDefinition<SalesDocument>>() { Builders<SalesDocument>.Filter.Empty };
            if (request.StartDate.HasValue)
            {
                filters.Add(Builders<SalesDocument>.Filter.Gte(w => w.Timestamp, request.StartDate.Value));
            }

            if (request.EndDate.HasValue)
            {
                filters.Add(Builders<SalesDocument>.Filter.Lte(w => w.Timestamp, request.EndDate.Value));
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
                throw new Exception("MongoOptions is null, please check app settings.");
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
