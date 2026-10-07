using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace TechShop_API_backend_.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderID { get; set; } = string.Empty;

        public int UserID { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public int TotalAmount { get; set; }

        public string? PaymentMethod { get; set; }

        public string Status { get; set; } = "Pending";

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }
        public ReceiveInfo? ReceiveInfo { get; set; }
    }

    public class OrderItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductID { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public int UnitPrice { get; set; }
    }
}
