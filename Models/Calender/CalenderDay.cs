using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.CalenderDay;
[Table("calendar")]
public class CalenderDay
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("day")]
    public string? Day { get; set; }
    [Column("date")]
    public int Date { get; set; }
    [Column("month")]
    public int Month { get; set; }
    [Column("year")]
    public int Year { get; set; }
    [Column("is_sunday")]
    public bool IsSunday { get; set; }
    [Column("is_holiday")]
    public bool IsHoliday { get; set; }
    [Column("holiday_name")]
    public string? HolidayName { get; set; }
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
}