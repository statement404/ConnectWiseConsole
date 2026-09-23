using System.Net;

namespace ConnectWiseConsole.Core.Http;

public class CwApiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string ResponseBody { get; }

    public CwApiException(HttpStatusCode statusCode, string responseBody)
        : base($"ConnectWise API returned {(int)statusCode} {statusCode}: {responseBody}")
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }
}
