using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Approval;
[Table("approval_workshops_logs")]
public class ApprovalWorkShopLogs
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("company_id")]
    public Guid CompanyId { get; set; }
    [Column("approval_id")]
    public Guid ApprovalId { get; set; }
    [Column("requestor_id")]
    public int RequestorId { get; set; }
    [Column("approver_id")]
    public int? ApproverId { get; set; }
    [Column("note")]
    public string? Note { get; set; }
    [Column("status")]
    public string? Status { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedDate { get; set; }
}