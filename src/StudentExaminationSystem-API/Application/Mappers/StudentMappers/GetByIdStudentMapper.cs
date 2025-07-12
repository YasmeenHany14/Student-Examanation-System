using Application.DTOs.CommonDtos;
using Application.DTOs.StudentDtos;
using Domain.DTOs.CommonDtos;
using Domain.DTOs.StudentDtos;

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
}
