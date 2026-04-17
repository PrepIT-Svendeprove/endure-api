namespace Endure.Service.Models.Filters;

public class ClimateTelemetryDateRangeFilter
{
    public DateTimeOffset From { get; set; }

    public DateTimeOffset To { get; set; }

    public Guid ClimateDeviceId { get; set; }

    public Guid WarehouseId { get; set; }
}
