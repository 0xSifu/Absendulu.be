using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Dashboard;
[Table("view_detail_subscriptions")]
public class ViewDetailSubscriptions
{
    [Column("employees")]
    public int Employees {get;set;}
    [Column("on_time")]
    public int? Ontime {get;set;}
    [Column("late")]
    public int? Late {get;set;}
    [Column("absent")]
    public int? Absent {get;set;}
    [Column("active_apps")]
    public string? ActiveApps {get;set;}
    [Column("active_device")]
    public string? ActiveDecices {get;set;}
    [Column("date")]
    public string? Date { get; set; }
    [Column("company_id")]
    public Guid CompanyId {get;set;}

}