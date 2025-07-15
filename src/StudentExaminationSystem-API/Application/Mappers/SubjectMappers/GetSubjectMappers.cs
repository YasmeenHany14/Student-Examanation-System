using Application.DTOs.SubjectsDtos;
using Domain.DTOs;
using Domain.Models;
using Shared.ResourceParameters;

namespace Application.Mappers.SubjectMappers;

public static class GetSubjectMappers
{
    public static GetSubjectAppDto ToGetSubjectAppDto(this GetSubjectInfraDto subject)
    {
        return new GetSubjectAppDto
        {
            Id = subject.Id,
            Name = subject.Name,
            Code = subject.Code
        };
    }
    
    public static GetSubjectAppDto ToGetSubjectAppDto(this Subject subject)
    {
        return new GetSubjectAppDto
        {
            Id = subject.Id,
            Name = subject.Name,
            Code = subject.Code
        };
    }
    
    public static PagedList<GetSubjectAppDto> ToListDto(this PagedList<GetSubjectInfraDto> subjects)
    {
        var count = subjects.TotalCount;
        var pageNumber = subjects.CurrentPage;
        var pageSize = subjects.PageSize;
        var totalPages = subjects.TotalPages;
        return new PagedList<GetSubjectAppDto>(
            subjects.Select(s => ToGetSubjectAppDto(s)).ToList(),
            count,
            pageNumber,
            pageSize,
            totalPages
        );
    }
}
