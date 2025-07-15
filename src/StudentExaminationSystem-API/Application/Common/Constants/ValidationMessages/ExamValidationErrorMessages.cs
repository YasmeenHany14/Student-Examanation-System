namespace Application.Common.Constants.ValidationMessages;

public static class ExamValidationErrorMessages
{
    public const string ExamSubjectMismatch =  "You have a running exam for a different subject. Please complete the current exam before starting a new one.";
    public const string ExamNotFound = "Exam either expired or was not found.";
}
