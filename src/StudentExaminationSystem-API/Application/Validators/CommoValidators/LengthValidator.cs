using FluentValidation;

namespace Application.Validators.CommoValidators;

public static class LengthValidator
{
    public static IRuleBuilderOptions<T, string> NameValidation<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("")
            .Length(5, 20).WithMessage("");
    }
}
