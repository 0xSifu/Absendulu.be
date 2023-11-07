using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Interfaces.IServices.Calender;
using AbsenDulu.BE.Models.Calender;
using AbsenDulu.BE.Models.CalenderDay;

namespace AbsenDulu.BE.Services.Calender;
public class GenerateCalenderServices : IGenerateCalenderServices
{

    private readonly DataContext _context;
    public GenerateCalenderServices(DataContext dataContext)
    {
        _context = dataContext;
    }
    public List<CalenderDay> GenerateCalendar(int year, List<Holiday> holidays)
    {
        List<CalenderDay> calendar = new List<CalenderDay>();
        var existingCalendar = _context.calenders.Where(c => c.Year == year);
        _context.calenders.RemoveRange(existingCalendar);
        _context.SaveChanges();

        // Iterasi melalui semua hari dalam tahun berjalan
        for (int month = 1; month <= 12; month++)
        {
            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                DateTime currentDate = new DateTime(year, month, day);
                bool isSunday = currentDate.DayOfWeek == DayOfWeek.Sunday;
                Holiday holiday = holidays.FirstOrDefault(h => h.Date.Date == currentDate.Date);

                CalenderDay Calender = new CalenderDay
                {
                    Id = Guid.NewGuid(),
                    Day = currentDate.ToString("dddd"),
                    Date = currentDate.Day,
                    Month = currentDate.Month,
                    Year = currentDate.Year,
                    IsSunday = isSunday,
                    CreatedAt = DateTime.UtcNow,
                    HolidayName = holiday?.Name,
                    IsHoliday = holiday != null
                };
                _context.calenders.Add(Calender);
                calendar.Add(Calender);
            }
        }

        _context.SaveChanges();

        return calendar;
    }


    public List<CalenderDay> GetCalendar(int year, int month)
    {
        try
        {
            var data = _context.calenders.Where(d => d.Year == year && d.Month == month)
            .OrderBy(x => x.Date).ToList();
            int daysBeforeSunday = DaysBeforeSundayInMonth(year, month);

            if (daysBeforeSunday > 0)
            {
                var data2 = _context.calenders
                    .Where(d => d.Year == year && d.Month == month - 1)
                    .OrderByDescending(d => d.Date) // Urutkan data2 secara menaik berdasarkan tanggal
                    .Take(daysBeforeSunday)
                    .ToList();
                data2 = data2.OrderBy(x=>x.Date).ToList();

                var mergedData = data2.Concat(data).ToList();

                // Urutkan hasil penggabungan berdasarkan bulan
                var finalResult = mergedData.OrderBy(x => x.Month).ToList();

                return finalResult;
            }

            return data.OrderBy(x => x.Date).ToList();
        }
        catch (Exception)
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
        DateTime firstDayOfMonth = new DateTime(year, month,Convert.ToInt32(endOfMonth));
        int daysBeforeSunday = 0;

        while (firstDayOfMonth.DayOfWeek != DayOfWeek.Sunday)
        {
            firstDayOfMonth = firstDayOfMonth.AddDays(-1);
            daysBeforeSunday++;
        }

        return daysBeforeSunday;
    }
}