using Domain.DTOs;
using Domain.Models;
using Shared.ResourceParameters;

namespace Domain.Repositories;

public interface IDifficultyProfileRepository : IBaseRepository<DifficultyProfile>
{
    Task<IEnumerable<GetDifficultyProfileInfraDto>> GetAllAsync();
    Task<PagedList<GetDifficultyProfileInfraDto>> GetAllAsync(DifficultyProfileResourceParameters resourceParameters);
    Task<GetDifficultyProfileInfraDto?> GetByIdAsync(int id);
    Task<DifficultyProfile?> GetEntityByIdAsync(int id);
}
