using PetShopAPIV2.Entities;

namespace PetShopAPIV2.Interfaces
{
    public interface IUserRepository
    {
        User? FindById(int id);

        User? FindByEmail(string email);

        ICollection<User> FindAll();

        void Save(User user);

        void Update(User user);

        void Delete(User user);

        void Commit();
    }
}
