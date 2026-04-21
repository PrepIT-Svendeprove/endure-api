using Endure.Data.Models.Enums;

namespace Endure.Data.Models;

public class AuditLog : BaseModel
{
    public required Guid WarehouseId { get; set; }
    public LogLevel LogLevel { get; set; }

    public required string Log { get; set; }

    /// <summary>
    /// The request of the Audit, used to trace and chain auditlogs together.
    /// </summary>
    public string? RequestId { get; set; }

    public Warehouse? Warehouse { get; set; }
}
