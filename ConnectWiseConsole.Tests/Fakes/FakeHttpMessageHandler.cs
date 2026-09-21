using System.Net;

namespace ConnectWiseConsole.Tests.Fakes;

public class FakeHttpMessageHandler(HttpStatusCode statusCode, string responseContent) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(responseContent)
        };

        return Task.FromResult(response);
    }
}