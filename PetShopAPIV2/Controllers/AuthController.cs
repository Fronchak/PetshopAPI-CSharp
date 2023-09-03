using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopAPIV2.DTOs.Users;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Services;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PetShopAPIV2.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;


        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            await _authService.RegisterAsync(userRegisterDTO);
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            TokenDTO tokenDTO = await _authService.LoginAsync(userLoginDTO);
            return Ok(tokenDTO);
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            UserDTOWithRoles dto = await _authService.GetAuthenticatedUserDTOAsync();

            return Ok(new { 
                id = dto.Id,
                Email = dto.Email,
                Roles = dto.Roles
            });
        }
    }
}
