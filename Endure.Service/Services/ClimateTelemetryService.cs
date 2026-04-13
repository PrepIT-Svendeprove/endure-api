using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.ClimateTelemetry;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services;

internal sealed class ClimateTelemetryService(
        DatabaseContext context
    )
    : BaseService<ClimateTelemetry>(context), IClimateTelemetryService
{
    public async Task<PaginatedResult<ClimateTelemetryDto>> GetPaginatedClimateTelemetryAsync(ClimateTelemetryPaginatedFilter filter)
    {
        var context = MakePaginatedQuery(filter)
                .OrderByDescending(x => x.CreatedAt);

        var maxPages = await context.CountAsync();

        return new PaginatedResult<ClimateTelemetryDto>(await context.MapToClimateTelemetryDto().ToListAsync(), maxPages);
    }
}

public interface IClimateTelemetryService
{
    Task<PaginatedResult<ClimateTelemetryDto>> GetPaginatedClimateTelemetryAsync(ClimateTelemetryPaginatedFilter filter);
}