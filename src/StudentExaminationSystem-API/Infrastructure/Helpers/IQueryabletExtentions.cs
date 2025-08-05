using System.Linq.Dynamic.Core;
using System.Text;

namespace Infrastructure.Helpers;

public static class IQueryabletExtentions
{
    public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> source,
        string orderBy,
        Dictionary<string, PropertyMappingValue> mappingDictionary)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return source;
        
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        
        if (mappingDictionary == null)
            throw new ArgumentNullException(nameof(mappingDictionary));

        var orderByStringSplit = orderBy.Trim().Split(',');
        
        var orderByQueryBuilder = new StringBuilder();
        foreach (var orderByClause in orderByStringSplit)
        {
            var clause = orderByClause.Trim();
            var propertyName = clause.Split(" ")[0];
            var sortingOrder = clause.EndsWith(" desc") ? "descending" : "ascending";

            if (!mappingDictionary.TryGetValue(propertyName, out var propertyMappingValue))
                continue;
            
            if (propertyMappingValue.Revert)
                sortingOrder = sortingOrder == "ascending" ? "descending" : "ascending";

            foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
                orderByQueryBuilder.Append($"{destinationProperty} {sortingOrder}, ");
        }
        var orderQuery = orderByQueryBuilder.ToString().TrimEnd(',', ' ');
        return source.OrderBy(orderQuery.ToString());

    }
}
