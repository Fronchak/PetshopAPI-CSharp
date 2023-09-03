using PetShopAPIV2.Entities;

namespace PetShopAPIV2.DTOs.Clients
{
    public class ClientDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ClientDTO() { }

        public ClientDTO(int id, string firstName, string lastName, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public ClientDTO(Client client) : this(
            client.Id, client.FirstName, client.LastName, client.Email
        ) { }
    }
}
