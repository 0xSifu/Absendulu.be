using System.Security.Cryptography;
using System.Text;
using AbsenDulu.BE.Interfaces.IServices.PasswordHash;

namespace AbsenDulu.BE.Services.PasswordHash;
public class CreatePasswordHashService : ICreatePasswordHashService
{
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordHash = hmac.Key;
            passwordSalt = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}