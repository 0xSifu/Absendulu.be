using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Attendances;
[Table("attendances")]
public class Attendance
{
    [Column("id")]
    public Guid Id {get;set;}
    [Column("attendance_method")]
    public string? AttendanceMethod {get;set;}
    [Column("attendance_type")]
    public string? AttendanceType {get;set;}
    [Column("attendance_image")]
    public string? AttendanceImage {get;set;}
    [Column("latitude")]
    public string? Latitude {get;set;}
    [Column("longitude")]
    public string? Longitude {get;set;}
    [Column("locations_address")]
    public string? LocationAddress {get;set;}
    [Column("is_approved")]
    public bool IsApproved {get;set;}
    [Column("note")]
    public string? Note {get;set;}
    [Column("employee_id")]
    public int EmployeeId {get;set;}
    [Column("employee_shift")]
    public string? EmployeeShift {get;set;}
    [Column("company_name")]
    public string? Company {get;set;}
    [Column("company_id")]
    public Guid CompanyId {get;set;}
    [Column("created_by")]
    public string? CreatedBy {get;set;}
    [Column("created_at")]
    public DateTime CreatedAt {get;set;}
    [Column("updated_by")]
    public string? UpdatedBy {get;set;}
    [Column("updated_at")]
    public DateTime? UpdatedDate {get;set;}
}