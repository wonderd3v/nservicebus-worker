using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Intoku.Checkout.Worker.AzureServiceBus
{
    public static class ServiceBusAuthentication
    {
        public static ServiceBusClient AuthenticateToAzureServiceBus(IConfiguration Configuration, ILogger Logger)
        {
            var authenticationMode = Configuration.GetValue<AuthenticationMode>("SERVICEBUS_AUTH_MODE");

            ServiceBusClient serviceBusClient;

            switch (authenticationMode)
            {
                case AuthenticationMode.ConnectionString:
                    Logger.LogInformation($"Authentication by using connection string");
                    serviceBusClient = ServiceBusClientFactory.CreateWithConnectionStringAuthentication(Configuration);
                    break;
                case AuthenticationMode.ServicePrinciple:
                    Logger.LogInformation("Authentication by using service principle");
                    serviceBusClient = ServiceBusClientFactory.CreateWithServicePrincipleAuthentication(Configuration);
                    break;
                case AuthenticationMode.PodIdentity:
                    Logger.LogInformation("Authentication by using pod identity");
                    serviceBusClient = ServiceBusClientFactory.CreateWithPodIdentityAuthentication(Configuration, Logger);
                    break;
                case AuthenticationMode.WorkloadIdentity:
                    Logger.LogInformation("Authentication by using workload identity");
                    serviceBusClient = ServiceBusClientFactory.CreateWithWorkloadIdentityAuthentication(Configuration, Logger);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return serviceBusClient;
        }
    }
}
