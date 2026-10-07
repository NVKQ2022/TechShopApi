namespace TechShop_API_backend_.DTOs.Auth
{
    public class ForgotPasswordDto
    {
        public string email { get; set; } = string.Empty;
        public string newPassword { get; set; } = string.Empty;

        public string confirmPassword { get; set; } = string.Empty;

        public string Otp { get; set; } = string.Empty;
    }
}
