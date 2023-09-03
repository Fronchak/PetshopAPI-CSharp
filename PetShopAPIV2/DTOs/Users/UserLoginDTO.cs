using System.ComponentModel.DataAnnotations;

namespace PetShopAPIV2.DTOs.Users
{
    public class UserLoginDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
