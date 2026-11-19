using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LibrarySystemApi.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            Exception failure = null;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                failure = ex;
                throw;
            }
            finally
            {
                stopwatch.Stop();

                if (failure is null)
                {
                    _logger.LogInformation(
                        "HTTP {Method} {Path} returned {StatusCode} in {ElapsedMilliseconds} ms",
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode,
                        stopwatch.Elapsed.TotalMilliseconds
                    );
                }
                else
                {
                    _logger.LogError(
                        "HTTP {Method} {Path} failed after {ElapsedMilliseconds} ms",
                        context.Request.Method,
                        context.Request.Path,
                        stopwatch.Elapsed.TotalMilliseconds
                    );
                }
            }
        }
    }
}
