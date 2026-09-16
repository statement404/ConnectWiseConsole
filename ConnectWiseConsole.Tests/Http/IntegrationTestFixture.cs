using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Http;

namespace ConnectWiseConsole.Tests.Http;

public class IntegrationTestFixture : IDisposable
{
    public CwHttpClient Client { get; }

    public IntegrationTestFixture()
    {
        var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
        var credentialsPath = Path.Combine(solutionRoot, "credentials.yaml");
        var credProvider = new YamlCredentialProvider(credentialsPath);
        var client = new CwHttpClient(credProvider);

        Client = client;
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}