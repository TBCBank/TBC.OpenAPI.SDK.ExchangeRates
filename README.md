# TBC.OpenAPI.SDK.ExchangeRates
​
[![NuGet version (TBC.OpenAPI.SDK.ExchangeRates)](https://img.shields.io/nuget/v/TBC.OpenAPI.SDK.ExchangeRates.svg?label=TBC.OpenAPI.SDK.ExchangeRates)](https://www.nuget.org/packages/TBC.OpenAPI.SDK.ExchangeRates/) [![CI](https://github.com/TBCBank/TBC.OpenAPI.SDK.ExchangeRates/actions/workflows/main.yml/badge.svg?branch=master)](https://github.com/TBCBank/TBC.OpenAPI.SDK.ExchangeRates/actions/workflows/main.yml)
​
Exchange Rates SDKs for TBC OpenAPI
​
## Exchange Rate SDK
​
Repository contains the SDK for simplifying TBC Open API Exchange Rate API invocations on C# client application side.\
​
Library is written in the C # programming language and is compatible with .netstandard2.0 and .net6.0.
​
## Prerequisites
​
In order to use the SDK it is mandatory to have **apikey** from TBC Bank's OpenAPI Devportal.\
​
[See more details how to get apikey](https://developers.tbcbank.ge/docs/get-apikey-and-secret)
​
## .Net Core Usage
​
First step is to configure appsettings.json file with Exchange rate endpoint and TBC Portal **apikey**\
​
appsettings.json
​
```json
​
"ExchangeRates": {
​
"BaseUrl": "https://tbcbank-test.apigee.net/v1/exchange-rates/",
​
"ApiKey": "{apikey}"
​
}
​
```
​
Then add Exchange rate client as an dependency injection\
​
Program.cs
​
```cs
​
builder.Services.AddExchangeRatesClient(builder.Configuration.GetSection("ExchangeRates").Get<ExchangeRatesClientOptions>());
​
```
​
After two steps above, setup is done and Exchange rate client can be injected in any container class:
​
Injection example:
​
```cs
​
private readonly IExchangeRatesClient _exchangeRatesClient;
​
public TestController(IExchangeRatesClient exchangeRatesClient)
​
{
​
_exchangeRatesClient = exchangeRatesClient;
​
}
​
```
​
Api invocation example:
​
```cs
​
var result = await _exchangeRatesClient.GetOfficialRates(
​
new string[] { "EUR", "USD"}
​
,cancellationToken
​
);
​
```
​
## NetFramework Usage
​
First step is to configure Web.config file with Exchange rate endpoint and TBC Portal **apikey**\
​
Web.config
​
```xml
​
<add key="ExchangeRatesUrl" value="https://tbcbank-test.apigee.net/v1/exchange-rates/" />
​
<add key="ExchangeRatesKey" value="{apikey}" />
​
```
​
In the Global.asax file at Application_Start() add following code\
​
Global.asax
​
```cs
​
new OpenApiClientFactoryBuilder()
​
.AddExchangeRatesClient(new ExchangeRatesClientOptions
​
{
​
BaseUrl = ConfigurationManager.AppSettings["ExchangeRatesUrl"],
​
ApiKey = ConfigurationManager.AppSettings["ExchangeRatesKey"]
​
})
​
.Build();
​
```
​
This code reads config parameters and then creates singleton OpenApiClientFactory, which is used to instantiate Exchange rate client.\
​
ExchangeRatesClient class instantiation and invocation example:
​
```cs
​
var exchangeRatesClient = OpenApiClientFactory.Instance.GetExchangeRatesClient();
​
var result = await exchangeRatesClient.GetOfficialRates(new string[] { "EUR", "USD"});
​
```
​
For more details see examples in repo:
​
​

[CoreApiAppExmaple](https://github.com/TBCBank/TBC.OpenAPI.SDK.ExchangeRates/tree/master/examples/CoreApiAppExmaple)
​

[CoreConsoleAppExample](https://github.com/TBCBank/TBC.OpenAPI.SDK.ExchangeRates/tree/master/examples/CoreConsoleAppExample)
​

[NetFrameworkExample](https://github.com/TBCBank/TBC.OpenAPI.SDK.ExchangeRates/tree/master/examples/NetFrameworkExample)