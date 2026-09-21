namespace ConnectWiseConsole.Core.Models;
public class CwPatchOperation
{
    public required string Op { get; set; }
    public required string Path { get; set; }
    public required object Value { get; set; }
}