namespace PetShopAPIV2.Exceptions
{
    public class ApiException : Exception
    {
        public string Error { get; set; }
        public int Status { get; set; }

        public ApiException(string message, string error, int status) : base(message)
        {
            Error = error;
            Status = status;
        }

        public ExceptionResponse GetExceptionResponse()
        {
            return new ExceptionResponse(Error, Message);
        }
    }
}
