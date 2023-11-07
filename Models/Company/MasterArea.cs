using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AbsenDulu.BE.Models.Company;
[Table("master_areas")]
public class MasterArea
{
    [Column("id")]
    public Guid Id {get;set;}
    [Column("area_code")]
    public string? AreaCode {get;set;}
    [Column("area_name")]
    public string? AreaName {get;set;}
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