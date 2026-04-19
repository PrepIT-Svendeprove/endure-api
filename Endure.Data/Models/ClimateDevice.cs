namespace Endure.Data.Models;

public class ClimateDevice : BaseModel
{
    public string Name { get; set; }
    public long? LastReceived { get; set; }
    public bool IsConnected { get; set; }
    public bool IsDisabled { get; set; }

    public double SetHumidity { get; set; }

    public double SetTemperature { get; set; }

    public string ClimateDeviceCode { get; set; }

    public Guid? StorageUnitId { get; set; }
    public Guid WareHouseId { get; set; }

    public List<ClimateTelemetry> ClimateTelemetry { get; set; } = [ ];
    public StorageUnit StorageUnit { get; set; } = default!;
}
