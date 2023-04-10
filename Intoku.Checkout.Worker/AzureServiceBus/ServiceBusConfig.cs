namespace Intoku.Checkout.Worker.AzureServiceBus
{
    public class ServiceBusConfig
    {
        public string? ConnectionString { get; set; }
        public string? QueueName { get; set; }
    }
}
