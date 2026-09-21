using ConnectWiseConsole.Core.Models;
namespace ConnectWiseConsole.Core.Auth;

public class CachingCredentialProvider(ICredentialProvider inner) : ICredentialProvider
{
    private readonly ICredentialProvider _inner = inner;
    private Credentials? _credentials;
    
    public Credentials GetCredentials()
    {
        return _credentials ??= _inner.GetCredentials();
    }
}
