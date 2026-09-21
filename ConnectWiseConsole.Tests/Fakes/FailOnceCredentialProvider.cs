using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Models;

namespace ConnectWiseConsole.Tests.Fakes;

public class FailOnceCredentialProvider : ICredentialProvider
{
    private bool _hasFailed;

    public Credentials GetCredentials()
    {
        if (!_hasFailed)
        {
            _hasFailed = true;
            throw new InvalidOperationException("First Credential Attempt");
        }

        return new Credentials
        {
            CompanyName = "test-company",
            PublicKey = "test-public",
            PrivateKey = "test-private",
            ClientId = "test-client",
            BaseUrl = "https://test.example.com"
        };
    }
}