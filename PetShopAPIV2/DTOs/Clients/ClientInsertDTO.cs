using PetShopAPIV2.Entities;
using PetShopAPIV2.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PetShopAPIV2.DTOs.Clients
{
    public class ClientInsertDTO : ClientInputDTO
    {
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IEnumerable<ValidationResult> result = new List<ValidationResult>();
            IClientRepository clientRepository = validationContext.GetRequiredService<IClientRepository>();
            Client? client = clientRepository.FindByEmail(Email);
            if (client != null)
            {
                ValidationResult validationResult = new ValidationResult("Email is already been used", new[] { nameof(Email) });
                yield return validationResult;
            }
        }
    }
}
