using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Activation;
[Table("user_activations")]
public class UserActivation
{

    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [Column("activation_code")]
    [MaxLength(100)]
    public string? ActivationCode { get; set; }

    [Required]
    [Column("is_activated")]
    public bool IsActivated { get; set; }

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