using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Schedules;
[Table("function_schedule")]
public class Schedule
{
    [Column("employee_name")]
    public string EmployeName { get; set; }
    [Column("department_name")]
    public string DepartmentName { get; set; }
    [Column("position_name")]
    public string PositionName { get; set; }
    [Column("shift_name")]
    public string ShiftName { get; set; }
    [Column("clock_in")]
    public string? ClockIn { get; set; }
    [Column("clock_out")]
    public string? ClockOut { get; set; }
    [Column("date")]
    public DateTime date { get; set; }
    [Column("attendance_status")]
    public string AttendanceStatus { get; set; }
}