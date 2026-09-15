using ConnectWiseConsole.Core.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ConnectWiseConsole.Core.Auth;

public class YamlCredentialProvider(string filePath) : ICredentialProvider
{
    private readonly string _filePath = filePath;

    public Credentials GetCredentials()
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(NullNamingConvention.Instance)
            .Build();
        var rawCreds = File.ReadAllText(_filePath);
        var creds = deserializer.Deserialize<Credentials>(rawCreds);

        if (creds is null)
        {
            throw new InvalidOperationException("Failed to deserialize credentials from file.");
        }

        return creds;
    }
}