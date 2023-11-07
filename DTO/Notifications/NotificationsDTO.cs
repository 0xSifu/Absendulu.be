using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Notifications;
public class NotificationsDTO
{
    public Guid Id { get; set; }
    public string? Header { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public bool Isread { get; set; }
}