using System.Net;
using System.Text.Json;
using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Core.Models;
using ConnectWiseConsole.Core.Serialization;
using ConnectWiseConsole.Tests.Http;
using Xunit;
using Xunit.Abstractions;

namespace ConnectWiseConsole.Tests;

[Collection("Integration")]
public class DataPatchTests(IntegrationTestFixture fixture, ITestOutputHelper output)
{

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TEMP()
    {
        // Arrange
        var client = fixture.Client;

        // Act
        var result = await client.PatchAsync("service/tickets/485116", new List<CwPatchOperation>
        {
            new CwPatchOperation
            {
                Op = "replace",
                Path = "automaticEmailCc",
                Value = "jonathonargent@outlook.com"
            }
        });
        

        output.WriteLine($"{result}");
    }

    [Fact]
    public async Task PatchAsync_500_TriesOnce()
    {
        //Arrange
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

        //Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            client.PatchAsync("fakepatchendpoint", new List<CwPatchOperation>{
                new CwPatchOperation
                {
                    Op = "replace",
                    Path = "testpath",
                    Value = "testvalue"
                }
            }));
        
        Assert.Equal(1, fakeHandler.CallCount);
    }
}