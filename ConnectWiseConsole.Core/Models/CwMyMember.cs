namespace ConnectWiseConsole.Core.Models;

public class CwMyMember
{
    public required int Id { get; set; }
    public required string Identifier { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string DefaultEmail { get; set; }
    public required CwReference TimeZone { get; set; }
    public required CwReference DefaultLocation { get; set; }
    public required CwReference DefaultDepartment { get; set; }
    public required CwReference WorkRole { get; set; }
    public required CwReference WorkType { get; set; }
    public required decimal DailyCapacity { get; set; }
    public required int[] ExcludedServiceBoardIds { get; set; }
    public required int[] ExcludedProjectBoardIds { get; set; }

    public string? MiddleInitial { get; set; }
    public string? FullName { get; set; }
    public CwReference? Photo { get; set; }
    public string? LicenseClass { get; set; }
    public bool? InactiveFlag { get; set; }
    public bool? UseBrowserLanguageFlag { get; set; }
    public bool? RequireExpenseEntryFlag { get; set; }
    public bool? RequireTimeSheetEntryFlag { get; set; }
    public bool? RequireStartAndEndTimeOnTimeEntryFlag { get; set; }
    public bool? EnterTimeAgainstCompanyFlag { get; set; }
    public bool? AllowExpensesEnteredAgainstCompaniesFlag { get; set; }
    public CwReference? ServiceDefaultBoard { get; set; }
    public CwReference? ServiceDefaultLocation { get; set; }
    public CwReference? ServiceDefaultDepartment { get; set; }
    public bool? RestrictServiceDefaultLocationFlag { get; set; }
    public bool? RestrictServiceDefaultDepartmentFlag { get; set; }
    public CwReference? ProjectDefaultLocation { get; set; }
    public CwReference? ProjectDefaultDepartment { get; set; }
    public CwReference? ProjectDefaultBoard { get; set; }
    public bool? RestrictProjectDefaultLocationFlag { get; set; }
    public bool? RestrictProjectDefaultDepartmentFlag { get; set; }
    public CwReference? ScheduleDefaultLocation { get; set; }
    public CwReference? ScheduleDefaultDepartment { get; set; }
    public decimal? ScheduleCapacity { get; set; }
    public CwReference? ServiceLocation { get; set; }
    public CwReference? SalesDefaultLocation { get; set; }
    public CwReference? Warehouse { get; set; }
    public CwReference? WarehouseBin { get; set; }
    public bool? RestrictDefaultWarehouseFlag { get; set; }
    public bool? RestrictDefaultWarehouseBinFlag { get; set; }
    public bool? SsoSessionFlag { get; set; }
    public string? SsoClientId { get; set; }
}