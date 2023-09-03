using PetShopAPIV2.Entities;

namespace PetShopAPIV2.Interfaces
{
    public interface IPetRepository
    {
        List<Pet> FindAll();
        Pet? FindById(int id);

        Pet? FindByIdWithRelationships(int id);

        bool ExistsAnyPetOfAnimalType(int animalId);

        void Save(Pet pet);

        void Update(Pet pet);

        void Delete(Pet pet);

        void Commit();
    }
}
