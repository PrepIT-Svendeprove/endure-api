namespace Endure.Service.Models.Dto.ProductBatchDtos;

public class CreateProductBatchDto
{
    /// <summary>
    /// Best before date, formatted as unix epoch time.
    /// </summary>
    public long BestBefore { get; set; }

    /// <summary>
    /// Amount of items that is within the product batch.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Id of the product that the product batch is being created for.
    /// </summary>
    public required Guid ProductId { get; set; }

    /// <summary>
    /// Id of the storage unit that the product batch is placed in.
    /// </summary>
    public required Guid StorageUnitId { get; set; }
}
