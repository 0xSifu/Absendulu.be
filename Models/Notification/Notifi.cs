using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Notification;
[Table("notifications")]
public class Notif
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("header")]
    public string Header { get; set; }
    [Column("title")]
    public string Title { get; set; }
    [Column("message")]
    public string Message { get; set; }
    [Column("user_id")]
    public Guid UserId { get; set; }
    [Column("company_id")]
    public Guid CompanyId { get; set; }
    [Column("is_read")]
    public bool IsRead { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("created_by")]
    public string CreatedBy { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    [Column("updated_by")]
    public string? UpdatedBy { get; set; }
}