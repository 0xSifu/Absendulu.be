using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.User;
[Table("admin_accounts")]
public class AdminAccount
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("username")]
    public string? UserName { get; set; }
    [Column("fullname")]
    public string? Fullname { get; set; }
    [Column("email")]
    public string? Email { get; set; }
    [Column("password_hash")]
    public byte[]? PasswordHash { get; set; }
    [Column("password_salt")]
    public byte[]? PasswordSalt { get; set; }
    [Column("role")]
    public string Role { get; set; } = string.Empty;
    [Column("company_id")]
    public Guid CompanyId { get; set; }
    [Column("company_name")]
    public string? CompanyName { get; set; }
    [Column("account_type_id")]
    public int? AccountTypeid { get; set; }
    [Column("is_active")]
    public bool IsActive { get; set; }
    [Column("expire_date")]
    public DateTime ExpireDate { get; set; }
    [Column("is_lock")]
    public bool IsLock { get; set; }
    [Column("is_used")]
    public bool IsUsed { get; set; }
    [Column("ip_address_used_login")]
    public string? IpAddressUserLogin { get; set; }
    [Column("last_login")]
    public DateTime? LastLogin { get; set; }
    [Column("created_by")]
    public string CreatedBy { get; set; } = string.Empty;
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_by")]
    public string UpdatedBy { get; set; } = string.Empty;
    [Column("updated_at")]
    public DateTime? UpdatedDate { get; set; }
}