namespace Shared.ResourceParameters;

public class PagedList<TEntity>
{
    public PaginationMetaData Pagination { get; set; }
    public List<TEntity> Data { get; set; }
    
    public PagedList(List<TEntity> items, int count, int pageNumber, int pageSize)
    {
        var totalPages = (int)Math.Ceiling(count / (double)pageSize);
        Pagination = new PaginationMetaData(pageNumber, totalPages, pageSize, count);
        Data = items;
    }
    
    public PagedList(PaginationMetaData pagination, List<TEntity> items)
    {
        Pagination = pagination;
        Data = items;
    }
}
