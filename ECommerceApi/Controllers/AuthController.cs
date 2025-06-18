using ECommerceApi.DTOs;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")] // /api/Auth
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/register
        [HttpPost("register")] // Route: /api/Auth/register
        public async Task<ActionResult<AuthResponseDto>> Register(UserRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.RegisterAsync(registerDto);
                return Ok(response); // Kayıt ve otomatik giriş başarılı
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Kullanıcı adı/e-posta zaten kullanımda gibi hatalar
            }
            catch (Exception)
            {
                return StatusCode(500, "Kayıt işlemi sırasında bir hata oluştu.");
            }
        }

        // POST: api/Auth/login
        [HttpPost("login")] // Route: /api/Auth/login
        public async Task<ActionResult<AuthResponseDto>> Login(UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.LoginAsync(loginDto);

                if (response == null)
                {
                    return Unauthorized("Kullanıcı adı/e-posta veya şifre hatalı."); // Kimlik doğrulama başarısız
                }

                return Ok(response); // Giriş başarılı, token döndür
            }
            catch (Exception)
            {
                return StatusCode(500, "Giriş işlemi sırasında bir hata oluştu.");
            }
        }
    }
}