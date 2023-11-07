using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices.CompanyData;
using AbsenDulu.BE.Models.Company;
using AbsenDulu.BE.Response;
using AbsenDulu.BE.Token.Helper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsenDulu.BE.Controllers;
[ApiController]
public class ShiftController : ControllerBase
{
    private IShiftService _service;
    private readonly TokenValidate _token;

    public ShiftController(IShiftService service,TokenValidate tokenValidate)
    {
        _service = service;
        _token=tokenValidate;
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/export")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Excel(List<MasterShift> request)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Shift");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Shift Code";
            worksheet.Cell(currentRow, 2).Value = "Shift Name";
            worksheet.Cell(currentRow, 3).Value = "Start Of Work Time";
            worksheet.Cell(currentRow, 4).Value = "End Of Work Time";
            worksheet.Cell(currentRow, 5).Value = "Start Of Break Time";
            worksheet.Cell(currentRow, 6).Value = "End Of Work Time";
            worksheet.Cell(currentRow, 7).Value = "Work Days";
            foreach (var Shift in request)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = Shift.ShiftCode;
                worksheet.Cell(currentRow, 2).Value = Shift.ShiftName;
                worksheet.Cell(currentRow, 3).Value = Shift.StartWorkTime;
                worksheet.Cell(currentRow, 4).Value = Shift.EndWorkTime;
                worksheet.Cell(currentRow, 5).Value = Shift.StartBreakTime;
                worksheet.Cell(currentRow, 6).Value = Shift.EndBreakTime;

                string workDaysString = string.Join(" , ", Shift.WorkDays);
                worksheet.Cell(currentRow, 7).Value = workDaysString;

            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Shift_Report.xlsx");
            }
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPagination")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterShift>>> GetArea([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = string.IsNullOrEmpty(filter.search) ?   await Task.Run(() => _service.GetShift(_token.CompanyId)) : await Task.Run(() => _service.FindByContains(filter.search,_token.CompanyId));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<MasterShift>>(paging, validFilter.PageNumber, validFilter.PageSize,totalPages,totalRecords,"suceess",true,null));
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetShift")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterShift>>> GetPosition()
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetShift(_token.CompanyId));
            return Ok(new ResponseMessage<List<MasterShift>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/AddShift")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult> AddPosition(MasterShiftDTO request)
    {
        try
        {
            request.CompanyId=_token.CompanyId;
            request.Company = _token.CompanyName;
            request.CreatedBy=_token.UserName;
            var responseContext = await Task.Run(() => _service.AddShift(request));
            return Created("",new ResponseMessage<MasterShift> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [Authorize]
    [HttpPut("[controller]/{id}")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Put(Guid id, [FromBody] MasterShiftDTO request)
    {
        try
        {
            request.Id=id;
            request.UpdatedBy=_token.UserName;
            var responseContext = _service.UpdateShift(request);
            return Ok(new ResponseMessage<MasterShift> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [Authorize]
    [HttpDelete("[controller]/{id}")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Delete(Guid id)
    {
        try
        {
            var responseContext = _service.RemoveShift(id);
            MasterShift data = new MasterShift();
            return Ok(new ResponseMessage<MasterShift> { IsError = false, Message = "Success Delete Shift Id : " + id, Data = data });
        }
        catch
        {
            throw;
        }
    }

}