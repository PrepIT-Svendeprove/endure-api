namespace Endure.Data.Models;

/// <summary>
/// An abstract model that inherits from <see cref="BaseModel{TKey}" />, and sets the key to <see cref="Guid"/>
/// </summary>
public abstract class BaseModel : BaseModel<Guid>;

public abstract class BaseModel<TKey>
{
    /// <summary>
    /// Identifier of the current object.
    /// </summary>
    public TKey Id { get; set; }
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
