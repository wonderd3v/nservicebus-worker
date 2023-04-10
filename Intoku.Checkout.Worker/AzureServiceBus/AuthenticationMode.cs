namespace Intoku.Checkout.Worker.AzureServiceBus
{
    public enum AuthenticationMode
    {
        ConnectionString,
        ServicePrinciple,
        PodIdentity,
        WorkloadIdentity
    }
}
