using Application.Common.Constants.ValidationMessages;
using Application.DTOs.DifficultyProfileDtos;
using FluentValidation;

namespace Application.Validators.DifficultyProfileValidators;

public class DifficultyProfileValidator : AbstractValidator<CreateUpdateDifficultyProfileAppDto>
{
    public DifficultyProfileValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleSet("Input", () =>
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage(x => string.Format(CommonValidationErrorMessages.NotNull, nameof(x.Name)))
                .NotEmpty().WithMessage(x => string.Format(CommonValidationErrorMessages.NotEmpty, nameof(x.Name)))
                .MaximumLength(30).WithMessage(x => string.Format(CommonValidationErrorMessages.MaxLength, nameof(x.Name), 100));
            
            RuleFor(x => x.EasyQuestionsPercent)
                .InclusiveBetween(0, 100).WithMessage(x => string.Format(CommonValidationErrorMessages.Range, nameof(x.EasyQuestionsPercent), 0, 100));
            RuleFor(x => x.MediumQuestionsPercent)
                .InclusiveBetween(0, 100).WithMessage(x => string.Format(CommonValidationErrorMessages.Range, nameof(x.MediumQuestionsPercent), 0, 100));
            RuleFor(x => x.HardQuestionsPercent)
                .InclusiveBetween(0, 100).WithMessage(x => string.Format(CommonValidationErrorMessages.Range, nameof(x.HardQuestionsPercent), 0, 100));
            
            RuleFor(x => x.EasyQuestionsPercent + x.MediumQuestionsPercent + x.HardQuestionsPercent)
                .Equal(100).WithMessage(x => string.Format(CommonValidationErrorMessages.TotalSum, "questions percent", 100));
        });
    }
}
