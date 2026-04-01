namespace Endure.Data.Models;

/// <summary>
/// The warehouse either defines a "Central lager" or "Distribution center"
/// </summary>
public class Warehouse : BaseModel<int>
{
    /// <summary>
    /// Name of the warehouse.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Defines wether or not the warehouse is the root warehouse.
    /// </summary>
    public bool IsRoot { get; set; }

    /// <summary>
    /// The Warehouse parentId, is only set if a warehouse is a sub-warehouse.
    /// </summary>
    public int? ParentWarehouseId { get; set; }

    public Warehouse? ParentWarehouse { get; set; }
    public List<StorageUnit> StorageUnits { get; set; } = [ ];
    public List<Warehouse> ChildWarehouses { get; set; } = [ ];
}
