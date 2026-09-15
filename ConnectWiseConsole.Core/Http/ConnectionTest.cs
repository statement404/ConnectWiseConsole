namespace ConnectWiseConsole.Core.Http;

public class ConnectionTest(CwHttpClient client)
{
    public async Task<bool> CheckConnectionAsync()
    {
        try
        {
            await client.GetAsync("system/info");
            return true;
        }
        catch
        {
            return false;
        }
    }
}