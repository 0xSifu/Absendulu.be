using AbsenDulu.BE.Models.Schedules;

namespace AbsenDulu.BE.Interfaces.IServices.Schedules;
public interface IEmployeeScheduleService
{
    List<EmployeeSchedule> GetEmployeeSchedules(Guid companyId, int year, int month, string employee);
}