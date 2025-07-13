using Application.Common.ErrorAndResults;
using Application.DTOs.SubjectsDtos;
using Shared.ResourceParameters;

namespace Application.Contracts;

public interface ISubjectService
{
    Task<Result<PagedList<GetSubjectAppDto>>> GetAllAsync(SubjectResourceParameters resourceParameters);
    Task<IEnumerable<GetSubjectAppDto>> GetAllAsync();
    Task<Result<int>> CreateAsync(CreateSubjectAppDto subjectAppDto);
    Task<Result<GetSubjectAppDto?>> GetByIdAsync(int id);
    Task<Result<int>> UpdateAsync(int id, UpdateSubjectAppDto subjectAppDto);
    Task<Result<bool>> DeleteAsync(int id);
}
