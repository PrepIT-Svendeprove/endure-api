using Endure.Service.Models.Dto.ProductDtos;
using Endure.Service.Models.Dto.StorageUnitDtose;

namespace Endure.Service.Models.Dto.ProductBatchDtos;

public class ProductBatchDto
{
    public required Guid Id { get; set; }

    public int Count { get; set; }

    public DateTimeOffset BestBeforeUtc { get; set; }

    public ProductDto Product { get; set; }

    public SelectStorageUnitDto StorageUnit { get; set; }
}
