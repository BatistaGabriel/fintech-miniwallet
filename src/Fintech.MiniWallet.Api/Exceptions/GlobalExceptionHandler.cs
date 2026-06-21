using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.MiniWallet.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private const string INVALID_RESOURCE_TITLE = "Resource Not Found";
    private const string INVALID_ARGUMENT_TILE = "Invalid Request";
    private const string GENERIC_TILE = "An unexpected error occurred";

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            InvalidOperationException => (StatusCodes.Status404NotFound, INVALID_RESOURCE_TITLE),
            ArgumentException => (StatusCodes.Status400BadRequest, INVALID_ARGUMENT_TILE),
            _ => (StatusCodes.Status500InternalServerError, GENERIC_TILE)
        };

        ProblemDetails problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}