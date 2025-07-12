using Application.Common.Constants.ValidationMessages;
using Application.DTOs.UserDtos;
using Domain.Repositories;
using FluentValidation;

namespace Application.Validators.UserValdiators;

public class CreateUserValidator<T> : AbstractValidator<T> where T : CreateUserAppDto
{
    public CreateUserValidator(IUserRepository userRepository)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleSet("Input", () =>
        {
            RuleFor(u => u.FirstName)
                .NotNull().WithMessage(string.Format(CommonValidationErrorMessages.NotNull, nameof(CreateUserAppDto.FirstName)))
                .NotEmpty().WithMessage(string.Format(CommonValidationErrorMessages.NotEmpty, nameof(CreateUserAppDto.FirstName)))
                .MaximumLength(50).WithMessage(string.Format(CommonValidationErrorMessages.StringLength, nameof(CreateUserAppDto.FirstName), 50));

            RuleFor(u => u.LastName)
                .NotNull().WithMessage(string.Format(CommonValidationErrorMessages.NotNull, nameof(CreateUserAppDto.LastName)))
                .NotEmpty().WithMessage(string.Format(CommonValidationErrorMessages.NotEmpty, nameof(CreateUserAppDto.LastName)))
                .MaximumLength(50).WithMessage(string.Format(CommonValidationErrorMessages.StringLength, nameof(CreateUserAppDto.LastName), 50));
            RuleFor(x => x.Email)
                .NotNull().WithMessage(string.Format(CommonValidationErrorMessages.NotNull, nameof(CreateUserAppDto.Email)))
                .NotEmpty().WithMessage(string.Format(CommonValidationErrorMessages.NotEmpty, nameof(CreateUserAppDto.Email)))
                .EmailAddress().WithMessage(AuthValidationMessages.InvalidEmail);

            RuleFor(x => x.Password)
                .NotNull().WithMessage(string.Format(CommonValidationErrorMessages.NotNull,
                    nameof(CreateUserAppDto.Password)))
                .NotEmpty().WithMessage(string.Format(CommonValidationErrorMessages.NotEmpty,
                    nameof(CreateUserAppDto.Password)))
                .MinimumLength(6).WithMessage(string.Format(AuthValidationMessages.InvalidLength, 6))
                .Matches(@"[A-Z]").WithMessage(AuthValidationMessages.UpperCaseRequired)
                .Matches(@"[0-9]").WithMessage(AuthValidationMessages.DigitRequired)
                .Matches(@"[!@#$%^&*(),.?""{}|<>]").WithMessage(AuthValidationMessages.SpecialCharacterRequired);
            
            RuleFor(x => x.ConfirmPassword)
                .NotNull().WithMessage(string.Format(CommonValidationErrorMessages.NotNull, nameof(CreateUserAppDto.ConfirmPassword)))
                .NotEmpty().WithMessage(string.Format(CommonValidationErrorMessages.NotEmpty, nameof(CreateUserAppDto.ConfirmPassword)))
                .Equal(x => x.Password).WithMessage(AuthValidationMessages.PasswordsDoNotMatch);
        });
        
        RuleSet("CreateBusiness", () =>
        {
            RuleFor(x => x.Email)
                .MustAsync(async (email, cancellation) =>
                    !await userRepository.CheckUserExistsAsync(email))
                .WithMessage(AuthValidationMessages.UserAlreadyExists);
        });
    }    
}
