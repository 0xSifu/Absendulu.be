using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Models.Company;

namespace AbsenDulu.BE.Interfaces.IServices.CompanyData;
public interface IShiftService
{
    List<MasterShift> GetShift(Guid companyId);
    List<MasterShift> GetShiftById(Guid? request);
    MasterShift AddShift(MasterShiftDTO request);
    bool RemoveShift(Guid Id);
    MasterShift UpdateShift(MasterShiftDTO request);
    List<MasterShift> FindByContains(string request,Guid companyId);
}