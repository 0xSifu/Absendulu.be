using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Identity;
[Table("user_reset_passwords")]
public class UserResetPassword
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public string? UserId { get; set; }

    [Required]
    [Column("reset_password_code")]
    [MaxLength(100)]
    public string? ResetPasswordCode { get; set; }

    [Required]
    [Column("pin_code")]
    [MaxLength(6)]
    public string? PinCode { get; set; }

    [Required]
    [Column("expire_time")]
    public DateTime ExpireTime { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Required]
    [Column("created_by")]
    public string? CreatedBy { get; set; }
}