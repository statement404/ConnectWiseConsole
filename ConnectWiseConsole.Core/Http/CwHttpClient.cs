using System.Net;
using System.Text.Json;
using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Models;
using ConnectWiseConsole.Core.Serialization;

namespace ConnectWiseConsole.Core.Http;

public class CwHttpClient : IDisposable
{
    private readonly HttpClient _httpClient;
    public const int MaxPages = 10;

    public CwHttpClient(ICredentialProvider credProvider)
    : this(new RetryHandler
    {
        InnerHandler = new AuthHandler(credProvider)
        {
            // Recycle pooled connections periodically so a long-running app
            // eventually notices if ConnectWise's DNS record changes.
            InnerHandler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(15) }
        }
    }, credProvider)
    {
    }

    private static string NormalizeEndpoint(string endpoint) => endpoint.TrimStart('/');

    public CwHttpClient(HttpMessageHandler handler, ICredentialProvider credProvider)
    {
        var creds = credProvider.GetCredentials();
        _httpClient = new HttpClient(handler);
        var baseUrl = creds.BaseUrl.EndsWith('/') ? creds.BaseUrl : creds.BaseUrl + "/";
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<List<string>> GetAllPagesAsync(string endpoint, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
    {
        List<string> result = [];
        Uri? nextUri = null;
        var pageCount = 0;

        do
        {
            pageCount++;
            if (pageCount > MaxPages) { throw new InvalidOperationException($"Number of pages exceeded set limit ({MaxPages})"); }

            var response = nextUri is null
                ? await GetResponseAsync(endpoint, queryParams, cancellationToken)
                : await GetResponseAsync(nextUri, cancellationToken);

            if (!response.IsSuccess) { throw new CwApiException(response.StatusCode, response.Body); }
            result.Add(response.Body);
            var links = CwPageLinks.FromHeaders(response.Headers);

            nextUri = links.Next is not null ? new Uri(links.Next) : null;
        } while (nextUri is not null);

        return result;
    }

    private static async Task<CwResponse> SendAndBuildResponseAsync(
    Task<HttpResponseMessage> sendTask,
    CancellationToken cancellationToken)
    {
        using var response = await sendTask;
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseHeaders = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        foreach (var header in response.Headers)
        {
            responseHeaders[header.Key] = [.. header.Value];
        }

        return new CwResponse(response.StatusCode, responseHeaders, responseContent);
    }

    public async Task<CwResponse> GetResponseAsync(string endpoint, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
    {
        endpoint = NormalizeEndpoint(endpoint);

        if (queryParams is { Count: > 0 })
        {
            var query = string.Join("&", queryParams.Select(kv =>
                $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
            endpoint = $"{endpoint}?{query}";
        }

        return await SendAndBuildResponseAsync(_httpClient.GetAsync(endpoint, cancellationToken), cancellationToken);
    }

    public async Task<CwResponse> GetResponseAsync(Uri absoluteUri, CancellationToken cancellationToken = default)
    {
        return await SendAndBuildResponseAsync(_httpClient.GetAsync(absoluteUri, cancellationToken), cancellationToken);
    }

    public async Task<string> GetAsync(string endpoint, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
    {
        var response = await GetResponseAsync(endpoint, queryParams, cancellationToken);

        if (!response.IsSuccess)
        {
            throw new CwApiException(response.StatusCode, response.Body);
        }

        return response.Body;
    }

    public async Task<string> PatchAsync(string endpoint, List<CwPatchOperation> operations, CancellationToken cancellationToken = default)
    {
        endpoint = NormalizeEndpoint(endpoint);

        var json = JsonSerializer.Serialize(operations, CwJsonOptions.Default);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PatchAsync(endpoint, content, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new CwApiException(response.StatusCode, responseContent);
        }

        return responseContent;
    }

    public async Task<string> PostAsync<T>(string endpoint, T body, CancellationToken cancellationToken = default)
    {
        endpoint = NormalizeEndpoint(endpoint);

        var json = JsonSerializer.Serialize(body, CwJsonOptions.Default);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

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