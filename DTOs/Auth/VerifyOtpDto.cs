namespace TechShop_API_backend_.DTOs.Auth
{
    public class VerifyOtpDto
    {
        public string email { get; set; } = string.Empty;
        public string otp { get; set; } = string.Empty;
    }
}
