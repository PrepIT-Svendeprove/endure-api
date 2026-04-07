namespace Endure.Service.Models.StatusCodes;

internal class DietaryRestrictionStatusCodes
{
    /// <summary>
    /// The entity was nout found, this could be because of incorrect ID or the entity has been deleted.
    /// </summary>
    internal const string ENTITY_MISSING = "30001";

    /// <summary>
    /// The entity that was trying to be created, already exists.
    /// </summary>
    internal const string ENTITY_ALREADY_EXISTS = "30002";
}
