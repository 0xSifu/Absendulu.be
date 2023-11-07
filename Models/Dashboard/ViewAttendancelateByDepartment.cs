using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Dashboard;
[Table("view_attendances_late_by_department")]
public class ViewAttendancelateByDepartment
{
    [Column("department_name")]
    public string? DepartmentName {get;set;}
    [Column("percentage_late")]
    public int PercentageLate {get;set;}
    [Column("company_id")]
    public Guid CompanyId {get;set;}
    [Column("date")]
    public string? Date { get; set; }
}