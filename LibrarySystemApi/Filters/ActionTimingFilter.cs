using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LibrarySystemApi.Filters
{
    public class ActionTimingFilter : IAsyncActionFilter
    {
        private readonly ILogger<ActionTimingFilter> _logger;

        public ActionTimingFilter(ILogger<ActionTimingFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await next();
            }
            finally
            {
                stopwatch.Stop();

                _logger.LogInformation(
                    "MVC action {ActionName} took {ElapsedMilliseconds} ms",
                    context.ActionDescriptor.DisplayName,
                    stopwatch.Elapsed.TotalMilliseconds);
            }
        }
    }
}
