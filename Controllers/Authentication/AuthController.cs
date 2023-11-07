using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.DTO;
using AbsenDulu.BE.DTO.Login;
using AbsenDulu.BE.DTO.User;
using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Interfaces.IServices.Activation;
using AbsenDulu.BE.Interfaces.IServices.Attendances;
using AbsenDulu.BE.Interfaces.IServices.Authentication;
using AbsenDulu.BE.Interfaces.IServices.DetailSubcribeService;
using AbsenDulu.BE.Interfaces.IServices.Email;
using AbsenDulu.BE.Interfaces.IServices.LogError;
using AbsenDulu.BE.Interfaces.IServices.MenuAccess;
using AbsenDulu.BE.Interfaces.IServices.ResetPassword;
using AbsenDulu.BE.Token.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsenDulu.BE.Controllers.Authentication;
[ApiController]
public class AuthController:ControllerBase
{
    private IHttpContextAccessor _httpContext;
    private IIdentityService _serviceAuth;
    private IUserServices _serviceUser;
    private readonly DataContext _context;
    private TokenValidate _token;
    private IDetailSubcribeService _serviceSubcribe;
    private readonly IEmailService _email;
    private readonly IUserActivationService _activation;
    private readonly ILogErrorService _logError;
    private IEmployeeService _serviceEmployee;
    private TokenConfiguration _tokenConfig;
    private IAttendanceService _serviceAttendance;
    private readonly IUserResetPasswordService _userResetPassword;

    public AuthController(IUserResetPasswordService userResetPasswordService, IAttendanceService attendanceService, TokenConfiguration tokenConfiguration, IEmployeeService employeeService, ILogErrorService logError, IUserActivationService userActivationService, IEmailService email, IDetailSubcribeService detailSubcribeService, TokenValidate tokenValidate, IHttpContextAccessor httpContext ,IIdentityService identityService,DataContext dataContext,IUserServices userServices )
    {
        _httpContext=httpContext;
        _serviceAuth=identityService;
        _serviceUser=userServices;
        _context=dataContext;
        _token=tokenValidate;
        _serviceSubcribe=detailSubcribeService;
        _email=email;
        _activation=userActivationService;
        _logError=logError;
        _serviceEmployee=employeeService;
        _tokenConfig = tokenConfiguration;
        _serviceAttendance = attendanceService;
        _userResetPassword=userResetPasswordService;
    }

    [HttpPost]
    [Authorize]
    [Route("users/register")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    public async Task<ActionResult> Register(UserRegisterDTO request)
    {
        try
        {
            var employee = _serviceEmployee.GetEmployeeUsername(request.UserName,_token.CompanyId);
            if(employee.Count < 1)
            {
                return NotFound("Employee Not Found, Please Add Empolyee");
            }
            var totalused = await Task.Run(() => _serviceSubcribe.GetDetailsSubcribes(_token.CompanyId));
            request.EmployeeId=employee.First().Id;
            request.CompanyId=_token.CompanyId;
            request.CompanyName=_token.CompanyName;
            request.CreatedBy=_token.UserName;
            if(request.AccountTypeId==1)
            {
                if(totalused.Count < 1)
                {
                    return NotFound("Company Not Found");
                }
                if(totalused.FirstOrDefault().RemainingApps < 1)
                {
                    return BadRequest("Device Apps remaining 0");
                }
                var data = await Task.Run(() => _serviceSubcribe.UpdateAppsUsed(totalused.FirstOrDefault().Id.ToString()));
                if(data == null)
                {
                    return BadRequest("Error Register");
                }
            }
            var responseContext = await Task.Run(() => _serviceUser.Register(request));
            await  Task.Run(() => _email.SendEmailAsync(request, "Registration", responseContext.Id));
            return Created("","Success Created "+ responseContext.UserName);

        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("[controller]/Login")]
    public async Task<ActionResult> Login(UserLoginDTO request)
    {
        try
        {
            var responseContext = await Task.Run(() => _serviceAuth.login(request));
            return Ok(responseContext);
        }

        catch
        {
            throw;
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("[controller]/Logout")]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult> Logout()
    {
        try
        {
            var responseContext = await Task.Run(() => _serviceAuth.logout());
            return Ok(responseContext);
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("[controller]/active/{Username}/{ActivationCode}")]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult> activation(string Username, string ActivationCode)
    {
        try
        {
            await Task.Run(() => _activation.UpdateUserActive(ActivationCode));
            await Task.Run(() => _serviceAuth.UpdateActivation(Username));
            return Ok($"Username {Username} Succes Activated Please Login");
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("[controller]/ResetPassword")]
    public async Task<ActionResult> ResetPassword(RequestChangePasswordDTO request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.email))
            {
                return BadRequest("The email field is required.");
            }
            // var res = await Task.Run(() => _userRepository.ResetPassword(email));
            var data = await Task.Run(() => _userResetPassword.GetData(request.email));
            if (data != null)
            {
                await _userResetPassword.SaveResetPassword(data);
                await _email.SendEmaiResetPassword(data, request.email, "Reset Password");
                return Ok($"link Reset Password send, Please Check your email {request.email} then click the link");
            }
            return BadRequest($"Reset Password Failed");
        }
        catch (System.Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
    [HttpPost]
    [AllowAnonymous]
    [Route("[controller]/ResetPasswordPIN")]
    public async Task<ActionResult> ResetPasswordPIN(ResetPasswordPIN request)
    {
        try
        {
            var data = await Task.Run(() => _userResetPassword.ValidatePIN(request));
            if(data!=null)
            {
                await Task.Run(() => _serviceUser.UpdatePassword(data, request.Password));
            }
            // await Task.Run(() => _userRepository.UpdatePassword(data,request.Password));
            return Ok(data);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("[controller]/ChangePassword")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    public async Task<ActionResult> ChangePassword(string Password )
    {
        try
        {
            await Task.Run(() => _serviceUser.UpdatePassword(new Guid(_token.Id), Password));
            return Ok($"Change Password Success");
        }
        catch
        {
            throw ;
        }
    }
}