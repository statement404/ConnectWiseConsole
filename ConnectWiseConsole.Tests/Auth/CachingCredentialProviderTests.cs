using ConnectWiseConsole.Core.Auth;
using ConnectWiseConsole.Tests.Fakes;

namespace ConnectWiseConsole.Tests.Auth;

public class CachingCredentialProviderTests
{
    [Fact]
    public void GetCredentials_CalledRepeatedly_LoadsFromInnerOnce()
    {
        // Arrange
        var counting = new CountingCredentialProvider(new YamlCredentialProvider("TestData/test-credentials.yaml"));
        var caching = new CachingCredentialProvider(counting);

        // Act
        caching.GetCredentials();
        caching.GetCredentials();
        caching.GetCredentials();

        // Assert
        Assert.Equal(1, counting.CallCount);
    }

    [Fact]
    public void GetCredentials_ReturnsSameObject()
    {
        // Arrange
        var caching = new CachingCredentialProvider(new YamlCredentialProvider("TestData/test-credentials.yaml"));

        //Act
        var cred1 = caching.GetCredentials();
        var cred2 = caching.GetCredentials();
        
        //Assert
        Assert.Same(cred1, cred2);
    }

    [Fact]
    public void GetCredentials_AfterFailedLoad_TriesInnerAgain()
    {
        // Arrange
        var caching = new CachingCredentialProvider(new FailOnceCredentialProvider());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => caching.GetCredentials());
        var creds = caching.GetCredentials();
        Assert.NotNull(creds);
    }

}
