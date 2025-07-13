using Domain.Models.Common;

namespace Domain.Repositories;

public interface IReadonlyBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<IEnumerable<TDto>> GetAllAsync<TDto>(Enum e)
        where TDto : BaseEntity;
    
    Task<IEnumerable<TDto>> GetAllAsync<TDto>()
        where TDto : BaseEntity;
}
