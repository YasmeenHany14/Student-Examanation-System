using Microsoft.AspNetCore.Mvc;
using IResult = Application.Common.ErrorAndResults.IResult;

namespace WebApi.Helpers.Extensions;

public static class ProblemDetailsExtensions
{
    public static ProblemDetails ToProblemDetails(this ActionResult<IResult> actionResult)
    {
        // if (actionResult is ObjectResult objectResult)
        // {
        //     return new ProblemDetails
        //     {
        //         Status = objectResult.StatusCode,
        //         Title = "An error occurred",
        //         Detail = objectResult.Value?.ToString() ?? "No details provided"
        //     };
        // }

        return new ProblemDetails
        {
            Status = 500,
            Title = "An unexpected error occurred",
            Detail = "Please try again later."
        };
    }
}
