using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Models.Company;
namespace AbsenDulu.BE.Interfaces.IServices;
public interface ILeaveService
{
    List<MasterLeave> GetLeave(Guid request);
    MasterLeave AddLeave(MasterLeaveDTO request);
    bool RemoveLeave(string Id);
    MasterLeave UpdateLeave(MasterLeaveDTO request);
    List<MasterLeave> FindByContains(string request,Guid companyId);
}