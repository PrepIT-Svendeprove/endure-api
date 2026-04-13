using Endure.Dispatcher.RabbitMQ.Options;
using RabbitMQ.Client;
using System.Net.Sockets;

namespace Endure.Dispatcher.RabbitMQ.Connection;

internal abstract class BaseRabbitMqConnection(BaseRabbitMqOptions options)
    : IAsyncDisposable
{
    private readonly BaseRabbitMqOptions _options = options;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private IConnection? _connection;

    public async Task<IConnection> GetConnectionAsync(CancellationToken ctx = default)
    {
        if (_connection is { IsOpen: true })
            return _connection;

        await _lock.WaitAsync(ctx);
        try
        {
            if (_connection is { IsOpen: true })
                return _connection;

            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost,
                ClientProvidedName = "Endure.Api"
            };

            _connection = await factory.CreateConnectionAsync(ctx);
            return _connection;
        }
        catch(SocketException)
        {
            throw;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
            await _connection.DisposeAsync();

        _lock.Dispose();
    }
}

public interface IBaseRabbitMqConnection
{
    Task<IConnection> GetConnectionAsync(CancellationToken ctx = default);
}