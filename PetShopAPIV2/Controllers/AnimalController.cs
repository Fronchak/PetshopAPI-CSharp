using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopAPIV2.DTOs.Animals;
using PetShopAPIV2.Services;
using PetShopAPIV2.Utils;

namespace PetShopAPIV2.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalController : Controller
    {
        private readonly AnimalService _animalService;

        public AnimalController(AnimalService animalService)
        {
            _animalService = animalService;
        }

        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            ICollection<AnimalDTO> animals = await _animalService.FindAllAsync();
            return Ok(animals);
        }

        [HttpPost, Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> Save([FromBody] AnimalInsertDTO animalInsertDTO)
        {
            AnimalDTO animalDTO = await _animalService.SaveAsync(animalInsertDTO);
            return Ok(animalDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindById(string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            AnimalDTO animalDTO = await _animalService.FindByIdAsync(entityId);
            if (animalDTO == null)
            {
                return NotFound();
            }
            return Ok(animalDTO);
        }

        [HttpPut("{id}"), Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> update([FromBody] AnimalUpdateDTO animalUpdateDTO, [FromRoute] string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            AnimalDTO animalDTO = await _animalService.UpdateAsync(animalUpdateDTO, entityId);
            return Ok(animalDTO);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {

            int entityId = ParseUtils.ParsePathParam(id);
            await _animalService.DeleteByIdAsync(entityId);
            return NoContent();
        }
    }
}
