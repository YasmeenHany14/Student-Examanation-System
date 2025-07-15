using Application.DTOs.ExamDtos;
using FluentValidation;

namespace Application.Validators.ExamValidators;

public class GenerateExamRequestValidator : AbstractValidator<GenerateExamRequestDto>
{
    public GenerateExamRequestValidator()
    {
        RuleFor(x => x.SubjectId)
            .NotEmpty()
            .WithMessage("Subject ID is required")
            .GreaterThan(0)
            .WithMessage("Subject ID must be greater than 0");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required")
            .NotNull()
            .WithMessage("User ID cannot be null");
    }

    private bool BeValidUserId(string userId)
    {
        // Check if the user ID is not empty, null, or just whitespace
        if (string.IsNullOrWhiteSpace(userId))
            return false;

        // Additional validation for user ID format if needed
        // For example, check if it's a valid GUID format or has specific length requirements
        return userId.Trim().Length > 0;
    }
}
