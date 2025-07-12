using Application.Common.Constants.ValidationMessages;
using Application.DTOs.AuthDtos;
using FluentValidation;

namespace Application.Validators.UserValdiators;

public class RefreshRequestValidator : AbstractValidator<RefreshRequestDto>
{
    public RefreshRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleSet("Input", () =>
        {
            RuleFor(x => x.RefreshToken)
                .NotNull().WithMessage(AuthValidationMessages.RefreshTokenRequired)
                .NotEmpty().WithMessage(AuthValidationMessages.RefreshTokenRequired);
            
            RuleFor(x => x.AccessToken)
                .NotNull().WithMessage(AuthValidationMessages.AccessTokenRequired)
                .NotEmpty().WithMessage(AuthValidationMessages.AccessTokenRequired);
        });
    }
}
