using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.AuditLogDtos;
using Endure.Service.Models.Filters;
using Endure.Service.Services.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services;

internal class AuditLogService(
        DatabaseContext context,
        IRequestContext requestContext,
        IWarehouseService warehouseService,
        IDispatcherAuditLogService dispatcherAuditLogService
    )
    : BaseService<AuditLog>(context), IAuditLogService
{
    private readonly IRequestContext _requestContext = requestContext;
    private readonly IWarehouseService _warehouseService = warehouseService;
    private readonly IDispatcherAuditLogService _dispatcherAuditLogService = dispatcherAuditLogService;

    public async Task<AuditLogDto?> GetAuditLogAsync(Guid id)
    {
        return await _context
                .AuditLog
                .MapToAuditLogDto()
                .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<AuditLogDto>> GetPaginatedAuditLogAsync(AuditlogPaginatedFilter filter)
    {
        var context = MakePaginatedQuery(filter)
            .OrderByDescending(x => x.CreatedAt)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.RequestId))
            context = context.Where(x => x.RequestId == filter.RequestId);

        return await context
                .MapToAuditLogDto()
                .ToListAsync();
    }

    public async Task<bool> CreateAuditLogAsync(CreateAuditlogDto entity)
    {
        entity.WarehouseId ??= await _warehouseService.GetRootWarehouseIdAsync();

        var mappedEntity = entity.MapToAuditLog(entity.WarehouseId.Value);
        mappedEntity.RequestId = _requestContext.TraceId;

        return await _dispatcherAuditLogService.CreateAuditLogAsync(mappedEntity);
    }
}

public interface IAuditLogService
{
    Task<bool> CreateAuditLogAsync(CreateAuditlogDto entity);

    /// <summary>
    /// Retrieves a single auditlog.
    /// </summary>
    Task<AuditLogDto?> GetAuditLogAsync(Guid id);

    /// <summary>
    /// Gets auditlogs in a paginated format, with the filter <paramref name="filter"/>
    /// </summary>
    /// <returns>
    ///     The paginated auditlogs.
    /// </returns>
    Task<List<AuditLogDto>> GetPaginatedAuditLogAsync(AuditlogPaginatedFilter filter);
}