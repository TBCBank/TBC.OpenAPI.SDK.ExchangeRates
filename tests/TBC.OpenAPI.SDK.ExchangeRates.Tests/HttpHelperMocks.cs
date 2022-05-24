using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using TBC.OpenAPI.SDK.Core.Models;
using TBC.OpenAPI.SDK.ExchangeRates.Models;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace TBC.OpenAPI.SDK.Core.Tests
{
    public class HttpHelperMocks : IDisposable
    {
        public static string ErrorMessage = "Error Message For Mock";
        public static decimal ConvertionAmount = 100.5M;
        public static decimal OfficialRateEur = 3.5M;
        public static decimal CommercialRateToSellEur = 100.5M;

        private readonly WireMockServer _mockServer;
        private readonly HttpClient _httpClient;

        public HttpClient HttpClient => _httpClient;

        public HttpHelperMocks()
        {
            _mockServer = WireMockServer.Start();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"{_mockServer.Urls[0]}/");

            AddGetMocks();
        }

        private void AddGetMocks()
        {

            #region Get
            _mockServer
                .Given(
                    Request.Create()
                    .WithPath("/nbg/convert")
                    .UsingMethod("GET")

                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithBodyAsJson(new ConvertOfficialRatesResponse
                        {
                            Amount = ConvertionAmount,
                            From = "GEL",
                            To = "USD",
                            Value = 100.5M
                        })
                );

            _mockServer
                .Given(
                    Request.Create()
                    .WithPath("/nbg/convert")
                    .UsingMethod("GET")
                    .WithParam("from", new ExactMatcher("AAA"))

                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(400)
                        .WithBodyAsJson(new ProblemDetails
                        {
                            Title = ErrorMessage,
                            Detail = ErrorMessage,
                            Type = "error_type",
                            Status = (int)HttpStatusCode.BadRequest
                        })
                );

            _mockServer
                .Given(
                    Request.Create()
                    .WithPath("/commercial/convert")
                    .UsingMethod("GET")
                    .WithParam("from", new ExactMatcher("GEL"))
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithBodyAsJson(new ConvertCommercialRatesResponse
                        {
                            Amount = ConvertionAmount,
                            From = "GEL",
                            To = "USD",
                            Value = 100.5M
                        })
                );

            _mockServer
                .Given(
                    Request.Create()
                    .WithPath("/commercial/convert")
                    .UsingMethod("GET")
                    .WithParam("from", new ExactMatcher("AAA"))

                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(400)
                        .WithBodyAsJson(new ProblemDetails
                        {
                            Title = ErrorMessage,
                            Detail = ErrorMessage,
                            Type = "error_type",
                            Status = (int)HttpStatusCode.BadRequest
                        })
                );

            _mockServer
                .Given(
                    Request.Create()
                    .WithPath("/nbg")
                    .UsingMethod("GET")

                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithBodyAsJson(new List<OfficialRate> {
                            new OfficialRate
                            {
                                Currency = "EUR",
                                Value = OfficialRateEur
                            },
                            new OfficialRate
                            {
                                Currency = "USD",
                                Value = 2.9M
                            }
                        })
                );

            _mockServer
                .Given(
                    Request.Create()
                    .WithPath("/nbg")
                    .UsingMethod("GET")
                    .WithParam("currency", new ExactMatcher("AAA"))

                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(400)
                        .WithBodyAsJson(new ProblemDetails
                        {
                            Title = ErrorMessage,
                            Detail = ErrorMessage,
                            Type = "error_type",
                            Status = (int)HttpStatusCode.BadRequest
                        })
                );

            _mockServer
                .Given(
                    Request.Create()
                    .WithPath("/commercial")
                    .UsingMethod("GET")

                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithBodyAsJson(new GetCommercialRatesResponse { 
                            Base = "GEL",
                            CommercialRatesList = new List<CommercialRates>
                            {
                                new CommercialRates
                                {
                                    Buy = 100.5M,
                                    Currency = "EUR",
                                    Sell = CommercialRateToSellEur
                                },
                                new CommercialRates {
                                    Buy = 90.5M,
                                    Currency = "USD",
                                    Sell = 92.7M
                                }
                            }
                        })
                );

            _mockServer
                .Given(
                    Request.Create()
                    .WithPath("/commercial")
                    .UsingMethod("GET")
                    .WithParam("currency", new ExactMatcher("AAA"))
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(400)
                        .WithBodyAsJson(new ProblemDetails
                        {
                            Title = ErrorMessage,
                            Detail = ErrorMessage,
                            Type = "error_type",
                            Status = (int)HttpStatusCode.BadRequest
                        })
                );
            #endregion
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _mockServer.Dispose();
        }
    }
}