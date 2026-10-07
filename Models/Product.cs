using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TechShop_API_backend_.Models
{
    public class Product
    {
        [BsonId] // Marks this property as MongoDB's _id
        [BsonRepresentation(BsonType.ObjectId)] // Allows mapping ObjectId to string
        public string ProductId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int Price { get; set; }
        public List<string> Color { get; set; } = new List<string>();
        public Dictionary<string, int> Rating { get; set; } = new Dictionary<string, int>();
        public List<string> ImageURL { get; set; } = new List<string>();
        public Dictionary<string, string> Detail { get; set; } = new Dictionary<string, string>();
        public string Category { get; set; } = string.Empty;
        public int Sold { get; set; }
        public int Stock { get; set; }

        public SaleInfo? Sale { get; set; }
    }
    public class SaleInfo
    {
        public double Percent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }


}
