namespace Endure.Service.Models.Dto.ClimateDeviceDtos;

public sealed class ClimateDeviceDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the device.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// DateTime in UTC for when the climatedevice last communicated with the worker.
    /// </summary>
    public DateTimeOffset? LastReceived { get; set; }

    /// <summary>
    /// Is false, if the climatedevice has not sent any data or a lastwill were requested with the clientid.
    /// </summary>
    public bool IsConnected { get; set; }

    /// <summary>
    /// Defines if the climatedevice is active and can be used.
    /// </summary>
    public bool IsDisabled { get; set; }

    public double? SetHumidity { get; set; }

    public double? SetTemperature { get; set; }

    /// <summary>
    /// Defines the storageunit that the climatedevice is used in.
    /// </summary>
    public Guid? StorageUnitId { get; set; }
}
