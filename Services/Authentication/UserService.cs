using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.DTO;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Interfaces.IServices.Email;
using AbsenDulu.BE.Interfaces.IServices.PasswordHash;
using AbsenDulu.BE.Models.User;

namespace AbsenDulu.BE.Services.Authentication;
public class UserService:IUserServices
{
        private readonly ICreatePasswordHashService _createPasswordService;
        private readonly DataContext _context;

        public UserService(ICreatePasswordHashService createPasswordHashService,DataContext context)
        {
            _context=context;
            _createPasswordService=createPasswordHashService;
        }

        public UserAccount Register(UserRegisterDTO request)
        {
            try
            {
                _createPasswordService.CreatePasswordHash(request.Password, out byte[] passwordSalt, out byte[] passwordHash);
                var ExpireDate = _context.master_companies.Where(x=> x.Id==request.CompanyId);
                UserAccount user = new UserAccount
                {
                    Id= Guid.NewGuid(),
                    UserName = request.UserName,
                    Fullname = request.Fullname,
                    Role = request.Role,
                    Email = request.Email,
                    EmployeeId=request.EmployeeId,
                    CompanyId = request.CompanyId,
                    CompanyName = request.CompanyName,
                    AccountTypeid = request.AccountTypeId,
                    ExpireDate = ExpireDate.FirstOrDefault().ExpiredDate,
                    PasswordSalt = passwordSalt,
                    PasswordHash = passwordHash,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy=request.CreatedBy
                };
                _context.user_account.Add(user);
                _context.SaveChanges();
                return user;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public bool ResetPassword(string? email)
        {
            try
            {
                var data = _context.user_account.FirstOrDefault(d => d.Email.Equals(email));
                if(data!=null)
                    return true;
                else
                    throw new IdentityException($"User with Email {email} Not Exist");
            }
            catch
            {
                throw;
            }
        }

        public void UpdatePassword(Guid UserId,string Password)
        {
            try
            {
                var data = _context.user_account.FirstOrDefault(d => d.Id.Equals(UserId));
                _createPasswordService.CreatePasswordHash(Password, out byte[] passwordSalt , out byte[] passwordHash);
                data.PasswordHash=passwordHash;
                data.PasswordSalt=passwordSalt;
                data.UpdatedBy="System";
                data.UpdatedDate=DateTime.UtcNow;
                _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
}