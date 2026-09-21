using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Tests.Http;
using Xunit;
using Xunit.Abstractions;

namespace ConnectWiseConsole.Tests;

[Collection("Integration")]
public class ConnectionTests(IntegrationTestFixture fixture, ITestOutputHelper output)
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task CheckConnectionAsync_ReturnsTrue_WithValidCredentials()
    {
        var connectionTest = new ConnectionTest(fixture.Client);

        var result = await connectionTest.CheckConnectionAsync();

        Assert.True(result);
        output.WriteLine($"Connection check result: {result}");
    }
}