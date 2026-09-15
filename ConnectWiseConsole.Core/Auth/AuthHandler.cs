using System.Net.Http.Headers;
using System.Text;

namespace ConnectWiseConsole.Core.Auth;

public class AuthHandler(ICredentialProvider credProvider) : DelegatingHandler
{
    private readonly ICredentialProvider _credProvider = credProvider;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //Grab creds and convert to a base64 string
        var creds = _credProvider.GetCredentials();
        var authString = $"{creds.CompanyName}+{creds.PublicKey}:{creds.PrivateKey}";
        var authBytes = Encoding.UTF8.GetBytes(authString);
        var base64Auth = Convert.ToBase64String(authBytes);

        //Configure Header
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64Auth);
        request.Headers.Add("clientId", creds.ClientId);

        return await base.SendAsync(request, cancellationToken);
    }
}
