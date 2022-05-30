using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using TBC.OpenAPI.SDK.Core.Exceptions;
using TBC.OpenAPI.SDK.Core.Models;
using TBC.OpenAPI.SDK.ExchangeRates;
using Xunit;

namespace TBC.OpenAPI.SDK.Core.Tests
{
    public class ExchangeRatesClientTests : IClassFixture<HttpHelperMocks>
    {
        private ExchangeRatesClient _client;
        public ExchangeRatesClientTests(HttpHelperMocks mocks)
        {
            var mock = new Mock<IHttpClientFactory>();
            mock.Setup(x => x.CreateClient(typeof(ExchangeRatesClient).FullName)).Returns(mocks.HttpClient);
            var http = new HttpHelper<ExchangeRatesClient>(mock.Object);

            _client = new ExchangeRatesClient(http);
        }

        [Fact]
        public async Task ConvertCommercialRate_WhenSuccess_ReturnData()
        {
            var expected = HttpHelperMocks.ConvertionAmount;

            var result = await _client.ConvertCommercialRate(100.5M, "GEL", "USD" , CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Amount.Should().Be(expected);
            }
        }

        [Fact]
        public async Task ConvertCommercialRate_WhenNotSuccess_ReturnProblemJson()
        {
            var message = HttpHelperMocks.ErrorMessage;

            Func<Task> act = async () => await _client.ConvertCommercialRate(100.5M, "AAA", "AAA", CancellationToken.None);

            await act.Should().ThrowAsync<OpenApiException>()
                .Where(e => e.Message.StartsWith(HttpHelperMocks.ErrorMessage));
        }

        [Fact]
        public async Task GetCommercialRates_WhenSuccess_ReturnData()
        {
            var expected = "GEL";
            var result = await _client.GetCommercialRates(new string[] { "EUR", "USD" }, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Base.Should().Be(expected);
                result.CommercialRatesList.Count.Should().Be(2);
                result.CommercialRatesList[0].Sell.Should().Be(HttpHelperMocks.CommercialRateToSellEur);
            }
        }

        [Fact]
        public async Task GetCommercialRates_WhenNotSuccess_ReturnProblemJson()
        {
            var message = HttpHelperMocks.ErrorMessage;

            Func <Task> act = async () => await _client.GetCommercialRates(new string[] { "AAA" }, CancellationToken.None);

            await act.Should().ThrowAsync<OpenApiException>()
                .Where(e => e.Message.StartsWith(HttpHelperMocks.ErrorMessage));
        }

        [Fact]
        public async Task GetOfficialRates_WhenSuccess_ReturnData()
        {
            var expectedValue = HttpHelperMocks.OfficialRateEur;
            var expectedCount = 2;

            var result = await _client.GetOfficialRates(new string[] { "EUR", "USD" }, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Count.Should().Be(expectedCount);
                result[0].Value.Should().Be(expectedValue);
            }
        }

        [Fact]
        public async Task GetOfficialRates_WhenNotSuccess_ReturnProblemJson()
        {
            var message = HttpHelperMocks.ErrorMessage;

            Func<Task> act = async () => await _client.GetOfficialRates(new string[] { "AAA" }, CancellationToken.None);

            await act.Should().ThrowAsync<OpenApiException>()
                .Where(e => e.Message.StartsWith(HttpHelperMocks.ErrorMessage));
        }

        [Fact]
        public async Task ConvertOfficialRates_WhenSuccess_ReturnData()
        {
            var expected = HttpHelperMocks.ConvertionAmount;
            var result = await _client.ConvertOfficialRates(100.5M, "GEL", "USD", CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Amount.Should().Be(expected);
            }
        }

        [Fact]
        public async Task ConvertOfficialRates_WhenNotSuccess_ReturnProblemJson()
        {
            var message = HttpHelperMocks.ErrorMessage;

            Func<Task> act = async () => await _client.ConvertOfficialRates(100.5M, "AAA", "AAA", CancellationToken.None);

            await act.Should().ThrowAsync<OpenApiException>()
                        .Where(e => e.Message.StartsWith(HttpHelperMocks.ErrorMessage));
        }
    }
}
