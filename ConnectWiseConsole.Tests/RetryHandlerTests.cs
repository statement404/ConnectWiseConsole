using System.Net;
using ConnectWiseConsole.Core.Http;
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
}
