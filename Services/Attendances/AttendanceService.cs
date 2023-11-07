using System.Globalization;
using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.DTO.Attendances;
using AbsenDulu.BE.DTO.Notifications;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Interfaces.IServices.Attendances;
using AbsenDulu.BE.Interfaces.IServices.CompanyData;
using AbsenDulu.BE.Interfaces.IServices.Notification;
using AbsenDulu.BE.Interfaces.IServices.PushNotification;
using AbsenDulu.BE.Models.Attendances;
using AbsenDulu.BE.Models.Notification;
using AbsenDulu.BE.TimeZone;
using AbsenDulu.BE.Token.Helper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Server.IIS;

namespace AbsenDulu.BE.Services.Attendances;
public class AttendanceService : IAttendanceService
{
    private readonly DataContext _context;
    private readonly IEmployeeService _employee;
    private readonly TokenValidate _tokenConfig;
    private readonly IShiftService _shift;
    private readonly IRabbitMQService _pushNotification;
    private readonly INotificationService _notifService;

    public AttendanceService(INotificationService notificationService, IRabbitMQService rabbitMQService, IShiftService shiftService, IEmployeeService employeeService, TokenValidate tokenValidate, DataContext context)
    {
        _context = context;
        _tokenConfig = tokenValidate;
        _employee = employeeService;
        _shift = shiftService;
        _pushNotification = rabbitMQService;
        _notifService = notificationService;
    }

    public List<AttendanceView> GetAttendances(Guid companyId)
    {
        try
        {
            var data = _context.attendances.Where(d => d.CompanyId.Equals(companyId)).ToList();
            foreach (var entry in data)
            {
                if (entry.ClockIn != null)
                    entry.ClockIn = DateTime.Parse(entry.ClockIn).AddHours(7).ToString("HH:mm:ss");
                if (entry.ClockOut != null)
                    entry.ClockOut = DateTimeOffset.Parse(entry.ClockOut).AddHours(7).ToString("HH:mm:ss");

            }
            return data;
        }
        catch
        {
            throw;
        }

    }
    public List<ViewMobileDashboard> GetAttendancesMobile(Guid companyId)
    {
        try
        {
            var data = _context.attendace_mobile.Where(d => d.CompanyId.Equals(companyId)).ToList();
            foreach (var entry in data)
            {
                if (entry.ClockIn != null)
                    entry.ClockIn = DateTime.Parse(entry.ClockIn).AddHours(7).ToString("HH:mm:ss");
                if (entry.ClockOut != null)
                    entry.ClockOut = DateTimeOffset.Parse(entry.ClockOut).AddHours(7).ToString("HH:mm:ss");

            }
            return data;
        }
        catch
        {
            throw;
        }

    }

