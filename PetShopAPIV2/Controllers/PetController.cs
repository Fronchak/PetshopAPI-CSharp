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
        public IActionResult FindById(string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            PetOutputDTO petOutputDTO = _petService.FindById(entityId);
            return Ok(petOutputDTO);
        }

        [HttpGet]
        public IActionResult FindAll()
        {
            ICollection<PetOutputDTO> petDTOs = _petService.FindAll();
            return Ok(petDTOs);
        }
  
        [HttpPost, Authorize(Roles = "Worker,Admin")]
        public IActionResult Save([FromBody] PetInputDTO petInsertDTO)
        {
            PetOutputDTO petOutputDTO = _petService.Save(petInsertDTO);
            return Ok(petOutputDTO);
        }

        [HttpPut("{id}"), Authorize(Roles = "Worker,Admin")]
        public IActionResult Update([FromBody] PetInputDTO petInputDTO, [FromRoute] string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            PetOutputDTO petOutputDTO = _petService.Update(petInputDTO, entityId);
            return Ok(petOutputDTO);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public IActionResult DeleteById(string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            _petService.DeleteById(entityId);
            return NoContent();
        }
    }
}
