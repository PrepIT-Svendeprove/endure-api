using Endure.Data.Models.Enums;

namespace Endure.Data.Models;

public class AuditLog : BaseModel
{
    public required int WarehouseId { get; set; }
    public LogLevel LogLevel { get; set; }
    public ModuleType ModuleType { get; set; }

    public required string Log { get; set; }

    public new long UpdatedAt { get; private set; }
}
