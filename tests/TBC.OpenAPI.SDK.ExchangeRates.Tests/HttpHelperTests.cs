using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using TBC.OpenAPI.SDK.Core.Models;
using TBC.OpenAPI.SDK.Core.Tests.Models;
using Xunit;

namespace TBC.OpenAPI.SDK.Core.Tests
{
    public class HttpHelperTests : IClassFixture<HttpHelperMocks>
    {
        private HttpHelper<TestClient> _http;
        public HttpHelperTests(HttpHelperMocks mocks)
        {
            var mock = new Mock<IHttpClientFactory>();
            mock.Setup(x => x.CreateClient(typeof(TestClient).FullName)).Returns(mocks.HttpClient);
            _http = new HttpHelper<TestClient>(mock.Object);
        }
        [Fact]
        public async Task GetJsonAsync_WhenClientNull_Throws()
        {
            //var mock = new Mock<IHttpClientFactory>();
            _http = new HttpHelper<TestClient>(null);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
              _http.GetJsonAsync<HttpTestResponseModel>("/some-resource", CancellationToken.None));

            using (new AssertionScope())
            {
                ex.Message.Equals("Value cannot be null. (Parameter 'httpClientFactory')");
            }
        }

        #region Get
        [Fact]
        public async Task GetJsonAsync_WhenResourceExists_ShouldRespondOkMessageWithData()
        {


            var response = await _http.GetJsonAsync<HttpTestResponseModel>("/some-resource", CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Data.Should().NotBeNull();
                response.Data.Id.Should().Be(1);
                response.Data.Name.Should().Be("One");
                response.Data.Date.Should().Be(new DateTime(2001, 1, 1));
                response.Data.Numbers.Should().Equal(new List<int>(3) { 1, 2, 3 });

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
            }
        }


        [Fact]
        public async Task GetJsonAsync_WhenResourceExistsWithHeader_ShouldRespondOkMessageWithData()
        {
            var header = new HeaderParamCollection()
            {
                {"param-header-key", "param-header-value"},
                {"param-header-key2", new List<string>{ "param-header-value2" } }
            };

            var response = await _http.GetJsonAsync<HttpTestResponseModel>("/some-resource", null, header, CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Data.Should().NotBeNull();
                response.Data.Id.Should().Be(1);
                response.Data.Name.Should().Be("One");
                response.Data.Date.Should().Be(new DateTime(2001, 1, 1));
                response.Data.Numbers.Should().Equal(new List<int>(3) { 1, 2, 3 });

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
                response.Headers.Should().Contain(x => x.Key == "param-header-key" && x.Value.Contains("param-header-value"));
            }
        }

