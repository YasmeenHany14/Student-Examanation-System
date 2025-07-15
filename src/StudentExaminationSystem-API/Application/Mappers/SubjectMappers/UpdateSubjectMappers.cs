using Application.DTOs.SubjectsDtos;
using Domain.Models;

namespace Application.Mappers.SubjectMappers;

public static class UpdateSubjectMappers
{
    public static void UpdateEntityFromDto(this Subject subject, UpdateSubjectAppDto subjectDto)
    {
        subject.Code = subjectDto.Code ?? subject.Code;
        subject.Name = subjectDto.Name ?? subject.Name;
    }
}
