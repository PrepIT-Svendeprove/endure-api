using Endure.Service.Models.Dto.ProductDtos;
using Endure.Service.Models.Dto.StorageUnitDtose;

namespace Endure.Service.Models.Dto.ProductBatchDtos;

public class ProductBatchDto
{
    /// <summary>
    /// Identifier of the product batch.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Amount of items in the product batch.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// The batches best before date.
    /// </summary>
    public DateTimeOffset BestBeforeUtc { get; set; }

    /// <summary>
    /// The batches product.
    /// </summary>
    public ProductDto Product { get; set; }

    public SelectStorageUnitDto StorageUnit { get; set; }
}
