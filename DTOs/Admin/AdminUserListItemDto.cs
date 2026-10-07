namespace TechShop_API_backend_.DTOs.Admin
{
  public class AdminUserListItemDto
  {
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsEmailVerified { get; set; }
  }
}
