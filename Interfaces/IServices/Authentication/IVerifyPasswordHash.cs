namespace AbsenDulu.BE.Interfaces.IServices.Authentication;
public interface IVerifyPasswordHash
{
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
}