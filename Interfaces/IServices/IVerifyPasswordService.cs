namespace AbsenDulu.BE.Interfaces.IServices;
public interface IVerifyPasswordService
{
    bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
}