using Endure.Service.Dto.AuditLogDtos;
using Endure.Service.Enums;
using System.Text.Json.Serialization;

namespace Endure.Service.Dto.AuditLog;

public sealed class CreateAuditlogDto
{
    /// <summary>
    /// The identifier of the warehouse were the auditlog occoured.
    /// </summary>
    [JsonPropertyName(AuditLogConstants.WAREHOUSEID_NAME)]
    public required Guid WarehouseId { get; set; }

    /// <summary>
    /// Represents how critical the Auditlog were.
    /// </summary>
    [JsonPropertyName(AuditLogConstants.LOGLEVEL_NAME)]
    public required LogLevel LogLevel { get; set; }

    /// <summary>
    /// Represents what module the Audit log occoured.
    /// </summary>
    [JsonPropertyName(AuditLogConstants.MODULETYPE_NAME)]
    public required ModuleType ModuleType { get; set; }

    /// <summary>
    /// Defines the log data that were originally stored, this is in a json format.
    /// </summary>
    [JsonPropertyName(AuditLogConstants.LOGDATA_NAME)]
    public required string LogData { get; set; }
}
