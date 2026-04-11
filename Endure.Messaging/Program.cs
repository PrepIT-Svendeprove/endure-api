using Endure.Messaging.Consumer;
using Endure.Service;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.RegisterServices(builder.Configuration);

// Configure all of the consumer services, and workers.
builder.Services.RegisterDispatcherServices(builder.Configuration);

var host = builder.Build();
host.Run();
