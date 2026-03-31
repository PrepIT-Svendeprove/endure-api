namespace Endure.Data.Models;

public abstract class BaseModel
{
    /// <summary>
    /// Identifier of the current object.
    /// </summary>
    public int Id { get; set; }
    public long CreatedAt { get; internal set; }
    public long UpdatedAt { get; set; }

    public uint Version { get; set; }
}
