namespace TechShop_API_backend_.DTOs.User
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
