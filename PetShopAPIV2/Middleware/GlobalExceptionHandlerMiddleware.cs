using PetShopAPIV2.Exceptions;
using System.Net;
using System.Text.Json;

namespace PetShopAPIV2.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException e)
            {
                context.Response.StatusCode = e.Status;
                ExceptionResponse exceptionResponse = e.GetExceptionResponse();
                string json = JsonSerializer.Serialize(exceptionResponse);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ExceptionResponse exceptionResponse = new ExceptionResponse("Internal server error", e.Message);
                string json = JsonSerializer.Serialize(exceptionResponse);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }

        }
    }
}
