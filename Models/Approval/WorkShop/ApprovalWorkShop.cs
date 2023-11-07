using System.ComponentModel.DataAnnotations.Schema;
using AbsenDulu.BE.Models.Workflow;
using Newtonsoft.Json;

namespace AbsenDulu.BE.Models.Approval;
[Table("approvals_workshop")]
public class ApprovalWorkShop
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
    [Column("document_attachments")]
    public string? DocumentAttachments { get; set; }
    [Column("note")]
    public string? Note { get; set; }
    [Column("status")]
    public string? Status { get; set; }
    [Column("workshop_name")]
    public string? WorkShopName { get; set; }
    [Column("workshop_address")]
    public string? WorkShopAddress { get; set; }
    [Column("workshop_start")]
    public DateTime WorkShopStart { get; set; }
    [Column("workshop_end")]
    public DateTime WorkShopEnd { get; set; }
    // [Column("properties")]
    [JsonProperty("properties")]
    [Column(TypeName = "json")]
    public List<JsonContentApprover>? properties { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}