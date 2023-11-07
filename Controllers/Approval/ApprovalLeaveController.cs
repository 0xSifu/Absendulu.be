using AbsenDulu.BE.DTO.Approval;
using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices.Approval;
using AbsenDulu.BE.Models.Approval;
using AbsenDulu.BE.Response;
using AbsenDulu.BE.Token.Helper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsenDulu.BE.Controllers.Approval;
[ApiController]
public class ApprovalLeaveController : ControllerBase
{
    private readonly IApprovalLeaveService _service;
    private readonly TokenValidate _tokenConfig;
    private readonly IApprovalLogService _approvalLogs;
    private readonly IApprovalRemainingService _remainingService;
    public ApprovalLeaveController(IApprovalRemainingService approvalRemainingService, IApprovalLogService approvalLogService, IApprovalLeaveService approvalService, TokenValidate tokenValidate)
    {
        _service=approvalService;
        _tokenConfig=tokenValidate;
        _approvalLogs=approvalLogService;
        _remainingService=approvalRemainingService;
    }

    // [HttpPost]
    // [Authorize]
    // [Route("[controller]/export")]
    // public IActionResult Excel(List<ApprovalsLogs> request)
    // {
    //     try
    //     {
    //         using (var workbook = new XLWorkbook())
    //         {
    //             var worksheet = workbook.Worksheets.Add("Approval Logs");
    //             var currentRow = 1;
    //             worksheet.Cell(currentRow, 1).Value = "Area Code";
    //             worksheet.Cell(currentRow, 2).Value = "Area Name";
    //             worksheet.Cell(currentRow, 1).Value = "Area Code";
    //             worksheet.Cell(currentRow, 2).Value = "Area Name";
    //             foreach (var area in request)
    //             {
    //                 currentRow++;
    //                 worksheet.Cell(currentRow, 1).Value = area.AreaCode;
    //                 worksheet.Cell(currentRow, 2).Value = area.AreaName;
    //             }

    //             using (var stream = new MemoryStream())
    //             {
    //                 workbook.SaveAs(stream);
    //                 var content = stream.ToArray();

    //                 return File(
    //                     content,
    //                     "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    //                     "Area_Report.xlsx");
    //             }
    //         }
    //     }
    //     catch (Exception exception)
    //     {
    //         return BadRequest(exception.Message);
    //     }
    // }//

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetApprovalDetails")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<ApprovalLeaveLogs>>> GetApprovalDetails([FromQuery] Guid id)
    {
        try
        {
            var responseContext = await Task.Run(() => _approvalLogs.GetApprovalsLogByIdApproval(_tokenConfig.CompanyId,id));
            return Ok(new ResponseMessage<List<ApprovalLeaveLogs>> { IsError = false, Message = "Success", Data = responseContext });
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

    public async Task<ActionResult<List<ApprovalLeaves>>> GetPagination([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = await Task.Run(() => _service.GetApprovals());
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<ApprovalLeaves>>(paging, validFilter.PageNumber, validFilter.PageSize, totalPages, totalRecords, "suceess", true, null));
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetRemaining")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<BalanceLeave>>> GetRemaining([FromQuery] string Leavename)
    {
        try
        {
            var responseContext = await Task.Run(() => _remainingService.GetRemaining(Leavename));
            return Ok(new ResponseMessage<BalanceLeave> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPaginationApprovalLogs")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    public async Task<ActionResult<List<ApprovalLeaveLogs>>> GetPaginationApprovalLogs([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = string.IsNullOrEmpty(filter.search) ? await Task.Run(() => _approvalLogs.GetApprovalsLog(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId))) : await Task.Run(() => _approvalLogs.FindByContains(filter.search, _tokenConfig.CompanyId));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<ApprovalLeaveLogs>>(paging, validFilter.PageNumber, validFilter.PageSize, totalPages, totalRecords, "suceess", true, null));
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPaginationApprover")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]

    public async Task<ActionResult<List<ApprovalLeaves>>> GetPaginationApprover([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = await Task.Run(() => _service.GetCurrentApprover());
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<ApprovalLeaves>>(paging, validFilter.PageNumber, validFilter.PageSize, totalPages, totalRecords, "suceess", true, null));
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/AddApproval")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    public async Task<ActionResult> AddApproval(RequestApprovalLeaveDTO request)
    {
        try
        {
            var responseContext = await Task.Run(() => _service.AddApproval(request));
            return Created("", new ResponseMessage<ApprovalLeaves> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("[controller]/{id}")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Put(Guid Id,[FromBody] ApprovalDTO request)
    {
        try
        {
            var responseContext = _service.UpdateApproval(Id, request);
            return Ok(new ResponseMessage<ApprovalLeaves> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPost("[controller]/ApproveAll")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult ApproveAll([FromQuery] string note)
    {
        try
        {
            var responseContext = _service.BulkApprove(note);
            return Ok(new ResponseMessage<List<ApprovalLeaves>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPost("[controller]/RejectAll")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult RejectAll([FromQuery] string note)
    {
        try
        {
            var responseContext = _service.BulkReject(note);
            return Ok(new ResponseMessage<List<ApprovalLeaves>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPost("[controller]/RejectSelectedId")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult RejectSelectedId([FromBody] BulkRejectedDTO request)
    {
        try
        {
            var responseContext = _service.BulkRejectSelected(request);
            return Ok(new ResponseMessage<List<ApprovalLeaves>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPost("[controller]/ApproveSelectedId")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult ApproveSelectedId([FromBody] BulkApprovedDTO request)
    {
        try
        {
            var responseContext = _service.BulkApproveSelected(request);
            return Ok(new ResponseMessage<List<ApprovalLeaves>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }
}