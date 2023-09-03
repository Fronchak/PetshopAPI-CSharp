using PetShopAPIV2.Entities;

namespace PetShopAPIV2.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> FindByIdAsync(int id);

        Task<User?> FindByEmailAsync(string email);

        User? FindByEmail(string email);

        Task<ICollection<User>> FindAllAsync();

        void Save(User user);

        void Update(User user);

        void Delete(User user);
        Task CommitAsync();
    }
}
