using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices.DetailSubcribeService;
using AbsenDulu.BE.Models.Subcribes;
using AbsenDulu.BE.Response;
using AbsenDulu.BE.Token.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsenDulu.BE.Controllers.DetailSubcribes;
[ApiController]
public class SubcribeController : ControllerBase
{

    private IDetailSubcribeService _service;
    private readonly DataContext _context;
    private TokenValidate _token;

    public SubcribeController(TokenValidate tokenValidate, DataContext dataContext, IDetailSubcribeService service)
    {
        _service = service;
        _context = dataContext;
        _token=tokenValidate;
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetDetailSubcribe")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    public async Task<ActionResult<List<DetailSubcribe>>> GetDetailSubcribe()
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetDetailsSubcribes(_token.CompanyId));
            return Ok(new ResponseMessage<List<DetailSubcribe>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

}