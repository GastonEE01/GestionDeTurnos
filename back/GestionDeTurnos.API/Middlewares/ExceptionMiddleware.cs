using System.Net;
using System.Text.Json;

namespace GestionDeTurnos.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // ejecuta el controller/UseCase
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context,ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Mapeo segun el mensaje o tipo de excepcion
            var statusCode = exception switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound, // 404
                ArgumentException => HttpStatusCode.BadRequest, // 400
                InvalidOperationException => HttpStatusCode.BadRequest, // 400
                _ => HttpStatusCode.InternalServerError, // 500 
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        
        }
    }
}
