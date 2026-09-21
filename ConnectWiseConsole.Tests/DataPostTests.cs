using System.Net;
using System.Text.Json;
using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Core.Models;
using ConnectWiseConsole.Core.Serialization;
using ConnectWiseConsole.Tests.Fakes;
using ConnectWiseConsole.Tests.Integration;
using Xunit;
using Xunit.Abstractions;

namespace ConnectWiseConsole.Tests;

[Collection("Integration")]
public class DataPostTests(IntegrationTestFixture fixture, ITestOutputHelper output)
{

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TEMP()
    {
        // Arrange
        var client = fixture.Client;

        // Act
        var result = await client.PostAsync("service/tickets/485116/notes", new CwTicketNote
        {      
            Text = "Test note from API",
            ProcessNotifications = true,
            DetailDescriptionFlag = true
        });
        

        output.WriteLine($"{result}");
    }

    [Fact]
    public async Task PostAsync_500_TriesOnce()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error")
            },
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Fake api OK response")
            });
        var retryHandler = new RetryHandler { InnerHandler = fakeHandler };
        var client = new CwHttpClient(retryHandler, credData);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            client.PostAsync("fakepostendpoint", new string[] { "test" }));

        Assert.Equal(1, fakeHandler.CallCount);
    }

    [Fact]
    public async Task PostAsync_503_TriesMultipleTimes()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.ServiceUnavailable,
                Content = new StringContent("Service Unavailable")
            },
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Fake api OK response")
            });
        var retryHandler = new RetryHandler { InnerHandler = fakeHandler };
        var client = new CwHttpClient(retryHandler, credData);

        // Act & Assert
        var result = await client.PostAsync("fakepostendpoint", new string[] { "test" });

        Assert.Equal(2, fakeHandler.CallCount);
    }

}