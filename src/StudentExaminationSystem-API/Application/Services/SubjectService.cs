using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.SubjectsDtos;
using Application.Helpers;
using Application.Mappers.SubjectMappers;
using Domain.Repositories;
using FluentValidation;
using Shared.ResourceParameters;

namespace Application.Services;

public class SubjectService(
    IUnitOfWork unitOfWork,
    IValidator<CreateSubjectAppDto> createDtoValidator
    ) : ISubjectService
{
    public async Task<Result<PagedList<GetSubjectAppDto>>> GetAllAsync(SubjectResourceParameters resourceParameters)
    {
        var subjects = await unitOfWork.SubjectRepository.GetAllAsync(resourceParameters);
        return Result<PagedList<GetSubjectAppDto>>.Success(
            subjects.ToListDto());
    }
    
    public async Task<IEnumerable<GetSubjectAppDto>> GetAllAsync()
    {
        var subjects = await unitOfWork.SubjectRepository.GetAllAsync();
        return subjects.Select(s => s.ToGetSubjectAppDto());
    }

    public async Task<Result<int>> CreateAsync(CreateSubjectAppDto subjectAppDto)
    {
        var validationResult = await ValidationHelper.
            ValidateAndReportAsync(createDtoValidator, subjectAppDto, "CreateBusiness");
        if (!validationResult.IsSuccess)
            return Result<int>.Failure(validationResult.Error);

        var subject = subjectAppDto.ToEntity();
        await unitOfWork.SubjectRepository.AddAsync(subject);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<int>.Failure(CommonErrors.InternalServerError);
        return Result<int>.Success(subject.Id);
    }

    public async Task<Result<GetSubjectAppDto?>> GetByIdAsync(int id)
    {
        var subject = await unitOfWork.SubjectRepository.GetByIdAsync(id);
        if (subject == null)
            return Result<GetSubjectAppDto?>.Failure(CommonErrors.NotFound);
        return Result<GetSubjectAppDto?>.Success(subject.ToGetSubjectAppDto());
    }

    public async Task<Result<int>> UpdateAsync(int id, UpdateSubjectAppDto subjectAppDto)
    {
        // var subject = await unitOfWork.SubjectRepository.GetByIdAsync(id);
        // if (subject == null)
        //     return Result<int>.Failure("Subject not found.");
        // subjectAppDto.MapUpdate(subject);
        // var result = await unitOfWork.SaveChangesAsync();
        // if (result <= 0)
        //     return Result<int>.Failure("Failed to update subject.");
        // return Result<int>.Success(subject.Id);
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}