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
    
    private static GetStudentByIdAppDto ToListDto(this GetStudentByIdInfraDto student)
    {
        return new GetStudentByIdAppDto
        {
            Id = student.Id,
            Name = student.Name,
            IsActive = student.IsActive
        };
    }
    
    public static PagedList<GetStudentByIdAppDto> ToListDto(this PagedList<GetStudentByIdInfraDto> students)
    {
        var count = students.TotalCount;
        var pageNumber = students.CurrentPage;
        var pageSize = students.PageSize;
        var totalPages = students.TotalPages;
        return new PagedList<GetStudentByIdAppDto>(
            students.Select(s => ToListDto(s)).ToList(),
            count,
            pageNumber,
            pageSize,
            totalPages
        );
    }
}
