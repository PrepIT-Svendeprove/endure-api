namespace Endure.Data.Models;

public class ClimateTelemetry : BaseModel
{
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }

    public required Guid ClimateDeviceId { get; set; }
    public required Guid WarehouseId { get; set; }

    public ClimateDevice ClimateDevice { get; set; } = default!;
}
