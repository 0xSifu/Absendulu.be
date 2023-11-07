using AbsenDulu.BE.DTO.User;
using AbsenDulu.BE.Models.Identity;

namespace AbsenDulu.BE.Interfaces.IServices.ResetPassword;
public interface IUserResetPasswordService
{
    UserResetPassword GetData(string email);
    Task SaveResetPassword(UserResetPassword data);
    string? ValidateData(ResetPasswordPIN data);
    Guid ValidatePIN(ResetPasswordPIN data);
}