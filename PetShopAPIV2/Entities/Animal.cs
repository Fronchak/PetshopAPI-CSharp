namespace PetShopAPIV2.Entities
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Animal() { }

        public Animal(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
