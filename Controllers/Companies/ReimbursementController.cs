using System.Net;
using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.DTO.LogError;
using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices.CompanyData;
using AbsenDulu.BE.Interfaces.IServices.LogError;
using AbsenDulu.BE.Models.Company;
using AbsenDulu.BE.Response;
using AbsenDulu.BE.Token.Helper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsenDulu.BE.Controllers;

[ApiController]
public class ReimbursementController : ControllerBase
{
    private IHttpContextAccessor _httpContext;
    private IReimbursementService _service;
    private readonly TokenValidate _tokenConfig;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogErrorService _logError;

    public ReimbursementController(ILogErrorService logError, IWebHostEnvironment webHostEnvironment, TokenValidate tokenConfig, IHttpContextAccessor httpContext, IReimbursementService service)
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
    [ApiExplorerSettings(GroupName = "v1")]
    public  IActionResult Excel(List<MasterReimbursementDTO> request)
    {
        try
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reimbursement");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Reimbursement Code";
                worksheet.Cell(currentRow, 2).Value = "Reimbursement Name";
                worksheet.Cell(currentRow, 3).Value = "Total Amount";
                foreach (var Reimbursement in request)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = Reimbursement.ReimbursementCode;
                    worksheet.Cell(currentRow, 2).Value = Reimbursement.ReimbursementName;
                    worksheet.Cell(currentRow, 3).Value = Reimbursement.TotalAmount;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Reimbursement.xlsx");
                }
            }
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
    public async Task<ActionResult<List<MasterReimbursements>>> GetReimbursement([FromQuery] PaginationFilter filter)
    {
        try
        {

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = string.IsNullOrEmpty(filter.search) ? await Task.Run(() => _service.GetReimbursement(_tokenConfig.CompanyId)) : await Task.Run(() => _service.FindByContains(filter.search, _tokenConfig.CompanyId));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<MasterReimbursements>>(paging, validFilter.PageNumber, validFilter.PageSize, totalPages, totalRecords, "suceess", true, null));
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/Reimbursement")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<MasterReimbursements>>> GetReimbursement()
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetReimbursement(_tokenConfig.CompanyId));
            return Ok(new ResponseMessage<List<MasterReimbursements>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/AddReimbursement")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult> AddReimbursement(MasterReimbursementDTO request)
    {
        try
        {
            request.CompanyId = _tokenConfig.CompanyId;
            request.Company = _tokenConfig.CompanyName;
            request.CreatedBy = _tokenConfig.UserName;
            var responseContext = await Task.Run(() => _service.AddReimbursement(request));
            return Created("", new ResponseMessage<MasterReimbursements> { IsError = false, Message = "Success", Data = responseContext });
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
    public IActionResult Put(string id, [FromBody] MasterReimbursementDTO request)
    {
        try
        {
            // var token = _httpContext?.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            request.Id = id;
            request.UpdatedBy = _tokenConfig.UserName;
            var responseContext = _service.UpdateReimbursement(request);

            return Ok(new ResponseMessage<MasterReimbursements> { IsError = false, Message = "Success", Data = responseContext });
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
            var responseContext = _service.RemoveReimbursement(id);
            MasterReimbursements data = new MasterReimbursements();
            return Ok(new ResponseMessage<MasterReimbursements> { IsError = false, Message = "Success Delete Reimburse Id : " + id, Data = data });
        }
        catch
        {
            throw;
        }
    }
}