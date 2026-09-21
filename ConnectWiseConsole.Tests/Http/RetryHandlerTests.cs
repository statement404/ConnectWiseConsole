using System.Net;
using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Core.Models;
using ConnectWiseConsole.Tests.Fakes;

namespace ConnectWiseConsole.Tests;

public class RetryHandlerTests
{
    [Fact]
    public async Task SendAsync_RetriesOnTransientFailure_ThenSucceeds()
    {
        // Arrange
        var responses = new[]
        {
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable),
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable),
            new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("success") }
        };
        var sequencedHandler = new SequencedFakeHttpMessageHandler(responses);
        var retryHandler = new RetryHandler { InnerHandler = sequencedHandler };
        var invoker = new HttpMessageInvoker(retryHandler);

        // Act
        var result = await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://test/endpoint"), CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task SendAsync_ThrowsAfterExhaustingRetries()
    {
        // Arrange
        var responses = new[]
        {
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable),
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable),
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable),
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
        };
        var sequencedHandler = new SequencedFakeHttpMessageHandler(responses);
        var retryHandler = new RetryHandler { InnerHandler = sequencedHandler };
        var invoker = new HttpMessageInvoker(retryHandler);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://test/endpoint"), CancellationToken.None));
    }

    [Fact]
    public async Task PostAsync_500_TriesOnce()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error")
            },
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Fake api OK response")
            });
        var retryHandler = new RetryHandler { InnerHandler = fakeHandler };
        var client = new CwHttpClient(retryHandler, credData);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            client.PostAsync("fakepostendpoint", new string[] { "test" }));

        Assert.Equal(1, fakeHandler.CallCount);
    }

    [Fact]
    public async Task PostAsync_503_TriesMultipleTimes()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.ServiceUnavailable,
                Content = new StringContent("Service Unavailable")
            },
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Fake api OK response")
            });
        var retryHandler = new RetryHandler { InnerHandler = fakeHandler };
        var client = new CwHttpClient(retryHandler, credData);

        // Act & Assert
        var result = await client.PostAsync("fakepostendpoint", new string[] { "test" });

        Assert.Equal(2, fakeHandler.CallCount);
    }

    [Fact]
    public async Task PatchAsync_500_TriesOnce()
    {
        //Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error")
            },
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Fake api OK response")
            });
        var retryHandler = new RetryHandler { InnerHandler = fakeHandler };
        var client = new CwHttpClient(retryHandler, credData);

        //Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            client.PatchAsync("fakepatchendpoint", new List<CwPatchOperation>{
                new CwPatchOperation
                {
                    Op = "replace",
                    Path = "testpath",
                    Value = "testvalue"
                }
            }));
        
        Assert.Equal(1, fakeHandler.CallCount);
    }
}
