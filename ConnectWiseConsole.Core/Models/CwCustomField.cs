namespace ConnectWiseConsole.Core.Models;

public class CwCustomField
{
    public required int Id { get; set; }
    public required string Caption { get; set; }
    public required string Type { get; set; }
    public required string EntryMethod { get; set; }
    public required int NumberOfDecimals { get; set; }
    public string? Value { get; set; }
    public required string ConnectWiseId { get; set; }
    public required int RowNum { get; set; }
    public required int UserDefinedFieldRecId { get; set; }
    public required string PodId { get; set; }
}