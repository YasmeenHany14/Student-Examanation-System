using Application.Common.ErrorAndResults;
using FluentValidation;
using FluentValidation.Internal;

namespace Application.Helpers;

public static class ValidationHelper
{
    public static async Task<Result> ValidateAndReportAsync<T>(IValidator<T> validator, T dto, string ruleSet = "Input")
    {
        var validationResult = await validator.ValidateAsync(
            dto,
            options => options.IncludeRuleSets(ruleSet)
        );
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure(new Error("ValidationError", errorMessage));
        }
        return Result.Success();
    }

    public static async Task<Result> ValidateAndReportAsync<T>(
        IValidator<T> validator,
        T dto,
        Action<ValidationContext<T>>? contextSetup = null,
        string ruleSet = "Input")
    {
        var context = new ValidationContext<T>(dto, new PropertyChain(), new RulesetValidatorSelector(new[] { ruleSet }));
        contextSetup?.Invoke(context);

        var validationResult = await validator.ValidateAsync(context);
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure(new Error("ValidationError", errorMessage));
        }
        return Result.Success();
    }
}
