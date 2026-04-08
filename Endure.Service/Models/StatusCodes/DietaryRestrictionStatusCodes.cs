namespace Endure.Service.Models.StatusCodes;

internal class DietaryRestrictionStatusCodes
{
    /// <summary>
    /// The entity was not found, this could be because of incorrect ID or the entity has been deleted.
    /// </summary>
    internal const string ENTITY_MISSING = "40001";

    /// <summary>
    /// The entity that was trying to be created, already exists.
    /// </summary>
    internal const string ENTITY_ALREADY_EXISTS = "40002";
}
