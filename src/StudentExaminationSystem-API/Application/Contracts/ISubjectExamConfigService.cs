using Application.DTOs.SubjectExamConfigDtos;
using Application.Common.ErrorAndResults;

namespace Application.Contracts;
public interface ISubjectExamConfigService
{
    Task<Result<GetSubjectExamConfigAppDto?>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateUpdateSubjectExamConfig dto, int id);
    Task<Result<bool>> UpdateAsync(int id, CreateUpdateSubjectExamConfig dto);
}
