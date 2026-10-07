using System.ComponentModel.DataAnnotations;

namespace TechShop_API_backend_.DTOs.User
{
    public class CreateUserDto
    {

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(3)]
        public string Username { get; set; } = string.Empty;

        [Required, MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required, MinLength(8)]
        public string ConfirmPassword { get; set; } = string.Empty;


    }
}
