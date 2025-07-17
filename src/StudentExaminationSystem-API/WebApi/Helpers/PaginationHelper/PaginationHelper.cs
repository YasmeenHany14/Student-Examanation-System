using Shared.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Helpers.PaginationHelper;

public class PaginationHelper<TDto, TResourceParameters> : IPaginationHelper<TDto, TResourceParameters>
    where TDto : class
    where TResourceParameters : BaseResourceParameters
{
    public void CreateMetaDataHeader(PagedList<TDto> items,
        TResourceParameters resourceParameters,
        IHeaderDictionary responseHeaders,
        IUrlHelper urlHelper,
        string routeName)
    {
        var previousPageLink = items.Pagination.HasPrevious ?
            CreateResourceUri(resourceParameters, routeName, ResourceUriType.PreviousPage, urlHelper) : null;

        var nextPageLink = items.Pagination.HasNext ?
            CreateResourceUri(resourceParameters, routeName, ResourceUriType.NextPage, urlHelper) : null;
        
        items.Pagination.Links = new PaginationLinks(previousPageLink, nextPageLink);
    }


    public string? CreateResourceUri(
        TResourceParameters resourceParameters,
        string routeName,
        ResourceUriType type,
        IUrlHelper urlHelper)
    {
        var orderBy = resourceParameters.OrderBy;
        var pageNumber = resourceParameters.PageNumber;
        if (type == ResourceUriType.PreviousPage)
        {
            pageNumber--;
        }
        else if (type == ResourceUriType.NextPage)
        {
            pageNumber++;
        }

        var queryParameters = new Dictionary<string, object>
        {
            { "pageNumber", pageNumber },
            { "pageSize", resourceParameters.PageSize },
            // { "orderBy", orderBy }
        };

        foreach (var prop in typeof(TResourceParameters).GetProperties())
        {
            if (prop.GetValue(resourceParameters) is string value && !string.IsNullOrEmpty(value))
            {
                queryParameters.Add(prop.Name, value);
            }
        }

        return urlHelper?.Link(routeName, queryParameters);
    }
}
