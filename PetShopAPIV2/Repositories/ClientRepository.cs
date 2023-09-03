using Microsoft.EntityFrameworkCore;
using PetShopAPIV2.Data;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Interfaces;

namespace PetShopAPIV2.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context)
        {
            _context = context;
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Delete(Client client)
        {
            _context.Clients.Remove(client);
        }

        public bool Exists(int id)
        {
            return _context.Clients.Any((client) => client.Id == id);
        }

        public async Task<ICollection<Client>> FindAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public Client? FindByEmail(string email)
        {
            return _context.Clients
                .Where((client) => client.Email == email)
                .FirstOrDefault();
        }

        public async Task<Client?> FindByEmailAsync(string email)
        {
            return await _context.Clients
                .Where((client) => client.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<Client?> FindByIdAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task<Client?> FindByIdWithPetsAsync(int id)
        {
            return await _context.Clients
                .Where((client) => client.Id == id)
                .Include((client) => client.Pets)
                .ThenInclude((pet) => pet.Animal)
                .FirstOrDefaultAsync();
        }

        public void Save(Client client)
        {
            _context.Clients.Add(client);
        }

        public void Update(Client client)
        {
            _context.Clients.Update(client);
        }
    }
}
