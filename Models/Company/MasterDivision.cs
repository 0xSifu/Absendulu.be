namespace AbsenDulu.BE.Models.Company;
public class MasterDivision
{
    public Guid id = Guid.NewGuid();
    public string? DivisionCode {get;set;}
    public string? DivisionName {get;set;}
    public string? Company {get;set;}
    public string? CompanyId {get;set;}
    public string? CreatedBy {get;set;}
    public DateTime CreatedAt {get;set;}
    public string? UpdatedBy {get;set;}
    public DateTime? UpdatedDate {get;set;}
}