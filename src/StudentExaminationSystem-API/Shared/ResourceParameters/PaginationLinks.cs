namespace Shared.ResourceParameters;

public class PaginationLinks
{
    public string? PreviousPage { get; set; }
    public string? NextPage { get; set; }

    public PaginationLinks()
    {
        
    }
    public PaginationLinks(string? previousPage, string? nextPage)
    {
        PreviousPage = previousPage;
        NextPage = nextPage;
    }
}