using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopAPIV2.DTOs.Pets;
using PetShopAPIV2.Services;
using PetShopAPIV2.Utils;

namespace PetShopAPIV2.Controllers
{
    [Route("api/pets")]
    [ApiController]
    public class PetController : Controller
    {
        private readonly PetService _petService;

        public PetController(PetService petService)
        {
            _petService = petService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindById(string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            PetOutputDTO petOutputDTO = await _petService.FindByIdAsync(entityId);
            return Ok(petOutputDTO);
        }

        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            ICollection<PetOutputDTO> petDTOs = await _petService.FindAllAsync();
            return Ok(petDTOs);
        }
  
        [HttpPost, Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> Save([FromBody] PetInputDTO petInsertDTO)
        {
            PetOutputDTO petOutputDTO = await _petService.SaveAsync(petInsertDTO);
            return Ok(petOutputDTO);
        }

        [HttpPut("{id}"), Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> Update([FromBody] PetInputDTO petInputDTO, [FromRoute] string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            PetOutputDTO petOutputDTO = await _petService.UpdateAsync(petInputDTO, entityId);
            return Ok(petOutputDTO);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteById(string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            await _petService.DeleteByIdAsync(entityId);
            return NoContent();
        }
    }
}
