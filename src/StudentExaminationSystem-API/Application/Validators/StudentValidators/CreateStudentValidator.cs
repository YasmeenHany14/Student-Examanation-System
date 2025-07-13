using Application.Common.Constants.ValidationMessages;
using Application.DTOs.StudentDtos;
using Application.Validators.UserValdiators;
using Domain.Repositories;
using FluentValidation;

namespace Application.Validators.StudentValidators;

public class CreateStudentValidator : CreateUserValidator<CreateStudentAppDto>
{
    public CreateStudentValidator(IUnitOfWork unitOfWork) : base(unitOfWork.UserRepository)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleSet("CreateBusiness", () =>
        {
            // Add rules to check whether courses, and gender are valid
            RuleFor(x => x.CourseIds)
                .MustAsync(async (ids, cancellation) => 
                    await unitOfWork.SubjectRepository.CheckSubjectsExists(ids))
                .WithMessage(s => string.Format(CommonValidationErrorMessages.InvalidIdList, nameof(s.CourseIds)));
        
            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage(s => string.Format(CommonValidationErrorMessages.InvalidId, nameof(s.Gender)));
        });
    }
}
