namespace AbsenDulu.BE.DTO.User;
public class ChangePasswordDTO
{
    public string? UserName { get; set; }
    public string? PasswordOld { get; set; }
    public string? PasswordNew { get; set; }
}