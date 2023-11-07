using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices.Notification;
using AbsenDulu.BE.Token.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AbsenDulu.BE.Models.Notification;
using AbsenDulu.BE.Response;

namespace AbsenDulu.BE.Controllers.Notifications;
[ApiController]
public class NotificationController : ControllerBase
{
    private TokenValidate _token;
    private readonly INotificationService _service;
    public NotificationController(TokenValidate tokenValidate, INotificationService notificationService)
    {
        _token=tokenValidate;
        _service=notificationService;
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetNotification")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<Notif>>> GetAllNotification()
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetNotifications(_token.CompanyId));
            return Ok(new ResponseMessage<List<Notif>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetNotificationAdmin")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<Admin>>> GetAllNotificationAdmin()
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetNotificationsAdmin(_token.CompanyId));
            return Ok(new ResponseMessage<List<Admin>> { IsError = false, Message = "Success", Data = responseContext });
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
    public async Task<ActionResult<List<Notif>>> GetNotification([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = await Task.Run(() => _service.GetNotificationsByEmployeeId(new Guid(_token.Id), _token.CompanyId));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<Notif>>(paging, validFilter.PageNumber, validFilter.PageSize, totalPages, totalRecords, "suceess", true, null));
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPaginationAdmin")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<Admin>>> GetNotificationAdmin([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = await Task.Run(() => _service.GetNotificationsByAdminId(new Guid(_token.Id), _token.CompanyId));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<Admin>>(paging, validFilter.PageNumber, validFilter.PageSize, totalPages, totalRecords, "suceess", true, null));
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("[controller]/SetNotification/{id}")]
    [Authorize]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult SetNotification(Guid id)
    {
        try
        {
            var responseContext = _service.UpdateNotification(id);
            return Ok(new ResponseMessage<Notif> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("[controller]/SetNotificationAdmin/{id}")]
    [Authorize]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Admin(Guid id)
    {
        try
        {
            var responseContext = _service.UpdateAdminNotification(id);
            return Ok(new ResponseMessage<Admin> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }


}