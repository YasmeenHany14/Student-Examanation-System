using System.Reflection;
using Application.DTOs.CommonDtos;
using Application.DTOs.StudentDtos;
using Domain.DTOs;
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

        return new PagedList<GetStudentByIdAppDto>(
            students.Pagination,
            students.Data.Select(s => ToListDto(s)).ToList()
        );
    }
}
