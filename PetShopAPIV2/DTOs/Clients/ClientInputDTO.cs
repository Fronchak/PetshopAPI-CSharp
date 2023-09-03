using System.ComponentModel.DataAnnotations;

namespace PetShopAPIV2.DTOs.Clients
{
    public abstract class ClientInputDTO : IValidatableObject
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; } = string.Empty;

        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}
