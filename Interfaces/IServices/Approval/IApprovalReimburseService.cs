using AbsenDulu.BE.DTO.Approval;
using AbsenDulu.BE.Models.Approval;

namespace AbsenDulu.BE.Interfaces.IServices.Approval;
public interface IApprovalReimburseService
{
    List<ApprovalReimburse> GetApprovals();
    ApprovalReimburse AddApproval(RequestApprovalReimburseDTO request);
    ApprovalReimburse UpdateApproval(Guid id, ApprovalDTO request);
    List<ApprovalReimburse> BulkApprove(string note);
    List<ApprovalReimburse> BulkReject(string note);
    List<ApprovalReimburse> BulkApproveSelected(BulkApprovedDTO request);
    List<ApprovalReimburse> BulkRejectSelected(BulkRejectedDTO request);
    List<ApprovalReimburse> GetCurrentApprover();

}