using System.Text.Json;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Core.Serialization;

namespace ConnectWiseConsole.Core.Services;

public class CwService(CwHttpClient client)
{
    public async Task<T> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
    {
        var json = await client.GetAsync(endpoint, queryParams, cancellationToken);

        T? result;
        try
        {
            result = JsonSerializer.Deserialize<T>(json, CwJsonOptions.Default);
        }
        catch (JsonException ex)
        {
            throw CwDeserializationException.ParseFailure(typeof(T), json, ex);
        }

        return result ?? throw CwDeserializationException.NullResult(typeof(T), json);
    }

    public async Task<List<T>> GetAllAsync<T>(string endpoint, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
    {
        var jsonArray = await client.GetAllPagesAsync(endpoint, queryParams, cancellationToken);

        List<T> result = [];

        foreach (var json in jsonArray)
        {
            List<T>? page;
            try
            {
                page = JsonSerializer.Deserialize<List<T>>(json, CwJsonOptions.Default);
            }
            catch (JsonException ex)
            {
                throw CwDeserializationException.ParseFailure(typeof(T), json, ex);
            }

            result.AddRange(page ?? throw CwDeserializationException.NullResult(typeof(T), json));
        }

        return result;
    }
}