using Application.DTOs.DifficultyProfileDtos;
using FluentValidation;

namespace Application.Validators.DifficultyProfileValidators;

public class DifficultyProfileValidator : AbstractValidator<CreateUpdateDifficultyProfileAppDto>
{
    public DifficultyProfileValidator()
    {
        RuleSet("Input", () =>
        {
            //TODO: Write valdiation rules
            // rule for not null not empty max string for name
            // rule for sum of all 3 configs is 100
            // rule for each config to be between 0 and 100
        });
    }
}
