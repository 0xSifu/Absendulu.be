using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Models.Company;

namespace AbsenDulu.BE.Interfaces.IServices.CompanyData;
public interface IReimbursementService
{
    List<MasterReimbursements> GetReimbursement(Guid companyId);
    MasterReimbursements AddReimbursement(MasterReimbursementDTO request);
    bool RemoveReimbursement(string Id);
    MasterReimbursements UpdateReimbursement(MasterReimbursementDTO request);
    List<MasterReimbursements> FindByContains(string request,Guid companyId);
}