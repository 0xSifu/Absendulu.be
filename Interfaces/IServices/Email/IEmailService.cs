using AbsenDulu.BE.DTO;
using AbsenDulu.BE.Models.Identity;

namespace AbsenDulu.BE.Interfaces.IServices.Email;
public interface IEmailService
{
    Task SendEmailAsync(UserRegisterDTO user, string subject,Guid userId);
    Task SendEmaiResetPassword(UserResetPassword dataResetPassword,string Email,string Subject);
}