using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace WebApi.Helpers.ExceptionHandlers;

public class SqlExceptionHandler : IExceptionHandler
{
    private readonly ILogger<SqlExceptionHandler> _logger;
    public SqlExceptionHandler(ILogger<SqlExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not SqlException sqlException)
            return false;
        
        _logger.LogError(
            sqlException,
            "Validation exception occurred: {Message}",
            sqlException.Message);

        
        
        
        var problemDetails = new ValidationProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Detail = sqlException.Errors.ToString(),
            Extensions = { { "TraceId", Activity.Current?.Id ?? httpContext.TraceIdentifier } }        };

        httpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
