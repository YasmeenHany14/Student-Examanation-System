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

    }
}
