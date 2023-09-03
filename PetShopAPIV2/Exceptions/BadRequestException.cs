namespace PetShopAPIV2.Exceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message) : base(message, "Bad request", 400) { }
    }
}
