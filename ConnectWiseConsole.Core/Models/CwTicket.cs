namespace ConnectWiseConsole.Core.Models;

public class CwTicket
{
    public required int Id { get; set; }
    public required string Summary { get; set; }
    public required CwReference Board { get; set; }
    public required CwReference WorkType { get; set; }
    public required CwReference Company { get; set; }
    public required CwReference Site { get; set; }
    public required string SiteName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? StateIdentifier { get; set; }
    public required CwReference Country { get; set; }
    public string? ContactName { get; set; }
    public string? ContactEmailAddress { get; set; }
    public required CwReference Type { get; set; }
    public required CwReference Team { get; set; }
    public required CwReference Priority { get; set; }
    public required CwReference ServiceLocation { get; set; }
    public required CwReference Source { get; set; }
    public string? AgreementType { get; set; }
    public required string Severity { get; set; }
    public required string Impact { get; set; }
    public required bool AutomaticEmailContactFlag { get; set; }
    public required bool AutomaticEmailResourceFlag { get; set; }
    public required bool AutomaticEmailCcFlag { get; set; }
    public string? AutomaticEmailCc { get; set; }
    public required bool ClosedFlag { get; set; }
    public required bool Approved { get; set; }
    public required int ResolveMinutes { get; set; }
    public required int ResPlanMinutes { get; set; }
    public required int RespondMinutes { get; set; }
    public DateTime? RespondByGoalUTC { get; set; }
    public DateTime? ResplanGoalUTC { get; set; }
    public DateTime? ResolutionGoalUTC { get; set; }
    public required bool IsInSla { get; set; }
    public required bool HasChildTicket { get; set; }
    public bool? HasMergedChildTicketFlag { get; set; }
    public required CwReference Location { get; set; }
    public required CwReference Department { get; set; }
    public required CwReference Sla { get; set; }
    public required string SlaStatus { get; set; }
    public required bool RequestForChangeFlag { get; set; }
    public required DateTime EscalationStartDateUTC { get; set; }
    public required int EscalationLevel { get; set; }
    public required int MinutesBeforeWaiting { get; set; }
    public required int RespondedSkippedMinutes { get; set; }
    public required int ResplanSkippedMinutes { get; set; }
    public required double RespondedHours { get; set; }
    public required double ResplanHours { get; set; }
    public required double ResolutionHours { get; set; }
    public required int MinutesWaiting { get; set; }
    public required CwCustomField[] CustomFields { get; set; }

    public string? RecordType { get; set; }
    public CwReference? Status { get; set; }
    public CwReference? WorkRole { get; set; }
    public CwReference? Contact { get; set; }
    public string? ContactPhoneNumber { get; set; }
    public string? ContactPhoneExtension { get; set; }
    public CwReference? SubType { get; set; }
    public CwReference? Item { get; set; }
    public CwReference? Owner { get; set; }
    public DateTime? RequiredDate { get; set; }
    public decimal? BudgetHours { get; set; }
    public CwReference? Opportunity { get; set; }
    public CwReference? Agreement { get; set; }
    public string? ExternalXRef { get; set; }
    public string? PoNumber { get; set; }
    public int? KnowledgeBaseCategoryId { get; set; }
    public int? KnowledgeBaseSubCategoryId { get; set; }
    public bool? AllowAllClientsPortalView { get; set; }
    public bool? CustomerUpdatedFlag { get; set; }
    public string? InitialDescription { get; set; }
    public string? InitialInternalAnalysis { get; set; }
    public string? InitialResolution { get; set; }
    public string? InitialDescriptionFrom { get; set; }
    public string? ContactEmailLookup { get; set; }
    public bool? ProcessNotifications { get; set; }
    public bool? SkipCallback { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string? ClosedBy { get; set; }
    public decimal? ActualHours { get; set; }
    public decimal? EstimatedExpenseCost { get; set; }
    public decimal? EstimatedExpenseRevenue { get; set; }
    public decimal? EstimatedProductCost { get; set; }
    public decimal? EstimatedProductRevenue { get; set; }
    public decimal? EstimatedTimeCost { get; set; }
    public decimal? EstimatedTimeRevenue { get; set; }
    public string? BillingMethod { get; set; }
    public decimal? BillingAmount { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? SubBillingMethod { get; set; }
    public decimal? SubBillingAmount { get; set; }
    public DateTime? SubDateAccepted { get; set; }
    public DateTime? DateResolved { get; set; }
    public DateTime? DateResplan { get; set; }
    public DateTime? DateResponded { get; set; }
    public int? EscalationLastUpdateMinutes { get; set; }
    public int? KnowledgeBaseLinkId { get; set; }
    public string? Resources { get; set; }
    public int? ParentTicketId { get; set; }
    public string? KnowledgeBaseLinkType { get; set; }
    public string? BillTime { get; set; }
    public string? BillExpenses { get; set; }
    public string? BillProducts { get; set; }
    public bool? BillTicketSeparately { get; set; }
    public bool? BillTicketOnlyAfterClosed { get; set; }
    public bool? BillUnapprovedTimeAndExpenses { get; set; }
    public bool? RestrictDownPayment { get; set; }
    public string? PredecessorType { get; set; }
    public int? PredecessorId { get; set; }
    public bool? PredecessorClosedFlag { get; set; }
    public int? LagDays { get; set; }
    public bool? LagNonworkingDaysFlag { get; set; }
    public DateTime? EstimatedStartDate { get; set; }
    public int? Duration { get; set; }
    public string? MobileGuid { get; set; }
    public CwReference? Currency { get; set; }
    public CwReference? MergedParentTicket { get; set; }
    public List<string>? IntegratorTags { get; set; }
    public string? RespondedBy { get; set; }
    public string? ResplanBy { get; set; }
    public string? ResolvedBy { get; set; }

    public List<string> ResourceList =>
        string.IsNullOrWhiteSpace(Resources)
            ? new List<string>()
            : Resources.Split(',').Select(r => r.Trim()).ToList();
}