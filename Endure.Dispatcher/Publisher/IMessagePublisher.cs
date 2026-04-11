namespace Endure.Dispatcher.Publisher;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T key, CancellationToken ctx = default);
}
