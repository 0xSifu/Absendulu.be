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
public class DepartmentController : ControllerBase
{
    private IHttpContextAccessor _httpContext;
    private IDepartmentService _service;
    private TokenValidate _token;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public DepartmentController(IWebHostEnvironment webHostEnvironment, TokenValidate tokenConfiguration, IHttpContextAccessor httpContext, IDepartmentService service)
    {
        _httpContext = httpContext;
        _service = service;
        _token=tokenConfiguration;
        _webHostEnvironment=webHostEnvironment;
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/export")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Excel(List<MasterDepartment> request)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Department");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Department Code";
            worksheet.Cell(currentRow, 2).Value = "Department Name";
            foreach (var area in request)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = area.DepartmentCode;
                worksheet.Cell(currentRow, 2).Value = area.DepartmentName;
            }
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Department_Report.xlsx");
            }
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPagination")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterDepartment>>> GetArea([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = string.IsNullOrEmpty(filter.search) ?   await Task.Run(() => _service.GetDepartment(_token.CompanyId)) : await Task.Run(() => _service.FindByContains(filter.search,_token.CompanyId));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<MasterDepartment>>(paging, validFilter.PageNumber, validFilter.PageSize,totalPages,totalRecords,"suceess",true,null));
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetDepartment")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterDepartment>>> GetDepartment()
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetDepartment(_token.CompanyId));
            return Ok(new ResponseMessage<List<MasterDepartment>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }
    
    [HttpPost]
    [Authorize]
    [Route("[controller]/AddDepartment")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult> AddDepartment(MasterDepartmentDTO request)
    {
        try
        {
            request.CompanyId = _token.CompanyId;
            request.Company = _token.CompanyName;
            request.CreatedBy= _token.UserName;
            var responseContext = await Task.Run(() => _service.AddDepartment(request));
            return Created("",new ResponseMessage<MasterDepartment> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }
    
    [HttpPut("[controller]/{id}")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Put(string id, [FromBody] MasterDepartmentDTO request)
    {
        try
        {
            request.Id=new Guid(id);
            request.UpdatedBy=_token.UserName;
            var responseContext = _service.UpdateDepartment(request);

            return Ok(new ResponseMessage<MasterDepartment> { IsError = false, Message = "Success", Data = responseContext });
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
            var responseContext = _service.RemoveDepartment(id);
            MasterDepartment data = new MasterDepartment();
            return Ok(new ResponseMessage<MasterDepartment> { IsError = false, Message = "Success Delete Department Id : " + id, Data = data });
        }
        catch
        {
            throw;
        }
    }
}