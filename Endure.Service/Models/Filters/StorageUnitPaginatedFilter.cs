namespace Endure.Service.Models.Filters;

public class StorageUnitPaginatedFilter : BasePaginatedFilter
{
    public required Guid WarehouseId { get; set; }
}
