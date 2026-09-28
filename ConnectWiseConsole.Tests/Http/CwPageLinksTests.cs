using ConnectWiseConsole.Core.Http;

namespace ConnectWiseConsole.Tests.Http;

public class CwPageLinksTests
{
    private const string NextUrl = "https://test-next-url/v4_6_release/apis/3.0/service/tickets/?orderBy=id+desc&pageSize=1&page=7357";
    private const string LastUrl = "https://test-last-url/v4_6_release/apis/3.0/service/tickets/?orderBy=id+desc&pageSize=1&page=7357";
    private const string PrevUrl = "https://test-prev-url/v4_6_release/apis/3.0/service/tickets/?orderBy=id+desc&pageSize=1&page=7357";
    private const string FirstUrl = "https://test-first-url/v4_6_release/apis/3.0/service/tickets/?orderBy=id+desc&pageSize=1&page=7357";
    private const string CommaUrl = "https://test-comma-url/v4_6_release/apis/3.0/service/tickets/?orderBy=id+desc&pageSize=1,100&page=73,57";
    
    

    [Fact]
    public void PageLinks_NextLastOnly_PrevFirstNull()
    {
        // Arrange
        var linkString = $"""<{NextUrl}>; rel="next", <{LastUrl}>; rel="last" """;

        // Act
        var links = CwPageLinks.Parse(linkString);

        // Assert
        Assert.Equal(NextUrl, links.Next);
        Assert.Equal(LastUrl, links.Last);
        Assert.Null(links.Prev);
        Assert.Null(links.First);
    }

    [Fact]
    public void PageLinks_AllFour_AllPopulated()
    {
        // Arrange
        var linkString = $"""<{NextUrl}>; rel="next", <{PrevUrl}>; rel="prev", <{FirstUrl}>; rel="first", <{LastUrl}>; rel="last" """;

        // Act
        var links = CwPageLinks.Parse(linkString);

        // Assert
        Assert.Equal(NextUrl, links.Next);
        Assert.Equal(PrevUrl, links.Prev);
        Assert.Equal(FirstUrl, links.First);
        Assert.Equal(LastUrl, links.Last);
    }

    [Fact]
    public void PageLinks_PrevFirstOnly_NextLastNull()
    {
        // Arrange
        var linkString = $"""<{PrevUrl}>; rel="prev", <{FirstUrl}>; rel="first" """;

        // Act
        var links = CwPageLinks.Parse(linkString);

        // Assert
        Assert.Equal(PrevUrl, links.Prev);
        Assert.Equal(FirstUrl, links.First);
        Assert.Null(links.Next);
        Assert.Null(links.Last);
    }

    [Fact]
    public void PageLinks_Handles_UnknownRel()
    {
        // Arrange
        var linkString = $"""<{NextUrl}>; rel="next", <{LastUrl}>; rel="unknown" """;

        // Act
        var links = CwPageLinks.Parse(linkString);

        // Assert
        Assert.NotNull(links);
        Assert.Null(links.Prev);
        Assert.Null(links.First);
        Assert.Equal(NextUrl, links.Next);
        Assert.Null(links.Last);
    }

    [Fact]
    public void PageLinks_HandlesCommas_InURL()
    {
        // Arrange
        var linkString = $"""<{CommaUrl}>; rel="next" """;

        // Act
        var links = CwPageLinks.Parse(linkString);

        // Assert
        Assert.Equal(CommaUrl, links.Next);
    }

    [Fact]
    public void PageLinks_FromHeaders_NoLink_ReturnsEmpty()
    {
        // Arrange
        var headers = new Dictionary<string, string[]>();

        // Act
        var links = CwPageLinks.FromHeaders(headers);

        // Assert
        Assert.NotNull(links);
        Assert.Null(links.Prev);
        Assert.Null(links.First);
        Assert.Null(links.Next);
        Assert.Null(links.Last);
    }
}
