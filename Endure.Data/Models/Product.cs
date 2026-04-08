namespace Endure.Data.Models;

public class Product : BaseModel
{
    public required string Name { get; set; }
    public required string EAN { get; set; }
    public string? Description { get; set; }

    public List<ProductBatch> ProductBatches { get; set; } = [ ];
}
