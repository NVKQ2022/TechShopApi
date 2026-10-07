namespace TechShop_API_backend_.DTOs.Review
{
    public class CreateReviewRequestDto
    {
        public string ProductId { get; set; } = string.Empty;
        public int Stars { get; set; }
        public string Comment { get; set; } = string.Empty;
        public List<IFormFile>? MediaFiles { get; set; }
    }
}
