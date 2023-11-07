using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.DTO.Login;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Interfaces.IServices.Authentication;
using AbsenDulu.BE.Interfaces.IServices.CompanyData;
using AbsenDulu.BE.Interfaces.IServices.DetailSubcribeService;
using AbsenDulu.BE.Interfaces.IServices.MenuAccess;
using AbsenDulu.BE.Interfaces.IServices.PasswordHash;
using AbsenDulu.BE.Models.Identity;
using AbsenDulu.BE.Models.User;
using AbsenDulu.BE.Token.Helper;

namespace AbsenDulu.BE.Services.Authentication;
public class IdentityService : IIdentityService
{
    private readonly IVerifyPasswordHashService _verifyPassword;
    private readonly DataContext _context;
    private readonly TokenConfiguration _tokenConfiguration;
    private readonly TokenValidate _token;
    private readonly IHttpContextAccessor _httpContext;
    private readonly ICompanyService _service;
    private readonly IDetailSubcribeService _subcription;
    private readonly IAvailableAccessService _packages;
    private readonly IMenuAccessService _menuAccess;

    private readonly IEmployeeService _employee;

    public IdentityService(IEmployeeService employeeService, IMenuAccessService menuAccessService, TokenValidate tokenValidate, IAvailableAccessService availableAccessService, IDetailSubcribeService subcribeService, ICompanyService companyService, TokenConfiguration tokenConfiguration, IVerifyPasswordHashService verifyPasswordHash, DataContext context, IHttpContextAccessor httpContext)
    {
        _context = context;
        _verifyPassword = verifyPasswordHash;
        _tokenConfiguration = tokenConfiguration;
        _httpContext = httpContext;
        _service = companyService;
        _subcription = subcribeService;
        _packages = availableAccessService;
        _token = tokenValidate;
        _menuAccess = menuAccessService;
        _employee = employeeService;
    }
    public UserIdentityResponses login(UserLoginDTO user)
    {
        try
        {
            var dataAdmin = _context.admin_account.FirstOrDefault(d => d.UserName.Equals(user.username));

            if (dataAdmin != null)
            {
                CheckAdminStatus(dataAdmin);
                CheckValidation(dataAdmin.CompanyId, user.password, dataAdmin.UserName, dataAdmin.PasswordHash, dataAdmin.PasswordSalt, user.IsMobile, dataAdmin.AccountTypeid);
                var dataCompany2 = _service.GetCompanies(dataAdmin.CompanyId);
                var token2 = _tokenConfiguration.CreateToken(new UserAccount { UserName = dataAdmin.UserName, Role = dataAdmin.Role, CompanyId = dataAdmin.CompanyId, Id = dataAdmin.Id, CompanyName = dataAdmin.CompanyName }, dataCompany2.FirstOrDefault().Icon);
                // UpdateUserUsedStatus(data, false);
                var menu = _packages.GetAvailableAccess(dataAdmin.CompanyId);
                return new UserIdentityResponses { IsAuthSuccessfull = true, Token = token2, Access=menu.FirstOrDefault().available_menu };
            }

            var data = _context.user_account.FirstOrDefault(d => d.UserName.Equals(user.username));                                                                                                                   // }
            if (data == null)
            {
                throw new ValidationException($"User {user.username} Not Exists");
            }
            CheckUserStatus(data);
            CheckValidation(data.CompanyId,user.password,user.username,data.PasswordHash,data.PasswordSalt,user.IsMobile,data.AccountTypeid);
            var dataCompany = _service.GetCompanies(data.CompanyId);
            var token = _tokenConfiguration.CreateToken(new UserAccount { UserName = data.UserName, Role = data.Role, CompanyId = data.CompanyId, Id = data.Id, CompanyName = data.CompanyName, EmployeeId = data.EmployeeId }, dataCompany.FirstOrDefault().Icon);
            UpdateUserUsedStatus(data, false);
            var positionId = _employee.GetEmployeeById(data.CompanyId,data.EmployeeId);
            var menuAccess = _menuAccess.GetMenuAccess(data.CompanyId,positionId.FirstOrDefault().PositionId);
            return new UserIdentityResponses { IsAuthSuccessfull = true, Token = token,Access=menuAccess.FirstOrDefault().access };
        }
        catch
        {
            throw;
        }
    }

    public void CheckValidation(Guid CompanyId,string password ,string username , byte[]? PasswordHash, byte[]? PasswordSalt,bool ismobile, int? AccountTypeid)
    {

        var subbcription = _subcription.GetDetailsSubcribes(CompanyId);
        if (subbcription.FirstOrDefault().CompanyExpiredSubcribe < DateTime.UtcNow)
        {
            throw new ValidationException($"Your Company Expired Subscription");
        }
        if (!_verifyPassword.VerifyPassword(password, PasswordHash,PasswordSalt))
        {
            throw new ValidationException($"Wrong Password for Username {username}");
        }
        if (ismobile)
        {
            if (AccountTypeid == 0)
            {
                throw new ValidationException($"Username {username} not register on mobile account");
            }
        }
    }

    public UserIdentityResponses? logout()
    {
        try
        {
            var token = _httpContext?.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var TokenClaim = _tokenConfiguration.GetTokenClaim(token);
            if (TokenClaim.Role.ToLower() == "admin")
            {
                AdminLogout(TokenClaim);
            }
            UserLogout(TokenClaim);
            return new UserIdentityResponses { IsAuthSuccessfull = true, Token = null };
        }
        catch
        {
            throw;
        }
    }

    private void AdminLogout(TokenValidate token)
    {
        var user = _context.admin_account.First(x => x.UserName == token.UserName);
        user.IsUsed=false;
        _context.SaveChangesAsync();
        return;
    }
    private void UserLogout(TokenValidate token)
    {
        var user = _context.user_account.First(x => x.UserName == token.UserName);
        user.IsUsed = false;
        _context.SaveChangesAsync();
        return;
    }

    private void CheckUserStatus(UserAccount? user)
    {
        if (user.IsUsed)
        {
            throw new IdentityException($"User '{user.UserName}' is Used by IP : {user.IpAddressUserLogin} LastLogin {user.LastLogin} ");
        }
        if (user.IsLock)
        {
            throw new IdentityException($"User '{user.UserName}' is locked");
        }
        if (user?.IsActive == false)
        {
            throw new IdentityException($"User '{user.UserName}' not active Please Confirm Email");
        }
    }

    private void CheckAdminStatus(AdminAccount? user)
    {
        if (user.IsUsed)
        {
            throw new IdentityException($"User '{user.UserName}' is Used by IP : {user.IpAddressUserLogin} LastLogin {user.LastLogin} ");
        }
        if (user.IsLock)
        {
            throw new IdentityException($"User '{user.UserName}' is locked");
        }
        if (user?.IsActive == false)
        {
            throw new IdentityException($"User '{user.UserName}' not active Please Confirm Email");
        }
    }
    public string GetIp()
    {
        return _httpContext.HttpContext.Connection.RemoteIpAddress.ToString();
    }

    public void UpdateActivation(string Username)
    {
        try
        {
            var user = _context.user_account.FirstOrDefault(u => u.UserName == Username);
            if (user != null)
            {
                user.IsActive = true;
                _context.SaveChanges();
            }
        }
        catch
        {
            throw;
        }
    }
    private void UpdateUserUsedStatus(UserAccount user, bool logout)
    {
        if (logout)
            user.IsUsed = false;
        else
            user.IsUsed = true;
        user.LastLogin = DateTime.UtcNow;
        user.IpAddressUserLogin = GetIp();
        _context.SaveChanges();
    }
}