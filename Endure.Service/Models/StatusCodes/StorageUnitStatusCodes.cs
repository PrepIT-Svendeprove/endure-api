namespace Endure.Service.Models.StatusCodes;

/// <summary>
/// Contains the status codes that can be returned when updating, creating or deleting a storageunit.
/// </summary>
internal class StorageUnitStatusCodes
{
    /// <summary>
    /// The Entity was not found, this could be because of incorrect ID, the entity has been deleted or the entity is not a part of the root warehouse.
    /// </summary>
    internal const string ENTITY_MISSING = "20001";

    /// <summary>
    /// The entity contains sub- units and cannot be modified.
    /// </summary>
    internal const string CONTAINING_SUBUNITS = "20002";

    /// <summary>
    /// The parent storageunit is not eligible as a parent, either because IsSlot is true or because it is deleted.
    /// </summary>
    internal const string PARENT_NOT_ELIGIBLE = "20003";
}
