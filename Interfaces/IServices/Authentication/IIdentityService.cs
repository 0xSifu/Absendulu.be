using AbsenDulu.BE.DTO.Login;
using AbsenDulu.BE.Models.Identity;

namespace AbsenDulu.BE.Interfaces.IServices.Authentication;
public interface IIdentityService
{
    UserIdentityResponses? login(UserLoginDTO user);
    UserIdentityResponses? logout();
    void UpdateActivation(string UserName);
}