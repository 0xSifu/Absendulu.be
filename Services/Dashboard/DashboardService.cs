using System.Globalization;
using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Interfaces.IServices.CompanyData;
using AbsenDulu.BE.Interfaces.IServices.Dashboard;
using AbsenDulu.BE.Interfaces.IServices.Schedules;
using AbsenDulu.BE.Models.Attendances;
using AbsenDulu.BE.Models.Company;
using AbsenDulu.BE.Models.Dashboard;
using AbsenDulu.BE.TimeZone;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace AbsenDulu.BE.Services.Dashboard;
public class DashboardService : IDashboardService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IShiftService _service;
    public readonly IEmployeeService _employee;
    public readonly IScheduleService _schedule;
    public DashboardService(IEmployeeService employeeService, IShiftService shiftService, DataContext dataContext,IHttpContextAccessor httpContext , IScheduleService scheduleService)
    {
        _context = dataContext;
        _httpContext = httpContext;
        _service = shiftService;
        _employee = employeeService;
        _schedule=scheduleService;
    }

    public ViewMobileDashboard GetMobileDashboard(Guid companyId,int  employeeId)
    {
        try
        {
            var data = _context.attendace_mobile
                .Where(d => d.CompanyId.Equals(companyId)
                            && d.EmployeeId == employeeId)
                .ToList();
            // var filteredData = data.Where(e => Convert.ToDateTime(e.Date) == DateTime.Now.Date).ToList();
            var date = TimeZoneUtility.ConvertToWIBTime(DateTime.UtcNow);
            var filteredData = data.Where(e =>
                Convert.ToDateTime(e.Date).Date == date.Date
            ).ToList();
            foreach (var entry in filteredData)
            {
                if (entry.ClockIn!=null)
                    entry.ClockIn = DateTime.Parse(entry.ClockIn).AddHours(7).ToString("HH:mm:ss");
                if(entry.ClockOut!=null)
                    entry.ClockOut = DateTimeOffset.Parse(entry.ClockOut).AddHours(7).ToString("HH:mm:ss");

            }
            return filteredData.FirstOrDefault();
        }
        catch
        {
            throw;
        }
    }

    public AttendanceDetail GetMobileDashboardMonth(Guid companyId,int employeeId,string Id)
    {
        try
        {
            var data = _context.attendace_mobile
                .Where(d => d.CompanyId == companyId && d.EmployeeId == employeeId)
                .ToList();

            // Filter data berdasarkan bulan dan tahun target
            var date = TimeZoneUtility.ConvertToWIBTime(DateTime.UtcNow);
            var filteredData = data.Where(e =>
                Convert.ToDateTime(e.Date).Month == date.Month
            ).ToList();
            if(filteredData.Count < 1)
            {
                return new AttendanceDetail();
            }
            foreach (var entry in filteredData)
            {
                if (entry.ClockIn != null)
                    entry.ClockIn = DateTime.Parse(entry.ClockIn).AddHours(7).ToString("HH:mm:ss");
                if (entry.ClockOut != null)
                    entry.ClockOut = DateTimeOffset.Parse(entry.ClockOut).AddHours(7).ToString("HH:mm:ss");
            }
            var employe = _employee.GetEmployeeById(companyId,Convert.ToInt32(employeeId)).FirstOrDefault();
            var ShiftId = _context.employees.Where(d => d.CompanyCode.Equals(companyId) && d.Id==employeeId).ToList();
            var Absent = _schedule.GetSchedules(companyId,date.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd")).Where(x=>x.EmployeName==employe.EmployeeName);
            var shift = _service.GetShiftById(ShiftId.First().ShiftId);
            var late = filteredData.Where(e => e.Late>0).Count();
            var present = filteredData.Where(e => e.Late < 1).Count();
            var absen = Absent.Count();
            var response = new AttendanceDetail
            {
                EmployeeName=filteredData.First().EmployeeName,
                Late=late,
                Present=present,
                Absent=absen,
                CompanyId=filteredData.First().CompanyId,
                StartWorkTime = shift.First().StartWorkTime,
                EndWorkTime = shift.First().EndWorkTime,
                StartBreakTime = shift.First().StartBreakTime,
                EndBreakTime = shift.First().EndBreakTime,
                WorkDays = shift.First().WorkDays,
                Shift = shift.First().ShiftName
            };
            return response;
        }
        catch
        {
            throw;
        }
    }
    public List<MasterCompany> GetCompanyData(Guid companyId)
    {
        try
        {
            var data = _context.master_companies
                .Where(d => d.Id == companyId)
                .ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }

    public List<ViewDetailSubscriptions> GetDetailSubscriptions(Guid companyId)
    {
        try
        {
            var data = _context.view_detail_subscriptions
                .Where(d => d.CompanyId == companyId).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }

    public List<ViewAttendanceLate> GetAttendanceLate(Guid companyId)
    {
        try
        {
            var data = _context.view_attendances_late
                .Where(d => d.CompanyId == companyId).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }
    public List<ViewAttendanceAbsent> GetAttendanceAbsent(Guid companyId)
    {
        try
        {
            var data = _context.view_attendances_absent
                .Where(d => d.CompanyId == companyId).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }
    public List<ViewAttendancePresent> GetAttendancePresent(Guid companyId)
    {
        try
        {
            var data = _context.view_attendances_present
                .Where(d => d.CompanyId == companyId).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }
    public List<ViewAttendancelateByDepartment> GetAttendanceLateByDepartment(Guid companyId)
    {
        try
        {
            var data = _context.view_attendances_late_by_department
                .Where(d => d.CompanyId == companyId).ToList();
            var filteredData = data.Where(e => Convert.ToDateTime(e.Date) == DateTime.Now.Date).ToList();
            return filteredData;
        }
        catch
        {
            throw;
        }
    }

    public List<ViewAttendancelateByEmployees> GetAttendanceLateByEmployee(Guid companyId)
    {
        try
        {
            var data = _context.view_attendances_late_by_employees
                .Where(d => d.CompanyId == companyId).ToList();
            var filteredData = data.Where(e => Convert.ToDateTime(e.Date) == DateTime.Now.Date).ToList();
            return filteredData;
        }
        catch
        {
            throw;
        }
    }
}