using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Dashboard;
[Table("view_attendances_late_by_employee")]
public class ViewAttendancelateByEmployees
{
    [Column("employee_name")]
    public string? Employee { get; set; }
    [Column("department_name")]
    public string? DepartmentName { get; set; }
    [Column("clock_in")]
    public string? ClockIn { get; set; }
    [Column("company_id")]
    public Guid CompanyId { get; set; }
    [Column("date")]
    public string? Date { get; set; }
}