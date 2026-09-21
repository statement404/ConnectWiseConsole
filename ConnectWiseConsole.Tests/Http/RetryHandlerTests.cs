using System.Net;

using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Core.Models;
using ConnectWiseConsole.Tests.Fakes;

namespace ConnectWiseConsole.Tests.Http;

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
    public async Task SendAsync_Post500_IsNotRetried()
    {
        // Arrange
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.InternalServerError),
            new HttpResponseMessage(HttpStatusCode.OK));
        var retryHandler = new RetryHandler { InnerHandler = fakeHandler };
        var invoker = new HttpMessageInvoker(retryHandler);

        // Act
        var result = await invoker.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, "http://test/endpoint"),
            CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Equal(1, fakeHandler.CallCount);
    }

    [Fact]
    public async Task SendASync_Post503_TriesMultipleTimes()
    {
        //Arrange
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable),
            new HttpResponseMessage(HttpStatusCode.OK));
        var retryHandler = new RetryHandler { InnerHandler = fakeHandler };
        var invoker = new HttpMessageInvoker(retryHandler);

        // Act
        var result = await invoker.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, "http://test/endpoint"),
            CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(2, fakeHandler.CallCount);
    }

    [Fact]
    public async Task SendASync_Patch500_TriesOnce()
    {
        
        // Arrange
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.InternalServerError),
            new HttpResponseMessage(HttpStatusCode.OK));
        var retryHandler = new RetryHandler { InnerHandler = fakeHandler };
        var invoker = new HttpMessageInvoker(retryHandler);

        // Act
        var result = await invoker.SendAsync(
            new HttpRequestMessage(HttpMethod.Patch, "http://test/endpoint"),
            CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Equal(1, fakeHandler.CallCount);
        
    }
}
