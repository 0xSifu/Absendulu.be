using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO;
public class EmployeeDTO
{
    public int Id { get; set; }
    [Required]
    public string? EmployeeId { get; set; }
    [Required]
    public string? EmployeeName { get; set; }
    [Required]
    public string? DepartmentCode { get; set; }
    [Required]
    public string? DepartmentName { get; set; }
    [Required]
    public string? PositionCode { get; set; }
    [Required]
    public string? PositionName { get; set; }
    public string? AreaCode { get; set; }
    public string? AreaName { get; set; }
    public string? EmployeeTypeCode { get; set; }

    public string? EmployeeTypeName { get; set; }
    public Guid CompanyCode { get; set; }
    public string? CompanyName { get; set; }
    public string? PIC { get; set; }
    [Required]
    public string? Gender { get; set; }
    [Required]
    public DateTime? Birthday { get; set; }
    [Required]
    public string? PhoneNumber { get; set; }
    [Required]
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    [Required]
    public string? National { get; set; }
    [Required]
    public string? Religion { get; set; }
    [Required]
    public string? EmailAddress { get; set; }
    public string? BankName { get; set; }
    public string? BankAccount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? BpjsEmployeeNo { get; set; }
    public DateTime? BpjsEmployeeStartPay { get; set; }
    public DateTime? BpjsEmployeeEndPay { get; set; }
    public string? BpjsHealthCareNo { get; set; }
    public DateTime? BpjsHealthCareStartPay { get; set; }
    public DateTime? BpjsHealthCareEndPay { get; set; }
    public string? NPWPNo { get; set; }
    public DateTime? TaxStartPay { get; set; }
    public DateTime? TaxEndPay { get; set; }
    public string? ProfilePicture { get; set; }
    public string? DevicePhoto { get; set; }
    public string? WorkTypeId { get; set; }
    public string? WorkTypeName { get; set; }
    public bool IsResign { get; set; }
    public DateTime? JoinDate { get; set; }
    public DateTime? ResignDate { get; set; }
    public DateTime EffectiveStart { get; set; }
    public DateTime? EffectiveEnd { get; set; }
    public int? AccountTypeid { get; set; }=0;
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

}