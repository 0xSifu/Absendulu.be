using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Interfaces.IServices.Schedules;
using AbsenDulu.BE.Models.Schedules;
using AbsenDulu.BE.Response;
using AbsenDulu.BE.Token.Helper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsenDulu.BE.Controllers.Schedules;
[ApiController]
public class ScheduleController : ControllerBase
{
    private TokenValidate _token;
    private readonly IScheduleService _service;
    private readonly IEmployeeScheduleService _serviceEmployeeSchedule;
    private readonly IEmployeeService _employeeService;
    public ScheduleController(IEmployeeService employeeService, TokenValidate tokenValidate, IScheduleService scheduleService, IEmployeeScheduleService employeeScheduleService)
    {
        _service=scheduleService;
        _token=tokenValidate;
        _serviceEmployeeSchedule=employeeScheduleService;
        _employeeService=employeeService;
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/export")]
    public IActionResult Excel(List<Schedule> request)
    {
        try
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ScheduleEmployees");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Employee Name";
                worksheet.Cell(currentRow, 2).Value = "Department";
                worksheet.Cell(currentRow, 3).Value = "Position";
                worksheet.Cell(currentRow, 4).Value = "Shift Name";
                worksheet.Cell(currentRow, 5).Value = "Clock In";
                worksheet.Cell(currentRow, 6).Value = "Clock Out";
                worksheet.Cell(currentRow, 7).Value = "Date";
                worksheet.Cell(currentRow, 8).Value = "Work Status";
                foreach (var schedule in request)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = schedule.EmployeName;
                    worksheet.Cell(currentRow, 2).Value = schedule.DepartmentName;
                    worksheet.Cell(currentRow, 3).Value = schedule.PositionName;
                    worksheet.Cell(currentRow, 4).Value = schedule.ShiftName;
                    worksheet.Cell(currentRow, 5).Value = schedule.ClockIn;
                    worksheet.Cell(currentRow, 6).Value = schedule.ClockOut;
                    worksheet.Cell(currentRow, 7).Value = schedule.date.ToString("dd-MM-yyyy");
                    worksheet.Cell(currentRow, 8).Value = schedule.AttendanceStatus;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Schedule_Employe{request.FirstOrDefault().date.ToString("dd-MM-yyyy")}.xlsx");
                }
            }
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetSchedule")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<Schedule>>> GetSchedule([FromQuery] DateRangeFilter daterange)
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetSchedules(_token.CompanyId,daterange.StartDate, daterange.ToDate));
            return Ok(new ResponseMessage<List<Schedule>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPagination")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<Schedule>>> GetPagination([FromQuery] DateRangeFilter daterange, [FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = string.IsNullOrEmpty(filter.search) ? await Task.Run(() => _service.GetSchedules(_token.CompanyId,daterange.StartDate, daterange.ToDate)) : await Task.Run(() => _service.FindByContains(filter.search, _token.CompanyId,daterange.StartDate, daterange.ToDate));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<Schedule>>(paging, validFilter.PageNumber, validFilter.PageSize, totalPages, totalRecords, "suceess", true, null));
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetEmployeeSchedule")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public ActionResult<List<EmployeeSchedule>> GetEmployeeSchedule([FromQuery] int year, int month)
    {
        var employee = _employeeService.GetEmployeeById(_token.CompanyId,Convert.ToInt32(_token.EmployeeId));
        var data = _serviceEmployeeSchedule.GetEmployeeSchedules(_token.CompanyId, year, month,employee.FirstOrDefault().EmployeeName);
        return Ok(new ResponseMessage<List<EmployeeSchedule>> { IsError = false, Message = "Success", Data = data });
    }
}