using System.Net;
using System.Text.Json;

namespace FieldExpenseManager.FieldExpense.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception Occurred! Path={Path}, Method={Method}, Query={Query}",
                 context.Request.Path, context.Request.Method, context.Request.QueryString);

                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                string message = _env.IsDevelopment()
                    ? $"Internal Server Error: {ex.Message}"
                    : "An internal server error has occurred.";

                var errorResponse = new { error = message };
                var result = JsonSerializer.Serialize(errorResponse);

                await response.WriteAsync(result);
            }
        }
    }
    public static class ErrorHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}