        [Fact]
        public async Task GetJsonAsync_WhenWithQuery_ShouldRespondOkMessageWithData()
        {

            var query = new QueryParamCollection();
            query.Add("some-param", "some-value");
            query.Add("int-param", 1);
            query.Add("bool-param-true", true);
            query.Add("bool-param-false", false);
            query.Add("byte-param", (byte)1);
            query.Add("sbyte-param", (sbyte)1);
            query.Add("short-param", (short)1);
            query.Add("ushort-param", (ushort)1);
            query.Add("uint-param", (uint)1);
            query.Add("long-param", (long)1);
            query.Add("ulong-param", (ulong)1);
            query.Add("decimal-param", (decimal)1);
            query.Add("float-param", (float)1);
            query.Add("double-param", (double)1);
            query.Add("DateTime-param", DateTime.Now);
            query.Add("Guid-param", Guid.NewGuid());
            query.Add("DateOnly-param", new DateOnly(1991, 07, 22));
            query.Add("TimeOnly-param", new TimeOnly(11, 00, 00));
            query.Add("Dictionary-param", new Dictionary<string, string>() { { "dicParam", "dicValue" } });
            query.Add("Enum-param", TestEnum.Two);

            query.Add("null-param", null);
            query.Add("null-bool-param", (bool?)null);
            query.Add("null-byte-param", (byte?)null);
            query.Add("null-sbyte-param", (sbyte?)null);
            query.Add("null-short-param", (short?)null);
            query.Add("null-ushort-param", (ushort?)null);
            query.Add("null-int-param", (int?)null);
            query.Add("null-uint-param", (uint?)null);
            query.Add("null-long-param", (long?)null);
            query.Add("null-ulong-param", (ulong?)null);
            query.Add("null-decimal-param", (decimal?)null);
            query.Add("null-float-param", (float?)null);
            query.Add("null-double-param", (double?)null);
            query.Add("null-DateTime-param", (DateTime?)null);
            query.Add("null-Guid-param", (Guid?)null);
            query.Add("null-DateOnly-param", (DateOnly?)null);
            query.Add("null TimeOnly-param", (TimeOnly?)null);
            query.Add("null-Enum-param", (TestEnum?)null);

            query.Add("string-param-empty", "");
            query.Add("nullable-bool-true-param", (bool?)true);
            query.Add("nullable-bool-false-param", (bool?)false);
            query.Add("nullable-byte-param", (byte?)1);
            query.Add("nullable-sbyte-param", (sbyte?)1);
            query.Add("nullable-short-param", (short?)1);
            query.Add("nullable-ushort-param", (ushort?)1);
            query.Add("nullable-int-param", (int?)1);
            query.Add("nullable-uint-param", (uint?)1);
            query.Add("nullable-long-param", (long?)1);
            query.Add("nullable-ulong-param", (ulong?)1);
            query.Add("nullable-decimal-param", (decimal?)1);
            query.Add("nullable-float-param", (float?)1);
            query.Add("nullable-double-param", (double?)1);
            query.Add("nullable-DateTime-param", (DateTime?)DateTime.Now);
            query.Add("nullable-Guid-param", (Guid?)Guid.NewGuid());
            query.Add("nullable-DateOnly-param", (DateOnly?)DateOnly.MinValue);
            query.Add("nullable TimeOnly-param", (TimeOnly?)TimeOnly.MinValue);
            query.Add("nullable-Enum-param", (TestEnum?)TestEnum.One);


            query.Add("string-params", new List<string> { "par1", "par2", null });
            query.Add("int-params", new List<int> { 1, 2 });
            query.Add("long-params", new List<long> { 1, 2 });
            query.Add("DateTime-params", new List<DateTime> { DateTime.Now });
            query.Add("Guid-params", new List<Guid> { Guid.NewGuid() });
            query.Add("DateOnly-params", new List<DateOnly> { new DateOnly(1991, 07, 22) });
            query.Add("TimeOnly-params", new List<TimeOnly> { new TimeOnly(11, 00, 00) });
            query.Add("enum-params", new List<Enum>{TestEnum.Two});

            query.Add("null-string-params", (List<string>)null);
            query.Add("null-int-params", (List<int>)null);
            query.Add("null-long-params", (List<long>)null);
            query.Add("null-DateTime-params", (List<DateTime>)null);
            query.Add("null-Guid-params", (List<Guid>)null);
            query.Add("null-DateOnly-params", (List<DateOnly>)null);
            query.Add("null-TimeOnly-params", (List<TimeOnly>)null);
            query.Add("null-enum-params", (List<Enum>)null);


            var response = await _http.GetJsonAsync<HttpTestResponseModel>("/some-resource", query, CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Data.Should().NotBeNull();
                response.Data.Id.Should().Be(1);
                response.Data.Name.Should().Be("Two");
                response.Data.Date.Should().Be(new DateTime(2001, 1, 1));
                response.Data.Numbers.Should().Equal(new List<int>(3) { 1, 2, 3 });

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
            }
        }

        [Fact]
        public async Task GetJsonAsync_WhenEmptyQuery_ShouldRespondOkMessageWithData()
        {

            var query = new QueryParamCollection();

            var response = await _http.GetJsonAsync<HttpTestResponseModel>("/some-resource", query, CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Data.Should().NotBeNull();
                response.Data.Id.Should().Be(1);
                response.Data.Name.Should().Be("One");
                response.Data.Date.Should().Be(new DateTime(2001, 1, 1));
                response.Data.Numbers.Should().Equal(new List<int>(3) { 1, 2, 3 });

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
            }
        }

        [Fact]
        public async Task GetJsonAsync_WhenResourceDoesNotExists_ShouldRespondNotFoundMessageWithProblemDetails()
        {
            var response = await _http.GetJsonAsync<HttpTestResponseModel>("/some-resource/5", CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeFalse();

                response.Problem.Should().NotBeNull();
                response.Problem.Code.Should().Be("ResourceNotFound");
                response.Problem.Type.Should().Be("ResourceNotFound");
                response.Problem.Title.Should().Be("Requested Resource Not Found");
                response.Problem.Detail.Should().Contain("ID=5");
                response.Problem.Status.Should().Be(404);
                response.Problem.Instance.Should().Be("/some-resource/5");
                response.Problem.TraceId.Should().NotBeEmpty();

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
            }
        }



