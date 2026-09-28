using System.Net;
using ConnectWiseConsole.Core.Http;

namespace ConnectWiseConsole.Tests.Http;

public class CwResponseTests
{
    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.NoContent, true)]
    [InlineData(HttpStatusCode.MultipleChoices, false)]   // 300
    [InlineData(HttpStatusCode.BadRequest, false)]
    public void IsSuccess_MatchesStatusRange(HttpStatusCode code, bool expected)
    {
        var response = new CwResponse(code, new Dictionary<string, string[]>(), "");
        Assert.Equal(expected, response.IsSuccess);
    }
}