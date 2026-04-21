namespace Endure.Service.Models.Filters;

public class ClimateDevicePaginatedFilter : BasePaginatedFilter
{
    /// <summary>
    /// If set, retrieves warehouses with the specified identifier.
    /// Default: Root warehouse
    /// </summary>
    public Guid? WarehouseId { get; set; }

    /// <summary>
    /// If set to true, excludes disabled climatedevices.
    /// Default: false
    /// </summary>
    public bool ExcludeDisabled { get; set; } = false;
}
