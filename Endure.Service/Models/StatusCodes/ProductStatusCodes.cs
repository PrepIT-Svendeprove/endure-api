namespace Endure.Service.Models.StatusCodes;

internal class ProductStatusCodes
{
    /// <summary>
    /// The entity was not found, this could be bcause of incorrect ID or the entity has been deleted.
    /// </summary>
    internal const string ENTITY_MISSING = "50001";

    /// <summary>
    /// There is already a product with the same EAN number.
    /// </summary>
    internal const string EAN_EXISTS = "50002";

    /// <summary>
    /// The product does not have an EAN defined.
    /// </summary>
    internal const string EAN_MISSING = "50003";

    /// <summary>
    /// The product's EAN cannot be changed after it first has been set.
    /// </summary>
    internal const string EAN_CANNOT_BE_CHANGED = "50004";

    /// <summary>
    /// The product has not been changed.
    /// </summary>
    internal const string ENTITY_UNCHANGED = "50005";
}
