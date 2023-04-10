namespace Intoku.Checkout.Stripe.Models
{
    public record StripeCustomer(
        string Name,
        string Email,
        string CustomerId);
}
