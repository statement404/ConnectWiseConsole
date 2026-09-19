using System.Text.Json;
using ConnectWiseConsole.Core.Http;
using ConnectWiseConsole.Core.Models;
using ConnectWiseConsole.Core.Serialization;
using ConnectWiseConsole.Tests.Http;
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

}