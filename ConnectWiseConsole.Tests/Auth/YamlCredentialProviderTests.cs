using ConnectWiseConsole.Core.Auth;

namespace ConnectWiseConsole.Tests.Auth;

public class YamlCredentialProviderTests
{
    [Fact]
    public void GetCredentials_ReturnsExpectedValues()
    {
        //Arrange
        var credData = new YamlCredentialProvider("TestData/test-credentials.yaml");
        //Act
        var creds = credData.GetCredentials();
        //Assert
        Assert.Equal("test-companyname", creds.CompanyName);
        Assert.Equal("pubkey-1234", creds.PublicKey);
        Assert.Equal("privkey-1234", creds.PrivateKey);
        Assert.Equal("clientid-1234", creds.ClientId);
        Assert.Equal("https://test.example.com", creds.BaseUrl);
    }

}
