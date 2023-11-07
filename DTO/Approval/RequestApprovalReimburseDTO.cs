using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Approval;
public class RequestApprovalReimburseDTO
{
    [Required]
    public string? ReimburseName { get; set; }
    [Required]
    public double? ReimburseBudget { get; set; }
    [Required]
    public double? ReimburseAmount { get; set; }
    [Required]
    public string? Document { get; set; }
    public string Status { get; set; }
    public string Note { get; set; }
}