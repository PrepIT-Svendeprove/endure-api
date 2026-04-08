namespace Endure.Data.Models;

public class ProductBatch : BaseModel
{
    public long BestBefore { get; set; }

    public required Guid ProductId { get; set; }
    public required Guid StorageUnitId { get; set; }
    public required Guid WarehouseId { get; set; }

    /// <summary>
    /// The amount of items left in the batch.
    /// </summary>
    public int Count { get; set; }

    public Product Product { get; set; } = default!;
    public StorageUnit StorageUnit { get; set; } = default!;
}
