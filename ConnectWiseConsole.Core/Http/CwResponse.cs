using System.Net;

namespace ConnectWiseConsole.Core.Http;

public record CwResponse(HttpStatusCode StatusCode, IReadOnlyDictionary<string, string[]> Headers, string Body)
{
    public bool IsSuccess => (int)StatusCode is >= 200 and <= 299;
}