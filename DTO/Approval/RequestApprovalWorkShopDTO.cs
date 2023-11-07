using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Approval;
public class RequestApprovalWorkShopDTO
{
    [Required]
    public string? WorkShopName { get; set; }
    [Required]
    public string? WorkShopAddress { get; set; }
    public string? Document { get; set; }
    [Required]
    public DateTime WorkShopStart { get; set; }
    [Required]
    public DateTime WorkShopEnd { get; set; }
    public string Status { get; set; }
    public string Note { get; set; }
}