using System.Text.Json.Serialization;

namespace Shared.ResourceParameters;

public class PaginationMetaData
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }

    [JsonIgnore]
    public bool HasPrevious => (CurrentPage > 1);
    [JsonIgnore]
    public bool HasNext => (CurrentPage < TotalPages);
    [JsonPropertyName("links")]
    public PaginationLinks Links { get; set; } = new PaginationLinks();

    public PaginationMetaData(int currentPage, int totalPages, int pageSize, int totalCount)
    {
        CurrentPage = currentPage;
        TotalPages = totalPages;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}
