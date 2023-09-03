using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopAPIV2.DTOs.Clients;
using PetShopAPIV2.Services;
using PetShopAPIV2.Utils;

namespace PetShopAPIV2.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : Controller
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("{id}"), Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> FindById(string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            ClientDTO clientDTO = await _clientService.FindByIdAsync(entityId);
            return Ok(clientDTO);
        }

        [HttpGet, Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> FindAll()
        {
            ICollection<ClientSimpleDTO> dtos = await _clientService.FindAllAsync();
            return Ok(dtos);
        }

        [HttpPost, Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> Save([FromBody] ClientInsertDTO clientInsertDTO)
        {
            ClientDTO clientDTO = await _clientService.SaveAsync(clientInsertDTO);
            return Ok(clientDTO);
        }

        [HttpPut("{id}"), Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> Update([FromBody] ClientUpdateDTO clientUpdateDTO, [FromRoute] string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            ClientDTO clientDTO = await _clientService.UpdateAsync(clientUpdateDTO, entityId);
            return Ok(clientDTO);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            await _clientService.DeleteById(entityId);
            return NoContent();
        }
    }
}
