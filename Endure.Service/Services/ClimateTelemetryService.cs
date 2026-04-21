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
    public async Task<List<ClimateTelemetryDto>> GetClimateTelemetryFromDateRange(ClimateTelemetryDateRangeFilter filter)
    {
        return await _context
                .ClimateTelemetry
                .OrderBy(x => x.CreatedAt)
                .Where(x => x.CreatedAt >= filter.From.ToUnixTimeSeconds() && x.CreatedAt <= filter.To.ToUnixTimeSeconds())
                .MapToClimateTelemetryDto()
                .ToListAsync();
    }
}

public interface IClimateTelemetryService
{
    Task<List<ClimateTelemetryDto>> GetClimateTelemetryFromDateRange(ClimateTelemetryDateRangeFilter filter);
}