using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AbsenDulu.BE.Models.Company;
[Table("master_departments")]
public class MasterDepartment
{
    [Column("id")]
    public Guid Id {get; set;}
    [Column("department_code")]
    public string? DepartmentCode {get;set;}
    [Column("department_name")]
    public string? DepartmentName {get;set;}
    [Column("company_name")]
    public string? Company {get;set;}
    [Column("company_id")]
    public Guid CompanyId {get;set;}
    [Column("created_by")]
    public string? CreatedBy {get;set;}
    [Column("created_at")]
    public DateTime CreatedAt {get;set;}
    [Column("updated_by")]
    public string? UpdatedBy {get;set;}
    [Column("updated_at")]
    public DateTime? UpdatedDate {get;set;}
}