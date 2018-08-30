using MongoDB.Bson.Serialization.Attributes;

namespace PastebinService.Models
{
    public class PastebinFeedObject
    {
        [BsonElement("scrape_url")]
        public string ScrapeUrl { get; set; }
        [BsonElement("full_url")]
        public string FullUrl { get; set; }
        [BsonElement("date")]
        public string Date { get; set; }
        [BsonElement("key")]
        public string Key { get; set; }
        [BsonElement("size")]
        public int Size { get; set; }
        [BsonElement("expire")]
        public string Expire { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("syntax")]
        public string Syntax { get; set; }
        [BsonElement("user")]
        public string User { get; set; }
    }
}
