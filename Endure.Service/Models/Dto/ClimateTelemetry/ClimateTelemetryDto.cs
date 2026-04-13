namespace Endure.Service.Models.Dto.ClimateTelemetry;

public class ClimateTelemetryDto
{
    public required Guid Id { get; set; }

    public required double Temperature { get; set; }

    public required double Humidity { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }
}
