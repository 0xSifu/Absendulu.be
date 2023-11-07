using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices.Attendances;
using AbsenDulu.BE.Interfaces.IServices.Dashboard;
using AbsenDulu.BE.Models.Attendances;
using AbsenDulu.BE.Models.Company;
using AbsenDulu.BE.Models.Dashboard;
using AbsenDulu.BE.Response;
using AbsenDulu.BE.Token.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsenDulu.BE.Controllers.Dashboard;
public class DashboardController : ControllerBase
{
    private readonly TokenValidate _tokenConfig;
    private IDashboardService _service;
    public DashboardController(IDashboardService dashboardService, TokenValidate tokenValidate)
    {
        _tokenConfig = tokenValidate;
        _service = dashboardService;
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetMobileDashboard")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<ViewMobileDashboard>>> GetMobileDashboard()
    {
        try
        {
            var AttendanceToday = await Task.Run(() => _service.GetMobileDashboard(_tokenConfig.CompanyId,Convert.ToInt32(_tokenConfig.EmployeeId)));
            var AttendanceMonth = await Task.Run(() => _service.GetMobileDashboardMonth(_tokenConfig.CompanyId,Convert.ToInt32(_tokenConfig.EmployeeId),_tokenConfig.Id));
            var CompanyData = await Task.Run(() => _service.GetCompanyData(_tokenConfig.CompanyId));
            return Ok(new DasboardMobileResponse<ViewMobileDashboard,AttendanceDetail,List<MasterCompany>> { IsError = false, Message = "Success", EmployeAttendanceToday = AttendanceToday,EmployeAttendanceMonth=AttendanceMonth,CompanyDetails=CompanyData });
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetWebDashboard")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<ViewMobileDashboard>>> GetwebDashboard()
    {
        try
        {
            if(_tokenConfig.Role.ToLower()!="admin")
            {
                var AttendanceToday = await Task.Run(() => _service.GetMobileDashboard(_tokenConfig.CompanyId,Convert.ToInt32(_tokenConfig.EmployeeId)));
                var AttendanceMonth = await Task.Run(() => _service.GetMobileDashboardMonth(_tokenConfig.CompanyId,Convert.ToInt32(_tokenConfig.EmployeeId), _tokenConfig.Id));
                var CompanyData = await Task.Run(() => _service.GetCompanyData(_tokenConfig.CompanyId));
                return Ok(new DasboardMobileResponse<ViewMobileDashboard, AttendanceDetail, List<MasterCompany>> { IsError = false, Message = "Success", EmployeAttendanceToday = AttendanceToday, EmployeAttendanceMonth = AttendanceMonth, CompanyDetails = CompanyData });
            }
            var DetailSubscription = await Task.Run(() => _service.GetDetailSubscriptions(_tokenConfig.CompanyId));
            var AttendanceLate = await Task.Run(() => _service.GetAttendanceLate(_tokenConfig.CompanyId));
            var AttendanceAbsent = await Task.Run(() => _service.GetAttendanceAbsent(_tokenConfig.CompanyId));
            var AttendancePresent = await Task.Run(() => _service.GetAttendancePresent(_tokenConfig.CompanyId));
            var AttendancelateByDepartment = await Task.Run(() => _service.GetAttendanceLateByDepartment(_tokenConfig.CompanyId));
            var AttendancelateByEmployee = await Task.Run(() => _service.GetAttendanceLateByEmployee(_tokenConfig.CompanyId));
            return Ok(new DasboardWebResponse<ViewDetailSubscriptions,List<ViewAttendanceLate>,List<ViewAttendanceAbsent>,List<ViewAttendancePresent>,List<ViewAttendancelateByDepartment>,List<ViewAttendancelateByEmployees>> { IsError = false, Message = "Success", DetailCompanies = DetailSubscription.FirstOrDefault(), AttendancesLate=AttendanceLate,AttendancesAbsent=AttendanceAbsent,AttendancesPresent=AttendancePresent,AttendancesLateByDepartment=AttendancelateByDepartment,AttendancesLateByEmployees=AttendancelateByEmployee });
        }
        catch
        {
            throw;
        }
    }
}