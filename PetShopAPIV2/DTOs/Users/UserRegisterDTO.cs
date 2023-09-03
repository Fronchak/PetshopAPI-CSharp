using PetShopAPIV2.Entities;
using PetShopAPIV2.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PetShopAPIV2.DTOs.Users
{
    public class UserRegisterDTO : IValidatableObject
    {
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [MinLength(4, ErrorMessage = "Password must have at least 4 characteres")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IEnumerable<ValidationResult> result = new List<ValidationResult>();
            IUserRepository userRepository = validationContext.GetRequiredService<IUserRepository>();
            User? user = userRepository.FindByEmail(Email);
            if (user != null)
            {
                ValidationResult validationResult = new ValidationResult("Email is already been used", new[] { nameof(Email) });
                yield return validationResult;
            }
        }
    }
}
