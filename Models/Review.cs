using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace TechShop_API_backend_.Models

{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReviewId { get; set; } = string.Empty;

        public string ProductId { get; set; } = string.Empty;
        public int UserID { get; set; }

        public int Stars { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedTime { get; set; }
        public List<string>? MediaURLs { get; set; }

    }
    
}
