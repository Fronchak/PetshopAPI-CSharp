using PetShopAPIV2.DTOs.Animals;
using PetShopAPIV2.DTOs.Clients;
using PetShopAPIV2.Entities;

namespace PetShopAPIV2.DTOs.Pets
{
    public class PetOutputDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public AnimalDTO Animal { get; set; }
        public ClientSimpleDTO Owner { get; set; }

        public PetOutputDTO() { }

        public PetOutputDTO(int Id, string Name, int Age)
        {
            this.Id = Id;
            this.Name = Name;
            this.Age = Age;
        }

        public PetOutputDTO(Pet pet) : this(pet.Id, pet.Name, pet.Age) { }
    }
}
