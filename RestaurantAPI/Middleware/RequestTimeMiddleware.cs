using System.Diagnostics;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private ILogger<RequestTimeMiddleware> _logger;
        private Stopwatch _stopWatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger, Stopwatch stopWatch)
        {
            _logger = logger;
            _stopWatch = stopWatch;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopWatch.Start();
            await next.Invoke(context);
            _stopWatch.Stop();

            if (_stopWatch.ElapsedMilliseconds / 1000 > 4)
                _logger.LogWarning($"Method {context.Request.Method} Path '{context.Request.Path}' duration: {_stopWatch.ElapsedMilliseconds / 1000} sec.");
        }
    }
}
