using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AbsenDulu.BE.Models.Workflow;
[Table("workflows")]
public class Workflow
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("position_id")]
    public Guid PositionId { get; set; }
    [Column("position_code")]
    public string? PositionCode { get; set; }
    [Column("position_name")]
    public string? PositionName { get; set; }
    [Column("department_id")]
    public Guid DepartmentId { get; set; }
    [Column("department_code")]
    public string? DepartmentCode { get; set; }
    [Column("department_name")]
    public string? DepartmentName { get; set; }
    [Column("company_id")]
    public Guid CompanyId { get; set; }
    [Column("approval_name")]
    public string ApprovalName { get; set; }

    [JsonProperty("approver")]
    [Column(TypeName = "json")]
    public List<JsonContentApprover> approver { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("created_by")]
    public string? CreatedBy { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    [Column("updated_by")]
    public string? UpdatedBy { get; set; }
}