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

        public ClientDTO Save(ClientInsertDTO clientInsertDTO)
        {
            Client client = new Client();
            CopyToClient(client, clientInsertDTO);
            _clientRepository.Save(client);
            _clientRepository.Commit();
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

        public ICollection<ClientSimpleDTO> FindAll()
        {
            ICollection<Client> clients = _clientRepository.FindAll();
            return clients
                .ToList()
                .Select((client) => MapToSimpleClientDTO((client)))
                .ToList();
        }

        public ClientDTO FindById(int id)
        {
            Client? client = _clientRepository.FindByIdWithPets(id);
            if(client == null)
            {
                throw new EntityNotFoundException("Client not found");
            }
            return MapToClientDTO(client);
        }

        public ClientDTO Update(ClientUpdateDTO clientUpdateDTO, int id)
        {
            Client? client = _clientRepository.FindById(id);
            if (client == null)
            {
                throw new EntityNotFoundException("Client not found");
            }
            CopyToClient(client, clientUpdateDTO);
            _clientRepository.Update(client);
            _clientRepository.Commit();
            return MapToClientDTO(client);
        }

        public void DeleteById(int id)
        {
            Client? client = _clientRepository.FindById(id);
            if (client == null)
            {
                throw new EntityNotFoundException("Client not found");
            }
            _clientRepository.Delete(client);
            _clientRepository.Commit();
        }
    }
}
