using AbsenDulu.BE.Models.Approval;

namespace AbsenDulu.BE.Interfaces.IServices.Approval;
public interface IApprovalReimburseLogService
{
    List<ApprovalReimburseLogs> FindByContains(string request, Guid companyId);
    List<ApprovalReimburseLogs> GetApprovalsLog(Guid companyId, int employeeId);
    List<ApprovalReimburseLogs> GetApprovalsLogByIdApproval(Guid companyId, Guid Id);
    ApprovalReimburseLogs AddApprovalLog(ApprovalReimburseLogs request);
}