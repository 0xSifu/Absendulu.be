using System.ComponentModel.DataAnnotations.Schema;
using AbsenDulu.BE.Models.Workflow;
using Newtonsoft.Json;

namespace AbsenDulu.BE.Models.Approval;
[Table("approvals_leave")]
public class ApprovalLeaves
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("company_id")]
    public Guid CompanyId { get; set; }
    [Column("user_account_id")]
    public Guid UserAccountId { get; set; }
    [Column("username")]
    public string? Username { get; set; }
    [JsonProperty("previous_approver")]
    [Column(TypeName = "json")]
    public JsonContentApprover? previous_approver { get; set; }
    [JsonProperty("current_approver")]
    [Column(TypeName = "json")]
    public JsonContentApprover? current_approver { get; set; }
    [JsonProperty("next_approver")]
    [Column(TypeName = "json")]
    public JsonContentApprover? next_approver { get; set; }
    [Column("leave_name")]
    public string? LeaveName { get; set; }
    [Column("document_attachments")]
    public string? DocumentAttachments { get; set; }
    [Column("note")]
    public string? Note { get; set; }
    [Column("status")]
    public string? Status { get; set; }
    [Column("from_date")]
    public DateTime FromDate { get; set; }
    [Column("to_date")]
    public DateTime ToDate { get; set; }
    [Column("total_days")]
    public double TotalDays { get; set; }
    // [Column("properties")]
    [JsonProperty("properties")]
    [Column(TypeName = "json")]
    public List<JsonContentApprover>? properties { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}