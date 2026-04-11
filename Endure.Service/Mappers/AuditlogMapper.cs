using Endure.Data.Models;
using Endure.Dispatcher.EventMessage.AuditLog;
using Endure.Service.Models.Dto.AuditLogDtos;
using Endure.Service.Models.Enums;

namespace Endure.Service.Mappers;

internal static class AuditlogMapper
{
    public static IQueryable<AuditLogDto> MapToAuditLogDto(this IQueryable<AuditLog> entity)
    {
        return entity.Select(x => new AuditLogDto
        {
            Id = x.Id,
            WarehouseId = x.WarehouseId,
            LogLevel = (LogLevel)x.LogLevel,
            LogData = x.Log,
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
            WarehouseId = entity.WarehouseId,
            Log = entity.LogData,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            IsDeleted = false
        };
    }

    /// <summary>
    /// Maps the <see cref="AuditLogDto" /> to <see cref="AuditLog"/>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static AuditLog MapToAuditLog(this CreateAuditlogDto entity, Guid warehouseId)
    {
        return new AuditLog
        {
            Log = entity.LogData,
            WarehouseId = warehouseId,
            LogLevel = (Data.Models.Enums.LogLevel)entity.LogLevel
        };
    }
}
