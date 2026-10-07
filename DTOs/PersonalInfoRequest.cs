namespace TechShop_API_backend_.DTOs
{
    public class PersonalInfoRequest
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;
    }
}
