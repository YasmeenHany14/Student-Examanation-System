using Application.Common.Constants.ValidationMessages;
using Application.DTOs.SubjectExamConfigDtos;
using Domain.Repositories;
using FluentValidation;

namespace Application.Validators.SubjectExamConfigValidators;
public class CreateSubjectExamConfigDtoValidator : AbstractValidator<CreateUpdateSubjectExamConfig>
{
    public CreateSubjectExamConfigDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleSet("Input", () =>
        {
            RuleFor(x => x.TotalQuestions)
                .NotNull().WithMessage(x => string.Format(CommonValidationErrorMessages.NotNull, nameof(CreateUpdateSubjectExamConfig.TotalQuestions)))
                .GreaterThanOrEqualTo(5).WithMessage(x => string.Format(CommonValidationErrorMessages.GreaterThanOrEqual, nameof(CreateUpdateSubjectExamConfig.TotalQuestions), 5))
                .LessThanOrEqualTo(100).WithMessage(x => string.Format(CommonValidationErrorMessages.LessThanOrEqual, nameof(CreateUpdateSubjectExamConfig.TotalQuestions), 100));
            
            RuleFor(x => x.DurationMinutes)
                .NotNull().WithMessage(x => string.Format(CommonValidationErrorMessages.NotNull, nameof(CreateUpdateSubjectExamConfig.DurationMinutes)))
                .GreaterThanOrEqualTo(10).WithMessage(x => string.Format(CommonValidationErrorMessages.GreaterThanOrEqual, nameof(CreateUpdateSubjectExamConfig.DurationMinutes), 5))
                .LessThanOrEqualTo(120).WithMessage(x => string.Format(CommonValidationErrorMessages.LessThanOrEqual, nameof(CreateUpdateSubjectExamConfig.DurationMinutes), 120));
            
            RuleFor(x => x.DifficultyProfileId)
                .NotNull().WithMessage(x => string.Format(CommonValidationErrorMessages.NotNull, nameof(CreateUpdateSubjectExamConfig.DifficultyProfileId)))
                .GreaterThan(0).WithMessage(x => string.Format(CommonValidationErrorMessages.GreaterThan, nameof(CreateUpdateSubjectExamConfig.DifficultyProfileId), 0));
        });
        
        
        RuleSet("CreateBusiness", () =>
        {
            RuleFor(x => x)
                .MustAsync(async (dto, _, context, _) =>
                {
                    var subjectId = (int)context.RootContextData["subjectId"];
                    return await unitOfWork.SubjectRepository.ExistsAsync(subjectId);
                })
                .WithMessage("Subject does not exist.");
        });
    }
}
