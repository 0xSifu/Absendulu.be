using AbsenDulu.BE.Models.Calender;
using AbsenDulu.BE.Models.CalenderDay;

namespace AbsenDulu.BE.Interfaces.IServices.Calender;
public interface IGenerateCalenderServices
{
    List<CalenderDay> GenerateCalendar(int year, List<Holiday> holidays);
    List<CalenderDay> GetCalendar(int year, int month);
}