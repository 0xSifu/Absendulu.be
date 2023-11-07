using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Approval;
public class RequestApprovalLeaveDTO
{

    [Required]
    public string? LeaveName { get; set; }
    public string? Document { get; set; }
    [Required]
    public DateTime FromDate  { get; set; }
    [Required]
    public DateTime ToDate { get; set; }
    public string Status { get; set; }
    public string Note { get; set; }
}