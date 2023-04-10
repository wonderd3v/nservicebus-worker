using Intoku.Checkout.Stripe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

namespace Intoku.Checkout.Worker.IoC
{
    public static class StripeInfrastructure
    {
        public static IServiceCollection AddStripeInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //StripeConfiguration.ApiKey = configuration.GetValue<string>("StripeSettings:SecretKey");
            StripeConfiguration.ApiKey = "sk_test_51MEvC7L7Wg73OsnZuSzOSZO0cyaoVPyry9WvQhNsamJ77O0h8du66e683LTjKMuQckYSdBs83YQpdQ0S9wpVPYjP00zCMZkDKc";

            return services
                .AddScoped<CustomerService>()
                .AddScoped<ChargeService>()
                .AddScoped<TokenService>()
                .AddScoped<IStripeAppService, StripeAppService>();
        }
    }
}
