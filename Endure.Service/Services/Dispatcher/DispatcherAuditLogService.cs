using Endure.Data;
using Endure.Data.Models;
using Endure.Dispatcher.EventMessage.AuditLog;
using Endure.Dispatcher.Publisher;
using Endure.Service.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services.Dispatcher;

internal sealed class DispatcherAuditLogService(
        DatabaseContext context,
        IMessagePublisher messagePublisher
    )
    : BaseService<AuditLog>(context), IDispatcherAuditLogService
{
    private readonly IMessagePublisher _messagePublisher = messagePublisher;

    public async Task<bool> CreateAuditLogAsync(AuditLog auditLog)
    {
        // If the entity already exists, we should not try to add it again.
        if (await _context.AuditLog.AnyAsync(x => x.Id == auditLog.Id && x.WarehouseId == auditLog.WarehouseId))
            return true;

        await _context.AddAsync(auditLog);

        var result = await _context.SaveChangesAsync() > 0;

        if (result && await ShouldSynchronizeWithParent(auditLog.WarehouseId))
            await _messagePublisher.PublishAsync(new AuditLogCreatedEventMessage
            {
                Id = auditLog.Id,
                LogData = auditLog.Log,
                LogLevel = auditLog.LogLevel,
                WarehouseId = auditLog.WarehouseId,
                CreatedAt = auditLog.CreatedAt,
                RequestId = auditLog.RequestId,
                UpdatedAt = auditLog.UpdatedAt
            });

        return result;
    }

    public async Task<bool> CreateAuditLogAsync(AuditLogCreatedEventMessage auditLogEventMessage)
    {
        var mappedEntity = auditLogEventMessage.MapToAuditLog();

        return await CreateAuditLogAsync(mappedEntity);
    }

    public async Task<bool> Exists(Guid id, Guid warehouseId)
        => await _context.AuditLog.AnyAsync(x => x.Id == id && x.WarehouseId == warehouseId);
}

/// <summary>
/// Service used for the Dispatcher.
/// </summary>
public interface IDispatcherAuditLogService
{
    Task<bool> CreateAuditLogAsync(AuditLog auditLog);
    Task<bool> CreateAuditLogAsync(AuditLogCreatedEventMessage auditLogEventMessage);
}
