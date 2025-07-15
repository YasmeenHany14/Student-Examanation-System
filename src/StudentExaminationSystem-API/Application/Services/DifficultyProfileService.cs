using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.DifficultyProfileDtos;
using Application.Helpers;
using Application.Mappers;
using Domain.DTOs;
using Domain.Models;
using Domain.Repositories;
using FluentValidation;
using Shared.ResourceParameters;

namespace Application.Services;

public class DifficultyProfileService(
    IUnitOfWork unitOfWork,
    IValidator<CreateUpdateDifficultyProfileAppDto> validator
) : IDifficultyProfileService
{
    public async Task<Result<IEnumerable<GetDifficultyProfileAppDto>>> GetAllAsync()
    {
        var profiles = await unitOfWork.DifficultyProfileRepository.GetAllAsync();
        return Result<IEnumerable<GetDifficultyProfileAppDto>>.Success(
             profiles.Select(p => p.MapTo<GetDifficultyProfileInfraDto, GetDifficultyProfileAppDto>()));
    }

    public async Task<Result<PagedList<GetDifficultyProfileAppDto>>> GetAllAsync(
        DifficultyProfileResourceParameters resourceParameters)
    {
        var pagedProfiles = await unitOfWork.DifficultyProfileRepository.GetAllAsync(resourceParameters);
        var mappedProfiles = pagedProfiles.Select(p => 
            p.MapTo<GetDifficultyProfileInfraDto, GetDifficultyProfileAppDto>());
        return Result<PagedList<GetDifficultyProfileAppDto>>.Success(pagedProfiles.ToListDto());
    }

    public async Task<Result<GetDifficultyProfileAppDto?>> GetByIdAsync(int id)
    {
        var profile = await unitOfWork.DifficultyProfileRepository.GetByIdAsync(id);
        if (profile == null)
            return Result<GetDifficultyProfileAppDto?>.Failure(CommonErrors.NotFound);
        return Result<GetDifficultyProfileAppDto?>.Success(profile.MapTo<GetDifficultyProfileInfraDto, GetDifficultyProfileAppDto>());
    }

    public async Task<Result<int>> CreateAsync(CreateUpdateDifficultyProfileAppDto createDto, int id)
    {
        var entity = createDto.ToEntity();
        await unitOfWork.DifficultyProfileRepository.AddAsync(entity);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<int>.Failure(CommonErrors.InternalServerError);
        return Result<int>.Success(entity.Id);
    }

    public async Task<Result<bool>> UpdateAsync(int id, CreateUpdateDifficultyProfileAppDto updateDto)
    {
        var entity = await unitOfWork.DifficultyProfileRepository.GetEntityByIdAsync(id);

        updateDto.MapUpdate(entity);
        unitOfWork.DifficultyProfileRepository.UpdateAsync(entity);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError);
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
