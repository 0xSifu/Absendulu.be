namespace AbsenDulu.BE.Response;
public class DasboardWebResponse<T,T2,T3,T4,T5,T6>
{
    public bool IsError { get; set; } = false;
    public string? Message { get; set; }
    public T? DetailCompanies { get; set; }
    public T2? AttendancesLate { get; set; }
    public T3? AttendancesAbsent { get; set; }
    public T4? AttendancesPresent { get; set; }
    public T5? AttendancesLateByDepartment { get; set; }
    public T6? AttendancesLateByEmployees { get; set; }
}