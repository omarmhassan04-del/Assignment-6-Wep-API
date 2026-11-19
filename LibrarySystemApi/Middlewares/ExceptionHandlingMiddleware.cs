using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LibrarySystemApi.Errors;

namespace LibrarySystemApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogError(ex,
                        "Request {TraceId} failed after the response started; the response cannot be replaced",
                        context.TraceIdentifier);
                    throw;
                }

                _logger.LogError(ex,
                    "Unhandled exception while processing request {TraceId}",
                    context.TraceIdentifier);

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.Headers["X-Request-Id"] = context.TraceIdentifier;

                var problem = ExceptionProblemDetailsFactory.Create(ex, context, _environment);

                await context.Response.WriteAsJsonAsync
                (
                    problem,
                    options: null,
                    contentType: "application/problem+json",
                    cancellationToken: context.RequestAborted
                );
            }
        }
    }
}
