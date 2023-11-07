using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Schedules;
[Table("view_schedule_employees")]
public class EmployeeSchedule
{
    [Column("calendar_id")]
    public Guid Id { get; set; }
    [Column("calendar_day")]
    public string? Day { get; set; }
    [Column("calendar_date")]
    public int Date { get; set; }
    [Column("calendar_month")]
    public int Month { get; set; }
    [Column("calendar_year")]
    public int Year { get; set; }
    [Column("is_sunday")]
    public bool IsSunday { get; set; }
    [Column("is_holiday")]
    public bool IsHoliday { get; set; }
    [Column("holiday_name")]
    public string? HolidayName { get; set; }
    [Column("employee_name")]
    public string? EmployeeName { get; set; }
    [Column("shift_code")]
    public string? ShiftCode { get; set; }
    [Column("shift_name")]
    public string? ShiftName { get; set; }
    [Column("start_work_time")]
    public string? StartWorkTime { get; set; }
    [Column("end_work_time")]
    public string? EndWorkTime { get; set; }
    [Column("start_break_time")]
    public string? StartBreakTime { get; set; }
    [Column("end_break_time")]
    public string? EndBreakTime { get; set; }
    [Column("position_name")]
    public string? PositionName { get; set; }
    [Column("work_status")]
    public string? WorkStatus { get; set; }
    [Column("work_shift")]
    public string? WorkShift { get; set; }
    [Column("company_id")]
    public Guid? CompanyId { get; set; }


}