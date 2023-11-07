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
public class PositionController : ControllerBase
{
    private IHttpContextAccessor _httpContext;
    private IPositionService _service;
    private TokenValidate _token;
    public PositionController(TokenValidate tokenValidate, IHttpContextAccessor httpContext, IPositionService service)
    {
        _httpContext = httpContext;
        _service = service;
        _token=tokenValidate;
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/export")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Excel(List<MasterPosition> request)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Position");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Department Name";
            worksheet.Cell(currentRow, 2).Value = "Position Code";
            worksheet.Cell(currentRow, 3).Value = "Position Name";
            foreach (var Position in request)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = Position.DepartmentName;
                worksheet.Cell(currentRow, 2).Value = Position.PositionCode;
                worksheet.Cell(currentRow, 3).Value = Position.PositionName;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Position_Report.xlsx");
            }
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPagination")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterPosition>>> GetArea([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = string.IsNullOrEmpty(filter.search) ?   await Task.Run(() => _service.GetPosition(_token.CompanyId)) : await Task.Run(() => _service.FindByContains(filter.search,_token.CompanyId));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<MasterPosition>>(paging, validFilter.PageNumber, validFilter.PageSize,totalPages,totalRecords,"suceess",true,null));
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPosition")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterPosition>>> GetPosition()
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetPosition(_token.CompanyId));
            return Ok(new ResponseMessage<List<MasterPosition>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPositionByDepartment/{department}")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterPosition>>> GetPositionByDepartment(Guid department)
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetPositionByDepartment(_token.CompanyId,department));
            return Ok(new ResponseMessage<List<MasterPosition>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/AddPosition")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult> AddPosition(MasterPositionDTO request)
    {
        try
        {
            request.CompanyId=_token.CompanyId;
            request.Company = _token.CompanyName;
            request.CreatedBy=_token.UserName;
            var responseContext = await Task.Run(() => _service.AddPosition(request));
            return Created("",new ResponseMessage<MasterPosition> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }
    [HttpPut("[controller]/{id}")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Put(string id, [FromBody] MasterPositionDTO request)
    {
        try
        {
            request.Id=new Guid(id);
            request.UpdatedBy=_token.UserName;
            var responseContext = _service.UpdatePosition(request);
            return Ok(new ResponseMessage<MasterPosition> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpDelete("[controller]/{id}")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Delete(string id)
    {
        try
        {
            var responseContext = _service.RemovePosition(id);
            MasterPosition data = new MasterPosition();
            return Ok(new ResponseMessage<MasterPosition> { IsError = false, Message = "Success Delete Position Id : " + id, Data = data });
        }
        catch
        {
            throw;
        }
    }


}