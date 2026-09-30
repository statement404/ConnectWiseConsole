using System.Net;
using System.Net.Http.Headers;
using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Core.Models;
using ConnectWiseConsole.Tests.Fakes;
using Xunit.Abstractions;

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

        await Assert.ThrowsAsync<CwApiException>(() => client.GetAsync("some/endpoint"));
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

    [Fact]
    public async Task GetResponseAsync_ReturnsStatusBodyAndHeaders()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("[]")
        };
        response.Headers.Add("Link", "<https://example.com/next>; rel=\"next\"");

        var fakeHandler = new SequencedFakeHttpMessageHandler(response);
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var client = new CwHttpClient(fakeHandler, credData);

        // Act
        var result = await client.GetResponseAsync("service/tickets");

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal("[]", result.Body);
        Assert.Equal("<https://example.com/next>; rel=\"next\"", result.Headers["link"].Single());
    }

    [Fact]
    public async Task GetAllPages_StopsWhenNextIsNull()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "fake api response ok");
        var client = new CwHttpClient(fakeHandler, credData);

        // Act
        var response = await client.GetAllPagesAsync("test/endpoint");

        // Assert
        Assert.Single(response);
        Assert.Equal("fake api response ok", response[0]);
    }

    [Fact]
    public async Task GetAllPages_ReturnsCorrectNumberInOrder()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            [
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("1"),
                    Headers = { { "Link", "<https://linkto2>; rel=\"next\"" } }
                },
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("2"),
                    Headers = { { "Link", "<https://linkto3>; rel=\"next\"" } }
                },
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("3"),
                    Headers = { { "Link", "<https://linkto4>; rel=\"next\"" } }
                },
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("4")
                },

            ]
        );
        var client = new CwHttpClient(fakeHandler, credData);

        // Act
        var response = await client.GetAllPagesAsync("test/endpoint");

        // Assert
        Assert.Equal(4, response.Count);
        Assert.Equal(["1", "2", "3", "4"], response);
    }

    [Fact]
    public async Task GetAllPages_StopsAtMaxPages()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var responses = new HttpResponseMessage[CwHttpClient.MaxPages + 5];
        for (int i = 0; i < responses.Length; i++)
        {
            responses[i] = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent($"{i + 1}"),
                Headers = { { "Link", $"<https://linkto{i + 1}>; rel=\"next\"" } }
            };
        }
        var fakeHandler = new SequencedFakeHttpMessageHandler(responses);
        var client = new CwHttpClient(fakeHandler, credData);

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => client.GetAllPagesAsync("test/endpoint"));

        // Assert
        Assert.Equal(CwHttpClient.MaxPages, fakeHandler.CallCount);
    }

    [Fact]
    public async Task GetAllPages_ReturnsCWApiException()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new SequencedFakeHttpMessageHandler(
            [
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("1"),
                    Headers = { { "Link", "<https://linkto2>; rel=\"next\"" } }
                },
                new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("2")
                },
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("3")
                }

            ]
        );
        var client = new CwHttpClient(fakeHandler, credData);

        // Act
        var ex = await Assert.ThrowsAsync<CwApiException>(() => client.GetAllPagesAsync("test/endpoint"));

        // Assert
        Assert.Equal("2", ex.ResponseBody);
        Assert.Equal(2, fakeHandler.CallCount);
    }
}
