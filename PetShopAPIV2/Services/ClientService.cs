using PetShopAPIV2.DTOs.Clients;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Exceptions;
using PetShopAPIV2.Interfaces;

namespace PetShopAPIV2.Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ClientDTO> SaveAsync(ClientInsertDTO clientInsertDTO)
        {
            Client client = new Client();
            CopyToClient(client, clientInsertDTO);
            _clientRepository.Save(client);
            await _clientRepository.CommitAsync();
            return MapToClientDTO(client);
        }

        public void CopyToClient(Client client, ClientInputDTO clientInputDTO)
        {
            client.FirstName = clientInputDTO.FirstName;
            client.LastName = clientInputDTO.LastName;
            client.Email = clientInputDTO.Email;
        }

        public static ClientDTO MapToClientDTO(Client client)
        {
            ClientDTO clientDTO = new ClientDTO(client);
            clientDTO.Pets = client.Pets
                .ToList()
                .Select((pet) =>
                {
                    ClientPetOutputDTO petDTO = new ClientPetOutputDTO(pet);
                    petDTO.Animal = AnimalService.MapToDTO(pet.Animal);
                    return petDTO;
                })
                .ToList();
            return clientDTO;
        }

        public static ClientSimpleDTO MapToSimpleClientDTO(Client client)
        {
            return new ClientSimpleDTO(client);
        }

        public async Task<ICollection<ClientSimpleDTO>> FindAllAsync()
        {
            ICollection<Client> clients = await _clientRepository.FindAllAsync();
            return clients
                .ToList()
                .Select((client) => MapToSimpleClientDTO((client)))
                .ToList();
        }

        public async Task<ClientDTO> FindByIdAsync(int id)
        {
            Client? client = await _clientRepository.FindByIdWithPetsAsync(id);
            if(client == null)
            {
                throw new EntityNotFoundException("Client not found");
            }
            return MapToClientDTO(client);
        }

        public async Task<ClientDTO> UpdateAsync(ClientUpdateDTO clientUpdateDTO, int id)
        {
            Client? client = await _clientRepository.FindByIdAsync(id);
            if (client == null)
            {
                throw new EntityNotFoundException("Client not found");
            }
            CopyToClient(client, clientUpdateDTO);
            _clientRepository.Update(client);
            await _clientRepository.CommitAsync();
            return MapToClientDTO(client);
        }

        public async Task DeleteById(int id)
        {
            Client? client = await _clientRepository.FindByIdAsync(id);
            if (client == null)
            {
                throw new EntityNotFoundException("Client not found");
            }
            _clientRepository.Delete(client);
            await _clientRepository.CommitAsync();
        }
    }
}
