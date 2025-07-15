using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.StudentDtos;
using Application.Helpers;
using Application.Mappers.StudentMappers;
using Domain.Models;
using Domain.Repositories;
using FluentValidation;
using Shared.ResourceParameters;

namespace Application.Services;

public class StudentService(
    IValidator<CreateStudentAppDto> createStudentValidator,
    IUnitOfWork unitOfWork,
    IAuthService authService
    ) : IStudentService
{
    private IStudentService _studentServiceImplementation;

    public async Task<Result<PagedList<GetStudentByIdAppDto>>> GetAllAsync(StudentResourceParameters resourceParameters)
    {
        var students = await unitOfWork.StudentRepository.GetAllAsync(resourceParameters);
        return Result<PagedList<GetStudentByIdAppDto>>.Success(students.ToListDto());
    }

    public async Task<Result<string>> AddAsync(CreateStudentAppDto studentAppDto)
    {
        var validationResult = await ValidationHelper.ValidateAndReportAsync(createStudentValidator, studentAppDto, "CreateBusiness");
        if(!validationResult.IsSuccess)
            return Result<string>.Failure(validationResult.Error);

        var createUserResult = await authService.RegisterAsync(studentAppDto.ToCreateUserAppDto());
        if (!createUserResult.IsSuccess)
            return Result<string>.Failure(createUserResult.Error);
        
        var newStudent = studentAppDto.ToEntity(createUserResult.Value.Id);
        await unitOfWork.StudentRepository.AddAsync(newStudent);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<string>.Failure(CommonErrors.InternalServerError());
        
        await authService.AddToRoleAsync("Student", createUserResult.Value, null);
        
        return Result<string>.Success(createUserResult.Value.Id);
    }

    public async Task<Result<GetStudentByIdAppDto>> GetByIdAsync(string id)
    {
        var student = await unitOfWork.StudentRepository.GetByIdAsync(id);
        if (student == null)
            return Result<GetStudentByIdAppDto>.Failure(CommonErrors.NotFound());
        
        return Result<GetStudentByIdAppDto>.Success(student.ToGetStudentAppDto());
    }

    public Task<Result<bool>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> AddStudentSubjectAsync(string studentId, int subjectId)
    {
        var student = await unitOfWork.StudentRepository.FindByUserIdAsync(studentId);
        if (student == null)
            return Result<bool>.Failure(CommonErrors.NotFound());
        
        var studentSubjectRepository = unitOfWork.GetRepository<StudentSubject>();
        studentSubjectRepository.AddAsync(new StudentSubject
        {
            StudentId = student.Id,
            SubjectId = subjectId
        });
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());
        return Result<bool>.Success(true);
    }
}
