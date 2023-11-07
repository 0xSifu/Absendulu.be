using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Company;
[Table("master_reimbursements")]
public class MasterReimbursements
{
    [Column("id")]
    public Guid Id {get;set;}
    [Column("reimbursement_code")]
    public string? ReimbursementCode {get;set;}
    [Column("reimbursement_name")]
    public string? ReimbursementName {get;set;}
    [Column("total_amount")]
    public int? TotalAmount {get;set;}
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