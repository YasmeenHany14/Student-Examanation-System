using Domain.Models.Common;

namespace Domain.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> AddAsync(TEntity entity);
    TEntity UpdateAsync(TEntity entity);
    void DeleteAsync(TEntity entity);
    // TODO: Violation of Interface Segregation Principle, Table with composite key doesnt need this
    Task<TEntity?> FindByIdAsync(int id);
    // Task<IPagedList<TEntity>> GetAllAsync(BaseResourceParameters resourceParameters);
}
