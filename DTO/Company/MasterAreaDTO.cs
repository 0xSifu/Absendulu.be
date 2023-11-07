using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Company;
public class MasterAreaDTO
{
    public string? Id {get;set;}
    [Required]
    public string? AreaCode {get;set;}
    [Required]
    public string? AreaName {get;set;}
    public string? Company {get;set;}
    public Guid CompanyId {get;set;}
    public string? CreatedBy {get;set;}
    public string? UpdatedBy { get; set; }
}