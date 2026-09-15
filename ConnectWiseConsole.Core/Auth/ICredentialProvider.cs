using ConnectWiseConsole.Core.Models;
namespace ConnectWiseConsole.Core.Auth;

public interface ICredentialProvider
{
    Credentials GetCredentials();
}
