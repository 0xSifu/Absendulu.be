using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AbsenDulu.BE.Models.Company;
[Table("master_workshops")]
public class MasterWorkshop
{
    [Column("id")]
    public Guid Id {get; set;}
    [Column("workshop_code")]
    public string? WorkshopCode {get;set;}
    [Column("workshop_name")]
    public string? WorkshopName {get;set;}
    [Column("description")]
    public string? Description {get;set;}
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