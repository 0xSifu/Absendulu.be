using AbsenDulu.BE.Models.Schedules;

namespace AbsenDulu.BE.Interfaces.IServices.Schedules;
public interface IScheduleService
{
    List<Schedule> GetSchedules(Guid companyId, string fromdate, string todate);
    List<Schedule> FindByContains(string request, Guid companyId, string fromdate, string todate);
}