using TechShop_API_backend_.Models;

namespace TechShop_API_backend_.DTOs.Order
{
    public class CreateOrderRequest
    {
        public List<Item> Items { get; set; } = new List<Item>();
        
    }
    public class Item
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
