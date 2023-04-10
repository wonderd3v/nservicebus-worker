using Intoku.Checkout.Stripe.Models;

namespace Intoku.Checkout.Stripe;

public interface IStripeAppService
{
    Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken cancellationToken);
    Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken cancelationToken);
}
