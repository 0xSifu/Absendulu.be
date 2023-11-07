namespace AbsenDulu.BE.Interfaces.IServices.PasswordHash;
public interface IVerifyPasswordHashService
{
    bool VerifyPassword(string? password, byte[]? passwordHash, byte[]? passwordSalt);
}