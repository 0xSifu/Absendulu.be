using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.DTO.Notifications;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Interfaces.IServices.Approval;
using AbsenDulu.BE.Interfaces.IServices.Notification;
using AbsenDulu.BE.Interfaces.IServices.PushNotification;
using AbsenDulu.BE.Models.Approval;
using AbsenDulu.BE.Token.Helper;

namespace AbsenDulu.BE.Services.Approval;
public class ApprovalLogService: IApprovalLogService
{
    private readonly DataContext _context;
    private readonly IEmployeeService _employee;
    private readonly TokenValidate _tokenConfig;


    public ApprovalLogService( DataContext dataContext, IEmployeeService employeeService, TokenValidate tokenValidate, IRabbitMQService rabbitMQService)
    {
        _context = dataContext;
        _employee = employeeService;
        _tokenConfig = tokenValidate;

    }

    public List<ApprovalLeaveLogs> FindByContains(string request, Guid companyId)
    {
        try
        {
            var data = _context.approvals_leave_log
            .Where(e => e.Status.ToLower().Contains(request.ToLower()) || e.ApprovalId.ToString().Contains(request) || e.Status.ToString().Contains(request)
            && e.CompanyId == companyId && e.ApproverId.ToString() == _tokenConfig.EmployeeId).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }

    public List<ApprovalLeaveLogs> GetApprovalsLog(Guid companyId, int employeeId)
    {
        try
        {
            var data = _context.approvals_leave_log.Where(d => d.CompanyId.Equals(companyId) && d.ApproverId == employeeId).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }
    public List<ApprovalLeaveLogs> GetApprovalsLogByIdApproval(Guid companyId, Guid Id)
    {
        try
        {
            var data = _context.approvals_leave_log.Where(d => d.CompanyId.Equals(companyId) && d.ApprovalId == Id).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }

    public ApprovalLeaveLogs AddApprovalLog(ApprovalLeaveLogs request)
    {
        try
        {
            ApprovalLeaveLogs ApprovalLog = new ApprovalLeaveLogs
            {
                Id = Guid.NewGuid(),
                ApprovalId = request.ApprovalId,
                RequestorId = request.RequestorId,
                ApproverId = request.ApproverId,
                Note = request.Note,
                Status = request.Status,
                CompanyId = request.CompanyId,
                CreatedAt = DateTime.UtcNow
            };
            _context.approvals_leave_log.Add(ApprovalLog);
            _context.SaveChanges();

            return ApprovalLog;
        }
        catch
        {
            throw;
        }
    }
}