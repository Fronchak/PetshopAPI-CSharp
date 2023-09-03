using PetShopAPIV2.Entities;

namespace PetShopAPIV2.DTOs.Animals
{
    public class AnimalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public AnimalDTO() { }

        public AnimalDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public AnimalDTO(Animal animal) : this(animal.Id, animal.Name) { }
    }
}
