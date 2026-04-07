namespace Endure.Service.Models.Filters;

/// <summary>
/// Represents a paginated filter type that can be applied to other filters
/// </summary>
public abstract class BasePaginatedFilter
{
    /// <summary>
    /// The page where to start the pagination filter from.
    /// </summary>
    public int Page { get; set; } = 0;

    /// <summary>
    /// The amount of items that has been requested.
    /// </summary>
    public int Take { get; set; } = 10;
}
