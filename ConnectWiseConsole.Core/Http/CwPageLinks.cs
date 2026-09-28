using System.Text.RegularExpressions;

namespace ConnectWiseConsole.Core.Http;

public record CwPageLinks(string? Next, string? Prev, string? First, string? Last)
{
    public static readonly CwPageLinks Empty = new(null, null, null, null);
    public static CwPageLinks Parse(string? linkHeader)
    {
        if (string.IsNullOrWhiteSpace(linkHeader)) { return Empty; }

        var data = new Dictionary<string, string>();

        foreach (Match match in Regex.Matches(linkHeader, """<([^>]*)>\s*;\s*rel="([^"]*)"""))
        {
            data[match.Groups[2].Value] = match.Groups[1].Value;
        }

        return new CwPageLinks(
            data.GetValueOrDefault("next"),
            data.GetValueOrDefault("prev"),
            data.GetValueOrDefault("first"),
            data.GetValueOrDefault("last"));
    }

    public static CwPageLinks FromHeaders(IReadOnlyDictionary<string, string[]> headers)
    {
        if (!headers.TryGetValue("Link", out var values)) { return Empty; }
        return Parse(string.Join(", ", values));
    }
    
}
