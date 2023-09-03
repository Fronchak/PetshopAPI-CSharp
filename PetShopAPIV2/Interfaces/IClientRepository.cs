using PetShopAPIV2.Entities;

namespace PetShopAPIV2.Interfaces
{
    public interface IClientRepository
    {
        Task<Client?> FindByIdAsync(int id);

        Task<Client?> FindByIdWithPetsAsync(int id);

        Task<Client?> FindByEmailAsync(string email);

        Client? FindByEmail(string email);

        bool Exists(int id);

        Task<ICollection<Client>> FindAllAsync();

        void Save(Client client);

        void Update(Client client);

        void Delete(Client client);

        Task CommitAsync();
    }
}
