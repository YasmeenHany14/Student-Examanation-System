﻿using Application.Common.Constants.Errors;
using Application.Common.Constants.ValidationMessages;
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
            nameof(CommonErrors.NotFound) or "NotFound" => new NotFoundObjectResult(error),
            nameof(CommonErrors.Unauthorized) or "Unauthorized" => new UnauthorizedObjectResult(error),
            nameof(CommonErrors.InvalidInput) or "InvalidInput" => new BadRequestObjectResult(error),
            nameof(CommonErrors.ValidationError) or "ValidationError" =>
                error.Description == CommonValidationErrorMessages.ResourceNotFound
                    ? new NotFoundObjectResult(error)
                    : new BadRequestObjectResult(error),
            nameof(CommonErrors.InternalServerError) or "InternalServerError" => new ObjectResult(error) { StatusCode = 500 },
            nameof(CommonErrors.WrongCredentials) or "WrongCredentials" => new UnauthorizedObjectResult(error),
            nameof(CommonErrors.CannotGenerateToken) or "CannotGenerateToken" => new ObjectResult(error) { StatusCode = 500 },
            nameof(CommonErrors.InvalidRefreshToken) or "InvalidRefreshToken" => new UnauthorizedObjectResult(error),
            nameof(AuthErrors.Forbidden) or "Forbidden" => new ForbidResult(),
            _ => new ObjectResult(error) { StatusCode = 500 }
        };
    }
}
