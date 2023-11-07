using AbsenDulu.BE.DTO.Attendances;
using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices.Attendances;
using AbsenDulu.BE.Models.Attendances;
using AbsenDulu.BE.Response;
using AbsenDulu.BE.Token.Helper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsenDulu.BE.Controllers;

[ApiController]
public class AttendancesController : ControllerBase
{
    private IAttendanceService _service;
    private readonly TokenValidate _tokenConfig;

    public AttendancesController(TokenValidate tokenValidate, IAttendanceService service)
    {
        _tokenConfig = tokenValidate;
        _service = service;
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/export")]
    public  IActionResult Excel(List<AttendanceView> request)
    {
        try
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Attendances");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Employee Name";
                worksheet.Cell(currentRow, 2).Value = "Department Name";
                worksheet.Cell(currentRow, 3).Value = "Position";
                worksheet.Cell(currentRow, 4).Value = "Date";
                worksheet.Cell(currentRow, 5).Value = "Clock In Time";
                worksheet.Cell(currentRow, 6).Value = "App/Device";
                worksheet.Cell(currentRow, 7).Value = "Clock In Address";
                worksheet.Cell(currentRow, 8).Value = "Clock In Note";
                worksheet.Cell(currentRow, 9).Value = "Clock Out Time";
                worksheet.Cell(currentRow, 10).Value = "App/Device";
                worksheet.Cell(currentRow, 11).Value = "Clock Out Address";
                worksheet.Cell(currentRow, 12).Value = "Clock Out Note";
                worksheet.Cell(currentRow, 13).Value = "Employee Shift";
                foreach (var attendances in request)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = attendances.EmployeeName;
                    worksheet.Cell(currentRow, 2).Value = attendances.DepartmentName;
                    worksheet.Cell(currentRow, 3).Value = attendances.PositionName;
                    worksheet.Cell(currentRow, 4).Value = attendances.Date;
                    worksheet.Cell(currentRow, 5).Value = attendances.ClockIn;
                    worksheet.Cell(currentRow, 6).Value = attendances.ClockInMethod;
                    worksheet.Cell(currentRow, 7).Value = attendances.ClockInAddress;
                    worksheet.Cell(currentRow, 8).Value = attendances.ClockInNote;
                    worksheet.Cell(currentRow, 9).Value = attendances.ClockOut;
                    worksheet.Cell(currentRow, 10).Value = attendances.ClockOutMethod;
                    worksheet.Cell(currentRow, 11).Value = attendances.ClockOutAddress;
                    worksheet.Cell(currentRow, 12).Value = attendances.ClockOutNote;
                    worksheet.Cell(currentRow, 13).Value = attendances.EmployeeShift;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Attendance_Report.xlsx");
                }
            }
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/AddAttendance")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    public async Task<ActionResult> AddAttendance(AttendanceDTO request)
    {
        try
        {
            var responseContext = await Task.Run(() => _service.AddAttendance(request));
            return Created("", new ResponseMessage<Attendance> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/RequestAttendance")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    public async Task<ActionResult> RequestAttendance(AttendanceDTO request)
    {
        try
        {
            var responseContext = await Task.Run(() => _service.RequestAttendance(request));
            return Created("", new ResponseMessage<Attendance> { IsError = false, Message = "Success", Data = responseContext });
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

    public async Task<ActionResult<List<AttendanceView>>> GetAttendance([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = await Task.Run(() => _service.FindByContainsAndDate(filter.search, _tokenConfig.CompanyId, filter.date));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<AttendanceView>>(paging, validFilter.PageNumber, validFilter.PageSize, totalPages, totalRecords, "suceess", true, null));
        }
        catch
        {
           throw;
        }
    }

}