using PetShopAPIV2.DTOs.Animals;
using PetShopAPIV2.Entities;

namespace PetShopAPIV2.DTOs.Clients
{
    public class ClientPetOutputDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public AnimalDTO Animal { get; set; }
        public ClientPetOutputDTO() { }

        public ClientPetOutputDTO(int Id, string Name, int Age)
        {
            this.Id = Id;
            this.Name = Name;
            this.Age = Age;
        }

        public ClientPetOutputDTO(Pet pet) : this(pet.Id, pet.Name, pet.Age) { }
    }
}
