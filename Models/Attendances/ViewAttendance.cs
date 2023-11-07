using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenDulu.BE.Models.Attendances;
[Table("view_attendances")]
    public class AttendanceView
    {
        [Column("employee_name")]
        public string? EmployeeName { get; set; }
        [Column("employee_id")]
        public string? EmployeeId { get; set; }

        [Column("date")]
        public string? Date { get; set; }
        [Column("maximum_late")]
        public string? MaximumLate { get; set; }
        [Column("late")]
        public double? Late { get; set; }
        [Column("start_work_time")]
        public string? StartWorkTime { get; set; }

        [Column("clock_in")]
        public string? ClockIn { get; set; }

        [Column("clock_in_method")]
        public string? ClockInMethod { get; set; }
        [Column("clock_in_address")]
        public string? ClockInAddress { get; set; }
        [Column("clock_in_note")]
        public string? ClockInNote { get; set; }

        [Column("clock_in_photo")]
        public string? ClockinPhoto { get; set; }

        [Column("clock_out")]
        public string? ClockOut { get; set; }

        [Column("clock_out_method")]
        public string? ClockOutMethod { get; set; }
        [Column("clock_out_address")]
        public string? ClockOutAddress { get; set; }
        [Column("clock_out_note")]
        public string? ClockOutNote { get; set; }

        [Column("clock_out_photo")]
        public string? ClockoutPhoto { get; set; }

        [Column("department_name")]
        public string? DepartmentName { get; set; }
        [Column("position_name")]
        public string? PositionName { get; set; }

        [Column("employee_shift")]
        public string? EmployeeShift { get; set; }
        [Column("company_id")]
        public Guid CompanyId { get; set; }
    }