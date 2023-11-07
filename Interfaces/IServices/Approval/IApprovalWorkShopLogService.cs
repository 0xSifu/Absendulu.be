using AbsenDulu.BE.Models.Approval;

namespace AbsenDulu.BE.Interfaces.IServices.Approval;
public interface IApprovalWorkShopLogService
{
    List<ApprovalWorkShopLogs> FindByContains(string request, Guid companyId);
    List<ApprovalWorkShopLogs> GetApprovalsLog(Guid companyId, int employeeId);
    List<ApprovalWorkShopLogs> GetApprovalsLogByIdApproval(Guid companyId, Guid Id);
    ApprovalWorkShopLogs AddApprovalLog(ApprovalWorkShopLogs request);
}