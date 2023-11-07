using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Attendances;
[Table("view_mobile_dashboard")]
public class ViewMobileDashboard
{
    [Column("employee_name")]
    public string? EmployeeName { get; set; }
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    [Column("position_name")]
    public string? PositionName { get; set; }
    [Column("department_name")]
    public string? DepartmentName { get; set; }
    [Column("date")]
    public string? Date { get; set; }
    [Column("late")]
    public double? Late { get; set; }
    [Column("clock_in")]
    public string? ClockIn { get; set; }
    [Column("clock_in_address")]
    public string? ClockInAddress { get; set; }
    [Column("clock_in_note")]
    public string? ClockInNote { get; set; }
    [Column("clock_out")]
    public string? ClockOut { get; set; }
    [Column("clock_out_address")]
    public string? ClockOutAddress { get; set; }
    [Column("clock_out_note")]
    public string? ClockOutNote { get; set; }
    [Column("employee_shift")]
    public string? EmployeeShift { get; set; }
    [Column("company_id")]
    public Guid CompanyId { get; set; }
    [Column("profile_picture")]
    public string? ProfilePicture { get; set; }
}