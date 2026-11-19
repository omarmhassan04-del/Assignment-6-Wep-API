using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LibrarySystemApi.Middlewares
{
    public class RequestIdHeaderMiddleware : IMiddleware
    {
        private readonly ILogger<RequestIdHeaderMiddleware> _logger;

        public RequestIdHeaderMiddleware(ILogger<RequestIdHeaderMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var requestId = context.TraceIdentifier;
            context.Response.Headers["X-Request-Id"] = requestId;

            _logger.LogDebug(
                "assigned request ID {RequestId} to {Method} {Path}",
                requestId,
                context.Request.Method,
                context.Request.Path
            );

            await next(context);
        }
    }
}
