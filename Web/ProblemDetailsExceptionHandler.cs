using ApplicationCore.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BackendLab01;

public class ProblemDetailsExceptionHandler(
    ProblemDetailsFactory factory, ILogger<ProblemDetailsExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ContactNotFoundException)
        {
            logger.Log(LogLevel.Information, $"Exception '{exception.Message}' handled!");
            var problem = factory.CreateProblemDetails(
                context,
                StatusCodes.Status404NotFound,
                "Contact service error!",
                "Service error",
                detail: exception.Message
            );
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true;
        }
        return false;
    }
}
