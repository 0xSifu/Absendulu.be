namespace AbsenDulu.BE.Token.Helper;
public class TokenValidate
{
    public bool isValidToken{get;set;}
    public string? Id {get;set;}
    public string? EmployeeId {get;set;}
    public string? UserName{get;set;}
    public Guid CompanyId{get;set;}
    public string? CompanyName { get; set; }
    public string? DepartmentCode { get; set; }
    public string? CompanyCity { get; set; }
    public string? Role {get;set;}
    public string? IconUrl {get;set;}
}