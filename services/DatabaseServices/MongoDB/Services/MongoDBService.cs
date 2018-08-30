using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBService.Config;
using MongoDBService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBService.Services
{
    public class MongoDBService : IMongoDBService
    {
        private readonly MongoDBSettings _config;
        private readonly ILogger<MongoDBService> _log;
        private readonly IMongoClient _client;
        private string _name; 

        public MongoDBService(IOptions<MongoDBSettings> config,
            ILogger<MongoDBService> log)
        {
            _config = config.Value;
            _log = log;
            _client = new MongoClient(_config.ConnectionString);
        }

        public IMongoCollection<T> GetCollection<T>(string db, string col)
        {
            var _db = _client.GetDatabase(db);
            return _db.GetCollection<T>(col);
        }

        public async Task<T> FetchOne<T>(string db, string col, BsonDocument filter)
        {
            var _col = GetCollection<T>(db, col);
            return await _col.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> FetchOne<T>(string db, string col, FilterDefinition<T> filter)
        {
            var _col = GetCollection<T>(db, col);
            return await _col.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> FetchOne<T>(string db, string col, BsonDocument filter, ProjectionDefinition<T> projection)
        {
            var _col = GetCollection<T>(db, col);
            return await _col.Find(filter).Project<T>(projection).FirstOrDefaultAsync(); 
        }

        public async Task Upsert<T>(string db, string col, FilterDefinition<T> filter, T record)
        {
            var _col = GetCollection<T>(db, col);
            await _col.ReplaceOneAsync(filter, record,
                new UpdateOptions { IsUpsert = true });
        }

        public async Task<List<T>> Fetch<T>(string db, string col, BsonDocument filter)
        {
            var _col = GetCollection<T>(db, col);
            return await _col.Find(filter).ToListAsync();
        }

        public async Task<List<T>> Fetch<T>(string db, string col, FilterDefinition<T> filter)
        {
            var _col = GetCollection<T>(db, col);
            return await _col.Find(filter).ToListAsync<T>();
        }

        public async Task Insert<T>(string db, string col, T record)
        {
            var _col = GetCollection<T>(db, col);
            await _col.InsertOneAsync(record);
        }

        public async Task InsertMany<T>(string db, string col, List<T> records)
        {
            var _col = GetCollection<T>(db, col);
            await _col.InsertManyAsync(records);
        }

        public async Task DeleteMany<T>(string db, string col, FilterDefinition<T> filter)
        {
            var _col = GetCollection<T>(db, col);
            await _col.DeleteManyAsync(filter);
        }

        public async Task<List<T>> FetchDistinct<T>(string db, string col, FieldDefinition<T, T> field, FilterDefinition<T> filter)
        {
            var _col = GetCollection<T>(db, col);
            return await _col.Distinct(field, filter).ToListAsync();
        }

        public string FetchName()
        {
            return _config.Name; 
        }
    }
}
