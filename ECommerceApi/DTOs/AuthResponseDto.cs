namespace ECommerceApi.DTOs
{
    public class AuthResponseDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        // public string Role { get; set; } // İleride roller eklenirse kullanılabilir
    }
}