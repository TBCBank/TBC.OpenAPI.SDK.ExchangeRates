// See https://aka.ms/new-console-template for more information

using TBC.OpenAPI.SDK.Core;
using TBC.OpenAPI.SDK.ExchangeRates;
using TBC.OpenAPI.SDK.ExchangeRates.Extensions;
using TBC.OpenAPI.SDK.ExchangeRates.Models;

var factory = new OpenApiClientFactoryBuilder()
    .AddExchangeRatesClient(new ExchangeRatesClientOptions
    {
        BaseUrl = "https://tbcbank-test.apigee.net/v1/exchange-rates/",
        ApiKey = "{apikey}"
    })
    .Build();


var client = factory.GetExchangeRatesClient();

var result = client.GetOfficialRates(new string[] { "EUR", "USD"}).GetAwaiter().GetResult();

Console.WriteLine($"Result: {result.Aggregate("", (str, rate)=> str + $"\n{rate.Currency}:{rate.Value}")}");

Console.ReadLine();