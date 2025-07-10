using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Helpers.ExceptionHandlers;

public class JsonReaderExceptionHandler : IExceptionHandler
{
    private readonly ILogger<JsonReaderExceptionHandler> _logger;
    public JsonReaderExceptionHandler(ILogger<JsonReaderExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not System.Text.Json.JsonException)
            return false;

        _logger.LogError(exception, exception.Message);
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid JSON format",
            Detail = "The request body contains invalid JSON."
        };
        httpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        httpContext.Response.StatusCode = problemDetails.Status.Value;
            

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}