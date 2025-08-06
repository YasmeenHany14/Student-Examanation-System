namespace Application.Common.Constants.ValidationMessages;

public static class ExamValidationErrorMessages
{
    public const string ExamSubjectMismatch =  "You have a running exam for a different subject. Please complete the current exam before starting a new one.";
    public const string ExamNotFound = "Exam either expired or was not found.";
    public const string AlreadyTookExam = "You have already taken this exam.";
    public const string DoesNotHaveSubject = "You do not have this subject registered.";
    public const string ExamNotSubmitted = "You have not submitted the exam yet. Please submit it before viewing results.";
    public const string ExamPendingEvaluation = "Your exam is pending evaluation. Please wait for the results to be available.";
    public const string ExamNotPendingEvaluation = "Exam is not pending evaluation. It has either been evaluated or is still running.";
    public const string NotEnoughQuestions = "Not enough questions available to generate exam, refer to the admin.";
    public const string ExamConfigDoesNotExist = "Exam configuration does not exist for the subject, please contact the administrator.";
}
