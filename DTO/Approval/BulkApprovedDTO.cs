using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Approval;
public class BulkApprovedDTO
{
    [Required]
    public List<Guid> Id { get; set; }
    public string? Note { get; set; }
}