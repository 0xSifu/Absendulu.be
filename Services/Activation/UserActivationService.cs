using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Interfaces.IServices.Activation;
using AbsenDulu.BE.Models.Activation;

namespace AbsenDulu.BE.Services.Activation;
public class UserActivationService
{
    public class USerActivationService:IUserActivationService
    {
        private readonly DataContext _context;
        public USerActivationService(DataContext context)
        {
            _context=context;
        }
        public void AddActivation(UserActivation user)
        {
            try
            {
                _context.user_activation.Add(user);
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public void UpdateUserActive(string activationCode)
        {
            try
            {
                var userActivation = _context.user_activation.FirstOrDefault(u => u.ActivationCode == activationCode);
                if (userActivation != null)
                {
                    var userAccount = _context.user_account.FirstOrDefault(u => u.Id == userActivation.UserId);
                    if (userActivation.IsActivated)
                    {
                        throw new Exception("User Already Activated");
                    }
                    if (userActivation.ExpireTime < DateTime.UtcNow)
                    {
                        _context.user_account.Remove(userAccount);
                        _context.user_activation.Remove(userActivation);
                        _context.SaveChanges();
                        throw new Exception("User Activation Expired Please Register");
                    }
                    userActivation.IsActivated = true;
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("No Activation Found");
                }
            }
            catch
            {
                throw;
            }
        }
    }
}