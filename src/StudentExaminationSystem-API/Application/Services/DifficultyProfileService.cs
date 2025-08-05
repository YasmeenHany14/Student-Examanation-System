using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.DifficultyProfileDtos;
using Application.Helpers;
using Application.Mappers;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Domain.Repositories;
using FluentValidation;
using Shared.ResourceParameters;

namespace Application.Services;

public class DifficultyProfileService(
    IUnitOfWork unitOfWork,
    IValidator<CreateUpdateDifficultyProfileAppDto> validator,
    IMapper mapper
) : IDifficultyProfileService
{
    public async Task<Result<IEnumerable<GetDifficultyProfileAppDto>>> GetAllAsync()
    {
        var profiles = await unitOfWork.DifficultyProfileRepository.GetAllAsync();
        var mappedProfiles = mapper.Map<IEnumerable<GetDifficultyProfileAppDto>>(profiles);
        return Result<IEnumerable<GetDifficultyProfileAppDto>>.Success(mappedProfiles);
    }

    public async Task<Result<PagedList<GetDifficultyProfileAppDto>>> GetAllAsync(
        DifficultyProfileResourceParameters resourceParameters)
    {
        var pagedProfiles = await unitOfWork.DifficultyProfileRepository.GetAllAsync(resourceParameters);
        var mappedProfiles = mapper.Map<PagedList<GetDifficultyProfileAppDto>>(pagedProfiles);
        return Result<PagedList<GetDifficultyProfileAppDto>>.Success(mappedProfiles);
    }

    public async Task<Result<GetDifficultyProfileAppDto?>> GetByIdAsync(int id)
    {
        var profile = await unitOfWork.DifficultyProfileRepository.GetByIdAsync(id);
        if (profile == null)
            return Result<GetDifficultyProfileAppDto?>.Failure(CommonErrors.NotFound());
        
        var mappedProfile = mapper.Map<GetDifficultyProfileAppDto>(profile);
        return Result<GetDifficultyProfileAppDto?>.Success(mappedProfile);
    }

    public async Task<Result<int>> CreateAsync(CreateUpdateDifficultyProfileAppDto createDto, int id)
    {
        var validationResult = await ValidationHelper.ValidateAndReportAsync(validator, createDto, "CreateBusiness");
        if (!validationResult.IsSuccess)
            return Result<int>.Failure(validationResult.Error);

        var entity = mapper.Map<DifficultyProfile>(createDto);
        await unitOfWork.DifficultyProfileRepository.AddAsync(entity);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<int>.Failure(CommonErrors.InternalServerError());
        return Result<int>.Success(entity.Id);
    }

    public async Task<Result<bool>> UpdateAsync(int id, CreateUpdateDifficultyProfileAppDto updateDto)
    {
        var entity = await unitOfWork.DifficultyProfileRepository.GetEntityByIdAsync(id);

        mapper.Map(updateDto, entity);
        unitOfWork.DifficultyProfileRepository.UpdateAsync(entity);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var entity = await unitOfWork.DifficultyProfileRepository.GetEntityByIdAsync(id);
        if (entity == null)
            return Result<bool>.Failure(CommonErrors.NotFound());

        unitOfWork.DifficultyProfileRepository.DeleteAsync(entity);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());
        
        return Result<bool>.Success(true);
    }
}
