using Microsoft.Extensions.DependencyInjection;
using TBC.OpenAPI.SDK.Core.Extensions;

namespace TBC.OpenAPI.SDK.ExchangeRates.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExchangeRatesClient(this IServiceCollection services, ExchangeRatesClientOptions options) 
            => AddExchangeRatesClient(services, options, null, null);

        public static IServiceCollection AddExchangeRatesClient(this IServiceCollection services, ExchangeRatesClientOptions options,
            Action<HttpClient> configureClient = null,
            Func<HttpClientHandler> configureHttpMessageHandler = null)
        {
            services.AddOpenApiClient<IExchangeRatesClient, ExchangeRatesClient, ExchangeRatesClientOptions>(options, configureClient, configureHttpMessageHandler);
            return services;
        }
    }
}
