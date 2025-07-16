using Application.Common.Constants.ValidationMessages;
using Application.DTOs.ExamDtos;
using Domain.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;

namespace Application.Validators.ExamValidators;

public class GenerateExamRequestValidator : AbstractValidator<GenerateExamRequestDto>
{
    public GenerateExamRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;        
        RuleSet("Business", () =>
        {
            RuleFor(x => x.SubjectId)
                .MustAsync(async (dto, _, cancellation) =>
                    await unitOfWork.StudentRepository.IsSubjectAvailableAsync(dto.UserId, dto.SubjectId))
                .WithMessage(ExamValidationErrorMessages.DoesNotHaveSubject)
                .MustAsync(async (dto, _, cancellation) =>
                    ! await unitOfWork.ExamHistoryRepository.ExamExistsAsync(dto.UserId, dto.SubjectId))
                .WithMessage(ExamValidationErrorMessages.AlreadyTookExam);
        });
    }
}
