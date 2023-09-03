using PetShopAPIV2.Entities;

namespace PetShopAPIV2.Interfaces
{
    public interface IAnimalRepository
    {
        Animal? FindById(int id);

        Animal? FindByName(string name);

        bool Exists(int id);

        ICollection<Animal> FindAll();

        void Save(Animal animal);

        void Update(Animal animal);

        void Delete(Animal animal);

        void Commit();
    }
}
