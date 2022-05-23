using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using TBC.OpenAPI.SDK.Core;
using TBC.OpenAPI.SDK.ExchangeRates;
using TBC.OpenAPI.SDK.ExchangeRates.Extensions;

namespace NetFrameworkExample
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            new OpenApiClientFactoryBuilder()
                .AddExchangeRatesClient(new ExchangeRatesClientOptions
                {
                    BaseUrl = ConfigurationManager.AppSettings["ExchangeRatesUrl"],
                    ApiKey = ConfigurationManager.AppSettings["ExchangeRatesKey"]
                })
                .Build();
        }
    }
}
