using PetShopAPIV2.DTOs.Animals;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Exceptions;
using PetShopAPIV2.Interfaces;

namespace PetShopAPIV2.Services
{
    public class AnimalService
    {
        private readonly IAnimalRepository _animalRepository;

        public AnimalService(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        public AnimalDTO Save(AnimalInsertDTO animalInsertDTO)
        {
            Animal animal = new Animal();
            animal.Name = animalInsertDTO.Name;
            _animalRepository.Save(animal);
            _animalRepository.Commit();
            return mapToDTO(animal);
        }

        private AnimalDTO mapToDTO(Animal animal)
        {
            AnimalDTO animalDTO = new AnimalDTO();
            animalDTO.Id = animal.Id;
            animalDTO.Name = animal.Name;
            return animalDTO;
        }

        public ICollection<AnimalDTO> FindAll()
        {
            ICollection<Animal> animals = _animalRepository.FindAll();
            return animals.ToList()
                .Select((animal) => mapToDTO(animal))
                .ToList();

        }

        public AnimalDTO FindById(int id)
        {
            Animal animal = GetAnimalById(id);
            return mapToDTO(animal);
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
            return mapToDTO(animal);
        }

        public void DeleteById(int id)
        {
            Animal animal = GetAnimalById(id);
            _animalRepository.Delete(animal);
            _animalRepository.Commit();
        }
    }
}
