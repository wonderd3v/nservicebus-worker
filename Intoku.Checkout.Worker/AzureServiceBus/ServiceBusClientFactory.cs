using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Intoku.Checkout.Worker.AzureServiceBus
{
    public static class ServiceBusClientFactory
    {
        public static ServiceBusClient CreateWithPodIdentityAuthentication(IConfiguration configuration, ILogger logger)
        {
            var hostname = configuration.GetValue<string>("SERVICEBUS_HOST_NAME");

            var clientIdentityId = configuration.GetValue<string>("SERVICEBUS_IDENTITY_USERASSIGNEDID", defaultValue: null);
            if (string.IsNullOrWhiteSpace(clientIdentityId) == false)
            {
                logger.LogInformation("Using user-assigned identity with ID {UserAssignedIdentityId}", clientIdentityId);
            }

            return new ServiceBusClient(hostname, new ManagedIdentityCredential(clientId: clientIdentityId));
        }

        public static ServiceBusClient CreateWithWorkloadIdentityAuthentication(IConfiguration configuration, ILogger logger)
        {
            var hostname = configuration.GetValue<string>("SERVICEBUS_HOST_NAME");

            return new ServiceBusClient(hostname, new ManagedIdentityCredential());
        }

        public static ServiceBusClient CreateWithServicePrincipleAuthentication(IConfiguration configuration)
        {
            var hostname = configuration.GetValue<string>("SERVICEBUS_HOST_NAME");
            var tenantId = configuration.GetValue<string>("SERVICEBUS_TENANT_ID");
            var appIdentityId = configuration.GetValue<string>("SERVICEBUS_IDENTITY_APPID");
            var appIdentitySecret = configuration.GetValue<string>("SERVICEBUS_IDENTITY_SECRET");

            return new ServiceBusClient(hostname, new ClientSecretCredential(tenantId, appIdentityId, appIdentitySecret));
        }

        public static ServiceBusClient CreateWithConnectionStringAuthentication(IConfiguration configuration)
        {
            var connectionString = "Endpoint=sb://intoku.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=R+tOdMuqU01TJpE6xcT9hwARKROGNfdZBmnMmzR3HZ8=";
            //var connectionString = configuration.GetValue<string>("SERVICEBUS_QUEUE_CONNECTIONSTRING");
            return new ServiceBusClient(connectionString);
        }
    }
}
