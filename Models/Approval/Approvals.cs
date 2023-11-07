using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AbsenDulu.BE.Models.Approval;
[Table("approvals")]
public class Approvals
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("company_id")]
    public Guid CompanyId { get; set; }
    [Column("approval_type")]
    public string? ApprovalType { get; set; }
    [Column("user_account_id")]
    public Guid UserAccountId { get; set; }
    [Column("username")]
    public string? Username { get; set; }
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    [Column("current_approver")]
    public int? CurrentApprover { get; set; }
    [Column("next_approver")]
    public int? NextApprover { get; set; }

    // [Column("properties")]
    [JsonProperty("properties")]
    [Column(TypeName = "json")]
    public JsonContentApproval? properties { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
