using Endure.Service.Enums;

namespace Endure.Service.Dto.AuditLog;

public sealed class AuditLogDto
{
    /// <summary>
    /// Identifier of the auditlog
    /// </summary>
    public required Guid Id { get; set; }

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

    /// <summary>
    /// Identifier of the request, where the Auditlog was made.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// The creation time of the entity.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}
