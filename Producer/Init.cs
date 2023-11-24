using Microsoft.Extensions.Configuration;
using Serilog;

namespace Producer;

public static class Init
{
    public static ILogger InitLogger() =>
        new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
    
    public static IConfigurationRoot InitConfiguration() =>
        new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
}