using System.Net;
using System.Text.Json;

namespace Forever.Api.Middleware
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
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";


                var respone = new
                {
                    Success = false,
                    Message = "Something went wrong",
                    Error = ex.Message
                };

                var json = JsonSerializer.Serialize(respone);

                await context.Response.WriteAsync(json);

            }

        }
    }
}
