using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Kullanıcı adı veya e-posta zorunludur.")]
        public string UsernameOrEmail { get; set; } // Hem kullanıcı adı hem de e-posta ile giriş yapılabilir

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }
    }
}