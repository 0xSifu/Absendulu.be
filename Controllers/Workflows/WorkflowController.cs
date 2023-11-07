using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices.Workflows;
using AbsenDulu.BE.Models.Workflow;
using AbsenDulu.BE.Token.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AbsenDulu.BE.Response;

namespace AbsenDulu.BE.Controllers.Workflows;
[ApiController]
public class WorkflowController: ControllerBase
{
    private readonly IWorkflowsService _service;
    private readonly TokenValidate _tokenConfig;
    public WorkflowController(IWorkflowsService workflowsService, TokenValidate tokenValidate)
    {
        _tokenConfig=tokenValidate;
        _service=workflowsService;
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPagination")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]

    public async Task<ActionResult<List<Workflow>>> GetPagination([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = await Task.Run(() => _service.GetWorkflows(_tokenConfig.CompanyId));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<Workflow>>(paging, validFilter.PageNumber, validFilter.PageSize, totalPages, totalRecords, "suceess", true, null));
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetWorkflowByDepartment")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<Workflow>>> GetWorkflowByDepartment([FromQuery] string DepartmentCode)
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetWorkflowsByDepartment(_tokenConfig.CompanyId, DepartmentCode));
            return Ok(new ResponseMessage<List<Workflow>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetWorkflowByPosition")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<Workflow>>> GetWorkflowByPosition([FromQuery] string PositionCode)
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetWorkflowsByPosition(_tokenConfig.CompanyId, PositionCode));
            return Ok(new ResponseMessage<List<Workflow>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/AddWorkflow")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult> AddWorkflow(Workflow request)
    {
        try
        {
            request.CompanyId = _tokenConfig.CompanyId;
            request.CreatedBy = _tokenConfig.UserName;
            var responseContext = await Task.Run(() => _service.AddWorkflow(request));
            return Created("", new ResponseMessage<Workflow> { IsError = false, Message = "Success", Data = responseContext });
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
    public IActionResult Put(Guid id, [FromBody] Workflow request)
    {
        try
        {
            request.Id = id;
            request.UpdatedBy = _tokenConfig.UserName;
            var responseContext = _service.UpdateWorkflow(request);
            return Ok(new ResponseMessage<Workflow> { IsError = false, Message = "Success", Data = responseContext });
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
    public IActionResult Delete(Guid id)
    {
        try
        {
            var responseContext = _service.RemoveWorkflow(id);
            Workflow data = new Workflow();
            return Ok(new ResponseMessage<Workflow> { IsError = false, Message = "Success Delete Workflow Id : " + id, Data = data});
        }
        catch
        {
            throw;
        }
    }
}