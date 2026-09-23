using System.Net;
using System.Text.Json;
using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Models;
using ConnectWiseConsole.Core.Serialization;

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

    public async Task<string> GetAsync(string endpoint, Dictionary<string, string>? queryParams = null)
    {

        if (queryParams is { Count: > 0 })
        {
            var query = string.Join("&", queryParams.Select(kv =>
                $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
            endpoint = $"{endpoint}?{query}";
        }
        
        var response = await _httpClient.GetAsync(endpoint);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new CwApiException(response.StatusCode, responseContent);
        }
        
        return responseContent;
    }

    public async Task<string> PatchAsync(string endpoint, List<CwPatchOperation> operations)
    {
        var json = JsonSerializer.Serialize(operations, CwJsonOptions.Default);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PatchAsync(endpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new CwApiException(response.StatusCode, responseContent);
        }
        
        return responseContent;
    }

    public async Task<string> PostAsync<T>(string endpoint, T body)
    {
        var json = JsonSerializer.Serialize(body, CwJsonOptions.Default);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(endpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new CwApiException(response.StatusCode, responseContent);
        }
        
        return responseContent;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}