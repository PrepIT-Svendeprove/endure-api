namespace Endure.Service.Models.Results;

public class PaginatedResult<T>(List<T>? entities, int maxPages)
{
    /// <summary>
    /// The entities found, on the request.
    /// </summary>
    public List<T> Entities { get; set; } = entities ?? [];

    /// <summary>
    /// The max number of pages, that were found on the request.
    /// </summary>
    public int MaxPages { get; set; } = maxPages;
}
