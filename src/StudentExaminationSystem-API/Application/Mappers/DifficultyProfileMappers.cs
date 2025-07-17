using Application.DTOs.DifficultyProfileDtos;
using Domain.DTOs;
using Domain.Models;
using Shared.ResourceParameters;

namespace Application.Mappers;

public static class DifficultyProfileMappers
{
    public static DifficultyProfile ToEntity(this CreateUpdateDifficultyProfileAppDto dto)
    {
        return new DifficultyProfile
        {
            Name = dto.Name!,
            EasyPercentage = dto.EasyQuestionsPercent ?? 0,
            MediumPercentage = dto.MediumQuestionsPercent ?? 0,
            HardPercentage = dto.HardQuestionsPercent ?? 0
        };
    }

    public static void MapUpdate(this CreateUpdateDifficultyProfileAppDto dto, DifficultyProfile entity)
    {
        entity.Name = dto.Name!;
        entity.EasyPercentage = dto.EasyQuestionsPercent ?? 0;
        entity.MediumPercentage = dto.MediumQuestionsPercent ?? 0;
        entity.HardPercentage = dto.HardQuestionsPercent ?? 0;
    }

    public static PagedList<GetDifficultyProfileAppDto> ToListDto(this PagedList<GetDifficultyProfileInfraDto> subjects)
    {
        return new PagedList<GetDifficultyProfileAppDto>(
            subjects.Pagination,
            subjects.Data.Select(s => s.MapTo<GetDifficultyProfileInfraDto, GetDifficultyProfileAppDto>()).ToList()
        );
    }
}
