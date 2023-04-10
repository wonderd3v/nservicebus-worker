using Intoku.Checkout.Worker.AzureServiceBus;
using Intoku.Checkout.Worker.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Intoku.Checkout.Worker;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddStripeInfrastructure(_configuration);
        services.AddHostedService<OrdersQueueProcessor>();
        services.AddOptions();
        services.Configure<ServiceBusConfig>(_configuration.GetSection("ServiceBusConfig"));
        services.AddHostedService<OrdersQueueProcessor>();
    }
}
