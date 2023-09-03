using PetShopAPIV2.DTOs.Roles;
using PetShopAPIV2.DTOs.Users;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Exceptions;
using PetShopAPIV2.Interfaces;

namespace PetShopAPIV2.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDTOWithRoles FindById(int id)
        {
            User? user = _userRepository.FindById(id);
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            UserDTOWithRoles dto = new UserDTOWithRoles();
            dto.Id = user.Id;
            dto.Email = user.Email;
            dto.Roles = user.UserRoles
                .Select((userRole) => userRole.Role)
                .Select((role) =>
                {
                    RoleDTO roleDTO = new RoleDTO();
                    roleDTO.Id = role.Id;
                    roleDTO.Name = role.Name;
                    return roleDTO;
                })
                .ToList();
            return dto;
        }

        private UserDTO MapToDTO(User user)
        {
            UserDTO dto = new UserDTO();
            dto.Id = user.Id;
            dto.Email = user.Email;
            return dto;
        }

        public ICollection<UserDTO> FindAll()
        {
            ICollection<User> users = _userRepository.FindAll();
            ICollection<UserDTO> dtos = users.ToList()
                .Select((user) => MapToDTO(user))
                .ToList();
            return dtos;
        }

        public void DeleteById(int id)
        {
            User? user = _userRepository.FindById(id);
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            _userRepository.Delete(user);
            _userRepository.Commit();
        }
    }
}
