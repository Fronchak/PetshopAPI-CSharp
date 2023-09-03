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

        public AnimalDTO Save(AnimalInsertDTO animalInsertDTO)
        {
            Animal animal = new Animal();
            animal.Name = animalInsertDTO.Name;
            _animalRepository.Save(animal);
            _animalRepository.Commit();
            return MapToDTO(animal);
        }

        public static AnimalDTO MapToDTO(Animal animal)
        {
            AnimalDTO animalDTO = new AnimalDTO(animal);
            return animalDTO;
        }

        public ICollection<AnimalDTO> FindAll()
        {
            ICollection<Animal> animals = _animalRepository.FindAll();
            return animals.ToList()
                .Select((animal) => MapToDTO(animal))
                .ToList();

        }

        public AnimalDTO FindById(int id)
        {
            Animal animal = GetAnimalById(id);
            return MapToDTO(animal);
        }

        private Animal GetAnimalById(int id)
        {
            Animal animal = _animalRepository.FindById(id);
            if (animal == null)
            {
                throw new EntityNotFoundException("Animal not found");
            }
            return animal;
        }

        public AnimalDTO Update(AnimalUpdateDTO animalUpdateDTO, int id)
        {
            Animal animal = GetAnimalById(id);
            animal.Name = animalUpdateDTO.Name;
            _animalRepository.Update(animal);
            _animalRepository.Commit();
            return MapToDTO(animal);
        }

        public void DeleteById(int id)
        {
            Animal animal = GetAnimalById(id);
            bool existAnyPet = _petRepository.ExistsAnyPetOfAnimalType(id);
            if(existAnyPet)
            {
                throw new BadRequestException("This animal type cannot be deleted");
            }
            _animalRepository.Delete(animal);
            _animalRepository.Commit();
        }
    }
}
