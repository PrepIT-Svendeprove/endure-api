using Endure.Data.Models;
using Endure.Service.Models.Dto.AuditLogDtos;
using Endure.Service.Models.Enums;

namespace Endure.Service.Mappers;

internal static class AuditlogMapping
{
    public static IQueryable<AuditLogDto> MapToAuditLogDto(this IQueryable<AuditLog> entity)
    {
        return entity.Select(x => new AuditLogDto
        {
            Id = x.Id,
            WarehouseId = x.WarehouseId,
            LogLevel = (LogLevel)x.LogLevel,
            ModuleType = (ModuleType)x.ModuleType,
            LogData = x.Log,
            RequestId = x.RequestId,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(x.CreatedAt)
        });
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
            ModuleType = (Data.Models.Enums.ModuleType)entity.ModuleType,
            LogLevel = (Data.Models.Enums.LogLevel)entity.LogLevel
        };
    }
}
