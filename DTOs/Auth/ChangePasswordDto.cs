namespace TechShop_API_backend_.DTOs.Auth
{
    public class ChangePasswordDto
    {
        public string NewPassword { get; set; } = string.Empty;

        public string CurrentPassword { get; set; } = string.Empty;
    }
}
