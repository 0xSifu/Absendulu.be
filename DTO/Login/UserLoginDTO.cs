using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Login;
public class UserLoginDTO
{
    [Required]
    public string? username { get; set; }
    [Required]
    public string? password { get; set; }
    public bool IsMobile { get; set; }
}