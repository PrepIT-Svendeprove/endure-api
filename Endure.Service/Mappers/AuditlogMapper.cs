using Endure.Data.Models;
using Endure.Dispatcher.RabbitMQ.EventMessage.AuditLog;
using Endure.Service.Models.Dto.AuditLogDtos;
using Endure.Service.Models.Enums;
using System.Text.Json;

namespace Endure.Service.Mappers;

internal static class AuditlogMapper
{
    public static IQueryable<AuditLogDto> MapToAuditLogDto(this IQueryable<AuditLog> entity)
    {
        return entity.Select(x => new AuditLogDto
        {
            Id = x.Id,
            LogLevel = (LogLevel)x.LogLevel,
            LogType = (LogType)x.LogType,
            Log = x.Log,
            RequestId = x.RequestId,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(x.CreatedAt)
        });
    }

    public static AuditLog MapToAuditLog(this AuditLogCreatedEventMessage entity)
    {
        return new AuditLog
        {
            Id = entity.Id,
            RequestId = entity.RequestId,
            LogLevel = entity.LogLevel,
            LogType = entity.LogType,
            WarehouseId = entity.WarehouseId,
            Log = entity.Log,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            IsDeleted = false
        };
    }

    /// <summary>
    /// Maps the <see cref="AuditLogDto" /> to <see cref="AuditLog"/>
    /// </summary>
    public static AuditLog MapToAuditLog(this CreateAuditlogDto entity)
    {
        return new AuditLog
        {
            Log = JsonSerializer.Serialize(entity.Log),
            LogLevel = (Data.Models.Enums.LogLevel)entity.LogLevel,
            UserId = entity.UserId,
            LogType = (Data.Models.Enums.LogType)entity.LogType,
        };
    }
}
