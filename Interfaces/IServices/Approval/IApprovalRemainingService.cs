using AbsenDulu.BE.Models.Approval;

namespace AbsenDulu.BE.Interfaces.IServices.Approval;
public interface IApprovalRemainingService
{
    BalanceLeave GetRemaining(string LeaveName);
}