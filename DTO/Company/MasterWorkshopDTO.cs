using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Company;
public class MasterWorkshopDTO
{
    public string? Id { get; set; }
    [Required]
    public string? WorkshopCode { get; set; }
    [Required]
    public string? WorkshopName { get; set; }
    public Guid CompanyId { get; set; }
    [Required]
    public string? Description { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}