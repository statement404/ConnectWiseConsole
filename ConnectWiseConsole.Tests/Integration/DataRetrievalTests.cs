using System.Text.Json;
using ConnectWiseConsole.Core.Models;
using ConnectWiseConsole.Core.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace ConnectWiseConsole.Tests.Integration;

[Collection("Integration")]
public class DataRetrievalTests(IntegrationTestFixture fixture, ITestOutputHelper output)
{

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TEMP()
    {
        // Arrange
        var client = fixture.Client;

        // Act
        var result = await client.GetAsync("service/tickets", new Dictionary<string, string>
        {
            ["pageSize"] = "10",
            ["orderBy"] = "id desc",
            ["conditions"] = "id = 485116"
        });
    
        output.WriteLine($"{result}");
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TEMP2()
    {
        // Arrange
        var client = fixture.Client;

        // Act
        var result = await client.GetAsync("service/tickets", new Dictionary<string, string>
        {
            ["pageSize"] = "100",
            ["orderBy"] = "id desc",
            //["conditions"] = "status/name like 'Active*' AND identifier like 'Roberts*'"
        });
    
        var rawTickets = JsonSerializer.Deserialize<List<JsonElement>>(result, CwJsonOptions.Default)!;
        var failures = new List<string>();

        foreach (var rawTicket in rawTickets)
        {
            try
            {
                var ticket = JsonSerializer.Deserialize<CwTicket>(rawTicket.GetRawText(), CwJsonOptions.Default);
            }
            catch (JsonException ex)
            {
                failures.Add($"Ticket {rawTicket.GetProperty("id")}: {ex.Message}");
            }
        }

        foreach (var f in failures) output.WriteLine(f);
        Assert.Empty(failures);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAsync_MyMemberInfo_DeserializesCorrectly()
    {
        // Arrange
        var client = fixture.Client;

        // Act
        var result = await client.GetAsync("system/myMembers/info");
        output.WriteLine($"Raw MyMemberInfo response: {result}");
        var deserializedResult = JsonSerializer.Deserialize<CwMyMember>(result, CwJsonOptions.Default);


        // Assert
        Assert.NotNull(deserializedResult);
        output.WriteLine($"MyMemberInfo name: {deserializedResult.FirstName} {deserializedResult.LastName}");
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAsync_BoardInfo_DeserializesCorrectly()
    {
        // Arrange
        var client = fixture.Client;

        // Act
        var result = await client.GetAsync("service/info/boards");
        output.WriteLine($"Raw BoardInfo response: {result}");
        var deserializedResult = JsonSerializer.Deserialize<List<CwBoard>>(result, CwJsonOptions.Default);

        // Assert
        Assert.NotNull(deserializedResult);
        Assert.NotEmpty(deserializedResult);
        output.WriteLine($"First board: {deserializedResult[0].Name}"); 
    }


    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAsync_CompanyInfo_DeserializesCorrectly()
    {
        // Arrange
        var client = fixture.Client;

        // Act
        var result = await client.GetAsync("company/companies", new Dictionary<string, string>
        {
            ["pageSize"] = "1000"
        });

        output.WriteLine($"Raw CompanyInfo response: {result}");
        var deserializedResult = JsonSerializer.Deserialize<List<CwCompany>>(result, CwJsonOptions.Default);

        // Assert
        Assert.NotNull(deserializedResult);
        Assert.NotEmpty(deserializedResult);
        output.WriteLine($"First company: {deserializedResult[0].Name}"); 
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAsync_TicketInfo_DeserializesCorrectly()
    {
        // Arrange
        var client = fixture.Client;

        // Act
        var result = await client.GetAsync("service/tickets", new Dictionary<string, string>
        {
            ["pageSize"] = "100"
        });

        output.WriteLine($"Raw TicketInfo response: {result}");
        var deserializedResult = JsonSerializer.Deserialize<List<CwTicket>>(result, CwJsonOptions.Default);

        // Assert
        Assert.NotNull(deserializedResult);
        Assert.NotEmpty(deserializedResult);
        output.WriteLine($"First ticket: {deserializedResult[0].Id}"); 
    }
}