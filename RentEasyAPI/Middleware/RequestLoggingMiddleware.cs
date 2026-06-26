using System.Diagnostics;

namespace RentEasyAPI.Middleware
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;

            // Optional: Log incoming details before passing to the next handler
            _logger.LogInformation("HTTP Request Started: {Method} {Path}", request.Method, request.Path);

            try
            {
                // Pass execution to the next middleware in the pipeline
                await next(context);
            }
            finally
            {
                stopwatch.Stop();
                var response = context.Response;

                // Log completion metrics with performance timing
                _logger.LogInformation(
                    "HTTP Request Finished: {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
                    request.Method,
                    request.Path,
                    response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
