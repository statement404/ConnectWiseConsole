namespace ConnectWiseConsole.Core.Models;

public class CWMyMember
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
}