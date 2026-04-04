namespace Endure.Service.Dto.Warehouse;

public class UpdateWarehouseDto
{
    public required Guid Id { get; set; }

    /// <summary>
    /// The name of the warehouse.
    /// </summary>
    public required string Name { get; set; }

    public string? ShortName { get; set; }

    public bool IsRoot { get; set; }

    public Guid? ParentWarehouseId { get; set; }
}
