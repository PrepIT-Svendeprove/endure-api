namespace Endure.Service.Models.Dto.ClimateDeviceDtos;

public sealed class CreateClimateDeviceDto
{
    public required string Name { get; set; }

    /// <summary>
    /// The wanted humidity for the climate device.
    /// </summary>
    public required double Humidity { get; set; }

    /// <summary>
    /// The wanted temperature for the climate device.
    /// </summary>
    public required double Temperature { get; set; }

    public Guid? StorageUnitId { get; set; }
}
