namespace PetShopAPIV2.Exceptions
{
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message) : base(message, "Unauthorized", 401) { }
    }
}
