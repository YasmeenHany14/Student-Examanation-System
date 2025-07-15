using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.SubjectExamConfigDtos;
using Application.Helpers;
using Application.Mappers;
using Application.Mappers.SubjectConfigMappers;
using Domain.DTOs;
using Domain.Repositories;
using FluentValidation;

namespace Application.Services;

public class SubjectExamConfigService(
    IUnitOfWork unitOfWork,
    IValidator<CreateSubjectExamConfig> configValidator
    ) : ISubjectExamConfigService
{
    public async Task<Result<GetSubjectExamConfigAppDto?>> GetByIdAsync(int id) // includes difficulty profile
    {
        var config = await unitOfWork.SubjectExamConfigRepository.GetByIdAsync(id);
        if (config == null)
            return Result<GetSubjectExamConfigAppDto?>.Failure(CommonErrors.NotFound());

        var configDto = config.MapTo<GetSubjectExamConfigInfraDto, GetSubjectExamConfigAppDto>();
        return Result<GetSubjectExamConfigAppDto?>.Success(configDto);
    }

    public async Task<Result<int>> CreateAsync(
        CreateSubjectExamConfig createDto,
        int id)
    {
        var validationResult = await ValidationHelper.ValidateAndReportAsync(configValidator,
            createDto,
            ctx => { ctx.RootContextData["subjectId"] = id; },
            "CreateBusiness");
        
        if (!validationResult.IsSuccess)
            return Result<int>.Failure(validationResult.Error);

        var configEntity = createDto.ToEntity(id);
        await unitOfWork.SubjectExamConfigRepository.AddAsync(configEntity);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<int>.Failure(CommonErrors.InternalServerError());
        return Result<int>.Success(configEntity.SubjectId);
    }

    public async Task<Result<bool>> UpdateAsync(int id, UpdateSubjectExamConfigAppDto updateDto)
    {
        var config = await unitOfWork.SubjectExamConfigRepository.FindAsync(id);
        
        updateDto.MapUpdate(config);
        unitOfWork.SubjectExamConfigRepository.UpdateAsync(config);
        
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());
        
        return Result<bool>.Success(true);
    }
}
