using Domain.Models.Common;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ReadonlyBaseRepository<TEntity>(DataContext context): IReadonlyBaseRepository<TEntity> 
    where TEntity : BaseEntity
{
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await context.Set<TEntity>().ToListAsync();
    }
}
