using FakeItEasy;
using FluentAssertions;
using PetShopAPIV2.DTOs.Animals;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Exceptions;
using PetShopAPIV2.Interfaces;
using PetShopAPIV2.Services;
using PetShopAPIV2.Tests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopAPIV2.Tests.Services
{
    public class AnimalServiceTests
    {
        private readonly AnimalService _animalService;
        private readonly IAnimalRepository _animalRepository;
        private readonly IPetRepository _petRepository;

        private int ExistingId;
        private int NonExistingId;

        private string AnimalName;

        private Animal? Animal;
        private Animal? AnimalNull;

        public AnimalServiceTests()
        {
            _animalRepository = A.Fake<IAnimalRepository>();
            _petRepository = A.Fake<IPetRepository>();
            _animalService = new AnimalService(_animalRepository, _petRepository);

            ExistingId = 1;
            NonExistingId = 2;

            AnimalName = "Cat";

            Animal = AnimalBuilder.Create()
                .WithId(ExistingId)
                .WithName(AnimalName)
                .Get();
            AnimalNull = null;

            A.CallTo(() => _animalRepository.FindByIdAsync(NonExistingId)).Returns(Task.FromResult(AnimalNull));
            A.CallTo(() => _animalRepository.FindByIdAsync(ExistingId)).Returns(Task.FromResult(Animal));
            
        }

        [Fact]
        public async Task FindByIdShouldReturnAnimalDTOWhenIdExists()
        {
            AnimalDTO result = await _animalService.FindByIdAsync(ExistingId);

            result.Id.Should().Be(ExistingId);
            result.Name.Should().Be(AnimalName);
            
        }

        [Fact]
        public async Task FindByIdShouldThrowEntityNotFoundExceptionWhenIdDoesNotExists()
        {
            Func<Task> action = async () => await _animalService.FindByIdAsync(NonExistingId);

            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Animal not found");
        }

        [Fact]
        public async Task SaveShouldReturnAnimalDTOAfterSave()
        {
            string name = "Bird";
            AnimalInsertDTO animalInsertDTO = AnimalInsertDTOBuilder.Create().WithName(name).Get();
            AnimalDTO result = await _animalService.SaveAsync(animalInsertDTO);

            result.Name.Should().Be(name);
            A.CallTo(() => _animalRepository.Save(A<Animal>.That.Matches((animal) => animal.Name.Equals(name)))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _animalRepository.CommitAsync()).MustHaveHappened();
        }

        [Fact]
        public async Task FindAllShouldReturnACollectionOfAnimalDTO()
        {
            int animalId1 = 10;
            string animalName1 = "Cat";
            int animalId2 = 12;
            string animalName2 = "Dog";
            Animal animal1 = AnimalBuilder.Create()
                .WithId(animalId1)
                .WithName(animalName1)
                .Get();
            Animal animal2 = AnimalBuilder.Create()
                .WithId(animalId2)
                .WithName(animalName2)
                .Get();
            ICollection<Animal> animals = new List<Animal>() { animal1, animal2 };
            A.CallTo(() => _animalRepository.FindAllAsync()).Returns(animals);

            IEnumerable<AnimalDTO> result = await _animalService.FindAllAsync();

            result.Should().HaveCount(2)
                .And.Contain((animalDTO) => animalDTO.Id.Equals(animalId1) && animalDTO.Name.Equals(animalName1))
                .And.Contain((animalDTO) => animalDTO.Id.Equals(animalId2) && animalDTO.Name.Equals(animalName2));
        }

        [Fact]
        public async Task UpdateShouldThrowEntityNotFoundExceptionWhenIdDoesNotExists()
        {
            AnimalUpdateDTO animalUpdateDTO = AnimalUpdateDTOBuilder.Create().Get();

            Func<Task> action = async () => await _animalService.UpdateAsync(animalUpdateDTO, NonExistingId);

            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Animal not found");
            A.CallTo(() => _animalRepository.CommitAsync()).MustNotHaveHappened();
        }

        [Fact]
        public async Task UpdateShouldReturnAnimalDTOWhenIdExists()
        {
            string name = "Dog";
            AnimalUpdateDTO animalUpdateDTO = AnimalUpdateDTOBuilder.Create()
                .WithName(name)
                .Get();

            AnimalDTO result = await _animalService.UpdateAsync(animalUpdateDTO, ExistingId);

            result.Id.Should().Be(ExistingId);
            result.Name.Should().Be(name);
            A.CallTo(() => _animalRepository.CommitAsync()).MustHaveHappened();
            A.CallTo(() => _animalRepository.Update(A<Animal>.That.Matches((animal) => animal.Id.Equals(ExistingId) && animal.Name.Equals(name))));
        }

        [Fact]
        public async Task DeleteShouldThrowEntityNotFoundExceptionWhenIdDoesNotExists()
        {
            Func<Task> action = async () => await _animalService.DeleteByIdAsync(NonExistingId);

            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Animal not found");
            A.CallTo(() => _animalRepository.CommitAsync()).MustNotHaveHappened();
        }

        [Fact]
        public async Task DeleteShouldThrowBadRequestWhenIdExistsButThereIsSomePetsOfThatType()
        {
            A.CallTo(() => _petRepository.ExistsAnyPetOfAnimalTypeAsync(ExistingId)).Returns(true);

            Func<Task> action = async () => await _animalService.DeleteByIdAsync(ExistingId);

            await action.Should().ThrowAsync<BadRequestException>()
                .WithMessage("This animal type cannot be deleted");
            A.CallTo(() => _animalRepository.CommitAsync()).MustNotHaveHappened();
        }

        [Fact]
        public async Task DeleteShouldDeleteEntityWhenIdExistsAndThereIsNoPetsOfThatType()
        {
            A.CallTo(() => _petRepository.ExistsAnyPetOfAnimalTypeAsync(ExistingId)).Returns(false);

            Func<Task> action = async () => await _animalService.DeleteByIdAsync(ExistingId);

            await action.Should().NotThrowAsync();
            A.CallTo(() => _animalRepository.Delete(A<Animal>.That.Matches((animal) => animal.Id.Equals(ExistingId)))).MustHaveHappened();
            A.CallTo(() => _animalRepository.CommitAsync()).MustHaveHappened();
        }
    }
}
