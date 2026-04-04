using Endure.Data;
using Endure.Service.Dto.AuditLog;
using Endure.Service.Filters;
using Endure.Service.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services.AuditLog;

internal class AuditLogService(DatabaseContext context, IRequestContext requestContext) : IAuditLogService
{
    private readonly DatabaseContext _context = context;
    private readonly IRequestContext _requestContext = requestContext;

    public async Task<AuditLogDto?> GetAuditLogAsync(Guid id)
    {
        return await _context
                .AuditLog
                .MapToAuditLogDto()
                .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<AuditLogDto>> GetPaginatedAuditLogAsync(AuditlogPaginatedFilter filter)
    {
        return await _context
                .AuditLog
                .MapToAuditLogDto()
                .Where(x => string.IsNullOrEmpty(filter.RequestId) && x.RequestId == filter.RequestId)
                .Take(filter.Take)
                .Skip(filter.Page)
                .ToListAsync();
    }

    public async Task<bool> CreateAuditLogAsync(CreateAuditlogDto entity)
    {
        var mappedEntity = entity.MapToAuditLog();
        mappedEntity.RequestId = _requestContext.TraceId;

        await _context.AddRangeAsync();

        return await _context.SaveChangesAsync() > 0;
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
