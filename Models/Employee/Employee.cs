using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Employee;
[Table("employees")]
public class Employee
{

    [Column("id")]
    public int Id { get; set; }
    [Column("employee_id")]
    public string? EmployeeId { get; set; }
    [Column("shift_id")]
    public Guid? ShiftId { get; set; }
    [Column("employee_name")]
    public string? EmployeeName { get; set; }

    [Column("department_code")]
    public string? DepartmentCode { get; set; }

    [Column("department_name")]
    public string? DepartmentName { get; set; }
    [Column("position_id")]
    public Guid PositionId { get; set; }

    [Column("position_code")]
    public string? PositionCode { get; set; }

    [Column("position_name")]
    public string? PositionName { get; set; }

    [Column("employee_type_code")]
    public string? EmployeeTypeCode { get; set; }

    [Column("employee_type_name")]
    public string? EmployeeTypeName { get; set; }

    [Column("company_id")]
    public Guid CompanyCode { get; set; }

    [Column("company_name")]
    public string? CompanyName { get; set; }
    [Column("gender")]
    public string? Gender { get; set; }

    [Column("birthday")]
    public DateTime? Birthday { get; set; }

    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("postal_code")]
    public string? PostalCode { get; set; }

    [Column("national")]
    public string? National { get; set; }
    [Column("religion")]
    public string? Religion { get; set; }

    [Column("email_address")]
    public string? EmailAddress { get; set; }

    [Column("profile_picture")]
    public string? ProfilePicture { get; set; }
     [Column("device_photo")]
    public string? DevicePhoto { get; set; }

    [Column("work_type_id")]
    public string? WorkTypeId { get; set; }

    [Column("work_type_name")]
    public string? WorkTypeName { get; set; }

    [Column("area_code")]
    public string? AreaCode { get; set; }

    [Column("area_name")]
    public string? AreaName { get; set; }
    [Column("avatar")]
    public string? Avatar { get; set; }


    [Column("is_resign")]
    public bool IsResign { get; set; }

    [Column("join_date")]
    public DateTime? JoinDate { get; set; }

    [Column("resign_date")]
    public DateTime? ResignDate { get; set; }

    [Column("effective_start")]
    public DateTime EffectiveStart { get; set; }

    [Column("effective_end")]
    public DateTime? EffectiveEnd { get; set; }
    [Column("account_type_id")]
    public int? AccountTypeid { get; set; }
    [Column("created_by")]
    public string? CreatedBy { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_by")]
    public string? UpdatedBy { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }


}