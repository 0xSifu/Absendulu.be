using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.DTO.Notifications;
using AbsenDulu.BE.Interfaces.IServices.Notification;
using AbsenDulu.BE.Models.Notification;
using AbsenDulu.BE.Token.Helper;

namespace AbsenDulu.BE.Services.PushNotification;
public class NotificationService:INotificationService
{
    private readonly DataContext _context;
    private readonly TokenValidate _tokenConfig;
    public NotificationService(DataContext dataContext, TokenValidate tokenValidate)
    {
        _context=dataContext;
        _tokenConfig=tokenValidate;
    }
    public List<Notif> GetNotifications(Guid request)
    {
        try
        {
            var data = _context.notifications.Where(d => d.CompanyId.Equals(request)).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }

    public List<Admin> GetNotificationsAdmin(Guid request)
    {
        try
        {
            var data = _context.admin_notifications.Where(d => d.CompanyId.Equals(request)).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }

    public List<Notif> GetNotificationsById(Guid id)
    {
        try
        {
            var data = _context.notifications.Where(d => d.Id.Equals(id)).ToList();
            return data;
        }
        catch
        {
            throw;
        }

    }

    public List<Admin> GetNotificationsAdminById(Guid id)
    {
        try
        {
            var data = _context.admin_notifications.Where(d => d.Id.Equals(id)).ToList();
            return data;
        }
        catch
        {
            throw;
        }

    }

    public List<Notif> GetNotificationsByEmployeeId(Guid userid,Guid companyid)
    {
        try
        {
            var data = _context.notifications.Where(d => d.UserId.Equals(userid) && d.CompanyId.Equals(companyid)).ToList();
            return data;
        }
        catch
        {
            throw;
        }

    }

    public List<Admin> GetNotificationsByAdminId(Guid userid, Guid companyid)
    {
        try
        {
            var data = _context.admin_notifications.Where(d => d.UserId.Equals(userid) && d.CompanyId.Equals(companyid)).ToList();
            return data;
        }
        catch
        {
            throw;
        }

    }

    public Notif AddNotification(NotificationsDTO request)
    {
        try
        {
            Notif notification = new Notif
            {
                Id = Guid.NewGuid(),
                Header = request.Header,
                Title = request.Title,
                Message = request.Message,
                UserId = request.UserId,
                CompanyId = request.CompanyId,
                IsRead = false,
                CreatedBy = "System",
                CreatedAt = DateTime.UtcNow
            };
            _context.notifications.Add(notification);
            _context.SaveChanges();
            return notification;
        }
        catch
        {
            throw;
        }
    }

    public Notif UpdateNotification(Guid id)
    {
        try
        {
            var data = _context.notifications.FirstOrDefault(d => d.Id.Equals(id));
            if (data != null)
            {
                data.IsRead=true;
                data.UpdatedBy="System";
                data.UpdatedAt=DateTime.UtcNow;
                _context.SaveChangesAsync();
                return data;
            }
            else
            {
                throw new ValidationException("Id Not Found");
            }
        }
        catch
        {
            throw;
        }

    }

    public Admin UpdateAdminNotification(Guid id)
    {
        try
        {
            var data = _context.admin_notifications.FirstOrDefault(d => d.Id.Equals(id));
            if (data != null)
            {
                data.IsRead = true;
                data.UpdatedBy = "System";
                data.UpdatedAt = DateTime.UtcNow;
                _context.SaveChangesAsync();
                return data;
            }
            else
            {
                throw new ValidationException("Id Not Found");
            }
        }
        catch
        {
            throw;
        }

    }
}