using ECommerceApi.Data;
using ECommerceApi.DTOs;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Appsettings'ten ayar okumak için
using Microsoft.IdentityModel.Tokens; // JWT için
using System.IdentityModel.Tokens.Jwt; // JWT için
using System.Security.Claims; // JWT için
using System.Text; // Encoding için

namespace ECommerceApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ECommerceDbContext _context;
        private readonly IConfiguration _configuration; // appsettings'ten ayarları okumak için

        public AuthService(ECommerceDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(UserRegisterDto registerDto)
        {
            // Kullanıcı adı veya e-posta zaten kullanımda mı kontrol et
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username || u.Email == registerDto.Email))
            {
                throw new ArgumentException("Kullanıcı adı veya e-posta zaten kullanılıyor.");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                // Şifreyi hashle
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password) // BCrypt kullanacağız
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Kayıt başarılı olduğunda otomatik giriş yap ve token döndür
            var loginDto = new UserLoginDto { UsernameOrEmail = user.Username, Password = registerDto.Password };
            return await LoginAsync(loginDto);
        }

        public async Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto)
        {
            // Kullanıcıyı kullanıcı adı veya e-posta ile bul
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.UsernameOrEmail || u.Email == loginDto.UsernameOrEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                // Kullanıcı bulunamadı veya şifre yanlış
                return null; // veya özel bir AuthenticationException fırlatılabilir
            }

            // JWT Token oluştur
            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                // new Claim(ClaimTypes.Role, "User") // İleride roller eklendiğinde
            };

            var jwtSecret = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new InvalidOperationException("JWT Secret key is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), // Token 7 gün geçerli olsun
                SigningCredentials = creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}