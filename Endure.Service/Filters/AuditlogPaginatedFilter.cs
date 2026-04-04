namespace Endure.Service.Filters;

public class AuditlogPaginatedFilter : BasePaginatedFilter
{
    /// <summary>
    /// Adds the filter to only include the RequestId.
    /// </summary>
    public string? RequestId { get; set; }
}
