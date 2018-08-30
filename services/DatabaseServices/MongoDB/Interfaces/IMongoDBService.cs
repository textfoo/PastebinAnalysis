using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBService.Interfaces
{
    public interface IMongoDBService
    {
        IMongoCollection<T> GetCollection<T>(string db, string col);

        Task Insert<T>(string db, string col, T record);
        Task InsertMany<T>(string db, string col, List<T> records);
        Task DeleteMany<T>(string db, string col, FilterDefinition<T> filter);
        Task Upsert<T>(string db, string col, FilterDefinition<T> filter, T record);
        Task<T> FetchOne<T>(string db, string col, BsonDocument filter);
        Task<T> FetchOne<T>(string db, string col, BsonDocument filter, ProjectionDefinition<T> projection);
        Task<List<T>> Fetch<T>(string db, string col, BsonDocument filter);
        Task<List<T>> Fetch<T>(string db, string col, FilterDefinition<T> filter);
        Task<List<T>> FetchDistinct<T>(string db, string col, FieldDefinition<T, T> field, FilterDefinition<T> filter);
        string FetchName();
    }
}
