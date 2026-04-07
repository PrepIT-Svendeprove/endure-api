namespace Endure.Service.Models.StatusCodes;

internal class WarehouseStatusCodes
{
    /// <summary>
    /// The entity is not a root entity, therefor it cannot be modified.
    /// </summary>
    internal const string ENTITY_IS_NOT_ROOT = "10000";

    /// <summary>
    /// The entity was not found, this could be because of incorrect ID or the entity has been deleted.
    /// </summary>
    internal const string ENTITY_MISSING = "10001";
}
;