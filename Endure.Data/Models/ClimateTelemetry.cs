namespace Endure.Data.Models;

public class ClimateTelemetry : BaseModel
{
    public required double Temperature { get; set; }
    public required double Humidity { get; set; }

    public required Guid ClimateDeviceId { get; set; }
    public required Guid WarehouseId { get; set; }

    public ClimateDevice ClimateDevice { get; set; } = default!;
}
