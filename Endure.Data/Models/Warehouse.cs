namespace Endure.Data.Models;

/// <summary>
/// The warehouse either defines a "Central lager" or "Distribution center"
/// </summary>
public class Warehouse : BaseModel
{
    /// <summary>
    /// The identifier is formatted, such as: {Post code}{Random 4 numbers (0-9)}
    /// </summary>
    public new int Id { get; set; }

    /// <summary>
    /// Name of the warehouse.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Defines wether or not the warehouse is the root warehouse.
    /// </summary>
    public bool IsCurrent { get; set; }

    /// <summary>
    /// The Warehouse parentId, is only set if a warehouse is a sub-warehouse.
    /// </summary>
    public int? ParentWarehouseId { get; set; }

    public List<StorageUnit> StorageUnits { get; set; } = [ ];
    public Warehouse? ParentWarehouse { get; set; }
    public List<Warehouse> ChildWarehouse { get; set; } = [ ];
}
