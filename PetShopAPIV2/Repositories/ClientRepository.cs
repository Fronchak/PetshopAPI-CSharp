﻿using PetShopAPIV2.Data;
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
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Delete(Client client)
        {
            _context.Clients.Remove(client);
        }

        public ICollection<Client> FindAll()
        {
            return _context.Clients.ToList();
        }

        public Client? FindByEmail(string email)
        {
            return _context.Clients
                .Where((client) => client.Email == email)
                .FirstOrDefault();
        }

        public Client? FindById(int id)
        {
            return _context.Clients.Find(id);
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