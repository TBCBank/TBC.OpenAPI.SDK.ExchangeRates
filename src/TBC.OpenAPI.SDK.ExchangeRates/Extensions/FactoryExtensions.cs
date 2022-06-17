using TBC.OpenAPI.SDK.Core;

namespace TBC.OpenAPI.SDK.ExchangeRates.Extensions
{
    public static class FactoryExtensions
    {
        public static OpenApiClientFactoryBuilder AddExchangeRatesClient(this OpenApiClientFactoryBuilder builder,
            ExchangeRatesClientOptions options) => AddExchangeRatesClient(builder, options, null, null);

        public static OpenApiClientFactoryBuilder AddExchangeRatesClient(this OpenApiClientFactoryBuilder builder,
            ExchangeRatesClientOptions options,
            Action<HttpClient> configureClient = null,
            Func<HttpClientHandler> configureHttpMessageHandler = null)
        {
            return builder.AddClient<IExchangeRatesClient, ExchangeRatesClient, ExchangeRatesClientOptions>(options, configureClient, configureHttpMessageHandler);
        }

        public static IExchangeRatesClient GetExchangeRatesClient(this OpenApiClientFactory factory) =>
            factory.GetOpenApiClient<IExchangeRatesClient>();

    }
}