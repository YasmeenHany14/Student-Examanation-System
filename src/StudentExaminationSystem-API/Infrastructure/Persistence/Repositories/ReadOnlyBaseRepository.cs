using Domain.Models.Common;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ReadonlyBaseRepository<TEntity>(DataContext context): IReadonlyBaseRepository<TEntity> 
    where TEntity : BaseEntity
{
    // public async Task<IEnumerable<TEntity>> GetAllAsync()
    // {
    //     return await context.Set<TEntity>().ToListAsync();
    // }

    public async Task<IEnumerable<TDto>> GetAllAsync<TDto>(Enum e) where TDto : BaseEntity
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TDto>> GetAllAsync<TDto>() where TDto : BaseEntity
    {
        throw new NotImplementedException();
    }
}
