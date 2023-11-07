using AbsenDulu.BE.Models.Activation;

namespace AbsenDulu.BE.Interfaces.IServices.Activation;
public interface IUserActivationService
{
    void AddActivation(UserActivation user);
    void UpdateUserActive(string activationCode);
}