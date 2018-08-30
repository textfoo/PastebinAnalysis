using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PastebinService.Models
{
    public class TagAnalysisTemplateModel
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        [BsonElement("tag")]
        public string Tag { get; set; }
        [BsonElement("action")]
        public string Action { get; set; }
        [BsonElement("keywords")]
        public string[] Keywords { get; set; }
    }
}
