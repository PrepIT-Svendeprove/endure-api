using Endure.Service.Models.Enums;
using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.AuditLogDtos;

public sealed class AuditLogDto
{
    /// <summary>
    /// Identifier of the auditlog
    /// </summary>
    [JsonPropertyName(AuditLogConstants.ID_NAME)]
    public required Guid Id { get; set; }

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
    /// Defines the log data that were originally stored, this is in a json format.
    /// </summary>
    [JsonPropertyName(AuditLogConstants.LOGDATA_NAME)]
    public required string LogData { get; set; }

    /// <summary>
    /// Identifier of the request, where the Auditlog was made.
    /// </summary>
    [JsonPropertyName(AuditLogConstants.REQUESTID_NAME)]
    public string? RequestId { get; set; }

    /// <summary>
    /// The creation time of the entity.
    /// </summary>
    [JsonPropertyName(AuditLogConstants.CREATEDAT_NAME)]
    public DateTimeOffset CreatedAt { get; set; }
}
