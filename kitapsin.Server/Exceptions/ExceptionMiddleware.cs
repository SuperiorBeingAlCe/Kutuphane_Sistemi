using System.Net;
using System.Text.Json;

namespace kitapsin.Server.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception ex)
            {
                // Hata detaylarını logla
                _logger.LogError(ex, "Beklenmeyen bir hata oluştu. Path: {Path}, Method: {Method}", context.Request.Path, context.Request.Method);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    error = "Bir hata oluştu.",
                    detail = ex.Message,
                    path = context.Request.Path,
                    method = context.Request.Method
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
