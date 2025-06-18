using ECommerceApi.DTOs;
using System.Threading.Tasks;

namespace ECommerceApi.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(UserRegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto);
        // Şifre hashleme için yardımcı bir metod da eklenebilir, veya doğrudan servis içinde olabilir.
        // Güvenlik açısından daha güçlü bir şifreleme algoritması kullanılacak.
    }
}