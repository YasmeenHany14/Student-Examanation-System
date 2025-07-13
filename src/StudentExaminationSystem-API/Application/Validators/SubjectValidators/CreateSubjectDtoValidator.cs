using Application.Common.Constants.ValidationMessages;
using Application.DTOs.SubjectsDtos;
using Domain.Repositories;
using FluentValidation;

namespace Application.Validators.SubjectValidators;

public class CreateSubjectDtoValidator : AbstractValidator<CreateSubjectAppDto>
{
    public CreateSubjectDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleSet("Input", () =>
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage(string.Format(CommonValidationErrorMessages.NotEmpty, nameof(CreateSubjectAppDto.Name)))
                .MaximumLength(100).WithMessage(string.Format(CommonValidationErrorMessages.StringLength, nameof(CreateSubjectAppDto.Name), 100));
            
            RuleFor(s => s.Code)
                .NotEmpty().WithMessage(string.Format(CommonValidationErrorMessages.NotEmpty, nameof(CreateSubjectAppDto.Code)))
                .MaximumLength(5).WithMessage(string.Format(CommonValidationErrorMessages.StringLength, nameof(CreateSubjectAppDto.Code), 5));
        });
        
        RuleSet("CreateBusiness", () =>
        {
            RuleFor(s => s)
                .MustAsync(async (dto, _, context, _) =>
                    !await unitOfWork.SubjectRepository.CheckCodeUniqueAsync(dto.Code))
                .WithMessage(s => string.Format(CommonValidationErrorMessages.NameExists, s.Code));
        });
    }
}
