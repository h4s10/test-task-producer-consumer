using DataContracts.Events;
using DataContracts.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Producer;
using Serilog;
using Utils.Extensions;

var configuration = Init.InitConfiguration();
var rabbitMQConfig = new RabbitMQSettings();
configuration.GetSection(nameof(RabbitMQSettings)).Bind(rabbitMQConfig);

Log.Logger = Init.InitLogger();

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    Log.Information("Canceling...");
    cts.Cancel();
    e.Cancel = true;
};

var filePath = Environment.GetCommandLineArgs()[1];
var urls = File.ReadAllLines(filePath).Where(x => x.IsUrl());
var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host(rabbitMQConfig.Host, "/", h => {
        h.Username(rabbitMQConfig.Username);
        h.Password(rabbitMQConfig.Password);
    });
});
await bus.StartAsync(cts.Token);
Log.Information("Bus started");

var tasks = new List<Task>(10);
tasks.AddRange(urls.Select(url => bus.Publish<IGetResponseByUrlEvent>(new { Url = url }, cts.Token)));
try
{
    await Task.WhenAll(tasks);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error while publishing events");
}

await bus.StopAsync(cts.Token);
Log.Information("Done");
await Log.CloseAndFlushAsync();