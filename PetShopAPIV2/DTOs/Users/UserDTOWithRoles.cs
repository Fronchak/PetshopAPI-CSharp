using PetShopAPIV2.DTOs.Roles;

namespace PetShopAPIV2.DTOs.Users
{
    public class UserDTOWithRoles
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<RoleDTO> Roles; 
    }
}
