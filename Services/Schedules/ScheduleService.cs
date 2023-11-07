using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Interfaces.IServices.Schedules;
using AbsenDulu.BE.Models.Schedules;

namespace AbsenDulu.BE.Services.Schedules;
public class ScheduleService: IScheduleService
{
    private readonly DataContext _context;
    public ScheduleService(DataContext dataContext)
    {
        _context=dataContext;
    }

    public List<Schedule> GetSchedules(Guid companyId,string fromdate,string todate)
    {
        try
        {
            var data = _context.GetSchedule(companyId,fromdate,todate).OrderBy(x=>x.date).ToList();
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

    public List<Schedule> FindByContains(string request, Guid companyId, string fromdate, string todate)
    {
        try
        {
            var data = _context.GetSchedule(companyId, fromdate, todate)
            .Where(e => e.EmployeName.ToLower().Contains(request.ToLower()) || e.DepartmentName.ToLower().Contains(request.ToLower())
            || e.PositionName.ToLower().Contains(request.ToLower()) || e.ShiftName.ToLower().Contains(request.ToLower())
            || e.date.ToString("dd-MM-yyyy").Contains(request.ToLower()) || e.AttendanceStatus.ToLower().Contains(request.ToLower())
            ).ToList();
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
}