using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Http;
using Xunit;
using Xunit.Abstractions;

namespace ConnectWiseConsole.Tests;

[Collection("Integration")]
public class DataRetrievalTests(IntegrationTestFixture fixture, ITestOutputHelper output)
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAsync_MyMemberInfo_ReturnsExpectedData()
    {
        // Arrange
        var client = fixture.Client;

        // Act
        var result = await client.GetAsync("system/myMembers/info");
        output.WriteLine($"MyMemberInfo response: {result}");
        

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(result));
    }
}