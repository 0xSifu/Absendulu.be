using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Models.Company;

namespace AbsenDulu.BE.Interfaces.IServices;
public interface IAreaService
{
    List<MasterArea> GetArea(Guid companyId);
    MasterArea AddArea(MasterAreaDTO request);
    bool RemoveArea(string Id);
    MasterArea UpdateArea(MasterAreaDTO request);
    List<MasterArea> FindByContains(string request,Guid companyId);
}