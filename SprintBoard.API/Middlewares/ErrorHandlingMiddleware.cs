using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SprintBoard.Entities;
using SprintBoard.Interfaces.IRepo;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SprintBoard.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async System.Threading.Tasks.Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async System.Threading.Tasks.Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            // Use IServiceScopeFactory to create a scope
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var errorLogRepository = scope.ServiceProvider.GetRequiredService<IErrorLogRepository>();

                // Log the error to the database
                var errorLog = new ErrorLog
                {
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    Source = exception.Source
                };

                await errorLogRepository.LogErrorAsync(errorLog);
            }

            // Return a generic error response
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
        }
    }
}
