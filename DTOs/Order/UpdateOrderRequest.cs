using TechShop_API_backend_.Models;

namespace TechShop_API_backend_.DTOs.Order
{
    public class UpdateOrderRequest
    {
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public string PaymentMethod { get; set; } = string.Empty;
        public ReceiveInfo? ReceiveInfo { get; set; }
    }
}
