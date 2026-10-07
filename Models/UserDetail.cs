using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using TechShop_API_backend_.Models;

namespace TechShop_API_backend_.Models
{


    public class UserDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        // e.g., { "Laptop": 2, "Keyboard": 1 }
        public Dictionary<string, int> Category { get; set; } = new Dictionary<string, int>();
        public List<CartItem> Cart { get; set; } = new List<CartItem>();
        public List<WishlistItem> Wishlist { get; set; } = new List<WishlistItem>();
        public List<ReceiveInfo> ReceiveInfo { get; set; } = new List<ReceiveInfo>();

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Birthday { get; set; }

        public Banking Banking { get; set; } = new Banking();
    }

    public class Banking
    {
        public string BankAccount { get; set; } = string.Empty;
        public string CreditCard { get; set; } = string.Empty;
    }
}
