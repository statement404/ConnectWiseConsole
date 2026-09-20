namespace ConnectWiseConsole.Tests.Http;

public class SequencedFakeHttpMessageHandler(params HttpResponseMessage[] responses) : HttpMessageHandler
{
    public int CallCount { get; private set; } = 0;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = responses[CallCount]; //no try here, as we want this to throw if out of bounds
        CallCount++;
        return Task.FromResult(response);
    }
}