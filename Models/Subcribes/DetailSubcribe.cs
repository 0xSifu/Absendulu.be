using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Subcribes;
[Table("detail_subscriptions")]
public class DetailSubcribe
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("company_id")]
    public Guid CompanyId { get; set; }
    [Column("company_name")]
    public string? CompanyName { get; set; }
    [Column("subscribed_package")]
    public Guid SubscribedPackage { get; set; }
    [Column("device_total")]
    public int DeviceTotal { get; set; }
    [Column("device_used")]
    public int DeviceUsed { get; set; }
    [Column("remaining_device")]
    public int RemainingDevice { get; set; }
    [Column("apps_total")]
    public int AppsTotal { get; set; }
    [Column("apps_used")]
    public int AppsUsed { get; set; }
    [Column("remaining_apps")]
    public int RemainingApps { get; set; }
    [Column("company_start_subscribe")]
    public DateTime CompanyStartSubcribe { get; set; }
    [Column("company_expired_subscribe")]
    public DateTime CompanyExpiredSubcribe { get; set; }
    [Column("created_by")]
    public string? CreatedBy { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_by")]
    public string? UpdatedBy { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}
