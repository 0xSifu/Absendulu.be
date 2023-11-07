namespace AbsenDulu.BE.Interfaces.IServices.Authentication;
public interface IPasswordhashService
{
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
}