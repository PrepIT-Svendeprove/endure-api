namespace Endure.Data.Models;

public class ProductBatch : BaseModel
{
    public long BestBefore { get; set; }
    public long LastUpdatedAt { get; set; }

    public required string ProductId { get; set; }
    public required Guid StorageUnitId { get; set; }
    public required Guid WarehouseId { get; set; }

    /// <summary>
    /// The amount of items left in the batch.
    /// </summary>
    public long Count { get; set; }

    public Product Product { get; set; } = default!;
    public StorageUnit StorageUnit { get; set; } = default!;
}
