using PetShopAPIV2.Controllers;
using PetShopAPIV2.Interfaces;
using PetShopAPIV2.Entities;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetShopAPIV2.DTOs.Animals;
using PetShopAPIV2.Tests.Builders;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using PetShopAPIV2.Exceptions;

namespace PetShopAPIV2.Tests.Controllers
{
    public class AnimalControllerTests
    {
        private IAnimalService _animalService;
        private AnimalController _controller;

        readonly string ExistingIdStr;
        readonly string NonExistingIdSrt;
        readonly string NonDeletableIdStr;
        readonly int ExistingId;
        readonly int NonExistingId;
        readonly int NonDeletableId;

        readonly int AnimalDTOId1;
        readonly string AnimalDTOName1;

        readonly int AnimalDTOId2;
        readonly string AnimalDTOName2;

        AnimalDTO AnimalDTO1;
        AnimalDTO AnimalDTO2;

        public AnimalControllerTests()
        {
            _animalService = A.Fake<IAnimalService>();
            _controller = new AnimalController(_animalService);

            ExistingIdStr = "1";
            NonExistingIdSrt = "10";
            NonDeletableIdStr = "20";
            ExistingId = 1;
            NonExistingId = 10;
            NonDeletableId = 20;

            AnimalDTOId1 = ExistingId;
            AnimalDTOName1 = "Cat";
            AnimalDTO1 = AnimalDTOBuilder.Create()
                .WithId(AnimalDTOId1)
                .WithName(AnimalDTOName1)
                .Get();

            AnimalDTOId2 = 2;
            AnimalDTOName2 = "Dog";
            AnimalDTO2 = AnimalDTOBuilder.Create()
                .WithId(AnimalDTOId2)
                .WithName(AnimalDTOName2)
                .Get();

            A.CallTo(() => _animalService.FindByIdAsync(ExistingId)).Returns(AnimalDTO1);
            A.CallTo(() => _animalService.FindByIdAsync(NonExistingId)).ThrowsAsync(new EntityNotFoundException("Animal not found"));
            A.CallTo(() => _animalService.UpdateAsync(A<AnimalUpdateDTO>.Ignored, NonExistingId)).ThrowsAsync(new EntityNotFoundException("Animal not found"));
            A.CallTo(() => _animalService.DeleteByIdAsync(NonExistingId)).ThrowsAsync(new EntityNotFoundException("Animal not found"));
            A.CallTo(() => _animalService.DeleteByIdAsync(NonDeletableId)).ThrowsAsync(new BadRequestException("This animal type cannot be deleted"));
        }

        [Fact]
        public async Task FindByIdShouldReturnSuccessAndAnimalDTOWhenIdExists()
        {
            IActionResult result = await _controller.FindById(ExistingIdStr);

            result.Should().BeOfType<OkObjectResult>();
            OkObjectResult okObjectResult = result as OkObjectResult;
            okObjectResult.Value.Should().BeOfType<AnimalDTO>();
            AnimalDTO animalDTO = okObjectResult.Value as AnimalDTO;
            animalDTO.Id.Should().Be(ExistingId);
            animalDTO.Name.Should().Be(AnimalDTOName1);
        }

        [Fact]
        public async Task FindByIdShouldThrowEntityNotFoundExceptionWhenIdDoesNotExists()
        {
            Func<Task> action = async () => await _controller.FindById(NonExistingIdSrt);

            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Animal not found");
        }

        [Fact]
        public async Task FindByIdShouldThrowBadRequestExceptionWhenIdStrDoNotRepresentAValidInteger()
        {
            Func<Task> action = async () => await _controller.FindById("A");

            await action.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Id must be a number");
        }

        [Fact]
        public async Task SaveShouldReturnSuccess()
        {
            string Name = "Rabbit";
            AnimalInsertDTO animalInsertDTO = AnimalInsertDTOBuilder.Create().WithName(Name).Get();
            A.CallTo(() => _animalService.SaveAsync(animalInsertDTO)).Returns(AnimalDTO1);

            IActionResult result = await _controller.Save(animalInsertDTO);

            A.CallTo(() => _animalService.SaveAsync(A<AnimalInsertDTO>.That.Matches((animalInsertDTO) => animalInsertDTO.Name == Name))).MustHaveHappenedOnceExactly();
            result.Should().BeOfType<OkObjectResult>();
            OkObjectResult okObjectResult = result as OkObjectResult;
            okObjectResult.Value.Should().BeOfType<AnimalDTO>();
            AnimalDTO value = okObjectResult.Value as AnimalDTO;
            value.Id.Should().Be(AnimalDTOId1);
            value.Name.Should().Be(AnimalDTOName1);
        }

        [Fact]
        public async Task UpdateShouldThrowBadRequestExceptionWhenIdDoesNotRepresentAnInteger()
        {
            AnimalUpdateDTO animalUpdateDTO = new AnimalUpdateDTO();

            Func<Task> action = async () => await _controller.update(animalUpdateDTO, "B");

            await action.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Id must be a number");
            A.CallTo(() => _animalService.UpdateAsync(A<AnimalUpdateDTO>.Ignored, A<int>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task UpdateShouldThrowEntityNotFoundExceptionWhenIdDoesNotExists()
        {
            AnimalUpdateDTO animalUpdateDTO = new AnimalUpdateDTO();

            Func<Task> action = async () => await _controller.update(animalUpdateDTO, NonExistingIdSrt);

            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Animal not found");
        }

        [Fact]
        public async Task UpdateShouldReturnSuccessWhenIdExists()
        {
            string name = "Rabbit";
            AnimalUpdateDTO animalUpdateDTO = AnimalUpdateDTOBuilder.Create().WithName(name).Get();
            A.CallTo(() => _animalService.UpdateAsync(animalUpdateDTO, ExistingId)).Returns(AnimalDTO1);

            IActionResult result = await _controller.update(animalUpdateDTO, ExistingIdStr);

            A.CallTo(() => _animalService.UpdateAsync(
                    A<AnimalUpdateDTO>.That.Matches((dto) => dto.Name.Equals(name)), ExistingId        
            )).MustHaveHappenedOnceExactly();
            result.Should().BeOfType<OkObjectResult>();
            OkObjectResult okObjectResult = result as OkObjectResult;
            okObjectResult.Value.Should().BeOfType<AnimalDTO>();
            AnimalDTO value = okObjectResult.Value as AnimalDTO;
            value.Id.Should().Be(AnimalDTOId1);
            value.Name.Should().Be(AnimalDTOName1);
        }

        [Fact]
        public async Task DeleteShouldThrowBadRequestExceptionWhenIdDoesNotRepresentANumber()
        {
            Func<Task> action = async () => await _controller.Delete("C");

            await action.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Id must be a number");
        }

        [Fact]
        public async Task DeleteShouldThrowEntityNotFoundExceptionWhenIdDoesNotExists()
        {
            Func<Task> action = async () => await _controller.Delete(NonExistingIdSrt);

            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Animal not found");
        }

        [Fact]
        public async Task DeleteShouldThrowBadRequestExceptionWhenAnimalCannotBeDeleted()
        {
            Func<Task> action = async () => await _controller.Delete(NonDeletableIdStr);

            await action.Should().ThrowAsync<BadRequestException>()
                .WithMessage("This animal type cannot be deleted");
        }

        [Fact]
        public async Task DeleteShouldReturnNoContentWhenIdExistsAndCanBeDeleted()
        {
            IActionResult result = await _controller.Delete(ExistingIdStr);

            A.CallTo(() => _animalService.DeleteByIdAsync(ExistingId)).MustHaveHappened();
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
