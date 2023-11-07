using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Company;
public class MasterLeaveDTO
{
    public string? Id { get; set; }
    [Required]
    public string? LeaveCode { get; set; }
    [Required]
    public string? LeaveName { get; set; }
    public int? TotalDays { get; set; }
    public bool? LeaveMoreThanBalance { get; set; }
    public Guid CompanyId { get; set; }
    public string? Company { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}