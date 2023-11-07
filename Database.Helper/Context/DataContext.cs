using Microsoft.EntityFrameworkCore;
using AbsenDulu.BE.Models.User;
using AbsenDulu.BE.Models.Approval;
using AbsenDulu.BE.Models.Company;
using AbsenDulu.BE.Models.Employee;
using AbsenDulu.BE.Models.Subcribes;
using AbsenDulu.BE.Models.Activation;
using AbsenDulu.BE.Models.LogError;
using AbsenDulu.BE.Models.Attendances;
using AbsenDulu.BE.Models.Dashboard;
using AbsenDulu.BE.Models.Identity;
using AbsenDulu.BE.Models.Notification;
using AbsenDulu.BE.Models.CalenderDay;
using AbsenDulu.BE.Models.Workflow;
using AbsenDulu.BE.Models.Schedules;
using Npgsql;
using NpgsqlTypes;
using AbsenDulu.BE.Models.Approval.Visit;
using AbsenDulu.BE.Models.MenuAccess;

namespace AbsenDulu.BE.Database.Helper.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<UserAccount> user_account { get; set; } //AdminAccount
        public DbSet<AdminAccount> admin_account { get; set; }
        // public DbSet<MasterArea> master_area { get; set; }
        public DbSet<MasterDepartment> master_department { get; set; }
        public DbSet<MasterPosition> master_position { get; set; }
        public DbSet<MasterLeave> master_leave { get; set; }
        public DbSet<MasterWorkshop> master_workshop { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<DetailSubcribe> details_subcribe { get; set; }
        public DbSet<UserActivation> user_activation { get; set; }
        public DbSet<UserResetPassword> user_reset_password { get; set; }
        public DbSet<LogErrorAbsenDulu> log_error_absendulu { get; set; }
        public DbSet<MasterShift> master_shift { get; set; }
        public DbSet<MasterReimbursements> master_reimbursements { get; set; }
        public DbSet<Attendance> attendance { get; set; }
        public DbSet<AttendanceView> attendances { get; set; }
        public DbSet<ViewMobileDashboard> attendace_mobile { get; set; }
        public DbSet<MasterCompany> master_companies { get; set; }
        public MasterCompany master_company { get; set; }
        public DbSet<ViewDetailSubscriptions> view_detail_subscriptions { get; set; }
        public DbSet<ViewAttendanceLate> view_attendances_late { get; set; }
        public DbSet<ViewAttendanceAbsent> view_attendances_absent { get; set; }
        public DbSet<ViewAttendancePresent> view_attendances_present { get; set; }
        public DbSet<ViewAttendancelateByDepartment> view_attendances_late_by_department { get; set; }
        public DbSet<ViewAttendancelateByEmployees> view_attendances_late_by_employees { get; set; }
        // public DbSet<Approvals> approvals { get; set; }
        public DbSet<ApprovalLeaveLogs> approvals_leave_log { get; set; }
        public DbSet<ApprovalReimburseLogs> approvals_reimburse_log { get; set; }
        public DbSet<ApprovalWorkShopLogs> approvals_workshop_log { get; set; }
        public DbSet<Notif> notifications { get; set; }
        public DbSet<CalenderDay> calenders { get; set; }
        public DbSet<Workflow> workflows { get; set; } 
        public DbSet<Schedule> schedule { get; set; }
        public DbSet<EmployeeSchedule> view_employee_schedule { get; set; }
        public DbSet<ApprovalLeaves> approval_leaves { get; set; }
        public DbSet<ApprovalReimburse> approval_reimburses { get; set; }
        public DbSet<ApprovalWorkShop> approval_workshop { get; set; }
        public DbSet<ApprovalVisit> approval_visit { get; set; }
        public DbSet<Admin> admin_notifications { get; set; }
        public DbSet<AccessMenu> menu_access { get; set; }
        public DbSet<AvailablePackage> available_menu { get; set; }

        public List<Schedule> GetSchedule(Guid companyId, string fromdate,string todate)
        {
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("_fromdate", NpgsqlDbType.Text) { Value = fromdate },
                new NpgsqlParameter("_todate", NpgsqlDbType.Text) { Value = todate },
                new NpgsqlParameter("_companyid", NpgsqlDbType.Uuid) { Value = companyId }
            };

            var results = schedule
                .FromSqlRaw("SELECT * FROM function_schedule(@_fromdate,@_todate, @_companyid)", parameters)
                .ToList();

            return results;
        }

        public List<EmployeeSchedule> GetEmployeeSchedule(string fromdate, string todate, string employee, Guid companyId)
        {
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("_fromdate", NpgsqlDbType.Text) { Value = fromdate },
                new NpgsqlParameter("_todate", NpgsqlDbType.Text) { Value = todate },
                new NpgsqlParameter("_employee", NpgsqlDbType.Text) { Value = employee },
                new NpgsqlParameter("_companyid", NpgsqlDbType.Uuid) { Value = companyId }
            };

            var results = view_employee_schedule
                .FromSqlRaw("SELECT * FROM function_employee_schedule(@_fromdate,@_todate,@_employee, @_companyid)", parameters)
                .ToList();

            return results;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendanceView>().HasNoKey();
            modelBuilder.Entity<ViewMobileDashboard>().HasNoKey();
            modelBuilder.Entity<ViewDetailSubscriptions>().HasNoKey();
            modelBuilder.Entity<ViewAttendanceLate>().HasNoKey();
            modelBuilder.Entity<ViewAttendanceAbsent>().HasNoKey();
            modelBuilder.Entity<ViewAttendancePresent>().HasNoKey();
            modelBuilder.Entity<ViewAttendancelateByDepartment>().HasNoKey();
            modelBuilder.Entity<ViewAttendancelateByEmployees>().HasNoKey();
            modelBuilder.Entity<Schedule>().HasNoKey();
            modelBuilder.Entity<EmployeeSchedule>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .ToList();

                var currentTime = DateTime.UtcNow;

                foreach (var entity in entities)
                {
                    if (entity.Entity is UserAccount user_account && entity.State == EntityState.Added)
                    {
                        var existingUser = this.Set<UserAccount>().FirstOrDefault(u => u.UserName == user_account.UserName);
                        var existingEmail = this.Set<UserAccount>().FirstOrDefault(u => u.Email == user_account.Email);
                        if (existingUser != null)
                        {
                            throw new Exception($"Username '{existingUser.UserName}' already exists");
                        }
                        if (existingEmail != null && existingEmail.Email != "ari.decentindonesia@gmail.com")
                        {
                            throw new Exception($"Email '{existingEmail.Email}' already exists");
                        }
                    }
                }
                return base.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}