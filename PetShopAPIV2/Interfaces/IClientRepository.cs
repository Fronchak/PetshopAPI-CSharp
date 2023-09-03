using PetShopAPIV2.Entities;

namespace PetShopAPIV2.Interfaces
{
    public interface IClientRepository
    {
        Client? FindById(int id);

        Client? FindByEmail(string email);

        ICollection<Client> FindAll();

        void Save(Client client);

        void Update(Client client);

        void Delete(Client client);

        void Commit();
    }
}