    public List<AttendanceView> FindByContains(string request, Guid companyId)
    {
        try
        {
            var data = _context.attendances
            .Where(e => e.EmployeeName.ToLower().Contains(request.ToLower())
            || e.ClockInMethod.ToLower().Contains(request.ToLower()) || e.ClockInAddress.ToLower().Contains(request.ToLower())
            || e.ClockInNote.ToLower().Contains(request.ToLower())
            || e.ClockOutMethod.ToLower().Contains(request.ToLower()) || e.ClockOutAddress.ToLower().Contains(request.ToLower())
            || e.ClockOutNote.ToLower().Contains(request.ToLower())
            && e.CompanyId.Equals(companyId)).ToList();
            foreach (var entry in data)
            {
                if (entry.ClockIn != null)
                    entry.ClockIn = DateTime.Parse(entry.ClockIn).AddHours(7).ToString("HH:mm:ss");
                if (entry.ClockOut != null)
                    entry.ClockOut = DateTimeOffset.Parse(entry.ClockOut).AddHours(7).ToString("HH:mm:ss");

            }
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public List<AttendanceView> FindByContainsAndDate(string request, Guid companyId, DateTime date)
    {
        try
        {
            var data = _context.attendances
            .Where(e => e.EmployeeName.ToLower().Contains(request.ToLower())
            || e.ClockInMethod.ToLower().Contains(request.ToLower()) || e.ClockInAddress.ToLower().Contains(request.ToLower())
            || e.ClockInNote.ToLower().Contains(request.ToLower())
            || e.ClockOutMethod.ToLower().Contains(request.ToLower()) || e.ClockOutAddress.ToLower().Contains(request.ToLower())
            || e.ClockOutNote.ToLower().Contains(request.ToLower())
            && e.CompanyId.Equals(companyId)).ToList();
            foreach (var entry in data)
            {
                if (entry.ClockIn != null)
                    entry.ClockIn = DateTime.Parse(entry.ClockIn).AddHours(7).ToString("HH:mm:ss");
                if (entry.ClockOut != null)
                    entry.ClockOut = DateTimeOffset.Parse(entry.ClockOut).AddHours(7).ToString("HH:mm:ss");

            }
            var filteredData = data.Where(e => Convert.ToDateTime(e.Date) == date.Date && e.CompanyId==companyId).ToList();
            return filteredData;
        }
        catch
        {
            throw;
        }
    }

    public List<ViewMobileDashboard> FindByContainsAndDateMobile(string request, Guid companyId, DateTime date)
    {
        try
        {
            var data = _context.attendace_mobile
            .Where(e => e.EmployeeName.ToLower().Contains(request.ToLower())
            || e.ClockInAddress.ToLower().Contains(request.ToLower())
            || e.ClockInNote.ToLower().Contains(request.ToLower())
            || e.ClockOutAddress.ToLower().Contains(request.ToLower())
            || e.ClockOutNote.ToLower().Contains(request.ToLower())
            && e.CompanyId.Equals(companyId)).ToList();
            var filteredData = data.Where(e => Convert.ToDateTime(e.Date) == date.Date).ToList();
            return filteredData;
        }
        catch
        {
            throw;
        }
    }
    public Attendance AddAttendance(AttendanceDTO request)
    {
        try
        {
            string dayName = TimeZoneUtility.ConvertToWIBTime(DateTime.UtcNow).ToString("dddd");
            var employee = _employee.GetEmployeeUsername(_tokenConfig.UserName, _tokenConfig.CompanyId);
            var shift = _shift.GetShiftById(employee.First().ShiftId);
            var timecheckin = DateTime.ParseExact(shift.First().StartWorkTime, "HH:mm", CultureInfo.InvariantCulture);
            var timecheckout = DateTime.ParseExact(shift.First().EndWorkTime, "HH:mm", CultureInfo.InvariantCulture);
            var mincheckin = timecheckin - TimeZoneUtility.ConvertToWIBTime(DateTime.UtcNow);
            var mincheckout = timecheckout - TimeZoneUtility.ConvertToWIBTime(DateTime.UtcNow);
            double minutesDifferencecheckin = mincheckin.TotalMinutes;
            double minutesDifferencecheckout = mincheckout.TotalMinutes;

            Attendance attendance = new Attendance
            {
                Id = Guid.NewGuid(),
                AttendanceMethod = request.AttendanceMethod,
                AttendanceType = request.AttendanceType,
                AttendanceImage = request.AttendanceImage,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                LocationAddress = request.LocationAddress,
                EmployeeShift = shift.FirstOrDefault().ShiftName,
                IsApproved = true,
                EmployeeId = employee.FirstOrDefault().Id,
                Note = request.Note,
                CompanyId = _tokenConfig.CompanyId,
                Company = _tokenConfig.CompanyName,
                CreatedBy = _tokenConfig.UserName,
                CreatedAt = DateTime.UtcNow
            };
                // DateTime today = DateTime.UtcNow.Date;
                // var data = _context.attendance
                //             .Where(d => d.CompanyId.Equals(_tokenConfig.CompanyId)
                //                     && d.EmployeeId.Equals(employee.FirstOrDefault().Id)
                //                     && d.CreatedAt.Date == today).ToList();
                // if (data.Count > 0 && data.FirstOrDefault().AttendanceType == "Check In")
                // {
                //     if(minutesDifferencecheckout>1)
                //     {
                //         throw new IdentityException($"You can check out time in {minutesDifferencecheckout} Minutes ");
                //     }
                //     attendance.AttendanceType = "Check Out";
                // }
                // else
                // {
                //     if(minutesDifferencecheckin>60)
                //     {
                //         throw new IdentityException($"You can check in time in {minutesDifferencecheckin-60} Minutes ");
                //     }
                //     attendance.AttendanceType = "Check In";
                // }


            if (shift.First().WorkDays.Contains(dayName))
            {
                var today = TimeZoneUtility.ConvertToWIBTime(DateTime.UtcNow);
                var data = _context.attendance
                            .Where(d => d.CompanyId.Equals(_tokenConfig.CompanyId)
                                    && d.EmployeeId.Equals(employee.FirstOrDefault().Id)
                                    && d.CreatedAt.Date == DateTime.UtcNow.Date).ToList();
                if (data.Count > 0 && data.FirstOrDefault().AttendanceType == "Check In")
                {
                    if(minutesDifferencecheckout>1)
                    {
                        throw new ValidationException($"You can check out time in {shift.First().EndWorkTime}");
                    }
                    attendance.AttendanceType = "Check Out";
                }
                else
                {
                    if(minutesDifferencecheckin>60)
                    {
                        throw new ValidationException($"You can check in time in {shift.First().StartWorkTime}");
                    }
                    attendance.AttendanceType = "Check In";
                }
            }
            else
            {
                throw new ValidationException($"today : {dayName} exclude from your workday");
            }
            _context.attendance.Add(attendance);
            _context.SaveChanges();

            NotificationsDTO notification = new NotificationsDTO
            {
                Header = "Attendances",
                Title = attendance.AttendanceType,
                Message = $"Success {attendance.AttendanceType}",
                UserId = new Guid(_tokenConfig.Id),
                CompanyId = _tokenConfig.CompanyId,
            };
            _notifService.AddNotification(notification);
            _pushNotification.SendMessageToQueue($"employee-{Convert.ToInt32(_tokenConfig.EmployeeId)}", $"Success {attendance.AttendanceType}");
            return attendance;
        }
        catch
        {
            throw;
        }
    }

    public Attendance RequestAttendance(AttendanceDTO request)
    {
        try
        {
            string dayName = DateTime.Now.ToString("dddd");
            var employee = _employee.GetEmployeeUsername(_tokenConfig.UserName, _tokenConfig.CompanyId);
            var shift = _shift.GetShiftById(employee.First().ShiftId);
            var timecheckin = DateTime.ParseExact(shift.First().StartWorkTime, "HH:mm", CultureInfo.InvariantCulture);
            var timecheckout = DateTime.ParseExact(shift.First().EndWorkTime, "HH:mm", CultureInfo.InvariantCulture);
            var mincheckin = timecheckin - DateTime.Now;
            var mincheckout = timecheckout - DateTime.Now;
            double minutesDifferencecheckin = mincheckin.TotalMinutes;
            double minutesDifferencecheckout = mincheckout.TotalMinutes;

            Attendance attendance = new Attendance
            {
                Id = Guid.NewGuid(),
                AttendanceMethod = request.AttendanceMethod,
                AttendanceType = request.AttendanceType,
                AttendanceImage = request.AttendanceImage,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                LocationAddress = request.LocationAddress,
                EmployeeShift = shift.FirstOrDefault().ShiftName,
                IsApproved = true,
                EmployeeId = employee.FirstOrDefault().Id,
                Note = request.Note,
                CompanyId = _tokenConfig.CompanyId,
                Company = _tokenConfig.CompanyName,
                CreatedBy = _tokenConfig.UserName,
                CreatedAt = DateTime.UtcNow
            };
            // DateTime today = DateTime.UtcNow.Date;
            // var data = _context.attendance
            //             .Where(d => d.CompanyId.Equals(_tokenConfig.CompanyId)
            //                     && d.EmployeeId.Equals(employee.FirstOrDefault().Id)
            //                     && d.CreatedAt.Date == today).ToList();
            // if (data.Count > 0 && data.FirstOrDefault().AttendanceType == "Check In")
            // {
            //     if(minutesDifferencecheckout>1)
            //     {
            //         throw new IdentityException($"You can check out time in {minutesDifferencecheckout} Minutes ");
            //     }
            //     attendance.AttendanceType = "Check Out";
            // }
            // else
            // {
            //     if(minutesDifferencecheckin>60)
            //     {
            //         throw new IdentityException($"You can check in time in {minutesDifferencecheckin-60} Minutes ");
            //     }
            //     attendance.AttendanceType = "Check In";
            // }


            if (shift.First().WorkDays.Contains(dayName))
            {
                var today = TimeZoneUtility.ConvertToWIBTime(DateTime.UtcNow);
                var data = _context.attendance
                            .Where(d => d.CompanyId.Equals(_tokenConfig.CompanyId)
                                    && d.EmployeeId.Equals(employee.FirstOrDefault().Id)
                                    && d.CreatedAt.Date == today).ToList();
                if (data.Count > 0 && data.FirstOrDefault().AttendanceType == "Request Check In")
                {
                    if (minutesDifferencecheckout > 1)
                    {
                        throw new ValidationException($"You can check out time in {shift.First().EndWorkTime}");
                    }
                    attendance.AttendanceType = "Request Check Out";
                }
                else
                {
                    if (minutesDifferencecheckin > 60)
                    {
                        throw new ValidationException($"You can check in time in {shift.First().StartWorkTime}");
                    }
                    attendance.AttendanceType = "Request Check In";
                }
            }
            else
            {
                throw new ValidationException($"today : {dayName} exclude from your workday");
            }
            _context.attendance.Add(attendance);
            _context.SaveChanges();

            NotificationsDTO notification = new NotificationsDTO
            {
                Header = "Attendances",
                Title = attendance.AttendanceType,
                Message = $"Success {attendance.AttendanceType}",
                UserId = new Guid(_tokenConfig.Id),
                CompanyId = _tokenConfig.CompanyId,
            };
            _notifService.AddNotification(notification);
            _pushNotification.SendMessageToQueue($"employee-{Convert.ToInt32(_tokenConfig.EmployeeId)}", $"Success {attendance.AttendanceType}");
            return attendance;
        }
        catch
        {
            throw;
        }
    }

    public static bool IsWithinGeofence(double requestLat, double requestLon, double geofenceLat, double geofenceLon, double radius)
    {
        double distance = CalculateDistance(requestLat, requestLon, geofenceLat, geofenceLon);

        return distance <= radius;
    }

    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371;

        lat1 = ToRadians(lat1);
        lon1 = ToRadians(lon1);
        lat2 = ToRadians(lat2);
        lon2 = ToRadians(lon2);

        double dlon = lon2 - lon1;
        double dlat = lat2 - lat1;
        double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = R * c;

        return distance;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }
}