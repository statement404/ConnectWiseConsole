using System.Text.Json;
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

}