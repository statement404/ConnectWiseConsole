using System.Net;

namespace ConnectWiseConsole.Core.Http;

public class RetryHandler : DelegatingHandler
{
    private const int MaxRetries = 3;
    private static readonly List<HttpStatusCode> retryCodes = [HttpStatusCode.TooManyRequests, HttpStatusCode.InternalServerError, HttpStatusCode.BadGateway, HttpStatusCode.ServiceUnavailable, HttpStatusCode.GatewayTimeout];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = null!;

        for (int attempt = 0; attempt <= MaxRetries; attempt++)
        {
            response = await base.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode || !retryCodes.Contains(response.StatusCode))
                return response;

            //wait on retryable status codes
            var delayMs = 1000 * Math.Pow(2, attempt);
            await Task.Delay((int)delayMs, cancellationToken);
        }

        response.EnsureSuccessStatusCode();
        return response;
    }
}
