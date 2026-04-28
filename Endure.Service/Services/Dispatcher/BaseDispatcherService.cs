using Endure.Data;
using Endure.Data.Models;
using Endure.Dispatcher.RabbitMQ.EventMessage;
using Endure.Dispatcher.RabbitMQ.Publisher;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services.Dispatcher;

internal class BaseDispatcherService<T>(
        DatabaseContext context,
        IMessagePublisher messagePublisher
    )
    : BaseService<T>(context)
    where T : BaseModel
{
    protected readonly IMessagePublisher _messagePublisher = messagePublisher;

    /// <summary>
    /// Checks if the root warehouse has a parent set.
    /// </summary>
    protected async Task<bool> RootWarehouseHasParentAsync()
        => await _context.Warehouse.AnyAsync(x => x.IsRoot && !x.IsDeleted && x.ParentId != null);

    protected async Task SynchronizeWithParent<T>(T message)
        where T : BaseEventMessage
    {
        Console.WriteLine("Should synchronize " + await RootWarehouseHasParentAsync());

        // If the root warehouse has a parent set, we should try and synchronize with the parent.
        if (!await RootWarehouseHasParentAsync())
            return;

        await _messagePublisher.PublishAsync(message);
    }
}
