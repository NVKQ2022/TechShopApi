namespace TechShop_API_backend_.DTOs.CartItem
{
    public class UpdateCartItemRequest
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
