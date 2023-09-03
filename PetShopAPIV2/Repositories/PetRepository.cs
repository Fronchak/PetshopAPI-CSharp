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
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Delete(Pet pet)
        {
            _context.Remove(pet);
        }

        public bool ExistsAnyPetOfAnimalType(int animalId)
        {
            return _context.Pets.Any((pet) => pet.Animal.Id == animalId);
        }

        public List<Pet> FindAll()
        {
            return _context.Pets
                .Include((pet) => pet.Animal)
                .Include((pet) => pet.Owner)
                .ToList();
        }

        public Pet? FindById(int id)
        {
            return _context.Pets.Find(id);
        }

        public Pet? FindByIdWithRelationships(int id)
        {
            return _context.Pets
                .Where((pet) => pet.Id == id)
                .Include((pet) => pet.Animal)
                .Include((pet) => pet.Owner)
                .FirstOrDefault();
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
