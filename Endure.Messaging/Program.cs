using Endure.Messaging.MqttConsumer;
using Endure.Messaging.RabbitMqConsumer;
using Endure.Service;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.RegisterWorkerServices(builder.Configuration);

// Configure all of the consumer services, and workers.
builder.Services.RegisterRabbitMqDispatcherServices(builder.Configuration);
builder.Services.RegisterMqttDispatcherServices(builder.Configuration);


var host = builder.Build();
host.Run();
