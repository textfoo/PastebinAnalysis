using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace PastebinService.Models
{
    public class PastebinObjectModel
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("contents")]
        public string Contents { get; set; }
        [BsonElement("scrape_url")]
        public string ScrapeUrl { get; set; }
        [BsonElement("full_url")]
        public string FullUrl { get; set; }
        [BsonElement("pb_key")]
        public string Key { get; set; }
        [BsonElement("size")]
        public int Size { get; set; }
        [BsonElement("tags")]
        public string[] Tags { get; set; }
    }
}
