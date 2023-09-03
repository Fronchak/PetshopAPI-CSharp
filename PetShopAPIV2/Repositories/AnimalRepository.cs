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
        public void Commit()
        {
            int commited = _context.SaveChanges();
            if (commited < 1)
            {
                throw new Exception("Erro ao salvar mudanças");
            }
        }

        public void Delete(Animal animal)
        {
            _context.Remove(animal);
        }

        public ICollection<Animal> FindAll()
        {
            return _context.Animals.ToList();
        }

        public Animal FindById(int id)
        {
            return _context.Animals.Find(id);
        }

        public Animal FindByName(string name)
        {
            return _context.Animals
                .Where((animal) => animal.Name == name)
                .FirstOrDefault();
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
