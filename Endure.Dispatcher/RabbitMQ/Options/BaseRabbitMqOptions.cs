namespace Endure.Dispatcher.RabbitMQ.Options;

public class BaseRabbitMqOptions
{
    public const string SectionName = "RabbitMQ";

    public const string ExchangeName = "endure";

    public const string ClientName = "Endure.Api";

    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string VirtualHost { get; set; }

    public required string HostName { get; set; }

    public required int Port { get; set; }

}
