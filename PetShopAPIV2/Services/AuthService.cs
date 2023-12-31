﻿using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using PetShopAPIV2.DTOs.Roles;
using PetShopAPIV2.DTOs.Users;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Exceptions;
using PetShopAPIV2.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PetShopAPIV2.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IHttpContextAccessor accessor)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _accessor = accessor;
        }

        public async Task RegisterAsync(UserRegisterDTO userRegisterDTO)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password, salt);

            User user = new User();
            user.Email = userRegisterDTO.Email;
            user.Password = hashPassword;

            _userRepository.Save(user);
            await _userRepository.CommitAsync();
        }

        public async Task<TokenDTO> LoginAsync(UserLoginDTO userLoginDTO)
        {
            User? user = await _userRepository.FindByEmailAsync(userLoginDTO.Email);
            if (user == null)
            {
                throw new UnauthorizedException("Email or password invalid");
            }
            bool passowordMatch = BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, user.Password);
            if (!passowordMatch)
            {
                throw new UnauthorizedException("Email or password invalid");
            }
            user = await _userRepository.FindByIdAsync(user.Id);
            TokenDTO tokenDTO = new TokenDTO();
            tokenDTO.Access_Token = CreateToken(user);
            return tokenDTO;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())); ;
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            user.UserRoles.ToList().ForEach((userRole) =>
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            });

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:SecretKey").Value));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

        public async Task<User> GetAuthenticatedUserAsync()
        {
            HttpContext? context = _accessor.HttpContext;
            int userId = Convert.ToInt32(context?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            User user = await _userRepository.FindByIdAsync(userId);
            if(user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            return user;
        }

        public async Task<UserDTOWithRoles> GetAuthenticatedUserDTOAsync()
        {
            User user = await GetAuthenticatedUserAsync();
            UserDTOWithRoles dto = new UserDTOWithRoles();
            dto.Id = user.Id;
            dto.Email = user.Email;
            dto.Roles = user.UserRoles
                .Select((userRole) =>
                {
                    RoleDTO roleDTO = new RoleDTO();
                    roleDTO.Id = userRole.Role.Id;
                    roleDTO.Name = userRole.Role.Name;
                    return roleDTO;
                })
                .ToList();
            return dto;
        }
    }
}
