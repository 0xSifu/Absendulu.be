namespace AbsenDulu.BE.Response;
public class DasboardMobileResponse<T,T2,T3>
{
    public bool IsError { get; set; } = false;
    public string? Message { get; set; }
    public T? EmployeAttendanceToday { get; set; }
    public T2? EmployeAttendanceMonth { get; set; }
    public T3? CompanyDetails { get; set; }
}