using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.StudentDtos;
using Application.Helpers;
using Application.Mappers.StudentMappers;
using Domain.Repositories;
using FluentValidation;

namespace Application.Services;

public class StudentService(
    IValidator<CreateStudentAppDto> createStudentValidator,
    IUnitOfWork unitOfWork,
    IAuthService authService
    ) : IStudentService
{ 
    public async Task<Result<GetStudentAppDto>> AddAsync(CreateStudentAppDto studentAppDto)
    {
        var validationResult = await ValidationHelper.ValidateAndReportAsync(createStudentValidator, studentAppDto, "CreateBusiness");
        if(!validationResult.IsSuccess)
            return Result<GetStudentAppDto>.Failure(validationResult.Error);

        var createUserResult = await authService.RegisterAsync(studentAppDto.ToCreateUserAppDto());
        if (!createUserResult.IsSuccess)
            return Result<GetStudentAppDto>.Failure(createUserResult.Error);
        
        var newStudent = studentAppDto.ToEntity(createUserResult.Value.Id);
        await unitOfWork.StudentRepository.AddAsync(newStudent);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<GetStudentAppDto>.Failure(CommonErrors.InternalServerError);
        
        await authService.AddToRoleAsync("Student", createUserResult.Value, null);
        
        return Result<GetStudentAppDto>.Success(studentAppDto.ToGetStudentAppDto(createUserResult.Value.Id));
    }

    public async Task<Result<GetStudentByIdAppDto>> GetByIdAsync(string id)
    {
        var student = await unitOfWork.StudentRepository.GetByIdAsync(id);
        if (student == null)
            return Result<GetStudentByIdAppDto>.Failure(CommonErrors.NotFound);
        
        return Result<GetStudentByIdAppDto>.Success(student.ToGetStudentAppDto());
    }

    public Task<Result> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
