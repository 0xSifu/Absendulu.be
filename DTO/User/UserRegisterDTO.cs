using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO;
public class UserRegisterDTO
{
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Fullname { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        public int? AccountTypeId { get; set; }
        public Guid CompanyId {get;set;}
        public string? CompanyName {get;set;}
        public bool IsActive { get; set; }
        public string? CreatedBy {get;set;}
        public DateTime CreatedAt {get;set;}
        public string? UpdatedBy {get;set;}
        public DateTime UpdatedAt {get;set;}

}