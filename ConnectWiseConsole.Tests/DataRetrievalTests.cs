using System.Text.Json;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Core.Models;
using ConnectWiseConsole.Core.Serialization;
using ConnectWiseConsole.Tests.Http;
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

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAsync_MyMemberInfo_DeserialzesCorrectly()
    {
        // Arrange
        var client = fixture.Client;

        // Act
        var result = await client.GetAsync("system/myMembers/info");
        var deserializedResult = JsonSerializer.Deserialize<CwMyMember>(result, CwJsonOptions.Default);
        

        // Assert
        Assert.NotNull(deserializedResult);
        output.WriteLine($"MyMemberInfo name: {deserializedResult.FirstName} {deserializedResult.LastName}");
        Assert.False(string.IsNullOrWhiteSpace(deserializedResult.FirstName));
        
    }
}