using System.Reflection;
using Application.DTOs.CommonDtos;
using Application.DTOs.StudentDtos;
using Domain.DTOs.CommonDtos;
using Domain.DTOs.StudentDtos;
using Shared.ResourceParameters;

namespace Application.Mappers.StudentMappers;

public static class GetByIdStudentMapper
{
    public static GetStudentByIdAppDto ToGetStudentAppDto(
        this GetStudentByIdInfraDto getStudentByIdInfraDto)
    {
        return new GetStudentByIdAppDto
        {
            Id = getStudentByIdInfraDto.Id,
            Name = getStudentByIdInfraDto.Name,
            JoinDate = getStudentByIdInfraDto.JoinDate,
            Birthdate = getStudentByIdInfraDto.Birthdate,
            Courses = getStudentByIdInfraDto.Courses
                .Select(c => c.MapTo<DropdownInfraDto, DropdownAppDto>())
                .ToList()
        };
    }
    
    private static GetStudentByIdAppDto ToListDto(this GetStudentByIdInfraDto owner)
    {
        return new GetStudentByIdAppDto
        {
            Id = owner.Id,
            Name = owner.Name,
        };
    }
    
    public static PagedList<GetStudentByIdAppDto> ToListDto(this PagedList<GetStudentByIdInfraDto> owners)
    {
        var count = owners.TotalCount;
        var pageNumber = owners.CurrentPage;
        var pageSize = owners.PageSize;
        var totalPages = owners.TotalPages;
        return new PagedList<GetStudentByIdAppDto>(
            owners.Select(s => ToListDto(s)).ToList(),
            count,
            pageNumber,
            pageSize,
            totalPages
        );
    }
}
