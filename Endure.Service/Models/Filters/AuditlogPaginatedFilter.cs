namespace Endure.Service.Models.Filters;

public class AuditlogPaginatedFilter : BasePaginatedFilter
{
    /// <summary>
    /// Adds the filter to only include the RequestId.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// If set, retrieves warehouses with the specified identifier.
    /// Default: Root warehouse
    /// </summary>
    public Guid? WarehouseId { get; set; }
}
