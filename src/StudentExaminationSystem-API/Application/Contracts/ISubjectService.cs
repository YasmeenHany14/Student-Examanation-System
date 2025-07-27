using Application.Common.ErrorAndResults;
using Application.DTOs.SubjectsDtos;
using Shared.ResourceParameters;

namespace Application.Contracts;

public interface ISubjectService
{
    Task<Result<PagedList<GetSubjectAppDto>>> GetAllAsync(SubjectResourceParameters resourceParameters);
    Task<Result<IEnumerable<GetSubjectAppDto>>> GetAllAsync(string? userId = null);
    Task<Result<int>> CreateAsync(CreateSubjectAppDto subjectAppDto);
    Task<Result<GetSubjectAppDto?>> GetByIdAsync(int id);
    Task<Result<bool>> UpdateAsync(int id, UpdateSubjectAppDto subjectAppDto);
    Task<Result<bool>> DeleteAsync(int id);
}
