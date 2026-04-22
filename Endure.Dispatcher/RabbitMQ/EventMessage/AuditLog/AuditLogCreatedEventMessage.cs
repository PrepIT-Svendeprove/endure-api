using Endure.Data.Models.Enums;
using Endure.Dispatcher.Attributes;
using System.Text.Json.Serialization;

namespace Endure.Dispatcher.RabbitMQ.EventMessage.AuditLog;

[EventQueue("auditlog.created")]
public class AuditLogCreatedEventMessage : BaseEventMessage
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("logLevel")]
    public required LogLevel LogLevel { get; set; }

    [JsonPropertyName("log")]
    public required string Log { get; set; }

    [JsonPropertyName("requestId")]
    public string? RequestId { get; set; }

    [JsonPropertyName("warehouseId")]
    public Guid WarehouseId { get; set; }
}
