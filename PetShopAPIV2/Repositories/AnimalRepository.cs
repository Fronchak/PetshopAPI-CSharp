using Microsoft.EntityFrameworkCore;
using PetShopAPIV2.Data;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Interfaces;

namespace PetShopAPIV2.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly DataContext _context;

        public AnimalRepository(DataContext context)
        {
            _context = context;
        }
        public async Task CommitAsync()
        {
            int commited = await _context.SaveChangesAsync();
            if (commited < 1)
            {
                throw new Exception("Erro ao salvar mudanças");
            }
        }

        public void Delete(Animal animal)
        {
            _context.Remove(animal);
        }

        public bool Exists(int id)
        {
            return _context.Animals.Any((animal) => animal.Id == id);
        }

        public async Task<ICollection<Animal>> FindAllAsync()
        {
            return await _context.Animals.ToListAsync();
        }

        public async Task<Animal?> FindByIdAsync(int id)
        {
            return await _context.Animals.FindAsync(id);
        }

        public Animal? FindByName(string name)
        {
            return _context.Animals
                .Where((animal) => animal.Name == name)
                .FirstOrDefault();
        }

        public async Task<Animal?> FindByNameAsync(string name)
        {
            return await _context.Animals
                .Where((animal) => animal.Name == name)
                .FirstOrDefaultAsync();
        }

        public void Save(Animal animal)
        {
            _context.Animals.Add(animal);
        }

        public void Update(Animal animal)
        {
            _context.Animals.Update(animal);
        }
    }
}
