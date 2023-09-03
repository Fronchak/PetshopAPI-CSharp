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
        public ActionResult Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            _authService.Register(userRegisterDTO);
            return NoContent();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLoginDTO userLoginDTO)
        {
            TokenDTO tokenDTO = _authService.Login(userLoginDTO);
            return Ok(tokenDTO);
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            UserDTOWithRoles dto = _authService.GetAuthenticatedUserDTO();

            return Ok(new { 
                id = dto.Id,
                Email = dto.Email,
                Roles = dto.Roles
            });
        }
    }
}
