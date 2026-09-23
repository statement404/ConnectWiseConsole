using System.Net;
using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Core.Models;
using ConnectWiseConsole.Tests.Fakes;
using Xunit.Abstractions;

namespace ConnectWiseConsole.Tests.Http;

public class CwHttpClientSerializationTests(ITestOutputHelper output)
{
    [Fact]
    public async Task PatchAsync_SerialisesCorrectly()
    {
        // Arrange
        var operation = new CwPatchOperation
        {       
            Op = "replace",
            Path = "testpath",
            Value = "testvalue"
        };

        var recorder = new RecordingFakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK));
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var client = new CwHttpClient(recorder, credData);

        // Act
        await client.PatchAsync("service/tickets/1", new List<CwPatchOperation> { operation });

        // Assert
        Assert.Equal("[{\"op\":\"replace\",\"path\":\"testpath\",\"value\":\"testvalue\"}]", recorder.LastRequestBody);
        output.WriteLine(recorder.LastRequestBody);
    }

    [Fact]
    public async Task PostAsync_TicketNote_OmitsUnsetFields()
    {
        // Arrange
        var note = new CwTicketNote
        {
            Text = "test note"
            // everything else intentionally left unset
        };

        var recorder = new RecordingFakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK));
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        var client = new CwHttpClient(recorder, credData);

        // Act
        await client.PostAsync("service/tickets/1/notes", note);

        // Assert
        output.WriteLine(recorder.LastRequestBody);
        Assert.Equal("{\"text\":\"test note\"}", recorder.LastRequestBody);
    }
}
