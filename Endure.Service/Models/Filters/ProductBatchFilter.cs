namespace Endure.Service.Models.Filters;

public class ProductBatchFilter : BasePaginatedFilter
{
    public Guid Id { get; set; }

    /// <summary>
    /// This is only required to be set, when retrieving the product batch from a storage unit, otherwise its value will be ignored.
    /// </summary>
    public Guid? WarehouseId { get; set; }
}
