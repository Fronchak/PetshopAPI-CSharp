using Microsoft.EntityFrameworkCore;
using PetShopAPIV2.Data;
using PetShopAPIV2.DTOs.Users;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Interfaces;

namespace PetShopAPIV2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Remove(user);
        }

        public ICollection<User> FindAll()
        {
            return _context.Users
                .ToList();
        }

        public User? FindByEmail(string email)
        {
            return _context.Users
                .Where((user) => user.Email == email)
                .FirstOrDefault();
        }

        public User? FindById(int id)
        {
            return _context.Users
                .Where((user) => user.Id == id)
                .Include((user) => user.UserRoles)
                .ThenInclude((userRole) => userRole.Role)
                .FirstOrDefault();
        }

        public void Save(User user)
        {
            _context.Users.Add(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
    }
}
