using System.Security.Cryptography;
using AbsenDulu.BE.Interfaces.IServices.PasswordHash;

namespace AbsenDulu.BE.Services.PasswordHash;
public class VerifypasswordHasService : IVerifyPasswordHashService
{
    public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i])
                {
                    return false; // Password doesn't match
                }
            }

            return true;
        }
    }
}