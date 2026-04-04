namespace Endure.Service.Dto.Warehouse;

/// <summary>
/// Represents a warehouse and related storage units.
/// </summary>
public sealed class WarehouseDto
{
    /// <summary>
    /// Identifier of the warehouse.
    /// </summary>
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string? ShortName { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastUpdatedAt { get; set; }

    /// <summary>
    /// Defines if the warehouse is the root warehouse.
    /// </summary>
    public bool IsRoot { get; set; }

    /// <summary>
    /// The parentwarehouse, it is only defined if the warehouse is not a sub-warehouse.
    /// </summary>
    public Guid? ParentWarehouseId { get; set; }
}
