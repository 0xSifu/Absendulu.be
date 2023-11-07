using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Approval.Visit;
[Table("approvals_visit")]
public class ApprovalVisit
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("company_id")]
    public Guid CompanyId { get; set; }
    [Column("user_account_id")]
    public Guid UserAccountId { get; set; }
    [Column("username")]
    public string? Username { get; set; }
    [Column("approver_id")]
    public string? ApproverId { get; set; }
    [Column("is_approved")]
    public bool IsApproved { get; set; }
    [Column("document_attachments")]
    public string? DocumentAttachments { get; set; }
    [Column("note")]
    public string? Note { get; set; }
    [Column("longitude")]
    public string? Longitude { get; set; }
    [Column("latitude")]
    public string? Latitude { get; set; }
    [Column("address")]
    public string? Address { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}