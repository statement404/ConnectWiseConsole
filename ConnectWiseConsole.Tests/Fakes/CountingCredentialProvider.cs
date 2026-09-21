using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Models;

namespace ConnectWiseConsole.Tests.Fakes;

public class CountingCredentialProvider(ICredentialProvider inner) : ICredentialProvider
{
    private readonly ICredentialProvider _inner = inner;

    public int CallCount { get; private set; }

    public Credentials GetCredentials()
    {
        CallCount++;
        return _inner.GetCredentials();
    }
}