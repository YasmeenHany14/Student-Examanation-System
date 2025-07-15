using Application.Common.ErrorAndResults;
using Application.DTOs.DifficultyProfileDtos;
using Shared.ResourceParameters;

namespace Application.Contracts;

public interface IDifficultyProfileService
{
    Task<Result<IEnumerable<GetDifficultyProfileAppDto>>> GetAllAsync();
    Task<Result<PagedList<GetDifficultyProfileAppDto>>> GetAllAsync(DifficultyProfileResourceParameters resourceParameters);
    Task<Result<GetDifficultyProfileAppDto?>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateUpdateDifficultyProfileAppDto dto, int id);
    Task<Result<bool>> UpdateAsync(int id, CreateUpdateDifficultyProfileAppDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}
