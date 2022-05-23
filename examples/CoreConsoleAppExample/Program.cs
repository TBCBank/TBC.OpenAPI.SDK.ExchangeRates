// See https://aka.ms/new-console-template for more information

using TBC.OpenAPI.SDK.Core;
using TBC.OpenAPI.SDK.ExchangeRates;
using TBC.OpenAPI.SDK.ExchangeRates.Extensions;

var factory = new OpenApiClientFactoryBuilder()
    .AddExchangeRatesClient(new ExchangeRatesClientOptions
    {
        BaseUrl = "https://run.mocky.io/v3/7690b5f0-cc43-4c03-b07f-2240b4448931/",
        ApiKey = "abc"
    })
    .Build();


var client = factory.GetExchangeRatesClient();

var result = client.GetSomeObjectAsync().GetAwaiter().GetResult();

Console.WriteLine($"Result: {result.Name}");

Console.ReadLine();