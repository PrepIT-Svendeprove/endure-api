using Endure.Service.Models.Enums;
using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.AuditLogDtos;

public sealed class CreateAuditlogDto
{
    /// <summary>
    /// The identifier of the warehouse were the auditlog occoured. If this is null, the root warehouse will be used.
    /// </summary>
    [JsonPropertyName(AuditLogConstants.WAREHOUSEID_NAME)]
    public Guid? WarehouseId { get; set; }

    /// <summary>
    /// Represents how critical the Auditlog were.
    /// </summary>
    [JsonPropertyName(AuditLogConstants.LOGLEVEL_NAME)]
    public required LogLevel LogLevel { get; set; }

    /// <summary>
    /// Defines the log data that were originally stored, this is in a json format.
    /// </summary>
    [JsonPropertyName(AuditLogConstants.LOGDATA_NAME)]
    public required string LogData { get; set; }
}
