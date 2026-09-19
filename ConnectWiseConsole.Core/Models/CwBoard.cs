namespace ConnectWiseConsole.Core.Models;

public class CwBoard
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required CwReference Location { get; set; }
    public required CwReference Department { get; set; }
    public required bool InactiveFlag { get; set; }
    public bool? ProjectFlag { get; set; }
    public bool? ClosedLoopDiscussionsFlag { get; set; }
    public bool? ClosedLoopInternalAnalysisFlag { get; set; }
    public bool? ClosedLoopResolutionFlag { get; set; }
    public bool? ClosedLoopAllFlag { get; set; }
    public bool? OverrideBillingSetupFlag { get; set; }
    public bool? BillTicketsAfterClosedFlag { get; set; }
    public bool? BillUnapprovedTimeExpenseFlag { get; set; }
    public string? BillTime { get; set; }
    public string? BillExpense { get; set; }
    public string? BillProduct { get; set; }
    public string? ProblemSort { get; set; }
    public string? InternalAnalysisSort { get; set; }
    public string? ResolutionSort { get; set; }
    public string? AllSort { get; set; }
}