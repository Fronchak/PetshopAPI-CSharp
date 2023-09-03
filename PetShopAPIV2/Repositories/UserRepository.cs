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
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Delete(User user)
        {
            _context.Remove(user);
        }

        public async Task<ICollection<User>> FindAllAsync()
        {
            return await _context.Users
                .ToListAsync();
        }

        public User? FindByEmail(string email)
        {
            return _context.Users
                .Where((user) => user.Email == email)
                .FirstOrDefault();
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _context.Users
                .Where((user) => user.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> FindByIdAsync(int id)
        {
            return await _context.Users
                .Where((user) => user.Id == id)
                .Include((user) => user.UserRoles)
                .ThenInclude((userRole) => userRole.Role)
                .FirstOrDefaultAsync();
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
