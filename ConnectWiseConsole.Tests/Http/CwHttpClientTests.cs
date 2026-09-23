using System.Net;
using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Core.Models;
using ConnectWiseConsole.Tests.Fakes;

namespace ConnectWiseConsole.Tests.Http;

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

    [Fact]
    public async Task GetAsync_FailureResponse_ThrowsCwApiExceptionWithBody()
    {
        // Arrange
        var errorBody = "{\"message\":\"Ticket not found\"}";
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new StringContent(errorBody) });
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var client = new CwHttpClient(fakeHandler, credData);

        // Act
        var ex = await Assert.ThrowsAsync<CwApiException>(() => client.GetAsync("service/tickets/999999"));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
        Assert.Equal(errorBody, ex.ResponseBody);
    }

    [Fact]
    public async Task PatchAsync_FailureResponse_ThrowsCwApiExceptionWithBody()
    {
        // Arrange
        var errorBody = "{\"message\":\"Invalid path\"}";
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(errorBody) });
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var client = new CwHttpClient(fakeHandler, credData);
        var operation = new CwPatchOperation { Op = "replace", Path = "summary", Value = "test" };

        // Act
        var ex = await Assert.ThrowsAsync<CwApiException>(() =>
            client.PatchAsync("service/tickets/1", new List<CwPatchOperation> { operation }));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        Assert.Equal(errorBody, ex.ResponseBody);
    }

    [Fact]
    public async Task PostAsync_FailureResponse_ThrowsCwApiExceptionWithBody()
    {
        // Arrange
        var errorBody = "{\"message\":\"Summary cannot be empty\"}";
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(errorBody) });
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var client = new CwHttpClient(fakeHandler, credData);
        var note = new CwTicketNote { Text = "test note" };

        // Act
        var ex = await Assert.ThrowsAsync<CwApiException>(() => client.PostAsync("service/tickets/1/notes", note));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        Assert.Equal(errorBody, ex.ResponseBody);
    }
}