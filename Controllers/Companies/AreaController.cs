using System.Net;
using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.DTO.LogError;
using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Interfaces.IServices.LogError;
using AbsenDulu.BE.Models.Company;
using AbsenDulu.BE.Response;
using AbsenDulu.BE.Token.Helper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsenDulu.BE.Controllers;

[ApiController]
public class AreaController : ControllerBase
{
    private IHttpContextAccessor _httpContext;
    private IAreaService _service;
    private readonly TokenValidate _tokenConfig;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogErrorService _logError;

    public AreaController(ILogErrorService logError, IWebHostEnvironment webHostEnvironment, TokenValidate tokenConfig, IHttpContextAccessor httpContext, IAreaService service)
    {
        _httpContext = httpContext;
        _service = service;
        _tokenConfig = tokenConfig;
        _webHostEnvironment = webHostEnvironment;
        _logError = logError;
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/export")]
    public  IActionResult Excel(List<MasterAreaDTO> request)
    {
        try
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Area");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Area Code";
                worksheet.Cell(currentRow, 2).Value = "Area Name";
                foreach (var area in request)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = area.AreaCode;
                    worksheet.Cell(currentRow, 2).Value = area.AreaName;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Area_Report.xlsx");
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
    [Route("[controller]/GetPagination")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterArea>>> GetArea([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = string.IsNullOrEmpty(filter.search) ? await Task.Run(() => _service.GetArea(_tokenConfig.CompanyId)) : await Task.Run(() => _service.FindByContains(filter.search, _tokenConfig.CompanyId));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<MasterArea>>(paging, validFilter.PageNumber, validFilter.PageSize, totalPages, totalRecords, "suceess", true, null));
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetArea")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterArea>>> GetArea()
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetArea(_tokenConfig.CompanyId));
            return Ok(new ResponseMessage<List<MasterArea>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/AddArea")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult> AddArea(MasterAreaDTO request)
    {
        try
        {
            request.CompanyId = _tokenConfig.CompanyId;
            request.Company = _tokenConfig.CompanyName;
            request.CreatedBy = _tokenConfig.UserName;
            var responseContext = await Task.Run(() => _service.AddArea(request));
            return Created("", new ResponseMessage<MasterArea> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("[controller]/{id}")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Put(string id, [FromBody] MasterAreaDTO request)
    {
        try
        {
            // var token = _httpContext?.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            request.Id = id;
            request.UpdatedBy = _tokenConfig.UserName;
            var responseContext = _service.UpdateArea(request);

            return Ok(new ResponseMessage<MasterArea> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch (Exception exception)
        {
            logAbsenDuluDTO logMessage = new logAbsenDuluDTO
            {
                Service = "Put",
                Severity = "Error",
                ClientName = _tokenConfig.UserName,
                ErrorMessage = exception.Message,
                Method = ControllerContext.RouteData.Values["controller"]?.ToString(),
                Payload = request.ToString(),
                StatusCode = "400",
                IpAddress = Dns.GetHostName(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };
            _logError.AddLog(logMessage);
            return BadRequest(exception.Message);
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
            var responseContext = _service.RemoveArea(id);

            MasterArea data = new MasterArea();
            return Ok(new ResponseMessage<MasterArea> { IsError = false, Message = "Success Delete Area Id : " + id, Data = data });
        }
        catch
        {
            throw;
        }
    }
}