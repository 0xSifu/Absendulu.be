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
public class WorkshopController : ControllerBase
{
    private IWorkshopService _service;
    private TokenValidate _token;
    public WorkshopController(TokenValidate tokenValidate, IWorkshopService service)
    {
        _service = service;
        _token =tokenValidate;
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/export")]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Excel(List<MasterWorkshop> request)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Workshop");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Workshop Code";
            worksheet.Cell(currentRow, 2).Value = "Workshop Name";
            worksheet.Cell(currentRow, 3).Value = "Description";
            foreach (var Workshop in request)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = Workshop.WorkshopCode;
                worksheet.Cell(currentRow, 2).Value = Workshop.WorkshopName;
                worksheet.Cell(currentRow, 3).Value = Workshop.Description;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Workshop.xlsx");
            }
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPagination")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterWorkshop>>> GetArea([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = string.IsNullOrEmpty(filter.search) ?   await Task.Run(() => _service.GetWorkshop(_token.CompanyId)) : await Task.Run(() => _service.FindByContains(filter.search,_token.CompanyId));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<MasterWorkshop>>(paging, validFilter.PageNumber, validFilter.PageSize,totalPages,totalRecords,"suceess",true,null));
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetWorkshop")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterWorkshop>>> GetWorkshop()
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetWorkshop(_token.CompanyId));
            return Ok(new ResponseMessage<List<MasterWorkshop>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("[controller]/AddWorkshop")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult> AddWorkshop(MasterWorkshopDTO request)
    {
        try
        {
            request.CompanyId = _token.CompanyId;
            request.CreatedBy=_token.UserName;
            var responseContext = await Task.Run(() => _service.AddWorkshop(request));
            return Created("",new ResponseMessage<MasterWorkshop> { IsError = false, Message = "Success", Data = responseContext });
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
    public IActionResult Put(string id, [FromBody] MasterWorkshopDTO request)
    {
        try
        {
            request.Id=id;
            request.UpdatedBy=_token.UserName;
            var responseContext = _service.UpdateWorkshop(request);
            return Ok(new ResponseMessage<MasterWorkshop> { IsError = false, Message = "Success", Data = responseContext });
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
    public IActionResult Delete(string id)
    {
        try
        {
            var responseContext = _service.RemoveWorkshop(id);
            MasterWorkshop data = new MasterWorkshop();
            return Ok(new ResponseMessage<MasterWorkshop> { IsError = false, Message = "Success Delete Workshop  Id : " + id, Data = data });
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }


}