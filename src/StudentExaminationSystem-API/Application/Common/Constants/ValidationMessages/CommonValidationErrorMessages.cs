﻿namespace Application.Common.Constants.ValidationMessages;

public static class CommonValidationErrorMessages
{
    public const string Required = "The {0} field is required.";
    public const string StringLength = "The {0} field must be a string with a maximum length of {1}.";
    public const string Range = "The {0} field must be between {1} and {2}.";
    public const string Email = "The {0} field is not a valid email address.";
    public const string Url = "The {0} field is not a valid URL.";
    public const string RegularExpression = "The {0} field is not in the correct format.";
    public const string NotEmpty = "The {0} field cannot be empty.";
    public const string NotNull = "The {0} field cannot be null.";
    public const string NameExists = "The name '{0}' already exists.";
    public const string BirthDateFuture = "The birth date cannot be in the future.";
    public const string InvalidId = "The field {0} ID is invalid.";
    public const string InvalidIdList = "The field {0} ID list is invalid.";
    public const string MaxCount = "The {0} field cannot have more than {1} items.";
    public const string DoesNotExist = "The {0} of ID {1} does not exist.";
    public const string ResourceNotFound = "The requested resource was not found.";
    public const string MaxLength = "The {0} field must have a maximum length of {1} characters.";
    public const string TotalSum = "Total {0} sum must be equal to {1}.";
    public const string GreaterThanOrEqual = "The {0} field must be greater than or equal to {1}.";
    public const string LessThanOrEqual = "The {0} field must be less than or equal to {1}.";
    public const string GreaterThan = "The {0} field must be greater than {1}.";
}
