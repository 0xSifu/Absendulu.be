using System.ComponentModel.DataAnnotations;
using Alachisoft.NCache.Common.Util;

namespace AbsenDulu.BE.DTO.Company;
public class MasterShiftDTO
{
    public Guid Id {get;set;}
    [Required]
    public string? ShiftCode {get;set;}
    [Required]
    public string? ShiftName {get;set;}
    public string? MaximumLate {get;set;}
    [Required]
    public string? StartWorkTime {get;set;}
    [Required]
    public string? EndWorkTime {get;set;}
    [Required]
    public string? StartBreakTime {get;set;}
    [Required]
    public string? EndBreakTime {get;set;}
    [Required]
    public string[]? WorkDays {get;set;}
    public string? Company {get;set;}
    public Guid CompanyId {get;set;}
    public string? CreatedBy {get;set;}
    public string? UpdatedBy { get; set; }
}