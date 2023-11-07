using AbsenDulu.BE.DTO;
using AbsenDulu.BE.Models.User;
using AbsenDulu.BE.Database.Helper.Context;

namespace AbsenDulu.BE.Interfaces.IServices;
public interface IUserServices
{
    public bool ResetPassword(string? username);
    UserAccount Register(UserRegisterDTO request);
    void UpdatePassword(Guid UserId, string Password);
}