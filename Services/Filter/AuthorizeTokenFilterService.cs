using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Token.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AbsenDulu.BE.Filters
{
    public class AuthorizeTokenFilterService : IAsyncActionFilter
    {
        private TokenConfiguration _token;
        private readonly DataContext _context;
        public AuthorizeTokenFilterService(DataContext dataContext, TokenConfiguration token)
        {
            _token=token;
            _context=dataContext;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = _token.GetTokenClaim(token);
            var data = _context.user_account.FirstOrDefault(d => d.UserName.Equals(user.UserName));
            var dataAdmin = _context.admin_account.FirstOrDefault(d => d.UserName.Equals(user.UserName));

            if (!_token.ValidateToken(token))
            {
                context.Result = new UnauthorizedObjectResult("Access Denied");
                return;
            }
            else
            {
                if(dataAdmin==null)
                {
                    if(data==null)
                    {
                        throw new TokenException("Access Denied");
                    }
                }
                if(data!=null)
                {
                    if (data.IsLock)
                    {
                        context.Result = new ForbidResult($"User '{user?.UserName}' is locked");
                        return;
                    }
                    if (data.IsActive == false)
                    {
                        context.Result = new ForbidResult($"User '{user?.UserName}' not active Please Confirm Email");
                        return;
                    }
                    if (data?.ExpireDate < DateTime.UtcNow)
                    {
                        throw new CredentialException($"User '{user?.UserName}' Expired , please contact Admin or email marketing@absendulu.com");
                    }
                }
            }

            await next();
        }
    }
}