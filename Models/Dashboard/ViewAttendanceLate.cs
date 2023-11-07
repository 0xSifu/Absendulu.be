using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Dashboard;
[Table("view_attendances_late")]
public class ViewAttendanceLate
{
    [Column("year")]
    public double Year {get;set;}
    [Column("month")]
    public double Month {get;set;}
    [Column("total")]
    public int Total {get;set;}
    [Column("company_id")]
    public Guid CompanyId {get;set;}
}