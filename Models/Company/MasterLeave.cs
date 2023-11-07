using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AbsenDulu.BE.Models.Company;
[Table("master_leaves")]
public class MasterLeave
{
    [Column("id")]
    public Guid Id {get; set;}
    [Column("leave_code")]
    public string? LeaveCode {get;set;}
    [Column("leave_name")]
    public string? LeaveName {get;set;}
    [Column("total_days")]
    public double? TotalDays {get;set;}
    [Column("leave_more_than_balance")]
    public bool? LeaveMoreThanBalance {get;set;}
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