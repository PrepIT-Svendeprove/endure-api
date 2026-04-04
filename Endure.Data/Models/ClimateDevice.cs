namespace Endure.Data.Models;

public class ClimateDevice : BaseModel
{
    public required string Name { get; set; }
    public long LastReceived { get; set; }
    public bool IsConnected { get; set; }
    public bool IsDisabled { get; set; }

    public Guid? StorageUnitId { get; set; }
    public required Guid WarehouseId { get; set; }

    public List<ClimateTelemetry> ClimateTelemetry { get; set; } = [ ];
    public StorageUnit StorageUnit { get; set; } = default!;
}
