using System.Security.Cryptography;
using System.Text;

namespace AbsenDulu.BE.Services.Authentication;
public class PasswordHashService
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