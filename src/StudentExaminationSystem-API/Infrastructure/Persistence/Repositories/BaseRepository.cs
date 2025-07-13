using Domain.Models.Common;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.ResourceParameters;

namespace Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity>(DataContext context) : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{    
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    public virtual TEntity UpdateAsync(TEntity entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        return entity;
    }

    // TODO: ASK WHETHER I SHOULD PASS ID OR ENTITY
    public void DeleteAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    protected static async Task<PagedList<T>> CreateAsync<T>(
        IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
