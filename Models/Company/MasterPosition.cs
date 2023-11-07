using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AbsenDulu.BE.Models.Company;
[Table("master_positions")]
public class MasterPosition
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("department_code")]
    public string? DepartmentCode { get; set; }

    [Column("department_name")]
    public string? DepartmentName { get; set; }

    [Column("position_code")]
    public string? PositionCode { get; set; }

    [Column("position_name")]
    public string? PositionName { get; set; }

    [Column("company_name")]
    public string? CompanyName { get; set; }

    [Column("department_id")]
    public Guid DepartmentId { get; set; }

    [Column("company_id")]
    public Guid CompanyId { get; set; }

    [Column("created_by")]
    public string? CreatedBy { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_by")]
    public string? UpdatedBy { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // public MasterDepartment Department { get; set; } // Properti dengan nama DepartmentId
    // public MasterCompany Company { get; set; } // Properti dengan nama CompanyId
}
