using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Company;
public class MasterReimbursementDTO
{
    public string? Id {get;set;}
    [Required]
    public string? ReimbursementCode {get;set;}
    [Required]
    public string? ReimbursementName {get;set;}
    [Required]
    public int? TotalAmount {get;set;}
    public string? Company {get;set;}
    public Guid CompanyId {get;set;}
    public string? CreatedBy {get;set;}
    public string? UpdatedBy { get; set; }
}