using AbsenDulu.BE.Models.Attendances;
using AbsenDulu.BE.Models.Company;
using AbsenDulu.BE.Models.Dashboard;

namespace AbsenDulu.BE.Interfaces.IServices.Dashboard;
public interface IDashboardService
{
    ViewMobileDashboard GetMobileDashboard(Guid companyId,int employeeId);
    AttendanceDetail GetMobileDashboardMonth(Guid companyId,int employeeId,string Id);
    List<MasterCompany> GetCompanyData(Guid companyId);
    // ViewDetailSubscriptions GetDetailSubscriptions(Guid companyId);
    List<ViewDetailSubscriptions> GetDetailSubscriptions(Guid companyId);
    List<ViewAttendanceLate> GetAttendanceLate(Guid companyId);
    List<ViewAttendanceAbsent> GetAttendanceAbsent(Guid companyId);
    List<ViewAttendancePresent> GetAttendancePresent(Guid companyId);
    List<ViewAttendancelateByDepartment> GetAttendanceLateByDepartment(Guid companyId);
    List<ViewAttendancelateByEmployees> GetAttendanceLateByEmployee(Guid companyId);

}