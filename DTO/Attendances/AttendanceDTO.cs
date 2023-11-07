using System.ComponentModel.DataAnnotations;

namespace AbsenDulu.BE.DTO.Attendances;
public class AttendanceDTO
{
    public Guid Id {get;set;}
    [Required]
    public string? AttendanceMethod {get;set;}
    public string? AttendanceType {get;set;}
    [Required]
    public string? AttendanceImage {get;set;}
    [Required]
    public string? Latitude {get;set;}
    [Required]
    public string? Longitude {get;set;}
    [Required]
    public string? LocationAddress {get;set;}
    public bool IsApproved {get;set;}
    public string? Note {get;set;}
    public string? EmployeeId {get;set;}
}