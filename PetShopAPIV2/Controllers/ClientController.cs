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
        public IActionResult FindById(string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            ClientDTO clientDTO = _clientService.FindById(entityId);
            return Ok(clientDTO);
        }

        [HttpGet, Authorize(Roles = "Worker,Admin")]
        public IActionResult FindAll()
        {
            ICollection<ClientSimpleDTO> dtos = _clientService.FindAll();
            return Ok(dtos);
        }

        [HttpPost, Authorize(Roles = "Worker,Admin")]
        public IActionResult Save([FromBody] ClientInsertDTO clientInsertDTO)
        {
            ClientDTO clientDTO = _clientService.Save(clientInsertDTO);
            return Ok(clientDTO);
        }

        [HttpPut("{id}"), Authorize(Roles = "Worker,Admin")]
        public IActionResult Update([FromBody] ClientUpdateDTO clientUpdateDTO, [FromRoute] string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            ClientDTO clientDTO = _clientService.Update(clientUpdateDTO, entityId);
            return Ok(clientDTO);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            int entityId = ParseUtils.ParsePathParam(id);
            _clientService.DeleteById(entityId);
            return NoContent();
        }
    }
}
