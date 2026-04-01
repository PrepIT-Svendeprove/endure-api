using Endure.Data.Models.Enums;

namespace Endure.Data.Models;

public class StorageUnit : BaseModel
{
    /// <summary>
    /// Name of the storage unit.
    /// </summary>
    public required string Name { get; set; }

    public string? ShortName { get; set; }

    /// <summary>
    /// Description of the storage unit.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Self-referencing identifier, for sub units.
    /// </summary>
    public Guid? ParentStorageUnitId { get; set; }

    public StorageType StorageType { get; set; }

    /// <summary>
    /// If this is set to true, the storage unit can contain product batches.
    /// </summary>
    public bool IsSlot { get; set; }

    /// <summary>
    /// The warehouse that the storage unit belongs to.
    /// </summary>
    public required int WarehouseId { get; set; }

    public Warehouse Warehouse { get; set; } = default!;
    public List<ProductBatch> Products { get; set; } = [ ];
    public List<ClimateDevice> ClimateDevices { get; set; } = [ ];
    public StorageUnit? ParentStorageUnit { get; set; }
    public List<StorageUnit> ChildStorageUnits { get; set; } = [ ];
}
