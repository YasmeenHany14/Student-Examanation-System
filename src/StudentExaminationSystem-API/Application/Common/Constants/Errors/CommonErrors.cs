using Application.Common.Constants.ValidationMessages;
using Application.Common.ErrorAndResults;

namespace Application.Common.Constants.Errors;

public class CommonErrors
{
    public static Error NotFound(string? customMessage = null) 
        => new("NotFound", customMessage ?? CommonValidationErrorMessages.ResourceNotFound);

    public static Error InvalidInput(string? customMessage = null) 
        => new("InvalidInput", customMessage ?? "The input provided is invalid.");

    public static Error Unauthorized(string? customMessage = null) 
        => new("Unauthorized", customMessage ?? "You are not authorized to perform this action.");

    public static Error InternalServerError(string? customMessage = null) 
        => new("InternalServerError", customMessage ?? "An internal server error occurred. Please try again later.");

    public static Error ValidationError(string? customMessage = null) 
        => new("ValidationError", customMessage ?? "There was a validation error with the provided data.");

    public static Error WrongCredentials(string? customMessage = null) 
        => new("WrongCredentials", customMessage ?? "The provided credentials are incorrect.");

    public static Error CannotGenerateToken(string? customMessage = null) 
        => new("CannotGenerateToken", customMessage ?? "Unable to login at this time. Please try again later.");

    public static Error InvalidRefreshToken(string? customMessage = null) 
        => new("InvalidRefreshToken", customMessage ?? "The provided token is invalid or has expired.");

    public static Error BadRequest(string? customMessage = null) 
        => new("BadRequest", customMessage ?? "The request could not be understood by the server due to malformed syntax.");
    
    // Generic method to create any error with custom message
    public static Error Create(string code, string message) 
        => new(code, message);

    // Method to override message of existing error
    public static Error WithMessage(Error baseError, string customMessage) 
        => new(baseError.Code, customMessage);
}
