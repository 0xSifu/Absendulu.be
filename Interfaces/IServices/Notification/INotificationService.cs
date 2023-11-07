using AbsenDulu.BE.DTO.Notifications;
using AbsenDulu.BE.Models.Notification;

namespace AbsenDulu.BE.Interfaces.IServices.Notification;
public interface INotificationService
{
    List<Notif> GetNotifications(Guid request);
    List<Admin> GetNotificationsAdmin(Guid request);
    List<Notif> GetNotificationsById(Guid id);
    List<Admin> GetNotificationsAdminById(Guid id);
    List<Notif> GetNotificationsByEmployeeId(Guid userid, Guid companyid);
    List<Admin> GetNotificationsByAdminId(Guid userid, Guid companyid);
    Notif AddNotification(NotificationsDTO request);
    Notif UpdateNotification(Guid id);
    Admin UpdateAdminNotification(Guid id);
}