using AbsenDulu.BE.Models.Approval;

namespace AbsenDulu.BE.Interfaces.IServices.Approval;
public interface IApprovalLogService
{
    List<ApprovalLeaveLogs> FindByContains(string request, Guid companyId);
    List<ApprovalLeaveLogs> GetApprovalsLog(Guid companyId, int employeeId);
    List<ApprovalLeaveLogs> GetApprovalsLogByIdApproval(Guid companyId, Guid Id);
    ApprovalLeaveLogs AddApprovalLog(ApprovalLeaveLogs request);
}