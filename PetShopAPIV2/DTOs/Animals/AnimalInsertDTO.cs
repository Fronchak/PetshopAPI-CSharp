using PetShopAPIV2.Entities;
using PetShopAPIV2.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PetShopAPIV2.DTOs.Animals
{
    public class AnimalInsertDTO : IValidatableObject
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IEnumerable<ValidationResult> result = new List<ValidationResult>();
            IAnimalRepository animalRepository = validationContext.GetRequiredService<IAnimalRepository>();
            Animal? animal = animalRepository.FindByName(Name);
            if (animal != null)
            {
                ValidationResult validationResult = new ValidationResult("Name is already been used", new[] { nameof(Name) });
                yield return validationResult;
            };
        }
    }
}
