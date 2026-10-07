using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TechShop_API_backend_.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; } = string.Empty;

        public string CateName { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;
        public int BuyTime { get; set; }

        public int ProductQuantity { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Products { get; set; } = new List<string>();
    }

   
}
