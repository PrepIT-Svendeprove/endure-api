namespace Endure.Service.Models.Dto.ProductBatchDtos;

public class Top10ProductBatchDto
{
    public required Guid Id { get; set; }

    public required string ProductEAN { get; set; }

    public int Count { get; set; }

    public DateTimeOffset BestBeforeUtc { get; set; }
}
