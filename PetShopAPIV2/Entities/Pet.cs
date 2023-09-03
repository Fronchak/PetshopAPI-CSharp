namespace PetShopAPIV2.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public Animal Animal { get; set; }
        public Client Owner { get; set; }
    }
}
