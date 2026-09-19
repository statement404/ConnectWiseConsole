namespace ConnectWiseConsole.Core.Models;

public class CwTicketNote
{
    public int? Id { get; set; }
    public int? TicketId { get; set; }
    public required string Text { get; set; }
    public string? HtmlContent { get; set; }
    public bool? DetailDescriptionFlag { get; set; }
    public bool? InternalAnalysisFlag { get; set; }
    public bool? ResolutionFlag { get; set; }
    public bool? IssueFlag { get; set; }
    public CwReference? Member { get; set; }
    public CwReference? Contact { get; set; }
    public bool? CustomerUpdatedFlag { get; set; }
    public bool? ProcessNotifications { get; set; }
    public DateTime? DateCreated { get; set; }
    public string? CreatedBy { get; set; }
    public bool? InternalFlag { get; set; }
    public bool? ExternalFlag { get; set; }
    public double? SentimentScore { get; set; }
}