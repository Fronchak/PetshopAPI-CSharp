using PetShopAPIV2.Entities;

namespace PetShopAPIV2.Interfaces
{
    public interface IPetRepository
    {
        Task<List<Pet>> FindAllAsync();
        Task<Pet?> FindByIdAsync(int id);

        Task<Pet?> FindByIdWithRelationshipsAsync(int id);

        Task<bool> ExistsAnyPetOfAnimalTypeAsync(int animalId);

        void Save(Pet pet);

        void Update(Pet pet);

        void Delete(Pet pet);

        Task CommitAsync();
    }
}
