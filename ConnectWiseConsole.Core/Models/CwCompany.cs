namespace ConnectWiseConsole.Core.Models;

public class CwCompany
{
    public required int Id { get; set; }
    public required string Identifier { get; set; }
    public required string Name { get; set; }
    public required CwReference Status { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public CwReference? Country { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FaxNumber { get; set; }
    public string? Website { get; set; }
    public CwReference? Territory { get; set; }
    public CwReference? Market { get; set; }
    public string? AccountNumber { get; set; }
    public CwReference? DefaultContact { get; set; }
    public required List<CwReference> Types { get; set; }
    public required CwReference Site { get; set; }
    public DateTime? DateAcquired { get; set; }
    public CwReference? SicCode { get; set; }
    public CwReference? ParentCompany { get; set; }
    public decimal? AnnualRevenue { get; set; }
    public decimal? CreditLimit { get; set; }
    public decimal? AdditionalDebt { get; set; }
    public int? NumberOfEmployees { get; set; }
    public int? YearEstablished { get; set; }
    public int? RevenueYear { get; set; }
    public CwReference? OwnershipType { get; set; }
    public CwReference? TimeZoneSetup { get; set; }
    public string? LeadSource { get; set; }
    public bool? LeadFlag { get; set; }
    public bool? UnsubscribeFlag { get; set; }
    public CwReference? Calendar { get; set; }
    public string? UserDefinedField1 { get; set; }
    public string? UserDefinedField2 { get; set; }
    public string? UserDefinedField3 { get; set; }
    public string? UserDefinedField4 { get; set; }
    public string? UserDefinedField5 { get; set; }
    public string? UserDefinedField6 { get; set; }
    public string? UserDefinedField7 { get; set; }
    public string? UserDefinedField8 { get; set; }
    public string? UserDefinedField9 { get; set; }
    public string? UserDefinedField10 { get; set; }
    public string? VendorIdentifier { get; set; }
    public string? TaxIdentifier { get; set; }
    public CwReference? TaxCode { get; set; }
    public CwReference? BillingTerms { get; set; }
    public CwReference? InvoiceTemplate { get; set; }
    public CwReference? PricingSchedule { get; set; }
    public CwReference? CompanyEntityType { get; set; }
    public CwReference? BillToCompany { get; set; }
    public CwReference? BillingSite { get; set; }
    public CwReference? BillingContact { get; set; }
    public CwReference? InvoiceDeliveryMethod { get; set; }
    public CwReference? EmailTemplate { get; set; }
    public string? InvoiceToEmailAddress { get; set; }
    public string? InvoiceCCEmailAddress { get; set; }
    public bool? DeletedFlag { get; set; }
    public DateTime? DateDeleted { get; set; }
    public string? DeletedBy { get; set; }
    public string? MobileGuid { get; set; }
    public string? FacebookUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public CwReference? Currency { get; set; }
    public CwReference? TerritoryManager { get; set; }
    public string? ResellerIdentifier { get; set; }
    public bool? IsVendorFlag { get; set; }
    public List<string>? IntegratorTags { get; set; }
    public List<CwCustomField>? CustomFields { get; set; }
}