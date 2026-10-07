namespace TechShop_API_backend_.Models
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string ProductCollectionName { get; set; } = string.Empty;
        public string OrderCollectionName { get; set; } = string.Empty;
        public string ReviewCollectionName { get; set; } = string.Empty;
        public string CategoryCollectionName { get; set; } = string.Empty;
        public string UserDetailCollectionName { get; set; } = string.Empty;
        public string ProductSaleEventCollectionName { get; set; } = string.Empty;
        public string NotificationCollectionName { get; set; } = string.Empty;
    }
}
