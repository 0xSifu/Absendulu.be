using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Approval;
public class BulkRejectedDTO
{
    [Required]
    public List<Guid> Id { get; set; }
    public string? Note { get; set; }
}