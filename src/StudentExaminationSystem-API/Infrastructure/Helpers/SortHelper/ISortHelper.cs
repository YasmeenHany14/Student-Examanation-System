namespace Infrastructure.Helpers.SortHelper;

public interface ISortHelper<T>
{
    IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString);
}
