namespace Endure.Data.Models;

public class ClimateTelemetry : BaseModel
{
    public double Temperature { get; set; }
    public double Humidity { get; set; }

    public int StorageUnitId { get; set; }
    public int ClimateDeviceId { get; set; }
    public int WarehouseId { get; set; }

    //public StorageUnit StorageUnit { get; set; }
    public ClimateDevice ClimateDevice { get; set; }
}
