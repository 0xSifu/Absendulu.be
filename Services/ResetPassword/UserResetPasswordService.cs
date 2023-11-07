using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.DTO.User;
using AbsenDulu.BE.Interfaces.IServices.ResetPassword;
using AbsenDulu.BE.Models.Identity;

namespace AbsenDulu.BE.Services.ResetPassword;
public class UserResetPasswordService : IUserResetPasswordService
{
    private readonly DataContext _context;
    public UserResetPasswordService(DataContext context)
    {
        _context = context;
    }
    public UserResetPassword GetData(string email)
    {
        try
        {
            var data = _context.user_account.FirstOrDefault(d => d.Email.Equals(email));
            if (data != null)
            {
                string resetPasswordCode = Guid.NewGuid().ToString();
                string pin = GenerateRandomPin();
                UserResetPassword codeResetDetail = new UserResetPassword
                {
                    UserId = data.Id.ToString(),
                    ResetPasswordCode = resetPasswordCode,
                    PinCode = pin,
                    ExpireTime = DateTime.UtcNow.AddHours(1),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                };
                return codeResetDetail;
            }
            else
                throw new ValidationException($"User with Email {email} Not Exist");

        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }
    private string GenerateRandomPin()
    {
        Random random = new Random();
        int pin = random.Next(100000, 999999);
        return pin.ToString("D6");
    }

    public Task SaveResetPassword(UserResetPassword data)
    {
        try
        {
            _context.user_reset_password.Add(data);
            return _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public string? ValidateData(ResetPasswordPIN data)
    {
        try
        {
            var dataResult = _context.user_reset_password.FirstOrDefault(d => d.PinCode.Equals(data.Pin));
            if (dataResult != null)
            {
                if (dataResult?.ExpireTime < DateTime.UtcNow)
                {
                    throw new ValidationException("PIN Expired");
                }
                return dataResult?.UserId;
            }
            else
            {
                throw new ValidationException("Data Not Found");
            }
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public Guid ValidatePIN(ResetPasswordPIN data)
    {
        try
        {
            var dataResult = _context.user_reset_password.FirstOrDefault(d => d.PinCode.Equals(data.Pin));
            if (dataResult != null)
            {
                var dataUser = _context.user_account.FirstOrDefault(d => d.Id.ToString().Equals(dataResult.UserId));
                if (dataResult?.ExpireTime < DateTime.UtcNow)
                {
                    throw new ValidationException("PIN Expired");
                }
                return dataUser.Id;
            }
            else
            {
                throw new ValidationException("PIN Not Valid");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}