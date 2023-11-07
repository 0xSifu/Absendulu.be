using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Models.Company;
namespace AbsenDulu.BE.Interfaces.IServices;
public interface IDepartmentService
{
    List<MasterDepartment> GetDepartment(Guid request);
    MasterDepartment AddDepartment(MasterDepartmentDTO request);
    bool RemoveDepartment(string Id);
    MasterDepartment UpdateDepartment(MasterDepartmentDTO request);
    List<MasterDepartment> FindByContains(string request,Guid companyId);
}