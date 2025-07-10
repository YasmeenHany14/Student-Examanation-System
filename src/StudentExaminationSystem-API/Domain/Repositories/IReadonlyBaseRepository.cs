using Domain.Models.Common;

namespace Domain.Repositories;

public interface IReadonlyBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();
}
