namespace AbsenDulu.BE.Interfaces.IServices;
public interface IPasswordHashService
{
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
}