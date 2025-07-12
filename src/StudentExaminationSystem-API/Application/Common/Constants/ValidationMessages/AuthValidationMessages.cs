namespace Application.Common.Constants.ValidationMessages;

public static class AuthValidationMessages
{
    // Register
    public const string InvalidEmail = "Invalid email format.";
    public const string InvalidLength = "Password must be at least {0} characters long.";
    public const string PasswordsDoNotMatch = "Passwords do not match.";
    public const string UpperCaseRequired = "Password must contain at least one uppercase letter.";
    public const string UserAlreadyExists = "User with this email already exists.";
    public const string DigitRequired = "Password must contain at least one digit.";
    public const string SpecialCharacterRequired = "Password must contain at least one special character.";
    
    // Login
    public const string InvalidCredentials = "Invalid email or password.";
    public const string UserNotFound = "User not found.";
    public const string IncorrectPassword = "Incorrect password.";
    public const string TokenExpired = "Token has expired.";
    public const string TokenInvalid = "Token is invalid.";
    public const string RefreshTokenNotFound = "Refresh token not found.";
    public const string RefreshTokenInvalid = "Refresh token is invalid.";
    
    // Refresh Token
    public const string RefreshTokenRequired = "Refresh token is required.";
    public const string AccessTokenRequired = "Token is required.";
}
