using PetShopAPIV2.Entities;
using PetShopAPIV2.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PetShopAPIV2.DTOs.Pets
{
    public class PetInputDTO : IValidatableObject
    {

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public int OwnerId { get; set; }
        public int AnimalId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IEnumerable<ValidationResult> result = new List<ValidationResult>();
            IAnimalRepository animalRepository = validationContext.GetRequiredService<IAnimalRepository>();
            bool animalExists = animalRepository.Exists(AnimalId);
            if (!animalExists)
            {
                ValidationResult validationResult = new ValidationResult("Animal does not exists", new[] { nameof(AnimalId) });
                yield return validationResult;
            };
            IClientRepository clientRepository = validationContext.GetRequiredService<IClientRepository>();
            bool clientExists = clientRepository.Exists(OwnerId);
            if(!clientExists)
            {
                ValidationResult validationResult = new ValidationResult("Client does not exits", new[] { nameof(OwnerId) });
                yield return validationResult;
            }
        }
    }
}
