using Endure.Service.Enums;

namespace Endure.Service.Dto.AuditLog;

public sealed class CreateAuditlogDto
{
    /// <summary>
    /// The identifier of the warehouse were the auditlog occoured.
    /// </summary>
    public required Guid WarehouseId { get; set; }

    /// <summary>
    /// Represents how critical the Auditlog were.
    /// </summary>
    public required LogLevel LogLevel { get; set; }

    /// <summary>
    /// Represents what module the Audit log occoured.
    /// </summary>
    public required ModuleType ModuleType { get; set; }

    /// <summary>
    /// Defines the log data that were originally stored, this is in a json format.
    /// </summary>
    public required string LogData { get; set; }
}
