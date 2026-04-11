using Endure.Data.Models.Enums;
using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.EventMessage.StorageUnit;

[EventQueue("storageunit.updated")]
public class StorageUnitUpdatedEventMessage : BaseEventMessage
{
    /// <summary>
    /// Identifier of the storageunit.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Name of the storageunit.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Short name of the storageunit, can be used to group storage units.
    /// </summary>
    public string? ShortName { get; set; }

    /// <summary>
    /// Description of the storageunit.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The parent storage unit, is only set if the unit is a sub-unit.
    /// </summary>
    public Guid? ParentStorageUnitId { get; set; }

    /// <summary>
    /// The type of products that the storage unit is supposed to be used for.
    /// </summary>
    public StorageType StorageType { get; set; }

    /// <summary>
    /// If this is set to true, the storageunit can contain product batches.
    /// </summary>
    public bool IsSlot { get; set; }
}
