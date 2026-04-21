using Endure.Messaging.Mqtt.Options;
using Microsoft.Extensions.Options;
using MQTTnet;

namespace Endure.Dispatcher.Mqtt;

internal sealed class MqttClientHelper(
        IOptions<MqttOptions> options
    ) : IMqttClientHelper
{
    private readonly MqttOptions _options = options.Value;
    public MqttClientOptions MqttOptions { get; private set; }
    public MqttClientFactory Factory { get; private set; }

    public async Task<IMqttClient> CreateMqttClientAsync()
    {
        Factory = new MqttClientFactory();
        var client = Factory.CreateMqttClient();

        MqttOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(_options.Broker, _options.Port)
                .WithCredentials(_options.UserName, _options.Password)
                .WithClientId(_options.ClientId)
                .Build();

        return client;
    }
}

public interface IMqttClientHelper
{
    MqttClientFactory Factory { get; }
    MqttClientOptions MqttOptions { get; }

    Task<IMqttClient> CreateMqttClientAsync();
}