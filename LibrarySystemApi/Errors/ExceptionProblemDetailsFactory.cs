using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace LibrarySystemApi.Errors
{
    public static class ExceptionProblemDetailsFactory
    {
        public static ProblemDetails Create(Exception exception, HttpContext context, IHostEnvironment environment)
        {
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = environment.IsDevelopment() ? exception.Message : "Please try again later.",
                Instance = context.Request.Path
            };

            problem.Extensions["traceId"] = context.TraceIdentifier;
            return problem;
        }
    }
}
