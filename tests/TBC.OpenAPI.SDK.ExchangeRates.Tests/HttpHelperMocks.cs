using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using TBC.OpenAPI.SDK.Core.Models;
using TBC.OpenAPI.SDK.Core.Tests.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace TBC.OpenAPI.SDK.Core.Tests
{
    public class HttpHelperMocks : IDisposable
    {
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
                    .WithPath("/some-resource")
                    .UsingMethod("GET")

                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("some-header", "some value")
                        .WithHeader("another-header", "another value")
                        .WithHeader( "param-header-key", "param-header-value")
                        .WithBodyAsJson(new HttpTestResponseModel
                        {
                            Id = 1,
                            Name = "One",
                            Date = new DateTime(2001, 1, 1),
                            Numbers = new List<int>(3) { 1, 2, 3 }
                        })
                );

            _mockServer
               .Given(
                   Request.Create()
                   .WithPath("/some-resource")
                   .WithParam("some-param")
                   .UsingMethod("GET")
               )
               .RespondWith(
                   Response.Create()
                       .WithStatusCode(200)
                       .WithHeader("some-header", "some value")
                       .WithHeader("another-header", "another value")
                       .WithBodyAsJson(new HttpTestResponseModel
                       {
                           Id = 1,
                           Name = "Two",
                           Date = new DateTime(2001, 1, 1),
                           Numbers = new List<int>(3) { 1, 2, 3 }
                       })
               );

            _mockServer
                .Given(
                    Request.Create()
                        .WithPath("/some-resource/5")
                        .UsingMethod("GET")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(404)
                        .WithHeader("some-header", "some value")
                        .WithHeader("another-header", "another value")
                        .WithBody(x =>
                        {
                            var problem = new ProblemDetails
                            {
                                Type = "ResourceNotFound",
                                Code = "ResourceNotFound",
                                Title = "Requested Resource Not Found",
                                Detail = "some-resource with ID=5 not found",
                                Status = 404,
                                Instance = "/some-resource/5",
                                TraceId = "00-0af7651916cd43dd8448eb211c80319c-00f067aa0ba902b7-01",
                                Errors = new Dictionary<string, string?[]?>()
                                {
                                    {"ErrorKey", new string[]{ "ResourceNotFound" } }
                                }
                            };
                            return JsonSerializer.Serialize(problem);
                        })
                );


            _mockServer
                .Given(
                    Request.Create()
                        .WithPath("/some-resource/6")
                        .UsingMethod("GET")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("some-header", "some value")
                        .WithHeader("another-header", "another value")
                        .WithBody(x =>
                        {
                            
                            return JsonSerializer.Serialize("{/");
                        })
                );


            #endregion

            #region Post
            _mockServer
               .Given(
                   Request.Create()
                   .WithPath("/some-resource")
                   .UsingMethod("POST")
               )
               .RespondWith(
                   Response.Create()
                       .WithStatusCode(200)
                       .WithHeader("some-header", "some value")
                       .WithHeader("another-header", "another value")
                       .WithBodyAsJson(new HttpTestResponseModel
                       {
                           Id = 1,
                           Name = "TestName",
                           Date = new DateTime(1991, 07, 22)
                       })
               );

            _mockServer
               .Given(
                   Request.Create()
                   .WithPath("/some-resource")
                   .WithParam("some-param")
                   .UsingMethod("POST")
               )
               .RespondWith(
                   Response.Create()
                       .WithStatusCode(200)
                       .WithHeader("some-header", "some value")
                       .WithHeader("another-header", "another value")
                       .WithBodyAsJson(new HttpTestResponseModel
                       {
                           Id = 1,
                           Name = "TestName",
                           Date = new DateTime(1991, 07, 22)
                       })
               );


            _mockServer
               .Given(
                   Request.Create()
                   .WithPath("/some-resource")
                   .WithBody(new HttpTestRequestModel
                   {
                       Id = 1,
                       Date = new DateTime(1991, 07, 22),
                       Name = "TestName"
                   })
                   .UsingMethod("POST")
               )
               .RespondWith(
                   Response.Create()
                       .WithStatusCode(200)
                       .WithHeader("some-header", "some value")
                       .WithHeader("another-header", "another value")
                       .WithBodyAsJson(new HttpTestResponseModel
                       {
                           Id = 1,
                           Name = "TestName",
                           Date = new DateTime(1991, 07, 22)
                       })
               );

            _mockServer
               .Given(
                   Request.Create()
                   .WithPath("/some-resource")
                   .WithBody(new HttpTestRequestModel
                   {
                       Id = 1,
                       Date = new DateTime(1991, 07, 22),
                       Name = "TestName"
                   })
                   .WithParam("some-param")
                   .UsingMethod("POST")
               )
                .RespondWith(
                   Response.Create()
                       .WithStatusCode(200)
                       .WithHeader("some-header", "some value")
                       .WithHeader("another-header", "another value")
                       .WithBodyAsJson(new HttpTestResponseModel
                       {
                           Id = 1,
                           Name = "TestName",
                           Date = new DateTime(1991, 07, 22)
                       })
               );
            #endregion

            #region Put

            _mockServer
               .Given(
                   Request.Create()
                   .WithPath("/some-resource")
                   .UsingMethod("PUT")
               )
               .RespondWith(
                   Response.Create()
                       .WithStatusCode(200)
                       .WithHeader("some-header", "some value")
                       .WithHeader("another-header", "another value")
                       .WithBodyAsJson(new HttpTestResponseModel
                       {
                           Id = 2,
                           Name = "TestName",
                           Date = new DateTime(1991, 07, 22)
                       })
               );

            _mockServer
               .Given(
                   Request.Create()
                   .WithPath("/some-resource")
                   .WithParam("some-param")
                   .UsingMethod("PUT")
               )
               .RespondWith(
                   Response.Create()
                       .WithStatusCode(200)
                       .WithHeader("some-header", "some value")
                       .WithHeader("another-header", "another value")
                       .WithBodyAsJson(new HttpTestResponseModel
                       {
                           Id = 2,
                           Name = "TestName",
                           Date = new DateTime(1991, 07, 22)
                       })
               );


            _mockServer
               .Given(
                   Request.Create()
                   .WithPath("/some-resource")
                   .UsingMethod("PUT")
                   .WithBody(new HttpTestRequestModel
                   {
                       Id = 2,
                       Date = new DateTime(1991, 07, 22),
                       Name = "TestName"
                   })
               )
               .RespondWith(
                   Response.Create()
                       .WithStatusCode(200)
                       .WithHeader("some-header", "some value")
                       .WithHeader("another-header", "another value")
                       .WithBodyAsJson(new HttpTestResponseModel
                       {
                           Id = 2,
                           Name = "TestName",
                           Date = new DateTime(1991, 07, 22)
                       })
               );

            _mockServer
               .Given(
                   Request.Create()
                   .WithPath("/some-resource")
                   .WithParam("some-param")
                   .WithBody(new HttpTestRequestModel
                   {
                       Id = 2,
                       Date = new DateTime(1991, 07, 22),
                       Name = "TestName"
                   })
                   .UsingMethod("PUT")
               )
                .RespondWith(
                   Response.Create()
                       .WithStatusCode(200)
                       .WithHeader("some-header", "some value")
                       .WithHeader("another-header", "another value")
                       .WithBodyAsJson(new HttpTestResponseModel
                       {
                           Id = 2,
                           Name = "TestName",
                           Date = new DateTime(1991, 07, 22)
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
