using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Models.Company;
using AbsenDulu.BE.Response;
using AbsenDulu.BE.Token.Helper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsenDulu.BE.Controllers;

[ApiController]
public class LeaveController : ControllerBase
{
    private ILeaveService _service;
    private TokenValidate _token;
    public LeaveController(TokenValidate tokenValidate, ILeaveService service)
    {
        _service = service;
        _token = tokenValidate;
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/export")]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Excel(List<MasterLeave> request)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Leave");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Leave Code";
            worksheet.Cell(currentRow, 2).Value = "Leave Name";
            worksheet.Cell(currentRow, 3).Value = "Total Days";
            worksheet.Cell(currentRow, 4).Value = "Leave More Than Balance";
            foreach (var Leave in request)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = Leave.LeaveCode;
                worksheet.Cell(currentRow, 2).Value = Leave.LeaveName;
                worksheet.Cell(currentRow, 3).Value = Leave.TotalDays;
                worksheet.Cell(currentRow, 4).Value = Leave.LeaveMoreThanBalance.ToString();
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Leave_Report.xlsx");
            }
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPagination")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterLeave>>> GetArea([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = string.IsNullOrEmpty(filter.search) ?   await Task.Run(() => _service.GetLeave(_token.CompanyId)) : await Task.Run(() => _service.FindByContains(filter.search,_token.CompanyId));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<MasterLeave>>(paging, validFilter.PageNumber, validFilter.PageSize,totalPages,totalRecords,"suceess",true,null));
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetLeave")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterLeave>>> GetLeave()
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetLeave(_token.CompanyId));
            return Ok(new ResponseMessage<List<MasterLeave>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/AddLeave")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult> AddLeave(MasterLeaveDTO request)
    {
        try
        {
            request.CompanyId =_token.CompanyId;
            request.Company = _token.CompanyName;
            request.CreatedBy=_token.UserName;
            var responseContext = await Task.Run(() => _service.AddLeave(request));
            return Created("",new ResponseMessage<MasterLeave> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }
    
    [HttpPut("[controller]/{id}")]
    [Authorize]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Put(string id, [FromBody] MasterLeaveDTO request)
    {
        try
        {
            request.Id=id;
            request.UpdatedBy=_token.UserName;
            var responseContext = _service.UpdateLeave(request);
            return Ok(new ResponseMessage<MasterLeave> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpDelete("[controller]/{id}")]
    [Authorize]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Delete(string id)
    {
        try
        {
            var responseContext = _service.RemoveLeave(id);
            MasterLeave data = new MasterLeave();
            return Ok(new ResponseMessage<MasterLeave> { IsError = false, Message = "Success Delete Leave Id : " + id, Data = data });
        }
        catch
        {
            throw;
        }
    }


}