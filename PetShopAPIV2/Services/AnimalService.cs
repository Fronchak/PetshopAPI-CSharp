using PetShopAPIV2.DTOs.Animals;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Exceptions;
using PetShopAPIV2.Interfaces;

namespace PetShopAPIV2.Services
{
    public class AnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IPetRepository _petRepository;

        public AnimalService(IAnimalRepository animalRepository, IPetRepository petRepository)
        {
            _animalRepository = animalRepository;
            _petRepository = petRepository;
        }

        public async Task<AnimalDTO> SaveAsync(AnimalInsertDTO animalInsertDTO)
        {
            Animal animal = new Animal();
            animal.Name = animalInsertDTO.Name;
            _animalRepository.Save(animal);
            await _animalRepository.CommitAsync();
            return MapToDTO(animal);
        }

        public static AnimalDTO MapToDTO(Animal animal)
        {
            AnimalDTO animalDTO = new AnimalDTO(animal);
            return animalDTO;
        }

        public async Task<ICollection<AnimalDTO>> FindAllAsync()
        {
            ICollection<Animal> animals = await _animalRepository.FindAllAsync();
            return animals.ToList()
                .Select((animal) => MapToDTO(animal))
                .ToList();

        }

        public async Task<AnimalDTO> FindByIdAsync(int id)
        {
            Animal animal = await GetAnimalByIdAsync(id);
            return MapToDTO(animal);
        }

        private async Task<Animal> GetAnimalByIdAsync(int id)
        {
            Animal? animal = await _animalRepository.FindByIdAsync(id);
            if (animal == null)
            {
                throw new EntityNotFoundException("Animal not found");
            }
            return animal;
        }

        public async Task<AnimalDTO> UpdateAsync(AnimalUpdateDTO animalUpdateDTO, int id)
        {
            Animal animal = await GetAnimalByIdAsync(id);
            animal.Name = animalUpdateDTO.Name;
            _animalRepository.Update(animal);
            await _animalRepository.CommitAsync();
            return MapToDTO(animal);
        }

        public async Task DeleteByIdAsync(int id)
        {
            Animal animal = await GetAnimalByIdAsync(id);
            bool existAnyPet = await _petRepository.ExistsAnyPetOfAnimalTypeAsync(id);
            if(existAnyPet)
            {
                throw new BadRequestException("This animal type cannot be deleted");
            }
            _animalRepository.Delete(animal);
            await _animalRepository.CommitAsync();
        }
    }
}
