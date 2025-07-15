using Application.DTOs.SubjectsDtos;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Application.Mappers.SubjectMappers;

public static class CreateSubjectMappers
{
    public static Subject ToEntity(this CreateSubjectAppDto dto)
    {
        return new Subject
        {
            Name = dto.Name,
            Code = dto.Code
        };
    }

    public static void MapUpdate(this UpdateSubjectAppDto dto, Subject subject)
    {
        if (!string.IsNullOrWhiteSpace(dto.Name))
            subject.Name = dto.Name;
        if (!string.IsNullOrWhiteSpace(dto.Code))
            subject.Code = dto.Code;
    }
}
