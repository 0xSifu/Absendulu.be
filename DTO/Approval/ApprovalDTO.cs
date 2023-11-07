using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Approval;
public class ApprovalDTO
{
    [Required]
    public string? Status { get; set; }
    public string? Note { get; set; }

}