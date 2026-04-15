using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.ClimateDeviceDtos;

public sealed class CreateClimateDeviceDto
{
    [JsonPropertyName(ClimateDeviceConstants.NAME_NAME)]
    public required string Name { get; set; }

    /// <summary>
    /// The wanted humidity for the climate device.
    /// </summary>
    [JsonPropertyName(ClimateDeviceConstants.SET_HUMIDITY_NAME)]
    public required double SetHumidity { get; set; }

    /// <summary>
    /// The wanted temperature for the climate device.
    /// </summary>
    [JsonPropertyName(ClimateDeviceConstants.SET_TEMPERATURE_NAME)]
    public required double SetTemperature { get; set; }

    [JsonPropertyName(ClimateDeviceConstants.STORAGEUNITID_NAME)]
    public Guid? StorageUnitId { get; set; }
}
