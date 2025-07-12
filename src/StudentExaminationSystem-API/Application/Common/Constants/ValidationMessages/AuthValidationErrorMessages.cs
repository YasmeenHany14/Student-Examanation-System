namespace Application.Common.Constants.ValidationMessages;

public static class AuthValidationErrorMessages
{
    public const string InvalidCredentials = "Invalid email or password.";
    public const string PasswordsDoNotMatch = "Passwords do not match.";
    public const string PasswordRequired = "Password is required.";
    public const string PasswordMinLength = "Password must be at least {0} characters long.";
    public const string EmailRequired = "Email is required.";
    public const string InvalidEmailFormat = "Invalid email format.";
    public const string UserAlreadyExists = "A user with this email already exists.";
}
