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
        public IActionResult FindAll()
        {
            ICollection<AnimalDTO> animals = _animalService.FindAll();
            return Ok(animals);
        }

        [HttpPost, Authorize(Roles = "Worker,Admin")]
        public IActionResult Save([FromBody] AnimalInsertDTO animalInsertDTO)
        {
            AnimalDTO animalDTO = _animalService.Save(animalInsertDTO);
            return Ok(animalDTO);
        }

        [HttpGet("{id}")]
        public IActionResult FindById(string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            AnimalDTO animalDTO = _animalService.FindById(entityId);
            if (animalDTO == null)
            {
                return NotFound();
            }
            return Ok(animalDTO);
        }

        [HttpPut("{id}"), Authorize(Roles = "Worker,Admin")]
        public IActionResult update([FromBody] AnimalUpdateDTO animalUpdateDTO, [FromRoute] string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            AnimalDTO animalDTO = _animalService.Update(animalUpdateDTO, entityId);
            return Ok(animalDTO);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {

            int entityId = ParseUtils.ParsePathParam(id);
            _animalService.DeleteById(entityId);
            return NoContent();
        }
    }
}
