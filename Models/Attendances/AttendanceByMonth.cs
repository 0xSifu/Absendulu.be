namespace AbsenDulu.BE.Models.Attendances;
public class AttendanceDetail
{
    public string? EmployeeName { get; set; }
    public Guid CompanyId { get; set; }
    public int Late { get; set; }
    public int Present { get; set; }
    public int Absent { get; set; }
    public string? Shift { get; set; }
    public string? StartWorkTime { get; set; }
    public string? EndWorkTime { get; set; }
    public string? StartBreakTime { get; set; }
    public string? EndBreakTime { get; set; }
    public string[]? WorkDays { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
}