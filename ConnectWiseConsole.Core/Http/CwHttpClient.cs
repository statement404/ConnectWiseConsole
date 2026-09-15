using ConnectWiseConsole.Core.Auth;

namespace ConnectWiseConsole.Core.Http;

public class CwHttpClient : IDisposable
{
    private readonly HttpClient _httpClient;

    public CwHttpClient(ICredentialProvider credProvider)
        : this(new RetryHandler { InnerHandler = new AuthHandler(credProvider) { InnerHandler = new HttpClientHandler() } }, credProvider)
    {
    }

    public CwHttpClient(HttpMessageHandler handler, ICredentialProvider credProvider)
    {
        var creds = credProvider.GetCredentials();
        _httpClient = new HttpClient(handler);
        _httpClient.BaseAddress = new Uri(creds.BaseUrl);
    }

    public async Task<string> GetAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return content;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}