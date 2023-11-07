using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Company;
public class MasterDepartmentDTO
{
    public Guid Id { get; set; }
    [Required]
    public string? DepartmentCode { get; set; }
    [Required]
    public string? DepartmentName { get; set; }
    public Guid CompanyId { get; set; }
    public string? Company { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}