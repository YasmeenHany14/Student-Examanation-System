using Application.Common.Constants.ValidationMessages;
using Application.DTOs.QuestionChoiceDtos;
using FluentValidation;

namespace Application.Validators.QuestionsAndChoicesValidators;

public class CreateChoiceDtoValidator : AbstractValidator<CreateQuestionChoiceAppDto>
{
    public CreateChoiceDtoValidator()
    {            
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleSet("Input", () =>
        {
            RuleFor(c => c.Content)
                .NotNull().WithMessage(c => string.Format(CommonValidationErrorMessages.NotNull, nameof(c.Content)))
                .NotEmpty().WithMessage(c => string.Format(CommonValidationErrorMessages.NotEmpty, nameof(c.Content)))
                .MaximumLength(30).WithMessage(c => string.Format(CommonValidationErrorMessages.MaxLength, nameof(c.Content), 150));
        });
    }
}
