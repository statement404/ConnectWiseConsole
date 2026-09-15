namespace ConnectWiseConsole.Core.Models;

public class Credentials
{
    public required string CompanyName { get; set; }
    public required string PublicKey { get; set; }
    public required string PrivateKey { get; set; }
    public required string ClientId { get; set; }
    public required string BaseUrl { get; set; }
}
