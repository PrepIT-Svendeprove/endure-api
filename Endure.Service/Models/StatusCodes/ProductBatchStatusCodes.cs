namespace Endure.Service.Models.StatusCodes;

internal class ProductBatchStatusCodes
{
    /// <summary>
    /// The entity was not found.
    /// </summary>
    internal const string ENTITY_MISSING = "60001";

    /// <summary>
    /// The entity has not been saved.
    /// </summary>
    internal const string ENTITY_UNCHANGED = "60002";

    /// <summary>
    /// One of the relations that is trying to be created, does not exist.
    /// </summary>
    internal const string RELATION_DOES_NOT_EXIST = "60003";
}
