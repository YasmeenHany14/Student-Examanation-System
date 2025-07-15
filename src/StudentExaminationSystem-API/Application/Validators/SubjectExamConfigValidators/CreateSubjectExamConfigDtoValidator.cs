using Application.DTOs.SubjectExamConfigDtos;
using Domain.Repositories;
using FluentValidation;

namespace Application.Validators.SubjectExamConfigValidators;
public class CreateSubjectExamConfigDtoValidator : AbstractValidator<CreateSubjectExamConfig>
{
    public CreateSubjectExamConfigDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleSet("Input", () =>
        {
            // RuleFor(x => x.TotalQuestions)
            //     .GreaterThan(5)
            //     .WithMessage("Total questions must be greater than 0.");
            //
            // RuleFor(x => x.DurationMinutes)
            //     .GreaterThan(10)
            //     .WithMessage("sss");
            // RuleFor(x => x.DifficultyProfileId)
            //     .GreaterThan(0)
            //     .WithMessage("sss");
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
