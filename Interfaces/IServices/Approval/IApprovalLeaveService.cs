using AbsenDulu.BE.DTO.Approval;
using AbsenDulu.BE.Models.Approval;

namespace AbsenDulu.BE.Interfaces.IServices.Approval;
public interface IApprovalLeaveService
{
    List<ApprovalLeaves> GetApprovals();
    ApprovalLeaves AddApproval(RequestApprovalLeaveDTO request);
    ApprovalLeaves UpdateApproval(Guid id, ApprovalDTO request);
    List<ApprovalLeaves> BulkApprove(string note);
    List<ApprovalLeaves> BulkReject(string note);
    List<ApprovalLeaves> BulkApproveSelected(BulkApprovedDTO request);
    List<ApprovalLeaves> BulkRejectSelected(BulkRejectedDTO request);
    List<ApprovalLeaves> GetCurrentApprover();

}