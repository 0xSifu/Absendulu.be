using System.ComponentModel.DataAnnotations.Schema;
using Alachisoft.NCache.Common.Util;

namespace AbsenDulu.BE.Models.Company;
[Table("master_shifts")]
public class MasterShift
{
    [Column("id")]
    public Guid Id {get; set;}
    [Column("shift_code")]
    public string? ShiftCode {get;set;}
    [Column("shift_name")]
    public string? ShiftName {get;set;}
    [Column("maximum_late")]
    public string? MaximumLate {get;set;}
    [Column("start_work_time")]
    public string? StartWorkTime {get;set;}
    [Column("end_work_time")]
    public string? EndWorkTime {get;set;}
    [Column("start_break_time")]
    public string? StartBreakTime {get;set;}
    [Column("end_break_time")]
    public string? EndBreakTime {get;set;}
    [Column("work_days")]
    public string[]? WorkDays {get;set;}
    [Column("company_name")]
    public string? Company {get;set;}
    [Column("company_id")]
    public Guid CompanyId {get;set;}
    [Column("created_by")]
    public string? CreatedBy {get;set;}
    [Column("created_at")]
    public DateTime CreatedAt {get;set;}
    [Column("updated_by")]
    public string? UpdatedBy {get;set;}
    [Column("updated_at")]
    public DateTime? UpdatedDate {get;set;}
}