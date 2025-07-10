using Application.Common.ErrorAndResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Helpers.Extensions;

public static class ResultExtension
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new OkResult();
        
        return result.Error.MapErrorResult();
    } 
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);
        return result.Error.MapErrorResult();
    }

    private static IActionResult MapErrorResult(this Error error)
    {
        return error.Code switch
        {
            // nameof(ErrorMessages.NotFound) or "NotFound" => new NotFoundObjectResult(error),
            // nameof(ErrorMessages.Unauthorized) or "Unauthorized" => new UnauthorizedObjectResult(error),
            // nameof(ErrorMessages.InvalidInput) or "InvalidInput" => new BadRequestObjectResult(error),
            // nameof(ErrorMessages.ValidationError) or "ValidationError" =>
            //     error.Description == CommonValidationErrorMessages.ResourceNotFound
            //         ? new NotFoundObjectResult(error)
            //         : new BadRequestObjectResult(error),
            // nameof(ErrorMessages.InternalServerError) or "InternalServerError" => new ObjectResult(error) { StatusCode = 500 },
            // nameof(ErrorMessages.WrongCredentials) or "WrongCredentials" => new UnauthorizedObjectResult(error),
            // nameof(ErrorMessages.CannotGenerateToken) or "CannotGenerateToken" => new ObjectResult(error) { StatusCode = 500 },
            // nameof(ErrorMessages.InvalidRefreshToken) or "InvalidRefreshToken" => new UnauthorizedObjectResult(error),
            _ => new ObjectResult(error) { StatusCode = 500 }
        };
    }
}
