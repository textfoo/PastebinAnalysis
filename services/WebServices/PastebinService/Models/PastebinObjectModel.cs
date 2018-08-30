using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace PastebinService.Models
{
    public class PastebinObjectModel
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        [BsonElement("contents")]
        public string Contents { get; set; }
        [BsonElement("url")]
        string Url { get; set; }
        [BsonElement("pb_key")]
        string Key { get; set; }
        [BsonElement("md5hash")]
        public string MD5Hash { get; set; }
        [BsonElement("tags")]
        public List<string> Tags { get; set; }
    }
}
