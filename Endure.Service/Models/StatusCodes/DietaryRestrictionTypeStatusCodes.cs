namespace Endure.Service.Models.StatusCodes;

/// <summary>
/// Contains the status codes that can be returned when updating, creating or deleting a dietaryrestrictiontype.
/// </summary>
internal class DietaryRestrictionTypeStatusCodes
{
    /// <summary>
    /// The entity was not found, this could be because of incorrect ID or the entity has been deleted.
    /// </summary>
    internal const string ENTITY_MISSING = "30001";

    /// <summary>
    /// The entity that was trying to be created, already exists.
    /// </summary>
    internal const string ENTITY_ALREADY_EXISTS = "30002";
}
