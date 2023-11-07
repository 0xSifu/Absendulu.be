using AbsenDulu.BE.DTO.Attendances;
using AbsenDulu.BE.Models.Attendances;

namespace AbsenDulu.BE.Interfaces.IServices.Attendances;
public interface IAttendanceService
{
    List<AttendanceView> FindByContains(string request,Guid companyId);
    List<AttendanceView> FindByContainsAndDate(string request,Guid companyId, DateTime date);
    List<ViewMobileDashboard> FindByContainsAndDateMobile(string request,Guid companyId, DateTime date);
    List<AttendanceView> GetAttendances(Guid companyId);
    List<ViewMobileDashboard> GetAttendancesMobile(Guid companyId);
    Attendance AddAttendance(AttendanceDTO request);
    Attendance RequestAttendance(AttendanceDTO request);
}