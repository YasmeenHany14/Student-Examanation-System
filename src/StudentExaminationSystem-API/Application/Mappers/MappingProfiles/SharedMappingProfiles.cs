using Application.DTOs.CommonDtos;
using AutoMapper;
using Domain.DTOs;

namespace Application.Mappers.MappingProfiles;

public class SharedMappingProfiles : Profile
{
    public SharedMappingProfiles()
    {
        // DropdownInfraDto to DropdownAppDto
        CreateMap<DropdownInfraDto, DropdownAppDto>();
    }
}
