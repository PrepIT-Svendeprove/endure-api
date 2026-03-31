namespace Endure.Data.Models;

public class ProductBatch : BaseModel
{
    /// <summary>
    /// The identifier is formatted, such as: {yyMMdd}{Random 4 numbers (0-9)}
    /// </summary>
    public new int Id { get; set; }

    public long BestBefore { get; set; }
    public long LastUpdatedAt { get; set; }

    public int ProductId { get; set; }
    public int StorageUnitId { get; set; }
    public int WarehouseId { get; set; }

    /// <summary>
    /// The amount of items left in the batch.
    /// </summary>
    public long Count { get; set; }

    public Product Product { get; set; }
    public StorageUnit StorageUnit { get; set; }
}
