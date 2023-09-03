using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopAPIV2.DTOs.Users;
using PetShopAPIV2.Services;
using PetShopAPIV2.Utils;

namespace PetShopAPIV2.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}"), Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> FindById(string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            UserDTOWithRoles userDTO = await _userService.FindByIdAsync(entityId);
            return Ok(new
            {
                id = userDTO.Id,
                Email = userDTO.Email,
                Roles = userDTO.Roles
            });
        }

        [HttpGet, Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> FindAll()
        {
            ICollection<UserDTO> userDTOs = await _userService.FindAllAsync();
            return Ok(userDTOs);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteById(string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            await _userService.DeleteByIdAsync(entityId);
            return NoContent();
        }
    }
}
