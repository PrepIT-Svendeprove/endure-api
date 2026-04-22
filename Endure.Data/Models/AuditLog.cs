using Endure.Data.Models.Enums;

namespace Endure.Data.Models;

public class AuditLog : BaseModel
{
    public Guid WarehouseId { get; set; }
    public LogLevel LogLevel { get; set; }

    public LogType LogType { get; set; }

    public required string Log { get; set; }

    public string? UserId { get; set; }

    /// <summary>
    /// The request of the Audit, used to trace and chain auditlogs together.
    /// </summary>
    public string? RequestId { get; set; }

    public Warehouse? Warehouse { get; set; }
}
