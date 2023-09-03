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

        public async Task<PetOutputDTO> FindByIdAsync(int id)
        {
            Pet? pet = await _petRepository.FindByIdWithRelationshipsAsync(id);
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

        public async Task<ICollection<PetOutputDTO>> FindAllAsync()
        {
            ICollection<Pet> pets = await _petRepository.FindAllAsync();
            return pets.ToList()
                .Select((pet) => MapToPetOutputDTO(pet))
                .ToList();
        }

        public async Task<PetOutputDTO> SaveAsync(PetInputDTO petInputDTO)
        {
            Pet pet = new Pet();
            pet.Name = petInputDTO.Name;
            pet.Age = petInputDTO.Age;
            pet.Animal = await _animalRepository.FindByIdAsync(petInputDTO.AnimalId);
            pet.Owner = await _clientRepository.FindByIdAsync(petInputDTO.OwnerId)!;
            _petRepository.Save(pet);
            await _petRepository.CommitAsync();
            return MapToPetOutputDTO(pet);
        }

        public async Task<PetOutputDTO> UpdateAsync(PetInputDTO petInputDTO, int id)
        {
            Pet pet = await _petRepository.FindByIdAsync(id);
            if(pet == null)
            {
                throw new EntityNotFoundException("Pet not found");
            }
            pet.Name = petInputDTO.Name;
            pet.Age = petInputDTO.Age;
            pet.Animal = await _animalRepository.FindByIdAsync(petInputDTO.AnimalId);
            pet.Owner = await _clientRepository.FindByIdAsync(petInputDTO.OwnerId)!;
            _petRepository.Update(pet);
            await _petRepository.CommitAsync();
            return MapToPetOutputDTO(pet);
        }

        public async Task DeleteByIdAsync(int id)
        {
            Pet? pet = await _petRepository.FindByIdAsync(id);
            if (pet == null)
            {
                throw new EntityNotFoundException("Pet not found");
            }
            _petRepository.Delete(pet);
            await _petRepository.CommitAsync();
        }
    }
}
