using System.Net;
using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Tests.Fakes;
using Xunit;

namespace ConnectWiseConsole.Tests;

public class CwHttpClientTests
{
    [Fact]
    public async Task GetAsync_ReturnsExpectedContent_OnSuccess()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "hello from fake api");
        var client = new CwHttpClient(fakeHandler, credData);

        // Act
        var result = await client.GetAsync("some/endpoint");

        // Assert
        Assert.Equal("hello from fake api", result);
    }

    [Fact]
    public async Task GetAsync_ThrowsOnFailureStatusCode()
    {
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.NotFound, "not found");
        var client = new CwHttpClient(fakeHandler, credData);

        await Assert.ThrowsAsync<HttpRequestException>(() => client.GetAsync("some/endpoint"));
    }
}