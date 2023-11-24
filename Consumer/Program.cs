using Consumer.Consumers;
using Consumer.DAL;
using DataContracts.Settings;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddDbContext<DataContext>(options =>
        {
            var connectionString = ctx.Configuration.GetConnectionString("MainDB");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        services.AddHttpClient();
        services.BuildServiceProvider();
        services.AddMassTransit(x =>
        {
            x.AddConsumer<GetResponseByUrlEventConsumer>();
            x.UsingRabbitMq((context,cfg) =>
            {
                var settings = ctx.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                cfg.Host(settings.Host, "/", h => {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    })
    .Build();

#if DEBUG
using (var serviceScope = host.Services.GetService<IServiceScopeFactory>().CreateScope()) {
    var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
    context.Database.Migrate();
}
#endif
await host.RunAsync();