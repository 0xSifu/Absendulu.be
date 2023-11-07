namespace AbsenDulu.BE.Interfaces.IServices.PasswordHash;
public interface ICreatePasswordHashService
{
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
}