namespace Endure.Data.Models;

public class ClimateDevice : BaseModel
{
    public string Name { get; set; }
    public long LastReceived { get; set; }
    public bool IsConnected { get; set; }
    public bool IsDisabled { get; set; }

    public int? StorageUnitId { get; set; }
    public int WarehouseId { get; set; }

    public List<ClimateTelemetry> ClimateTelemetry { get; set; } = [ ];
    public StorageUnit StorageUnit { get; set; }
}
