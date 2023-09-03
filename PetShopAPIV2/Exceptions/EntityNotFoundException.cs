namespace PetShopAPIV2.Exceptions
{
    public class EntityNotFoundException : ApiException
    {
        public EntityNotFoundException(string msg) : base(msg, "Entity not found", 404) { }
    }
}
