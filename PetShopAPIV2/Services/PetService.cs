using PetShopAPIV2.DTOs.Pets;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Exceptions;
using PetShopAPIV2.Interfaces;
using System.Diagnostics;

namespace PetShopAPIV2.Services
{
    public class PetService
    {
        private readonly IPetRepository _petRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IClientRepository _clientRepository;

        public PetService(IPetRepository petRepository, IAnimalRepository animalRepository, IClientRepository clientRepository)
        {
            _petRepository = petRepository;
            _animalRepository = animalRepository;
            _clientRepository = clientRepository;
        }

        public PetOutputDTO FindById(int id)
        {
            Pet? pet = _petRepository.FindByIdWithRelationships(id);
            if(pet == null)
            {
                throw new EntityNotFoundException("Pet not found");
            }
            return MapToPetOutputDTO(pet);
        }

        public static PetOutputDTO MapToPetOutputDTO(Pet pet)
        {
            PetOutputDTO petOutputDTO = new PetOutputDTO(pet);
            petOutputDTO.Animal = AnimalService.MapToDTO(pet.Animal);
            petOutputDTO.Owner = ClientService.MapToSimpleClientDTO(pet.Owner);
            return petOutputDTO;
        }

        public ICollection<PetOutputDTO> FindAll()
        {
            ICollection<Pet> pets = _petRepository.FindAll();
            return pets.ToList()
                .Select((pet) => MapToPetOutputDTO(pet))
                .ToList();
        }

        public PetOutputDTO Save(PetInputDTO petInputDTO)
        {
            Pet pet = new Pet();
            pet.Name = petInputDTO.Name;
            pet.Age = petInputDTO.Age;
            pet.Animal = _animalRepository.FindById(petInputDTO.AnimalId);
            pet.Owner = _clientRepository.FindById(petInputDTO.OwnerId)!;
            _petRepository.Save(pet);
            _petRepository.Commit();
            return MapToPetOutputDTO(pet);
        }

        public PetOutputDTO Update(PetInputDTO petInputDTO, int id)
        {
            Pet pet = _petRepository.FindById(id);
            if(pet == null)
            {
                throw new EntityNotFoundException("Pet not found");
            }
            pet.Name = petInputDTO.Name;
            pet.Age = petInputDTO.Age;
            pet.Animal = _animalRepository.FindById(petInputDTO.AnimalId);
            pet.Owner = _clientRepository.FindById(petInputDTO.OwnerId)!;
            _petRepository.Update(pet);
            _petRepository.Commit();
            return MapToPetOutputDTO(pet);
        }

        public void DeleteById(int id)
        {
            Pet? pet = _petRepository.FindById(id);
            if (pet == null)
            {
                throw new EntityNotFoundException("Pet not found");
            }
            _petRepository.Delete(pet);
            _petRepository.Commit();
        }
    }
}
