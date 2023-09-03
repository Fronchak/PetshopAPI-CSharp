using Microsoft.EntityFrameworkCore;
using PetShopAPIV2.Data;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Interfaces;

namespace PetShopAPIV2.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly DataContext _context;

        public PetRepository(DataContext context)
        {
            _context = context;
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Delete(Pet pet)
        {
            _context.Remove(pet);
        }

        public async Task<bool> ExistsAnyPetOfAnimalTypeAsync(int animalId)
        {
            return await _context.Pets.AnyAsync((pet) => pet.Animal.Id == animalId);
        }

        public async Task<List<Pet>> FindAllAsync()
        {
            return await _context.Pets
                .Include((pet) => pet.Animal)
                .Include((pet) => pet.Owner)
                .ToListAsync();
        }

        public async Task<Pet?> FindByIdAsync(int id)
        {
            return await _context.Pets.FindAsync(id);
        }

        public async Task<Pet?> FindByIdWithRelationshipsAsync(int id)
        {
            return await _context.Pets
                .Where((pet) => pet.Id == id)
                .Include((pet) => pet.Animal)
                .Include((pet) => pet.Owner)
                .FirstOrDefaultAsync();
        }

        public void Save(Pet pet)
        {
            _context.Pets.Add(pet);
        }

        public void Update(Pet pet)
        {
            _context.Update(pet);
        }
    }
}
