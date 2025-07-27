using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.SubjectsDtos;
using Application.Helpers;
using Application.Mappers.SubjectMappers;
using Domain.DTOs;
using Domain.Repositories;
using Domain.UserContext;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shared.ResourceParameters;

namespace Application.Services;

public class SubjectService(
    IUnitOfWork unitOfWork,
    IValidator<CreateSubjectAppDto> createDtoValidator,
    IValidator<UpdateSubjectAppDto> updateDtoValidator,
    IUserContext userContext
    ) : ISubjectService
{
    public async Task<Result<PagedList<GetSubjectAppDto>>> GetAllAsync(SubjectResourceParameters resourceParameters)
    {
        var subjects = await unitOfWork.SubjectRepository.GetAllAsync(resourceParameters);
        return Result<PagedList<GetSubjectAppDto>>.Success(
            subjects.ToListDto());
    }
    
    public async Task<Result<IEnumerable<GetSubjectAppDto>>> GetAllAsync(string? userId)
    {
        IEnumerable<GetSubjectInfraDto> subjects;
        if (string.IsNullOrEmpty(userId))
            subjects = await unitOfWork.SubjectRepository.GetAllAsync();
        else
            subjects = await unitOfWork.SubjectRepository.GetAllAsync(userId);
        return Result<IEnumerable<GetSubjectAppDto>>.Success(subjects.Select(s => s.ToGetSubjectAppDto()));
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
            return Result<int>.Failure(CommonErrors.InternalServerError());
        return Result<int>.Success(subject.Id);
    }

    public async Task<Result<GetSubjectAppDto?>> GetByIdAsync(int id)
    {
        var subject = await unitOfWork.SubjectRepository.GetByIdAsync(id);
        if (subject == null)
            return Result<GetSubjectAppDto?>.Failure(CommonErrors.NotFound());
        return Result<GetSubjectAppDto?>.Success(subject.ToGetSubjectAppDto());
    }

    public async Task<Result<bool>> UpdateAsync(int id, UpdateSubjectAppDto subjectAppDto)
    {
        var validationResult = await ValidationHelper.ValidateAndReportAsync(updateDtoValidator,
            subjectAppDto,
            ctx => { ctx.RootContextData["subjectId"] = id; },
            "CreateBusiness");
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);
        
        var subject = await unitOfWork.SubjectRepository.GetEntityByIdAsync(id);
        subject.UpdateEntityFromDto(subjectAppDto);
        unitOfWork.SubjectRepository.UpdateAsync(subject);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var subject = await unitOfWork.SubjectRepository.GetEntityByIdAsync(id);
        if (subject == null)
            return Result<bool>.Failure(CommonErrors.NotFound());

        unitOfWork.SubjectRepository.DeleteAsync(subject);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());
        return Result<bool>.Success(true);
    }
}
