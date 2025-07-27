using Application.Common.Constants.ValidationMessages;
using Application.DTOs.QuestionDtos;
using FluentValidation;

namespace Application.Validators.QuestionsAndChoicesValidators;

public class CreateQuestionDtoValidator : AbstractValidator<CreateQuestionAppDto>
{
    public CreateQuestionDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleSet("Input", () =>
        {
            RuleFor(q => q.Content)
                .NotEmpty().WithMessage(q => string.Format(CommonValidationErrorMessages.NotEmpty, nameof(q.Content)))
                .MaximumLength(300).WithMessage(q => string.Format(CommonValidationErrorMessages.MaxLength, nameof(q.Content), 300));
                
            RuleFor(q => q.Choices)
                .NotNull().WithMessage(c => string.Format(CommonValidationErrorMessages.NotNull, nameof(c.Choices)))
                .Must(choices => choices != null && choices.Count >= 2 && choices.Count <= 5)
                .WithMessage(q => string.Format(CommonValidationErrorMessages.Range, nameof(q.Choices), 2, 5));

            RuleFor(q => q.Choices)
                .Must(choices => choices != null && choices.Count(c => c.IsCorrect) == 1)
                .WithMessage(QuestionValidationMessages.OneCorrectAnswerRequired);

            RuleForEach(q => q.Choices).SetValidator(new CreateChoiceDtoValidator());
        });
    }
}
