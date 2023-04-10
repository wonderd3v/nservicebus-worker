using Intoku.Checkout.Stripe;
using Intoku.Checkout.Stripe.Models;
using Intoku.Checkout.Worker.AzureServiceBus.QueueWorker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedContracts;

namespace Intoku.Checkout.Worker;

public class OrdersQueueProcessor : QueueWorker<Order>
{
    private readonly IStripeAppService _stripeAppService;
    public OrdersQueueProcessor(IConfiguration configuration, ILogger<OrdersQueueProcessor> logger, IStripeAppService stripeAppService)
        : base(configuration, logger) => _stripeAppService = stripeAppService;

    protected override async Task ProcessMessage(Order order, string messageId, IReadOnlyDictionary<string, object> userProperties, CancellationToken cancellationToken)
    {
        if (order.PaymentGateway is PaymentGateway.Stripe)
            await CallStripeGatewayAsync(order);

        Logger.LogInformation("Processing order {OrderId} for {OrderAmount} units of {OrderArticle} bought by {CustomerFirstName} {CustomerLastName}", order.Id, order.Amount, order.ArticleNumber, order.Customer.FirstName, order.Customer.LastName);
          
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

        Logger.LogInformation("Order {OrderId} processed", order.Id);
    }

    private async Task CallStripeGatewayAsync(Order order, CancellationToken cancellationToken = default)
    {
        // cus_NEgsup6awhMVhy

        var stripePayment = new AddStripePayment("cus_NEgsup6awhMVhy", "mejiahuascar@gmail.com", "Hola",  "USD", 2000);
            

        await _stripeAppService.AddStripePaymentAsync(stripePayment, cancellationToken);
        
    }
}