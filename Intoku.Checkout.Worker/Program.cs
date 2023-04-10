using Intoku.Checkout.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
        })
        .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
        {
            _ = loggingBuilder.AddConsole(consoleLoggerOptions => consoleLoggerOptions.TimestampFormat = "[HH:mm:ss]");
        })
        .ConfigureServices((context, services) =>
        {
            Startup startup = new(context.Configuration);
            startup.ConfigureServices(services);
        });
}
