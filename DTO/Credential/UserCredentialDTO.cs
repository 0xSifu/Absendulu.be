namespace AbsenDulu.BE.DTO.Credential;
public class UserCredentialDTO
{
    public string? Id{get;set;}
    public string? EmployeeId{get;set;}
    public string? UserName{get;set;}
    public string? CompanyId{get;set;}
    public string? CompanyName { get; set; }
    public string? Role {get;set;}
    public string? DepartmentCode { get; set; }
    public string? CompanyCity { get; set; }
}