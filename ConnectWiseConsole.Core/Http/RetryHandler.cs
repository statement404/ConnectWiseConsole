using System.Net;

namespace ConnectWiseConsole.Core.Http;

public class RetryHandler : DelegatingHandler
{
    private const int MaxRetries = 3;
    private static readonly List<HttpStatusCode> safeRetryCodes =
    [
        HttpStatusCode.TooManyRequests,      // 429
        HttpStatusCode.ServiceUnavailable    // 503
    ];

    private static readonly List<HttpStatusCode> riskyRetryCodes =
    [
        HttpStatusCode.InternalServerError,  // 500
        HttpStatusCode.BadGateway,           // 502
        HttpStatusCode.GatewayTimeout        // 504
    ];

    private static readonly List<HttpStatusCode> allRetryCodes = [.. safeRetryCodes, .. riskyRetryCodes];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = null!;

        var retryCodes = request.Method == HttpMethod.Get ? allRetryCodes : safeRetryCodes;

        for (int attempt = 0; attempt <= MaxRetries; attempt++)
        {
            response = await base.SendAsync(request, cancellationToken);

            
            if (response.IsSuccessStatusCode || !retryCodes.Contains(response.StatusCode))
                return response;

            //wait on retryable status codes
            if (attempt < MaxRetries)
            {
                var delayMs = 1000 * Math.Pow(2, attempt);
                await Task.Delay((int)delayMs, cancellationToken);
            }
        }

        response.EnsureSuccessStatusCode();
        return response;
    }
}