using System.ComponentModel.DataAnnotations;
namespace AbsenDulu.BE.DTO.Approval;
public class ApprovalWorkshopDTO
{
    public string? Id { get; set; }
    [Required]
    public string? WorkshopCode { get; set; }
    [Required]
    public string? WorkshopName { get; set; }
    [Required]
    public double Total { get; set; }
    [Required]
    public string? Unit { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public string? StartTime { get; set; }
    [Required]
    public string? EndTime { get; set; }
    [Required]
    public string? CompanyId { get; set; }
    [Required]
    public string? Company { get; set; }
    [Required]
    public int StatusId { get; set; }
    [Required]
    public string? Status { get; set; }
    [Required]
    public string? Note { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}