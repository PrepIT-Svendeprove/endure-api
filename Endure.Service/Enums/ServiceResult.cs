namespace Endure.Service.Enums;

public enum ServiceResult
{
    /// <summary>
    /// The result did not update or create the entity.
    /// </summary>
    Failed = 0,

    /// <summary>
    /// The service sucessfully ran.
    /// </summary>
    Success = 1,

    /// <summary>
    /// The entity were not created because a relation there were requested does not exist any more.
    /// </summary>
    RelationNotFound = 2,

    /// <summary>
    /// 
    /// </summary>
    AlreadyExists = 3
}
