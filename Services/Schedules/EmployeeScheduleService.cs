using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Interfaces.IServices.Schedules;
using AbsenDulu.BE.Models.Schedules;

namespace AbsenDulu.BE.Services.Schedules;
public class EmployeeScheduleService: IEmployeeScheduleService
{
    private readonly  DataContext _context;
    public EmployeeScheduleService(DataContext dataContext)
    {
        _context=dataContext;
    }

    public List<EmployeeSchedule> GetEmployeeSchedules(Guid companyId, int year, int month,string employee)
    {
        try
        {
            var data  = _context.GetEmployeeSchedule($"{year}-{month}-01", $"{year}-{month}-{lastdate(year, month)}", employee,companyId).OrderBy(x => x.Date).ToList();
            // var data = _context.view_employee_schedule.Where(d => d.Year == year && d.Month == month && d.EmployeeName==employee && d.CompanyId==companyId)
            // .OrderBy(x => x.Date).ToList();
            int daysBeforeSunday = DaysBeforeSundayInMonth(year, month);

            if (daysBeforeSunday > 0)
            {
                var xt = _context.GetEmployeeSchedule($"{year}-{month-1}-01", $"{year}-{month-1}-{lastdate(year,month-1)}", employee, companyId)
                // var data2 = _context.view_employee_schedule
                //     .Where(d => d.Year == year && d.Month == month - 1 && d.EmployeeName == employee && d.CompanyId == companyId)
                    .OrderByDescending(d => d.Date)
                    .Take(daysBeforeSunday)
                    .ToList();
                xt = xt.OrderBy(x => x.Date).ToList();
                var mergedData = xt.Concat(data).ToList();
                var finalResult = mergedData.OrderBy(x => x.Month).Where(x=>x.CompanyId==companyId).ToList();

                return finalResult;
            }

            return data.OrderBy(x => x.Date).ToList();
        }
        catch
        {
            throw;
        }

    }

    public int DaysBeforeSundayInMonth(int year, int month)
    {
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        int daysBeforeSunday = 0;

        while (firstDayOfMonth.DayOfWeek != DayOfWeek.Sunday)
        {
            firstDayOfMonth = firstDayOfMonth.AddDays(-1);
            daysBeforeSunday++;
        }

        return daysBeforeSunday;
    }

    public int DaysBeforeSaturdayInMonth(int year, int month)
    {
        int daysInMonth = DateTime.DaysInMonth(year, month);
        DateTime endOfMonth = new DateTime(year, month, daysInMonth);
        DateTime firstDayOfMonth = new DateTime(year, month, Convert.ToInt32(endOfMonth));
        int daysBeforeSunday = 0;

        while (firstDayOfMonth.DayOfWeek != DayOfWeek.Sunday)
        {
            firstDayOfMonth = firstDayOfMonth.AddDays(-1);
            daysBeforeSunday++;
        }

        return daysBeforeSunday;
    }

    public int lastdate (int year,int month)
    {
        // Menghitung akhir tanggal dari bulan tertentu
        int lastDayOfMonth = DateTime.DaysInMonth(year, month);
        return lastDayOfMonth;


    }
}