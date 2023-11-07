using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Models.Company;
namespace AbsenDulu.BE.Interfaces.IServices;
public interface IPositionService
{
    List<MasterPosition> GetPosition(Guid request);
    List<MasterPosition> GetPositionByDepartment(Guid request,Guid department);
    MasterPosition AddPosition(MasterPositionDTO request);
    bool RemovePosition(string Id);
    MasterPosition UpdatePosition(MasterPositionDTO request);
    List<MasterPosition> FindByContains(string request ,Guid companyId);
}