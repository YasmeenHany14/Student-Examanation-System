using Application.Common.Constants.ValidationMessages;
using Application.DTOs.SubjectsDtos;
using Domain.Repositories;
using FluentValidation;

namespace Application.Validators.SubjectValidators;

public class UpdateSubjectValidator : AbstractValidator<UpdateSubjectAppDto>
{
    public UpdateSubjectValidator(IUnitOfWork unitOfWork)
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
                    {
                        var subjectId = (int)context.RootContextData["subjectId"];
                        return !await unitOfWork.SubjectRepository.CheckCodeUniqueAsync(dto.Code, subjectId);
                    })
                .WithMessage(s => string.Format(CommonValidationErrorMessages.NameExists, s.Code));
        });
    }
}