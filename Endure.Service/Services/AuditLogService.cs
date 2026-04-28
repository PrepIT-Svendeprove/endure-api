using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.AuditLogDtos;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Services.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services;

internal class AuditLogService(
        DatabaseContext context,
        IRequestContext requestContext,
        IDispatcherAuditLogService dispatcherAuditLogService
    )
    : BaseService<AuditLog>(context), IAuditLogService
{
    private readonly IRequestContext _requestContext = requestContext;
    private readonly IDispatcherAuditLogService _dispatcherAuditLogService = dispatcherAuditLogService;

    public async Task<AuditLogDto?> GetAuditLogAsync(Guid id)
    {
        return await _context
                .AuditLog
                .MapToAuditLogDto()
                .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PaginatedResult<AuditLogDto>> GetPaginatedAuditLogAsync(AuditlogPaginatedFilter filter)
    {
        var _ = await _context.AuditLog.Where(x => x.WarehouseId == filter.WarehouseId).ToListAsync();

        var context = MakePaginatedQuery(filter)
            .OrderByDescending(x => x.CreatedAt)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.RequestId))
            context = context.Where(x => x.RequestId == filter.RequestId);

        if (filter.WarehouseId.HasValue)
            context = context.Where(x => x.WarehouseId == filter.WarehouseId);
        else
            context = context.Where(x => x.Warehouse!.IsRoot);

        var maxPages = (await context.CountAsync() / filter.Take) + 1;

        return new PaginatedResult<AuditLogDto>(await context.MapToAuditLogDto().ToListAsync(), maxPages);
    }

    public async Task<int> GetAuditLogCount(Guid warehouseId)
    {
        return await _context
                .AuditLog
                .Where(x => x.WarehouseId == warehouseId && !x.IsDeleted)
                .CountAsync();
    }

    public async Task<bool> CreateAuditLogAsync(CreateAuditlogDto entity, Guid warehouseId)
    {
        var mappedEntity = entity.MapToAuditLog();
        mappedEntity.WarehouseId = warehouseId;
        mappedEntity.RequestId = _requestContext.TraceId;
        mappedEntity.UserId = _requestContext.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

        return await _dispatcherAuditLogService.CreateAuditLogAsync(mappedEntity);
    }
}

public interface IAuditLogService
{
    Task<bool> CreateAuditLogAsync(CreateAuditlogDto entity, Guid warehouseId);

    /// <summary>
    /// Retrieves a single auditlog.
    /// </summary>
    Task<AuditLogDto?> GetAuditLogAsync(Guid id);
    Task<int> GetAuditLogCount(Guid warehouseId);

    /// <summary>
    /// Gets auditlogs in a paginated format, with the filter <paramref name="filter"/>
    /// </summary>
    /// <returns>
    ///     The paginated auditlogs.
    /// </returns>
    Task<PaginatedResult<AuditLogDto>> GetPaginatedAuditLogAsync(AuditlogPaginatedFilter filter);
}