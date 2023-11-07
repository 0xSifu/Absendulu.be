using AbsenDulu.BE.DTO.Approval;
using AbsenDulu.BE.Models.Approval;

namespace AbsenDulu.BE.Interfaces.IServices.Approval;
public interface IApprovalWorkShopService
{
    List<ApprovalWorkShop> GetApprovals();
    ApprovalWorkShop AddApproval(RequestApprovalWorkShopDTO request);
    ApprovalWorkShop UpdateApproval(Guid id, ApprovalDTO request);
    List<ApprovalWorkShop> BulkApprove(string note);
    List<ApprovalWorkShop> BulkReject(string note);
    List<ApprovalWorkShop> BulkApproveSelected(BulkApprovedDTO request);
    List<ApprovalWorkShop> BulkRejectSelected(BulkRejectedDTO request);
    List<ApprovalWorkShop> GetCurrentApprover();

}