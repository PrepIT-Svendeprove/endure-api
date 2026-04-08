namespace Endure.Data.Models;

public abstract class BaseModel
{
    /// <summary>
    /// Identifier of the current object.
    /// </summary>
    public Guid Id { get; set; }
    public long CreatedAt { get; internal set; }
    public long UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    /// <summary>
    /// Used as concurrency token, for npgsql.
    /// 
    /// https://www.npgsql.org/efcore/modeling/concurrency.html?tabs=fluent-api
    /// </summary>
    public uint Version { get; set; }
}
