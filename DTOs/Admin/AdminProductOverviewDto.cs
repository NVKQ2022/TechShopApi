namespace TechShop_API_backend_.DTOs.Admin
{
  public class AdminProductOverviewDto
  {
    public string ProductId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Price { get; set; }
    public List<string> Color { get; set; } = new List<string>();
    public int Stock { get; set; }
    public int Sold { get; set; }

    // Rating
    public double RatingAverage { get; set; }
    public int Rating1 { get; set; }
    public int Rating2 { get; set; }
    public int Rating3 { get; set; }
    public int Rating4 { get; set; }
    public int Rating5 { get; set; }

    // Sale info
    public bool IsOnSale { get; set; }
    public double? SalePercent { get; set; }
    public DateTime? SaleStart { get; set; }
    public DateTime? SaleEnd { get; set; }
    public List<string> ImageURL { get; set; } = new List<string>();

    // Order statistics for this product
    public int TotalOrders { get; set; }
    public int TotalQuantityOrdered { get; set; }
    public int TotalRevenue { get; set; }

    public int NotConfirmOrders { get; set; }
    public int PendingOrders { get; set; }
    public int ConfirmedOrders { get; set; }
    public int ProcessingOrders { get; set; }
    public int ShippedOrders { get; set; }
    public int DeliveredOrders { get; set; }
    public int CancelledOrders { get; set; }
  }
}
