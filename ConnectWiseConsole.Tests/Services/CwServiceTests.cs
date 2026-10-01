using System.Net;
using System.Text.Json;
using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Core.Services;
using ConnectWiseConsole.Tests.Fakes;
using Xunit.Sdk;

namespace ConnectWiseConsole.Tests.Services;

public class CwServiceTests
{
    private record TestItem(int Id, string Name);

    [Fact]
    public async Task GetAsync_Deserializes_ExpectedValues()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"id":1,"name":"example"}""");
        var client = new CwHttpClient(fakeHandler, credData);
        var service = new CwService(client);

        // Act
        var result = await service.GetAsync<TestItem>("https://example.com/testitem");

        // Assert
        Assert.Equal(new TestItem(1, "example"), result);
    }

    [Fact]
    public async Task GetAllAsync_Deserializes_ExpectedValues()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new SequencedFakeHttpMessageHandler(
          [
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""[{"id":1,"name":"example"}]"""),
                Headers = { { "Link", "<https://linkto2>; rel=\"next\"" } }
            },
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""[{"id":2,"name":"example"}]"""),
                Headers = { { "Link", "<https://linkto3>; rel=\"next\"" } }
            },
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""[{"id":3,"name":"example"}]""")
            },

          ]
        );
        var client = new CwHttpClient(fakeHandler, credData);
        var service = new CwService(client);

        var comparisonResult = new List<TestItem>(
            [
                new TestItem(1,"example"),
                new TestItem(2,"example"),
                new TestItem(3,"example")
            ]
        );

        // Act
        var result = await service.GetAllAsync<TestItem>("https://example.com/testitem");

        // Assert
        Assert.Equal(comparisonResult, result);
    }

    [Fact]
    public async Task GetAsync_ParseFailure_ThrowsCorrectException()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"id":1,"name":"example""");
        var client = new CwHttpClient(fakeHandler, credData);
        var service = new CwService(client);

        // Act
        var ex = await Assert.ThrowsAsync<CwDeserializationException>(() => service.GetAsync<TestItem>("https://example.com/testitem"));

        // Assert
        Assert.IsType<JsonException>(ex.InnerException);
        Assert.Equal("""{"id":1,"name":"example""", ex.RawJson);
        Assert.Equal(typeof(TestItem), ex.TargetType);

    }

    [Fact]
    public async Task GetAsync_NullResult_ThrowsCorrectException()
    {
        // Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "null");
        var client = new CwHttpClient(fakeHandler, credData);
        var service = new CwService(client);

        // Act
        var ex = await Assert.ThrowsAsync<CwDeserializationException>(() => service.GetAsync<TestItem>("https://example.com/testitem"));

        // Assert
        Assert.Null(ex.InnerException);
        Assert.Equal("null", ex.RawJson);
        Assert.Equal(typeof(TestItem), ex.TargetType);
    }
}
