using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Company;
[Table("master_companies")]
public class MasterCompany
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Required]
    [Column("company_name")]
    public string? CompanyName { get; set; }
    [Required]
    [Column("company_location")]
    public string? CompanyLocation { get; set; }
    [Required]
    [Column("company_city")]
    public string? CompanyCity { get; set; }
    [Column("longitude")]
    public string? Longitude { get; set; }
    [Column("latitude")]
    public string? Latitude { get; set; }
    [Column("company_business_type")]
    public string? CompanyBusinessType { get; set; }
    [Column("director")]
    public string? Director { get; set; }
    [Required]
    [Column("contact_number")]
    public string? ContactNumber { get; set; }
    [Required]
    [Column("phone_number_1")]
    public string? PhoneNuber1 { get; set; }
    [Column("phone_number_2")]
    public string? PhoneNuber2 { get; set; }
    [Required]
    [Column("pic")]
    public string? PIC { get; set; }
    [Required]
    [Column("is_active")]
    public bool IsActive { get; set; }
    [Column("is_lock")]
    public bool IsLock { get; set; }
    [Column("subcribe_date")]
    public DateTime SubcribeDate { get; set; }
    [Column("expired_date")]
    public DateTime ExpiredDate { get; set; }
    [Column("icon")]
    public string? Icon { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedDate { get; set; }
}