        [Fact]
        public async Task GetJsonAsync_WhenReturnsIncorectJson_ShouldReturnProblemDetails()
        {
            var response = await _http.GetJsonAsync<HttpTestResponseModel>("/some-resource/6", CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Problem.Should().NotBeNull();
                response.Problem.Code.Should().Be("MessageDeserializationError");
                response.Problem.Type.Should().Be("MessageDeserializationError");
                response.Problem.Title.Should().Be("Unable to deserialize response message");
                response.Problem.Status.Should().Be(200);

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));

            }
        }


        #endregion


        #region Post
        [Fact]
        public async Task PostJsonAsync_WhenQueryNull_ShouldRespondOk()
        {
            var request = new HttpTestRequestModel
            {
                Id = 1,
                Date = new DateTime(1991, 07, 22),
                Name = "TestName"
            };

            var response = await _http.PostJsonAsync<HttpTestRequestModel>("/some-resource", request, CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
            }
        }

        [Fact]
        public async Task PostJsonAsync_WhenQuery_ShouldRespondOk()
        {
            var request = new HttpTestRequestModel
            {
                Id = 1,
                Date = new DateTime(1991, 07, 22),
                Name = "TestName"
            };

            var query = new QueryParamCollection();
            query.Add("some-param", "some-value");

            var response = await _http.PostJsonAsync<HttpTestRequestModel>("/some-resource", request, query, CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
            }
        }

        [Fact]
        public async Task PostJsonAsync_WhenResponceHasDataAndNotQuery_ShouldRespondOk()
        {
            var request = new HttpTestRequestModel
            {
                Id = 1,
                Date = new DateTime(1991, 07, 22),
                Name = "TestName"
            };

            var response = await _http.PostJsonAsync<HttpTestRequestModel, HttpTestResponseModel>("/some-resource", request, CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Data.Id.Should().Be(1);
                response.Data.Name.Should().Be("TestName");
                response.Data.Date.Should().Be(new DateTime(1991, 07, 22));

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
            }
        }

        [Fact]
        public async Task PostJsonAsync_WhenResponceHasDataAndWithQuery_ShouldRespondOk()
        {
            var request = new HttpTestRequestModel
            {
                Id = 1,
                Date = new DateTime(1991, 07, 22),
                Name = "TestName"
            };

            var query = new QueryParamCollection();
            query.Add("some-param", "some-value");

            var response = await _http.PostJsonAsync<HttpTestRequestModel, HttpTestResponseModel>("/some-resource", request, query, CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
            }
        }

        #endregion



        #region Put
        [Fact]
        public async Task PutJsonAsync_WhenQueryNull_ShouldRespondOk()
        {
            var request = new HttpTestRequestModel
            {
                Id = 2,
                Date = new DateTime(1991, 07, 22),
                Name = "TestName"
            };

            var response = await _http.PutJsonAsync<HttpTestRequestModel>("/some-resource", request, CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
            }
        }

        [Fact]
        public async Task PutJsonAsync_WhenQuery_ShouldRespondOk()
        {
            var request = new HttpTestRequestModel
            {
                Id = 1,
                Date = new DateTime(1991, 07, 22),
                Name = "TestName"
            };

            var query = new QueryParamCollection();
            query.Add("some-param", "some-value");

            var response = await _http.PutJsonAsync<HttpTestRequestModel>("/some-resource", request, query, CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
            }
        }

        [Fact]
        public async Task PutJsonAsync_WhenResponceHasDataAndNotQuery_ShouldRespondOk()
        {
            var request = new HttpTestRequestModel
            {
                Id = 2,
                Date = new DateTime(1991, 07, 22),
                Name = "TestName"
            };

            var response = await _http.PutJsonAsync<HttpTestRequestModel, HttpTestResponseModel>("/some-resource", request, CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Data.Id.Should().Be(2);
                response.Data.Name.Should().Be("TestName");
                response.Data.Date.Should().Be(new DateTime(1991, 07, 22));

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
            }
        }

        [Fact]
        public async Task PutJsonAsync_WhenResponceHasDataAndWithQuery_ShouldRespondOk()
        {
            var request = new HttpTestRequestModel
            {
                Id = 2,
                Date = new DateTime(1991, 07, 22),
                Name = "TestName"
            };

            var query = new QueryParamCollection();
            query.Add("some-param", "some-value");

            var response = await _http.PutJsonAsync<HttpTestRequestModel, HttpTestResponseModel>("/some-resource", request, query, CancellationToken.None)
                .ConfigureAwait(false);

            using (new AssertionScope())
            {
                response.IsSuccess.Should().BeTrue();

                response.Headers.Should().NotBeNull();
                response.Headers.Should().Contain(x => x.Key == "some-header" && x.Value.Contains("some value"));
                response.Headers.Should().Contain(x => x.Key == "another-header" && x.Value.Contains("another value"));
            }
        }

        #endregion

    }
}
