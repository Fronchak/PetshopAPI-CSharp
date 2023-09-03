using PetShopAPIV2.Entities;

namespace PetShopAPIV2.Interfaces
{
    public interface IAnimalRepository
    {
        Task<Animal?> FindByIdAsync(int id);

        Task<Animal?> FindByNameAsync(string name);

        Animal? FindByName(string name);

        bool Exists(int id);

        Task<ICollection<Animal>> FindAllAsync();

        void Save(Animal animal);

        void Update(Animal animal);

        void Delete(Animal animal);

        Task CommitAsync();
    }
}
