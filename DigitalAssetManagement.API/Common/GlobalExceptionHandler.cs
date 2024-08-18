using DigitalAssetManagement.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.API.Common
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Error at: {message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Detail = exception.Message
            };

            if (exception is NotFoundException)
            {
                problemDetails.Status = StatusCodes.Status404NotFound;
            }
            else if (exception is BadRequestException)
            {
                problemDetails.Status = StatusCodes.Status400BadRequest;
            }
            else
            {
                problemDetails.Status = StatusCodes.Status500InternalServerError;
            }

            httpContext.Response.StatusCode = problemDetails.Status!.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
