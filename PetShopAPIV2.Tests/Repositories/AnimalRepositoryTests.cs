using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PetShopAPIV2.Data;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Interfaces;
using PetShopAPIV2.Repositories;
using PetShopAPIV2.Tests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopAPIV2.Tests.Repositories
{
    public class AnimalRepositoryTests
    {
        private DataContext _dataContext;
        private IAnimalRepository _animalRepository;

        readonly int AnimalId1;
        readonly string AnimalName1;
        readonly int AnimalId2;
        readonly string AnimalName2;
        readonly int AnimalId3;
        readonly string AnimalName3;

        readonly int NonExistingId;
        readonly int TotalCount;

        public AnimalRepositoryTests()
        {
            AnimalId1 = 1;
            AnimalName1 = "Dog";
            AnimalId2 = 2;
            AnimalName2 = "Cat";
            AnimalId3 = 3;
            AnimalName3 = "Rabbit";

            NonExistingId = 30;
            TotalCount = 3;
        }

        private async Task<DataContext> GetDataContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("fake_database")
                .Options;
            DataContext dataContext = new DataContext(options);
            dataContext.Database.EnsureDeleted();
            dataContext.Database.EnsureCreated();
            
            Animal animal1 = new Animal(AnimalId1, AnimalName1);
            Animal animal2 = new Animal(AnimalId2, AnimalName2);
            Animal animal3 = new Animal(AnimalId3, AnimalName3);

            dataContext.Animals.AddRange(new Animal[] { animal1, animal2, animal3 });
            await dataContext.SaveChangesAsync();
            
            return dataContext;
        }

        [Fact]
        public async Task FindByIdShouldReturnAnimalWhenIdExists()
        {
            _dataContext = await GetDataContext();
            _animalRepository = new AnimalRepository(_dataContext);

            Animal? result = await _animalRepository.FindByIdAsync(AnimalId2);

            result.Id.Should().Be(AnimalId2);
            result.Name.Should().Be(AnimalName2);
        }

        [Fact]
        public async Task FindByIdShouldReturnNullWhenIdDoesNotExits()
        {
            _dataContext = await GetDataContext();
            _animalRepository = new AnimalRepository(_dataContext);
            Animal? animal = await _animalRepository.FindByIdAsync(NonExistingId);

            animal.Should().BeNull();
        }

        [Fact]
        public async Task SaveShouldSaveEntityInTheDatabase()
        {
            _dataContext = await GetDataContext();
            _animalRepository = new AnimalRepository(_dataContext);
            Animal animal = AnimalBuilder.Create().WithName("Bird").Get();

            _animalRepository.Save(animal);
            await _animalRepository.CommitAsync();

            animal.Id.Should().NotBe(0);
            int count = await _dataContext.Animals.CountAsync();
            count.Should().Be(TotalCount + 1);
        }

        [Fact]
        public async Task DeleteShouldRemoveEntityFromDatabase()
        {
            _dataContext = await GetDataContext();
            _animalRepository = new AnimalRepository(_dataContext);
            Animal? animal = await _animalRepository.FindByIdAsync(AnimalId1);

            _animalRepository.Delete(animal!);
            await _animalRepository.CommitAsync();

            int count = await _dataContext.Animals.CountAsync();
            count.Should().Be(TotalCount - 1);
            Assert.Null(await _animalRepository.FindByIdAsync(AnimalId1));
        } 
    }
}
