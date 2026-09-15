namespace ConnectWiseConsole.Tests.Http;

public class SequencedFakeHttpMessageHandler(params HttpResponseMessage[] responses) : HttpMessageHandler
{
    private int _callCount = 0;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = responses[_callCount]; //no try here, as we want this to throw if out of bounds
        _callCount++;
        return Task.FromResult(response);
    }
}
