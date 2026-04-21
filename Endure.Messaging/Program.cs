using Endure.Data;
using Endure.Messaging.MqttConsumer;
using Endure.Messaging.RabbitMqConsumer;
using Endure.Service;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.RegisterWorkerServices(builder.Configuration);

// Configure all of the consumer services, and workers.
builder.Services.RegisterRabbitMqDispatcherServices(builder.Configuration);
builder.Services.RegisterMqttDispatcherServices(builder.Configuration);

var host = builder.Build();

// Run migrations on start
using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    await db.Database.MigrateAsync();
}

host.Run();
