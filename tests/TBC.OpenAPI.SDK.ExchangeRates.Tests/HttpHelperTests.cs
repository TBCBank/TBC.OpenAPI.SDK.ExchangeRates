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
