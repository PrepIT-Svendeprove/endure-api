namespace Endure.Service.Models.Filters;

public class ClimateTelemetryPaginatedFilter : BasePaginatedFilter
{
    public Guid ClimateDeviceId { get; set; }
}
