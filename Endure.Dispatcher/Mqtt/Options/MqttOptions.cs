namespace Endure.Messaging.Mqtt.Options;

public class MqttOptions
{
    public const string SectionName = "Mqtt";

    public required string Broker { get; set; }

    public required int Port { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string ClientId { get; set; }
}
