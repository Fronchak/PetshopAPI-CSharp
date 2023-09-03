namespace PetShopAPIV2.Exceptions
{
    public class ExceptionResponse
    {
        public string Error { get; set; }
        public string Message { get; set; }

        public ExceptionResponse() { }

        public ExceptionResponse(string error, string message)
        {
            Error = error;
            Message = message;
        }

    }
}
