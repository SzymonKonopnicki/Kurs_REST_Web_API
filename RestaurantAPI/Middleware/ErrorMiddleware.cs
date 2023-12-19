using System.Diagnostics;

namespace RestaurantAPI.Middleware
{
    public class ErrorMiddleware : IMiddleware
    {
        private ILogger<ErrorMiddleware> _logger;
        public ErrorMiddleware(ILogger<ErrorMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                _logger.LogError(ex, ex.Message);

                await context.Response.WriteAsync("An unexpected error occurred");
            }
        }
    }
}
